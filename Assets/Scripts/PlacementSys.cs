using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlacementSys : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject cellIndicator;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjDBScriptableObj database;
    private int selectObjIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    // set parent
    [SerializeField]
    private Transform groundTransform;

    [SerializeField]
    public string rotationKeyPress;
    [SerializeField]
    public string levelUpKeyPress;
    [SerializeField]
    public string levelDownKeyPress;
    [SerializeField]
    public string destroyKeyPress;
    [SerializeField]
    public string damageKeyPress;

    private GameObject placementHint;

    private Vector3 levelStep;
    private int currentLevel = 0;

    private Renderer[] previewRenderCellIndicator;

    [SerializeField]
    private Material previewRenderMaterial;
    private Material previewMaterialInstance;

    private bool canPlace = true;

    [SerializeField]
    private GameEconomyManager economyManager;

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectObjIndex = database.objData.FindIndex(data => data.ID == ID);
        if (selectObjIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        placementHint = Instantiate(database.objData[selectObjIndex].Prefab);
        Collider collider = placementHint.GetComponentInChildren<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
            Debug.Log("Collider found!");
        }
        previewMaterialInstance = new Material(previewRenderMaterial);

        Renderer[] renderers = placementHint.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    private void RotateHint(Vector3 center, int degree)
    {
        placementHint.transform.RotateAround(center, Vector3.up, degree);
    }

    private void Start()
    {
        StopPlacement();
        Vector3 cellSize = grid.cellSize;
        levelStep = new Vector3(0, cellSize.x, 0);
        previewRenderCellIndicator = cellIndicator.GetComponentsInChildren<Renderer>();
    }

    public void StopPlacement()
    {
        selectObjIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        if (placementHint != null) Destroy(placementHint);
        placementHint = null;
        currentLevel = 0;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        if (!canPlace)
        {
            return;
        }
        if (database.objData[selectObjIndex].Inventory > 0)
        {
            database.objData[selectObjIndex].Inventory -= 1;
        } else if (!economyManager.SpendCoins(database.objData[selectObjIndex].Price))
        {
            return;
        }

        GameObject newGameObj = Instantiate(database.objData[selectObjIndex].Prefab);
        newGameObj.transform.position = placementHint.transform.position;
        newGameObj.transform.rotation = placementHint.transform.rotation;
        newGameObj.layer = LayerMask.NameToLayer("PlacementObject");
        BoxCollider newObjectCollider = newGameObj.GetComponentInChildren<BoxCollider>();
        if ( newObjectCollider != null )
        {
            // newObjectCollider.center = new Vector3(0, 0, 0);
            // newObjectCollider.size = 0.9f * grid.cellSize;
        }
        Health health = newGameObj.GetComponent<Health>();
        if (health == null)
        {
            health = newGameObj.AddComponent<Health>();
        }
        health.maxHitPoints = database.objData[selectObjIndex].HP;
        health.currentHitPoints = database.objData[selectObjIndex].HP;

        // add obj into navmesh
        /***NavMeshObstacle obstacle = newGameObj.AddComponent<NavMeshObstacle>();
        obstacle.shape = NavMeshObstacleShape.Box;
        obstacle.center = new Vector3(0, 0, 0);
        obstacle.size = new Vector3(0.1f, 1, 0.1f); // NEED change
        obstacle.carving = true;***/

        newGameObj.transform.SetParent(groundTransform, true);

    }

    private void Update()
    {
        if (selectObjIndex < 0)
        {
            return;
        }

        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        Vector3 cellCenterInWorld = grid.GetCellCenterWorld(gridPos) + currentLevel * levelStep - new Vector3(0, grid.cellSize.y / 2, 0);
        cellIndicator.transform.position = cellCenterInWorld;//+ currentLevel * levelStep;
        placementHint.transform.position = cellCenterInWorld;

        Vector3 colliderCenter = grid.cellSize * 0.4f;
        bool isCellOccupied = Physics.CheckBox(cellCenterInWorld, colliderCenter, Quaternion.identity, LayerMask.GetMask("PlacementObject"));
        canPlace = !isCellOccupied;

        foreach (Renderer renderer in previewRenderCellIndicator)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = canPlace ? Color.white : Color.red;
            }
            renderer.materials = materials;
        }
        
        if (Input.GetKeyDown(rotationKeyPress))
        {
            RotateHint(cellCenterInWorld, 90);
        }
        if (Input.GetKeyDown(levelUpKeyPress))
        {
            currentLevel += 1;
        }
        if (Input.GetKeyDown(levelDownKeyPress))
        {
            if (currentLevel > 0)
            {
                currentLevel -= 1;
            }
        }
        if (Input.GetKey(destroyKeyPress))
        {
            if (!canPlace)
            {
                Vector3 halfExtents = grid.cellSize * 0.4f;
                Collider[] hitColliders = Physics.OverlapBox(cellCenterInWorld, halfExtents, Quaternion.identity, LayerMask.GetMask("PlacementObject"));
                foreach (Collider hitCollider in hitColliders)
                {
                    GameObject parent = hitCollider.gameObject.transform.parent.gameObject;
                    Destroy(hitCollider.gameObject);
                    if (parent != null)
                    {
                        NavMeshObstacle obstacle = parent.GetComponent<NavMeshObstacle>();
                        if (obstacle != null)
                        {
                            Destroy(parent);
                        }
                    }
                    economyManager.AddCoins(1);
                }
            }
        }
        // Test
        if (Input.GetKeyDown(damageKeyPress))
        {
            if (!canPlace)
            {
                Vector3 halfExtents = grid.cellSize * 0.5f;
                Collider[] hitColliders = Physics.OverlapBox(cellCenterInWorld, halfExtents, Quaternion.identity, LayerMask.GetMask("PlacementObject"));
                foreach (Collider hitCollider in hitColliders)
                {
                    GameObject target = hitCollider.gameObject;
                    Health healthComponent = target.GetComponent<Health>();
                    if (healthComponent != null) // Check if the Health component is found
                    {
                        healthComponent.TakeDamage(1);
                    }
                    else
                    {
                        Debug.LogError("The target does not have a Health component.");
                    }

                }
            }
        }
    }
}
