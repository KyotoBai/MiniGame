using UnityEngine;
using System.Collections;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed;
    public int damage;
    public float aoESize;
    public GameObject enemyParent;
    public float fireInterval;
    public LayerMask targetLayers;

    private float timeSinceLastFire;

    void Start()
    {
        timeSinceLastFire = 0f;
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        timeSinceLastFire += Time.deltaTime;
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            if (timeSinceLastFire >= fireInterval)
            {
                if (enemyParent  != null)
                {
                    FireProjectile();
                    timeSinceLastFire = 0f;
                } else
                {
                    Debug.Log("Enemy parent object is not assigned.");
                }
            }
            yield return null;
        }
    }

    void FireProjectile()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            GameObject bullet = bulletPrefab != null ? Instantiate(bulletPrefab, transform.position, Quaternion.identity) : CreateDefaultBullet();
            bullet.transform.position = transform.position;
            BulletController bulletController = bullet.AddComponent<BulletController>();
            bullet.transform.LookAt(nearestEnemy.transform.position);
            bulletController.type = BulletController.BulletType.Projectile;
            bulletController.target = nearestEnemy.transform.position;
            bulletController.speed = speed;
            bulletController.damage = damage;
            bulletController.aoESize = aoESize;
            bulletController.targetLayers = targetLayers;
            
            // Adjust bullet controller target layers and other parameters as needed
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Transform child in enemyParent.transform)
        {
            ProjectileLauncher projectile = child.GetComponent<ProjectileLauncher>();
            if (projectile == null)
            {
                float distance = Vector3.Distance(transform.position, child.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = child.gameObject;
                }
            }
        }

        return nearestEnemy;
    }

    GameObject CreateDefaultBullet()
    {
        GameObject defaultBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        defaultBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Set the size of the default bullet
        // defaultBullet.AddComponent<Rigidbody>().useGravity = false; // Add Rigidbody and disable gravity
        defaultBullet.transform.position = transform.position;
        return defaultBullet;
    }
}
