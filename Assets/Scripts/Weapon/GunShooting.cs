using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [Header("Bullet Properties")]
    [SerializeField] private bool shootgun = false;
    [SerializeField] private bool projectile = false;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletAoESize = 0f;
    [SerializeField] private float bulletCooldown = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletScale = 1f;
    [SerializeField] private Vector3 bulletOffset = Vector3.zero;
    [SerializeField] private LayerMask bulletTargetLayers;
    [SerializeField, Range(0.1f, 1.5f)] private float drag = 1f;
    // Start is called before the first frame update
    public void Fire(Vector3 vec)
    {
        if (!projectile && !shootgun)
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
        else if (shootgun)
        {
            float spreadAngle = 5f;

            GameObject bullet_1 = InstantiateBullet(bulletPrefab);
            bullet_1.transform.position += bulletOffset;
            bullet_1.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
            BulletController bulletController_1 = bullet_1.AddComponent<BulletController>();
            bulletController_1.type = BulletController.BulletType.BulletPlayer;
            bullet_1.transform.LookAt(vec);
            Vector3 targetPoint_1 = transform.position + vec * 10f;
            bulletController_1.target = targetPoint_1;
            bulletController_1.speed = bulletSpeed;
            bulletController_1.damage = bulletDamage;
            bulletController_1.aoESize = bulletAoESize;
            bulletController_1.targetLayers = bulletTargetLayers;

            int amount = 1;

            GameObject bullet_2 = InstantiateBullet(bulletPrefab);
            bullet_2.transform.position += bulletOffset;
            bullet_2.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
            BulletController bulletController_2 = bullet_2.AddComponent<BulletController>();
            bulletController_2.type = BulletController.BulletType.BulletPlayer;
            Vector3 v = new Vector3(vec.x, vec.y ,vec.z + amount * 5f);
            bullet_2.transform.rotation = Quaternion.LookRotation(vec) * Quaternion.Euler(0, -spreadAngle, 0); // Rotate to the left
            Vector3 targetPoint_2 = transform.position + bullet_2.transform.forward * 10f;
            bulletController_2.target = targetPoint_2;
            bulletController_2.speed = bulletSpeed;
            bulletController_2.damage = bulletDamage;
            bulletController_2.aoESize = bulletAoESize;
            bulletController_2.targetLayers = bulletTargetLayers;

            GameObject bullet_3 = InstantiateBullet(bulletPrefab);
            bullet_3.transform.position += bulletOffset;
            bullet_3.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
            BulletController bulletController_3 = bullet_3.AddComponent<BulletController>();
            bulletController_3.type = BulletController.BulletType.BulletPlayer;
            bullet_3.transform.rotation = Quaternion.LookRotation(vec) * Quaternion.Euler(0, spreadAngle, 0); // Rotate to the right
            Vector3 targetPoint_3 = transform.position + bullet_3.transform.forward * 10f;
            bulletController_3.target = targetPoint_3;
            bulletController_3.speed = bulletSpeed;
            bulletController_3.damage = bulletDamage;
            bulletController_3.aoESize = bulletAoESize;
            bulletController_3.targetLayers = bulletTargetLayers;
        }
        else
        {
            //Debug.Log("vec3 is: " + vec);
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

    public float GetDrag()
    {
        return drag;
    }
}
