using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float terrainWidth = 100f;
    public float terrainHeight = 100f;

    void Start()
    {
        GenerateTrees();
    }

    void GenerateTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Random position on the terrain
            float xPos = Random.Range(-(terrainWidth/2), terrainWidth/2);
            float zPos = Random.Range(-(terrainHeight / 2), terrainHeight/2);
            Vector3 treePosition = new Vector3(xPos, 0, zPos); // Assuming terrain is at y = 0

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
            newTree.layer = LayerMask.NameToLayer("PlacementObject");
            newTree.AddComponent<CapsuleCollider>();
            newTree.transform.localScale = treeScale;
        }
    }
}

