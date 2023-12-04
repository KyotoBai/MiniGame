using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayerMask;     // LayerMask for detecting objects
    [SerializeField] private List<string> attackObjectParentNames = new List<string>();
    [SerializeField] private List<string> attackObjectNames = new List<string>();

    private List<GameObject> attackObjectParents = new List<GameObject>();
    private List<GameObject> attackObjects = new List<GameObject>();
    [SerializeField] private float attackResponseTime = 0.7f; // Time before start of attack
    [SerializeField] private float attackInterval = 0.3f;     // Time between each attack
    [SerializeField] private int damageValue = 10;          // Damage value to apply
    [SerializeField] private float detectionDistance = 0.2f;  // Distance in front of the enemy for AoE
    [SerializeField] private float knockbackForce = 0;      // Knockback force

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

        // for each attackObjectParentNames, search for the object
        // and add it to attackObjectParents
        foreach (string name in attackObjectParentNames)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                attackObjectParents.Add(obj);
            }
            else
            {
                Debug.Log("Could not find " + name);
            }
        }
        // also for each attackObjectNames, search for the object
        // and add it to attackObjects
        foreach (string name in attackObjectNames)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                attackObjects.Add(obj);
            }
            else
            {
                Debug.Log("Could not find " + name);
            }
        }
    }

    private void Update()
    {
        Vector3 aoeCenter = transform.position + transform.forward * detectionDistance;
        List<GameObject> objectsInAoE = aoeHandler.GetObjectsInSphereAoE(aoeCenter, detectionRadius);
        if (objectsInAoE.Count > 0)
        {
            if (foundTarget)
            {

                if (objectsInAoE.Contains(targetObject))
                {
                    attackResponseTimer -= Time.deltaTime;
                }
                else
                {
                    foundTarget = false;
                    targetObject = null;
                    attackResponseTimer = attackResponseTime;
                    attackTimer = attackInterval;
                    Debug.Log("Lost Target!");
                }

            }
            else
            {
                // iterate through the list of objects in AoE
                // get parent of each, then we need the first
                // parent that is in attackObjectParents
                foreach (GameObject obj in objectsInAoE)
                {
                    bool addObj = false;
                    if (attackObjects.Contains(obj))
                    {
                        addObj = true;
                    }
                    else
                    {
                        GameObject parent = obj.transform.parent.gameObject;
                        if (attackObjectParents.Contains(parent))
                        {
                            addObj = true;
                        }
                    }
                    if (addObj)
                    {
                        foundTarget = true;
                        targetObject = obj;
                        attackResponseTimer = attackResponseTime;
                        attackTimer = attackInterval;
                        Debug.Log("New Target Found! " + targetObject);
                        break;
                    }
                }
            }
        } else
        {
            if (foundTarget) {
                Debug.Log("Lost Target!");
            }
            foundTarget = false;
            targetObject = null;
            attackResponseTimer = attackResponseTime;
            attackTimer = attackInterval;
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
        if (foundTarget && targetObject != null)
        {
            Debug.Log("Attack " + targetObject.name);
            Health healthComponent = targetObject.GetComponent<Health>(); 
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damageValue, transform.forward, knockbackForce);
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
