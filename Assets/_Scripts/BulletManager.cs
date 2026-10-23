using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BulletFactory))]
public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [SerializeField] private int poolSize = 20;

    private BulletFactory factory;
    private Queue<GameObject> bulletPool;

    private void Awake()
    {
        Instance = this;
        factory = GetComponent<BulletFactory>();

        // Fill the pool using the factory
        bulletPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool.Enqueue(factory.CreateBullet(transform));
        }
    }

    // Takes a bullet from the pool (the factory makes a new one if the pool is empty)
    public GameObject GetBullet(Vector3 position)
    {
        GameObject bullet = bulletPool.Count > 0 ? bulletPool.Dequeue() : factory.CreateBullet(transform);
        bullet.transform.position = position;
        bullet.SetActive(true);
        return bullet;
    }

    // Puts a bullet back into the pool
    public void ReturnBullet(GameObject bullet)
    {
        if (!bullet.activeSelf) return;
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
