using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float bottomBoundary = -10f;

    void Update()
    {
        // 障碍物向下掉落
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 超出底部屏幕后回收到对象池（直接隐藏）
        if (transform.position.y < bottomBoundary)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit obstacle!");
            gameObject.SetActive(false); // 撞击后回收
        }
    }
}
