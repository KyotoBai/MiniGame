using UnityEngine;
using System.Collections.Generic;

public class BulletController : MonoBehaviour
{
    public enum BulletType { Bullet, Projectile }

    [SerializeField] public BulletType type;
    [SerializeField] public Vector3 target;
    [SerializeField] public float speed = 5;
    [SerializeField] public int damage = 100;
    [SerializeField] public float aoESize = 0;
    [SerializeField] public LayerMask targetLayers;

    private float timeSinceLaunch;
    private Vector3 startPosition;
    private float flightDuration;
    private AoEHandler aoEHandler;

    void Start()
    {
        aoEHandler = new AoEHandler(targetLayers);
        startPosition = transform.position;

        if (type == BulletType.Projectile)
        {
            float targetDistance = Vector3.Distance(startPosition, target);
            flightDuration = targetDistance / speed;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        else
        {
            Debug.Log("BulletController requires a Collider component.");
        }
    }

    void Update()
    {
        if (type == BulletType.Bullet)
        {
            MoveStraight();
        }
        else if (type == BulletType.Projectile)
        {
            MoveParabolic();
        }
    }

    void MoveStraight()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            HitTarget();
        }
    }

    void MoveParabolic()
    {
        timeSinceLaunch += Time.deltaTime;
        float timeRatio = timeSinceLaunch / flightDuration;

        Vector3 nextPosition = Vector3.Lerp(startPosition, target, timeRatio);
        nextPosition.y += Mathf.Sin(timeRatio * Mathf.PI) * (flightDuration * speed * 0.25f); // Adjust the height factor as needed

        transform.position = nextPosition;

        if (timeSinceLaunch >= flightDuration)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        Debug.Log("Target Hit!!!");
        if (aoESize <= 0)
        {
            aoESize = 0.5f;
        }

        List<GameObject> targetsInAoE = aoEHandler.GetObjectsInSphereAoE(transform.position, aoESize);
        Debug.Log("AoE on " + targetsInAoE.Count);

        foreach (var obj in targetsInAoE)
        {
            Health health = obj.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject); // Destroy the bullet after hitting the target
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if ((targetLayers & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("Collider Hit!!!");
            HitTarget();
        }
    }
}
