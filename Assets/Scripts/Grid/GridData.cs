using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridData
{
    private Cell[,,] gridArray;

    public GridData(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        gridArray = new Cell[bounds.size.x, 5, bounds.size.z];
    }

    public void AddObjAt(Vector3Int gridPosition, Vector3Int objSize, int ID)
    {
        List<Vector3Int> positionToOccupy = CalcPositions(gridPosition, objSize);

        foreach (var position in positionToOccupy)
        {
            gridArray[position.x, position.y, position.z] = new Cell(true, ID);
        }
    }

    public bool CanPlaceObj(Vector3Int gridPosition, Vector3Int objSize)
    {
        bool canPlace = true;
        List<Vector3Int> positionToOccupy = CalcPositions(gridPosition, objSize);

        foreach (var position in positionToOccupy)
        {
            if (gridArray[position.x, position.y, position.z].isOccupy)
            {
                return false;
            }
        }
        return canPlace;
    }

    public void RemoveObj(Vector3Int gridPosition, Vector3Int objSize)
    {
        List<Vector3Int> positionToOccupy = CalcPositions(gridPosition, objSize);
        foreach (var position in positionToOccupy)
        {
            gridArray[position.x, position.y, position.z] = new Cell();
        }
    }

    private List<Vector3Int> CalcPositions(Vector3Int gridPositions, Vector3Int objSize)
    {
        List<Vector3Int> lst = new List<Vector3Int>();
        for (int x = 0;x < objSize.x; x++)
        {
            for (int y = 0; y < objSize.y; y++)
            {
                for (int z = 0; z < objSize.z; z++)
                {
                    lst.Add(new Vector3Int(gridPositions.x + x, gridPositions.y + y, gridPositions.z + z));
                }
            }
        }
        return lst;
    }
}

public class Cell
{
    // true if obj is using this cell
    public bool isOccupy {  get; private set; }
    public int objID { get; private set; }

    public Cell()
    {
        isOccupy = false;
        objID = -1;
    }
    public Cell(bool b, int i)
    {
        isOccupy = b;
        objID = i;
    }
}

