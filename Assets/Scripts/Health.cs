using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // This can be set in the inspector and will be visible there.
    [SerializeField] private int maxHitPoints = 10;

    // This is private and won't be shown in the inspector.
    private int currentHitPoints;

    // Declare a delegate type for the destruction event
    public delegate void OnDestroyed();

    // Declare an event of the delegate type
    public event OnDestroyed onDestroyedEvent;


    // Read-only property to access current hit points from other scripts
    public int CurrentHitPoints
    {
        get { return currentHitPoints; }
    }

    void Start()
    {
        // Initialize current HP to max HP when the object is spawned
        currentHitPoints = maxHitPoints;
    }

    void Update()
    {
        if (currentHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        currentHitPoints = Mathf.Max(currentHitPoints, 0); // Ensure HP doesn't go below 0
    }
}
