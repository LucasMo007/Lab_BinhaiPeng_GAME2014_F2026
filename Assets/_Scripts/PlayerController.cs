
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 18f;
    [SerializeField] private float paddingX = 0.4f;
    [SerializeField] private float paddingY = 0.6f;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private AudioClip shootSound;

    private Camera mainCamera;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float nextFireTime = 0f;

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateScreenBounds();
    }

    private void Update()
    {
        HandleTouchAndMove();
    }

    private void CalculateScreenBounds()
    {
        if (mainCamera == null) return;
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minBounds = new Vector2(bottomLeft.x + paddingX, bottomLeft.y + paddingY);
        maxBounds = new Vector2(topRight.x - paddingX, topRight.y - paddingY);
    }

    private void HandleTouchAndMove()
    {
        bool isInteracting = false;
        Vector3 targetPos = transform.position;

        // 1. Mobile touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(touch.position);
            targetPos = new Vector3(worldPos.x, worldPos.y, 0f);
            isInteracting = true;
        }
        // 2. PC simulator / mouse input
        else if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPos = new Vector3(mousePos.x, mousePos.y, 0f);
            isInteracting = true;
        }

        // Move and fire continuously while holding/touching the screen
        if (isInteracting)
        {
            float clampedX = Mathf.Clamp(targetPos.x, minBounds.x, maxBounds.x);
            float clampedY = Mathf.Clamp(targetPos.y, minBounds.y, maxBounds.y);
            transform.position = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, 0f), moveSpeed * Time.deltaTime);

            FireLaser();
        }
    }

    private void FireLaser()
    {
        if (Time.time >= nextFireTime && BulletManager.Instance != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            // Get a pooled bullet instead of instantiating a new one
            BulletManager.Instance.GetBullet(spawnPos);

            if (shootSound != null && mainCamera != null)
            {
                AudioSource.PlayClipAtPoint(shootSound, mainCamera.transform.position, 0.4f);
            }

            nextFireTime = Time.time + fireRate;
        }
    }
}
