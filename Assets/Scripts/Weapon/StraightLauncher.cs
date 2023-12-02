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
    public GameObject enemyParent;
    public float fireInterval;
    public LayerMask targetLayers;
    public Vector3 bulletOffset;
    public string gunObjectName;

    private float timeSinceLastFire;
    private GameObject gunObject = null;

    void Start()
    {
        timeSinceLastFire = 0f;
        gunObject = transform.Find(gunObjectName).gameObject;
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
        Vector3 velocity = nearestEnemy.GetComponent<NavMeshAgent>().velocity;

        if (nearestEnemy != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.position = transform.position + bulletOffset;
            BulletController bulletController = bullet.AddComponent<BulletController>();
            bullet.transform.LookAt(nearestEnemy.transform.position);
            bulletController.type = BulletController.BulletType.Straight;
            Vector3 target = nearestEnemy.transform.position;
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
