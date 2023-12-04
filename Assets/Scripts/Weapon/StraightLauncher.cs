using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StraightLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed;
    public int damage;
    public float aoESize;
    public float fireInterval;
    public float attackRange = 0f;
    public GameObject enemyParent;
    public LayerMask targetLayers;
    public Vector3 bulletOffset;
    public string gunObjectName;

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
                if (enemyParent != null)
                {
                    FireStraight();
                    timeSinceLastFire = 0f;
                }
                else
                {
                    Debug.Log("Enemy parent object is not assigned.");
                }
            }
            yield return null;
        }
    }

    void FireStraight()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        
        if (nearestEnemy != null)
        {
            Vector3 velocity = nearestEnemy.GetComponent<NavMeshAgent>().velocity;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.position = transform.position + bulletOffset;
            BulletController bulletController = bullet.AddComponent<BulletController>();
            bullet.transform.LookAt(nearestEnemy.transform.position);
            bulletController.type = BulletController.BulletType.Straight;
            Vector3 target = new Vector3(nearestEnemy.transform.position.x, nearestEnemy.transform.position.y + 0.7f, nearestEnemy.transform.position.z);
            bulletController.target = target;
            bulletController.speed = speed;
            bulletController.damage = damage;
            bulletController.aoESize = aoESize;
            bulletController.targetLayers = targetLayers;
            bulletController.targetVelocity = velocity;

            if (gunObject != null)
            {
                Transform gunObjectTransform = gunObject.transform;
                Vector3 direction = target - transform.position;
                direction.y = 0f;
                direction.Normalize();
                gunObjectTransform.rotation = Quaternion.LookRotation(direction);
            }
            // Adjust bullet controller target layers and other parameters as needed
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        if (attackRange > 0.1f)
        {
            nearestDistance = attackRange;
        }

        foreach (Transform child in enemyParent.transform)
        {
            StraightLauncher straight = child.GetComponent<StraightLauncher>();
            if (straight == null)
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

    
}
