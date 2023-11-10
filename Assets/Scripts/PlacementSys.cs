using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
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

    [SerializeField]
    public string rotationKeyPress;
    [SerializeField]
    public string levelUpKeyPress;
    [SerializeField]
    public string levelDownKeyPress;
    [SerializeField]
    public string destroyKeyPress;

    private GameObject placementHint;

    private Vector3 levelStep;
    private int currentLevel = 0;

    private Renderer[] previewRenderCellIndicator;

    [SerializeField]
    private Material previewRenderMaterial;
    private Material previewMaterialInstance;

    private bool canPlace = true;

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

    private void StopPlacement()
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

        GameObject newGameObj = Instantiate(database.objData[selectObjIndex].Prefab);
        newGameObj.transform.position = placementHint.transform.position;
        newGameObj.transform.rotation = placementHint.transform.rotation;
        newGameObj.layer = LayerMask.NameToLayer("PlacementObject");
        BoxCollider newObjectCollider = newGameObj.GetComponent<BoxCollider>();
        newObjectCollider.center = new Vector3(0, 0, 0);
        newObjectCollider.size = 0.9f * grid.cellSize;
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
        cellIndicator.transform.position = grid.WorldToCell(mousePos);// + currentLevel * levelStep;
        placementHint.transform.position = cellCenterInWorld;

        Vector3 colliderCenter = grid.cellSize * 0.5f;
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
        if (Input.GetKeyDown(destroyKeyPress))
        {
            if (!canPlace)
            {
                Vector3 halfExtents = grid.cellSize * 0.5f;
                Collider[] hitColliders = Physics.OverlapBox(cellCenterInWorld, halfExtents, Quaternion.identity, LayerMask.GetMask("PlacementObject"));
                foreach (Collider hitCollider in hitColliders)
                {
                    Destroy(hitCollider.gameObject); // Destroys the game objects that are in the occupied cell.
                }
            }
        }
    }
}
