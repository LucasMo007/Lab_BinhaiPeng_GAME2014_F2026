using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 3f;
    [SerializeField] private float backgroundHeight = 10f; // 对应你的 Scale Y = 10

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 沿 Y 轴向下滚动
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // 移出一整块背景的高度（10单位）后，复位回初始位置
        if (transform.position.y <= startPosition.y - backgroundHeight)
        {
            transform.position = startPosition;
        }
    }
}
