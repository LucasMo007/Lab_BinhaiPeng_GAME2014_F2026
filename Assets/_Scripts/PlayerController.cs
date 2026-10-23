//using UnityEngine;

//public class PlayerTouchController : MonoBehaviour
//{
//    private Camera mainCamera;
//    [SerializeField] private float smoothSpeed = 15f;
//    [SerializeField] private Vector2 xBounds = new Vector2(-3.5f, 3.5f);
//    [SerializeField] private Vector2 yBounds = new Vector2(-7f, 7f);

//    void Start()
//    {
//        mainCamera = Camera.main;
//    }

//    void Update()
//    {
//        HandleTouchInput();
//    }

//    private void HandleTouchInput()
//    {
//        Vector3 targetWorldPosition = transform.position;
//        bool hasInput = false;

//        // 1. 真机 / 设备模拟器多点触控输入
//        if (Input.touchCount > 0)
//        {
//            Touch touch = Input.GetTouch(0);
//            Vector3 touchScreenPos = new Vector3(touch.position.x, touch.position.y, -mainCamera.transform.position.z);
//            targetWorldPosition = mainCamera.ScreenToWorldPoint(touchScreenPos);
//            hasInput = true;
//        }
//        // 2. 兼容编辑器鼠标点击拖拽（方便在 Device Simulator 调试）
//        else if (Input.GetMouseButton(0))
//        {
//            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
//            targetWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);
//            hasInput = true;
//        }

//        if (hasInput)
//        {
//            // 限制玩家活动范围在屏幕内
//            float clampedX = Mathf.Clamp(targetWorldPosition.x, xBounds.x, xBounds.y);
//            float clampedY = Mathf.Clamp(targetWorldPosition.y, yBounds.x, yBounds.y);
//            Vector3 clampedTarget = new Vector3(clampedX, clampedY, 0);

//            // 平滑插值移动
//            transform.position = Vector3.Lerp(transform.position, clampedTarget, smoothSpeed * Time.deltaTime);
//        }
//    }
//}
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
