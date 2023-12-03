using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] private bool projectile = false;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletAoESize = 0f;
    [SerializeField] private float bulletCooldown = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletScale = 1f;
    [SerializeField] private Vector3 bulletOffset = Vector3.zero;
    [SerializeField] private LayerMask bulletTargetLayers;
    // Start is called before the first frame update
    public void Fire(Vector3 vec)
    {
        if (!projectile)
        {
            GameObject bullet = InstantiateBullet(bulletPrefab);
            bullet.transform.position += bulletOffset;
            bullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
            BulletController bulletController = bullet.AddComponent<BulletController>();
            bulletController.type = BulletController.BulletType.BulletPlayer;
            bullet.transform.LookAt(vec);
            Vector3 targetPoint = transform.position + vec * 10f;
            bulletController.target = targetPoint;
            bulletController.speed = bulletSpeed;
            bulletController.damage = bulletDamage;
            bulletController.aoESize = bulletAoESize;
            bulletController.targetLayers = bulletTargetLayers;
        }
        else
        {
            Debug.Log("vec3 is: " + vec);
            if (vec != null)
            {
                GameObject projectile = InstantiateBullet(bulletPrefab);
                projectile.transform.position += bulletOffset;
                projectile.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
                BulletController bulletController = projectile.AddComponent<BulletController>();
                bulletController.type = BulletController.BulletType.Projectile;
                bulletController.target = vec;
                bulletController.speed = bulletSpeed;
                bulletController.damage = bulletDamage;
                bulletController.aoESize = bulletAoESize;
                bulletController.targetLayers = bulletTargetLayers;
            }
        }
    }

    GameObject InstantiateBullet(GameObject prefab)
    {
        if (prefab == null)
        {
            return CreateDefaultBullet();
        }
        else
        {
            return Instantiate(prefab, transform.position, Quaternion.LookRotation(transform.forward));
        }
    }

    GameObject CreateDefaultBullet()
    {
        GameObject defaultBullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        defaultBullet.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        defaultBullet.transform.position = transform.position;
        return defaultBullet;
    }

    public float GetCoolDown()
    {
        return bulletCooldown;
    }
}
