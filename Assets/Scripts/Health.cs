using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] public int maxHitPoints = 10;
    [SerializeField] public float minimumAttackInterval = 0.5f;
    public int currentHitPoints;

    // Updated event name for clarity
    public delegate void OnHealthDepleted();

    // Event declaration using the new name
    public event OnHealthDepleted onHealthDepletedEvent;


    // Rigidbody of the GameObject
    private Rigidbody rb;

    // last attack time
    private float lastAttackTime;
    public bool useSound = false;

    public int CurrentHitPoints
    {
        get { return currentHitPoints; }
    }

    public HealthBar healthBar;

    void Start()
    {
        currentHitPoints = maxHitPoints;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHitPoints);
        }
        rb = GetComponent<Rigidbody>();
        lastAttackTime = Time.time;
    }

    void Update()
    {
        if (currentHitPoints <= 0)
        {
            // Only destroy the game object and trigger event if not already done.
            if (gameObject.activeSelf)
            {
                Destroy(gameObject);
                // Trigger the event if it has subscribers
                if (onHealthDepletedEvent != null)
                {
                    onHealthDepletedEvent();
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - lastAttackTime < minimumAttackInterval)
        {
            return;
        }
        lastAttackTime = Time.time;
        currentHitPoints -= damage;
        if (currentHitPoints > 0 && useSound)
        {
            if (transform.GetComponent<AudioSource>() != null)
            {
                transform.GetComponent<AudioSource>().Play();
            }
        }
        currentHitPoints = Mathf.Max(currentHitPoints, 0); // Prevent HP from going below 0
        
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHitPoints);
        }
    }

    public void TakeDamage(int damage, Vector3 attackDirection, float knockbackForce)
    {
        if (Time.time - lastAttackTime < minimumAttackInterval)
        {
            return;
        }
        // Call the original TakeDamage method to handle the damage
        TakeDamage(damage);

        Vector3 knockbackDirection = new Vector3(attackDirection.x, 0, attackDirection.z).normalized;
        float upwardForce = knockbackForce * 0.2f;
        knockbackDirection = knockbackDirection + Vector3.up * upwardForce;
        knockbackDirection.Normalize();

        // Apply knockback
        if (rb != null)
        {
            Debug.Log("Knockback!");
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        }
    }
}
