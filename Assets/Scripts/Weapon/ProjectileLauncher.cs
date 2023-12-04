using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletScale;
    public float speed;
    public int damage;
    public float aoESize;
    public float minRange = 0;
    public GameObject enemyParent;
    public float fireInterval;
    public LayerMask targetLayers;
    public Vector3 bulletOffset;
    public string gunObjectName;
    public AudioSource audioSourceShoot;
    public AudioSource audioSourceHitProjectile;

    private float timeSinceLastFire;
    private GameObject gunObject = null;

    void Start()
    {
        timeSinceLastFire = 0f;
        Transform gunTransform = transform.Find(gunObjectName);
        if (gunTransform != null )
        {
            gunObject = gunTransform.gameObject;
        }
        
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
            bullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
            bullet.transform.position = transform.position + bulletOffset;
            BulletController bulletController = bullet.AddComponent<BulletController>();
            bullet.transform.LookAt(nearestEnemy.transform.position);
            bulletController.type = BulletController.BulletType.Projectile;
            Vector3 target = nearestEnemy.transform.position;
            bulletController.target = target;
            bulletController.speed = speed;
            bulletController.damage = damage;
            bulletController.aoESize = aoESize;
            bulletController.targetLayers = targetLayers;
            bulletController.projectileHitSound = audioSourceHitProjectile;
            Vector3 velocity = Vector3.zero;
            NavMeshAgent navMeshAgent = nearestEnemy.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null ) {
                velocity = navMeshAgent.velocity;
            }
            bulletController.targetVelocity = velocity;

            if (gunObject != null)
            {
                float targetDistance = Vector3.Distance(transform.position, target);
                float flightDuration = targetDistance / speed;
                float timeSinceLaunch = Time.deltaTime * 5;
                float timeRatio = timeSinceLaunch / flightDuration;
                Vector3 nextPosition = Vector3.Lerp(transform.position, target, timeRatio);
                nextPosition.y += Mathf.Sin(timeRatio * Mathf.PI) * (flightDuration * speed * 0.25f);
                Vector3 tangent = nextPosition - transform.position;
                Transform gunObjectTransform = gunObject.transform;
                gunObjectTransform.rotation = Quaternion.LookRotation(tangent.normalized);
            }
            audioSourceShoot.Play();
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
                if (distance < minRange) {
                    continue;
                }
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
