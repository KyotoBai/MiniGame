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
    private GameObject mouseIndicator, cellIndicator;
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

    private GameObject placementHint;
    private GameObject placementHintParent;

    private Vector3 levelStep;
    private int currentLevel = 0;

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
        placementHintParent = new GameObject("Placement Hint Parent");
        placementHint.transform.parent = placementHintParent.transform;
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
        if (placementHintParent != null) Destroy(placementHintParent);
        placementHintParent = null;
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
        Vector3Int cellPosition = grid.WorldToCell(mousePos);
        Vector3 cellCenterInWorld = grid.CellToWorld(cellPosition) + (grid.cellSize * 0.5f) + new Vector3(0, currentLevel * grid.cellSize.y, 0);
        Vector3 colliderCenter = grid.cellSize * 0.5f;
        bool isCellOccupied = Physics.CheckBox(cellCenterInWorld, colliderCenter, Quaternion.identity, LayerMask.GetMask("PlacementObject"));
        if (isCellOccupied)
        {
            return;
        }

        GameObject newGameObj = Instantiate(database.objData[selectObjIndex].Prefab);
        newGameObj.transform.position = placementHint.transform.position;
        newGameObj.transform.rotation = placementHint.transform.rotation;
        newGameObj.layer = LayerMask.NameToLayer("PlacementObject");
        BoxCollider newObjectCollider = newGameObj.GetComponent<BoxCollider>();
        newObjectCollider.center = new Vector3(grid.cellSize.x * 0.5f, grid.cellSize.y * 0.5f, grid.cellSize.z * 0.5f);
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
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos) + currentLevel * levelStep;
        placementHintParent.transform.position = grid.CellToWorld(gridPos) + currentLevel * levelStep;
        if (Input.GetKeyDown(rotationKeyPress))
        {
            RotateHint(grid.GetCellCenterWorld(gridPos), 90);
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
    }
}
