using UnityEngine;

public class PlayerTouchController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private float smoothSpeed = 15f;
    [SerializeField] private Vector2 xBounds = new Vector2(-3.5f, 3.5f);
    [SerializeField] private Vector2 yBounds = new Vector2(-7f, 7f);

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        Vector3 targetWorldPosition = transform.position;
        bool hasInput = false;

        // 1. 真机 / 设备模拟器多点触控输入
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchScreenPos = new Vector3(touch.position.x, touch.position.y, -mainCamera.transform.position.z);
            targetWorldPosition = mainCamera.ScreenToWorldPoint(touchScreenPos);
            hasInput = true;
        }
        // 2. 兼容编辑器鼠标点击拖拽（方便在 Device Simulator 调试）
        else if (Input.GetMouseButton(0))
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z);
            targetWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            hasInput = true;
        }

        if (hasInput)
        {
            // 限制玩家活动范围在屏幕内
            float clampedX = Mathf.Clamp(targetWorldPosition.x, xBounds.x, xBounds.y);
            float clampedY = Mathf.Clamp(targetWorldPosition.y, yBounds.x, yBounds.y);
            Vector3 clampedTarget = new Vector3(clampedX, clampedY, 0);

            // 平滑插值移动
            transform.position = Vector3.Lerp(transform.position, clampedTarget, smoothSpeed * Time.deltaTime);
        }
    }
}
