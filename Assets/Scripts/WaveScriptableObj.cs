using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WaveScriptableObj : ScriptableObject
{
    public List<WaveData> waveData;
}

/// <summary>
/// Fence obj start with 00 + obj number xx
/// For example 0001
/// 
/// </summary>
[Serializable]
public class WaveData
{
    [field: SerializeField]
    public string name;
    [field: SerializeField]
    public int wave_ID { get; private set; }
    [field: SerializeField]
    public int totalEnemy { get; private set;}
    [field: SerializeField, Range(0f, 1f)]
    public float ProbOfSettingBaseAsTarget { get; private set; }
    [field: SerializeField, Range(1f, 10f)]
    public float Radius { get; private set; }
    
    [field: SerializeField]
    public GameObject ememyPrefabs { get; private set; }
}
