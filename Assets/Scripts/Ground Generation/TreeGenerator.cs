using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] treePrefabs; // Array to hold tree prefabs
    [SerializeField]
    private int numberOfTrees = 50; // The number of trees you want to generate
    [SerializeField]
    private Vector2 treeScaleRange = new Vector2(0.8f, 1.2f); // Min and Max scale
    [SerializeField]
    private Vector2 treeHeightRange = new Vector2(0.5f, 1.5f); // Min and Max height

    public Transform groundTransform;
    public float terrainWidth = 100f;
    public float terrainHeight = 100f;

    public float collisionCheckRadius = 1.0f;

    void Start()
    {
        GenerateTrees();
    }

    void GenerateTrees()
    {
        int treesPlaced = 0;
        while(treesPlaced < numberOfTrees)
        {
            // Random position on the terrain
            float xPos = Random.Range(-(terrainWidth/2), terrainWidth/2);
            float zPos = Random.Range(-(terrainHeight / 2), terrainHeight/2);
            Vector3 treePosition = new Vector3(xPos, 0, zPos); // Assuming terrain is at y = 0

            Collider[] hitColliders = Physics.OverlapSphere(treePosition, collisionCheckRadius, LayerMask.GetMask("PlacementObject"));

            if (hitColliders.Length == 0) // No collision, safe to place tree
            {
                // Random rotation
                Quaternion treeRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                // Random scale
                float scale = Random.Range(treeScaleRange.x, treeScaleRange.y);
                float height = Random.Range(treeHeightRange.x, treeHeightRange.y);
                Vector3 treeScale = new Vector3(scale, height, scale);

                // Randomly select a tree prefab
                GameObject selectedTreePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                // Instantiate the tree
                GameObject newTree = Instantiate(selectedTreePrefab, treePosition, treeRotation);

                newTree.transform.SetParent(groundTransform, true);
                newTree.transform.localScale = treeScale;

                // add tree into navmesh
                NavMeshObstacle obstacle = newTree.AddComponent<NavMeshObstacle>();
                obstacle.shape = NavMeshObstacleShape.Capsule; 
                obstacle.center = new Vector3(0, height / 2, 0); 
                obstacle.size = new Vector3(0.1f, height, 0.1f);
                obstacle.carving = true;

                newTree.layer = LayerMask.NameToLayer("PlacementObject");
                newTree.AddComponent<CapsuleCollider>();
                

                treesPlaced++;
            }
        }
    }
}

