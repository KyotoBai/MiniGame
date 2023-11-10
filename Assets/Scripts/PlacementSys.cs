using System.Collections;
using System.Collections.Generic;
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

    private GameObject placementHint;
    private GameObject placementHintParent;

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
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        GameObject newGameObj = Instantiate(database.objData[selectObjIndex].Prefab);
        newGameObj.transform.position = placementHint.transform.position;
        newGameObj.transform.rotation = placementHint.transform.rotation;
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
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
        placementHintParent.transform.position = grid.CellToWorld(gridPos);
        if (Input.GetKeyDown(rotationKeyPress))
        {
            RotateHint(grid.GetCellCenterWorld(gridPos), 90);
        }
    }
}
