using UnityEngine;
using System.Collections.Generic;

public class BulletController : MonoBehaviour
{
    public enum BulletType { BulletPlayer, Projectile, Straight }

    [SerializeField] public BulletType type;
    [SerializeField] public Vector3 target;
    [SerializeField] public float speed = 5;
    [SerializeField] public int damage = 100;
    [SerializeField] public float aoESize = 0;
    [SerializeField] public LayerMask targetLayers;
    public Vector3 targetVelocity;

    private float timeSinceLaunch;
    private Vector3 startPosition;
    private float flightDuration;
    private AoEHandler aoEHandler;

    void Start()
    {
        aoEHandler = new AoEHandler(targetLayers);
        startPosition = transform.position;

        if (type == BulletType.Projectile || type == BulletType.Straight)
        {

            target = Prediction(speed, startPosition, target, targetVelocity, 1);

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
        if (type == BulletType.BulletPlayer)
        {
            MoveStraight();
        }
        else if (type == BulletType.Projectile)
        {
            MoveParabolic();
        }
        else if (type == BulletType.Straight)
        {
            MoveStraightLockTarget();
        }
    }

    void MoveStraight()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(90, 0, 0);
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
        //Debug.Log("next post: " + nextPosition);
        nextPosition.y += Mathf.Sin(timeRatio * Mathf.PI) * (flightDuration * speed * 0.25f); // Adjust the height factor as needed

        Vector3 tangent = nextPosition - transform.position;

        // Rotate the bullet to point in the direction of the tangent
        if (tangent != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(tangent.normalized) * Quaternion.Euler(90, 0, 0);
        }

        transform.position = nextPosition;

        Vector3 direction = nextPosition - transform.position;

        // Rotate the object to face the moving direction
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction.normalized);
        }

        if (timeSinceLaunch >= flightDuration)
        {
            HitTarget();
        }
    }

    void MoveStraightLockTarget()
    {
        timeSinceLaunch += Time.deltaTime;
        float timeRatio = timeSinceLaunch / flightDuration;

        Vector3 nextPosition = Vector3.Lerp(startPosition, target, timeRatio);
        //Debug.Log("next post: " + nextPosition);

        Vector3 tangent = nextPosition - transform.position;

        // Rotate the bullet to point in the direction of the tangent
        if (tangent != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(tangent.normalized) * Quaternion.Euler(90, 0, 0);
        }

        transform.position = nextPosition;

        Vector3 direction = nextPosition - transform.position;

        // Rotate the object to face the moving direction
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction.normalized);
        }


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

    Vector3 Prediction(float BulletSpeed, Vector3 startPos, Vector3 targetPos, Vector3 targetVelocity, int iter)
    {
        Vector3 targetNewPos = targetPos;
        float t = Vector3.Distance(targetNewPos, startPos) / BulletSpeed;
        for (int i = 0; i < iter; i++)
        {
            if (type == BulletType.Projectile)
            {
                targetNewPos = targetPos + targetVelocity * (t * 0.7f);
            }
            else if (type == BulletType.Straight)
            {
                targetNewPos = targetPos + targetVelocity * t;
            }
            t = Vector3.Distance(targetNewPos, startPos) / BulletSpeed;
        }
        return targetNewPos;
    }

    public Quaternion GetOrientation()
    {
        Quaternion lookDirc = Quaternion.LookRotation(startPosition - target);
        return Quaternion.Euler(0, lookDirc.eulerAngles.y, 0);
    }
}