using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    // Creates a new bullet, inactive, ready to be stored in the pool
    public GameObject CreateBullet(Transform parent)
    {
        GameObject bullet = Instantiate(bulletPrefab, parent);
        bullet.SetActive(false);
        return bullet;
    }
}
