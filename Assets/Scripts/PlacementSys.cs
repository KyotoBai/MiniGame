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
        newGameObj.transform.position = grid.CellToWorld(gridPos);
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
    }
}
