using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathExplosion : MonoBehaviour
{
    [SerializeField]
    public LayerMask layer;
    public float size = 1f;
    public int damage = 10; // The amount of damage to deal in the AoE

    private Health healthComponent; // Reference to the Health component
    private AoEHandler aoeHandler;

    void Awake()
    {
        aoeHandler = new AoEHandler(layer);
        // If Health is not assigned, get the Health component attached to this GameObject
        if (healthComponent == null) healthComponent = GetComponent<Health>();

        // Subscribe to the onHealthDepletedEvent to trigger the AoE damage
        if (healthComponent != null)
        {
            healthComponent.onHealthDepletedEvent += CauseAoEDamage;
        }
        else
        {
            Debug.LogError("No Health component found on this GameObject.");
        }
    }

    void CauseAoEDamage()
    {
        // Get all objects in the specified AoE
        List<GameObject> objectsInAoE = aoeHandler.GetObjectsInSphereAoE(transform.position, size);

        // Loop through each object and apply damage if they have a Health component
        foreach (GameObject obj in objectsInAoE)
        {
            Health targetHealth = obj.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage); // Apply damage
            }
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event when the GameObject is destroyed to prevent memory leaks
        if (healthComponent != null)
        {
            healthComponent.onHealthDepletedEvent -= CauseAoEDamage;
        }
    }
}
