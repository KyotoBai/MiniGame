using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AoEHandler
{
    [SerializeField] public LayerMask layer;    // The layer to detect objects within the AoE
    public AoEHandler(LayerMask layer)
    {
        this.layer = layer;
    }

    // This function returns a list of game objects affected by a spherical AoE
    public List<GameObject> GetObjectsInSphereAoE(Vector3 center, float size)
    {
        // Calculate the actual size using the grid's cell size

        // Create a list to hold all affected game objects
        List<GameObject> affectedObjects = new List<GameObject>();

        // Find all colliders within the sphere
        Collider[] colliders = Physics.OverlapSphere(center, size, layer);

        // Add their game objects to the list if they're not already in it
        foreach (Collider collider in colliders)
        {
            // Add the game object associated with the collider to the list
            GameObject obj = collider.gameObject;
            while (obj != null)
            {
                Health health = obj.GetComponent<Health>();
                if (health != null)
                {
                    break;
                } else
                {
                    obj = obj.transform.parent.gameObject;
                }
            }
            if (obj != null)
            {
                affectedObjects.Add(obj);
            }
        }

        // Return the list of affected game objects
        return affectedObjects;
    }
}
