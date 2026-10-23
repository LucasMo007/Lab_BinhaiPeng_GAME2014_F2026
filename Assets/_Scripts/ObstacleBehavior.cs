using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float bottomBoundary = -10f;

    void Update()
    {
        
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        
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
            gameObject.SetActive(false); 
        }
    }
}
