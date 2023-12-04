using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameMechanicsController;

public class EnemyGenOnWave : MonoBehaviour
{
    // Start is called before the first frame updates
    [SerializeField]
    private WaveScriptableObj dataBase;
    [SerializeField]
    private GameMechanicsController gameController;
    [SerializeField]
    private Transform PlaceAiHere;
    [SerializeField]
    private Transform MainCamera;
    [SerializeField]
    private Transform[] GenerateLocationParent;
    [SerializeField]
    private Transform[] Target;

    private int currentAiGenerated = 0;

    void Start()
    {
        if (gameController != null)
        {
            gameController.OnWaveBegin += OnWaveBegin;
            gameController.OnWaveEnd += OnWaveEnd;
        }
    }

    private void OnWaveBegin()
    {
        GenertaEnemies(dataBase);
    }

    private void OnWaveEnd()
    {
        PrepareForNext(dataBase);
    }

    private void GenertaEnemies(WaveScriptableObj wd)
    {
        // get the Wave number from Scriptable Obj
        int currentWaveIndex = wd.waveData.FindIndex(data => data.wave_ID == gameController.wave);

        // get the total enemy that need to be generated
        int totalAiNeeded = wd.waveData[currentWaveIndex].totalEnemy;

        // get the prefab
        GameObject prefab = wd.waveData[currentWaveIndex].ememyPrefabs;

        float r = wd.waveData[currentWaveIndex].Radius;

        int remainder;
        int numForEachParent;
        int subRemainder;
        int numForEachSubLoaction;

        if (GenerateLocationParent.Length == 1)
        {
            remainder = 0;
            subRemainder = totalAiNeeded % 4;
            numForEachSubLoaction = totalAiNeeded / 4;
            remainder += subRemainder;
        }
        else
        {
            // save the remainder
            remainder = totalAiNeeded % GenerateLocationParent.Length;
            // this give us the total number that each parent need generates
            numForEachParent = (totalAiNeeded - remainder) / 2;
            // each parent have 4 sub location
            subRemainder = numForEachParent % 4;
            // get each number of AI subloaction need generate
            numForEachSubLoaction = (numForEachParent - subRemainder) / 4;
            // get all remainder together

            remainder += (subRemainder*2);
        }


        for (int i = 0; i < 4; i++)
        {
            int num1 = numForEachSubLoaction;
            int num2 = numForEachSubLoaction;
            if (i == 3)
            {
                if (remainder % 2 == 0)
                {
                    num1 += (remainder / 2);
                    num2 += (remainder / 2);
                }
                else
                {
                    num1 += (remainder - 1) / 2;
                    num2 += (remainder + 1) / 2;
                }
            }

            if (GenerateLocationParent[0].GetChild(i) != null)
            {
                Transform subLocation1 = GenerateLocationParent[0].GetChild(i);
                subLocation1.AddComponent<AIGenerator>();

                // add prefab
                subLocation1.GetComponent<AIGenerator>().enemyPrefab.Add(prefab);
                subLocation1.GetComponent<AIGenerator>().numberOfEnemyToAdd = num1;
                subLocation1.GetComponent<AIGenerator>().generateCenter = subLocation1;
                subLocation1.GetComponent<AIGenerator>().generateRadius = r;
                subLocation1.GetComponent<AIGenerator>().AiLayer = LayerMask.NameToLayer("Enemy");
                subLocation1.GetComponent<AIGenerator>().PlacementLayer = LayerMask.NameToLayer("PlacementObject");
                subLocation1.GetComponent<AIGenerator>().HealthBarCamera = MainCamera;
                subLocation1.GetComponent<AIGenerator>().enemyNullObj = PlaceAiHere;
                subLocation1.GetComponent<AIGenerator>().target = Target[0]; // need change

            }
            if (GenerateLocationParent.Length > 1)
            {
                if (GenerateLocationParent[1].GetChild(i) != null)
                {

                    Transform subLocation2 = GenerateLocationParent[1].GetChild(i);
                    subLocation2.AddComponent<AIGenerator>();

                    // add prefab
                    subLocation2.GetComponent<AIGenerator>().enemyPrefab.Add(prefab);
                    subLocation2.GetComponent<AIGenerator>().numberOfEnemyToAdd = num2;
                    subLocation2.GetComponent<AIGenerator>().generateCenter = subLocation2;
                    subLocation2.GetComponent<AIGenerator>().generateRadius = r;
                    subLocation2.GetComponent<AIGenerator>().AiLayer = LayerMask.NameToLayer("Enemy");
                    subLocation2.GetComponent<AIGenerator>().PlacementLayer = LayerMask.NameToLayer("PlacementObject");
                    subLocation2.GetComponent<AIGenerator>().HealthBarCamera = MainCamera;
                    subLocation2.GetComponent<AIGenerator>().enemyNullObj = PlaceAiHere;
                    subLocation2.GetComponent<AIGenerator>().target = Target[0]; // need change
                }
            }
        }
    }

    private void PrepareForNext(WaveScriptableObj wd)
    {
        int currentWaveIndex = wd.waveData.FindIndex(data => data.wave_ID == gameController.wave);

        currentAiGenerated = PlaceAiHere.childCount;

        for (int i = 0; i < 4; i++)
        {
            if (GenerateLocationParent[0].GetChild(i) != null)
            {
                Transform subLocation1 = GenerateLocationParent[0].GetChild(i);
                if (subLocation1.GetComponent<AIGenerator>() != null)
                {
                    Destroy(subLocation1.GetComponent<AIGenerator>());
                }
            }
            if (GenerateLocationParent.Length > 1)
            {
                if (GenerateLocationParent[1].GetChild(i) != null)
                {

                    Transform subLocation2 = GenerateLocationParent[1].GetChild(i);
                    if (subLocation2.GetComponent<AIGenerator>() != null)
                    {
                        Destroy(subLocation2.GetComponent<AIGenerator>());
                    }
                }
            }
        }
    }
}
