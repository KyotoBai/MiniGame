using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ObjDBScriptableObj : ScriptableObject
{
    public List<ObjectData> objData;
}

/// <summary>
/// Fence obj start with 00 + obj number xx
/// For example 0001
/// 
/// </summary>
[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector3Int Size { get; private set; } = Vector3Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public int Price { get; private set; }
    [field: SerializeField]
    public int InitialInventory { get; set; }
    [field: SerializeField]
    public int Inventory { get; set; }
    [field: SerializeField]
    public int HP { get; private set; }
}
