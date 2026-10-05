using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int poolSize = 10;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnY = 8f;
    [SerializeField] private float spawnXMin = -3f;
    [SerializeField] private float spawnXMax = 3f;

    private List<GameObject> pool;
    private float timer;

    void Start()
    {
        // 初始化对象池
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    private void SpawnObstacle()
    {
        GameObject obstacle = GetPooledObject();
        if (obstacle != null)
        {
            float randomX = Random.Range(spawnXMin, spawnXMax);
            obstacle.transform.position = new Vector3(randomX, spawnY, 0f);
            obstacle.SetActive(true);
        }
    }

    // 从池中查找未激活的对象
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null; // 池已耗尽（可在需求时动态扩容）
    }
}
