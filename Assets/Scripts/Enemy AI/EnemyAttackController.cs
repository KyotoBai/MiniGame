using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayerMask;     // LayerMask for detecting objects
    [SerializeField] private float attackResponseTime = 2f; // Time before start of attack
    [SerializeField] private float attackInterval = 1f;     // Time between each attack
    [SerializeField] private int damageValue = 10;          // Damage value to apply
    [SerializeField] private float detectionDistance = 1f;  // Distance in front of the enemy for AoE

    private AoEHandler aoeHandler; // Reference to AoEHandler
    private float attackResponseTimer;
    private float attackTimer;     // Timer to track attack intervals
    private float detectionRadius;    // Radius for AoE detection
    private bool foundTarget;
    private GameObject targetObject;

    private void Start()
    {
        aoeHandler = new AoEHandler(attackLayerMask);
        attackTimer = attackInterval;
        attackResponseTimer = attackResponseTime;
        detectionRadius = detectionDistance / 2f;
        foundTarget = false;
    }

    private void Update()
    {
        Vector3 aoeCenter = transform.position + transform.forward * detectionDistance;
        List<GameObject> objectsInAoE = aoeHandler.GetObjectsInSphereAoE(aoeCenter, detectionRadius);

        if (foundTarget)
        {
            if (objectsInAoE.Contains(targetObject))
            {
                attackResponseTimer -= Time.deltaTime;
            } else
            {
                foundTarget = false;
                targetObject = null;
                attackResponseTimer = attackResponseTime;
            }

        } else
        {
            if (objectsInAoE.Count > 0)
            {
                foundTarget = true;
                targetObject = objectsInAoE[0];
                attackResponseTimer = attackResponseTime;
            }
        }

        attackTimer -= Time.deltaTime;

        if (attackResponseTimer <= 0f && attackTimer <= 0f)
        {
            PerformAttack();
            attackTimer = attackInterval;
        }
    }

    private void PerformAttack()
    {
        if (foundTarget)
        {
            Health healthComponent = targetObject.GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageValue);
            }
        }
        
        /*
        // Calculate the center point for the AoE in front of the enemy
        Vector3 aoeCenter = transform.position + transform.forward * detectionDistance;

        // Detect objects in AoE
        List<GameObject> objectsInAoE = aoeHandler.GetObjectsInSphereAoE(aoeCenter, detectionRadius);

        if (objectsInAoE.Count > 0)
        {
            // Attack the first object in the list
            Health healthComponent = objectsInAoE[0].GetComponent<Health>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageValue);
            }
        }
        */
    }
}
