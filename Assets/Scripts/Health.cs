using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 10;
    private int currentHitPoints;

    // Updated event name for clarity
    public delegate void OnHealthDepleted();

    // Event declaration using the new name
    public event OnHealthDepleted onHealthDepletedEvent;

    public int CurrentHitPoints
    {
        get { return currentHitPoints; }
    }

    void Start()
    {
        currentHitPoints = maxHitPoints;
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
        currentHitPoints -= damage;
        currentHitPoints = Mathf.Max(currentHitPoints, 0); // Prevent HP from going below 0
    }
}
