using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float topBoundary = 8f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > topBoundary)
        {
            BulletManager.Instance.ReturnBullet(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bullet hits an enemy/obstacle
        ObstacleBehavior obstacle = other.GetComponent<ObstacleBehavior>();
        if (obstacle != null && other.gameObject.activeSelf)
        {
            GameManager.Instance.AddScore(obstacle.PointValue);
            other.gameObject.SetActive(false);
            BulletManager.Instance.ReturnBullet(gameObject);
        }
    }
}
