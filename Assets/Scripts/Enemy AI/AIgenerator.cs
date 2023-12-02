using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIgenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefab;
    [SerializeField, Range(0, 20)]
    private int numberOfEnemyToAdd = 50;
    [SerializeField]
    private Transform generateCenter;
    [SerializeField, Range(0, 50f)]
    private float generateRadius = 0f;
    [SerializeField]
    private LayerMask AiLayer;
    [SerializeField]
    private LayerMask PlacementLayer;
    [SerializeField]
    private Vector3 groundCenter = new Vector3(0f, 0f, 0f);
    [SerializeField]
    private Transform HealthBarCamera;
    [SerializeField]
    private Transform enemyNullObj;
    [SerializeField]
    private Transform Player;

    public float collisionCheckRadius = 1.0f;
    public bool FixedPointGeneration = true;
    // Start is called before the first frame update
    void Start()
    {
        GenerateEnemy();
    }

    public void GenerateEnemy()
    {
        int numOfEnemyPlaced = 0;
       

        while (numOfEnemyPlaced < numberOfEnemyToAdd)
        {
            Vector3 placePosition = GetPositionInCircle(generateCenter.position, generateRadius);

            Collider[] hitCollidersEnemy = Physics.OverlapSphere(placePosition, collisionCheckRadius, AiLayer.value);
            Collider[] hitCollidersProps = Physics.OverlapSphere(placePosition, collisionCheckRadius, PlacementLayer.value);

            if (hitCollidersEnemy.Length == 0 && hitCollidersProps.Length == 0) // No collision, safe to place tree
            {
                // Random rotation
                Quaternion enemyRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                // Randomly select a enemy prefab
                GameObject selectedEnemyPrefab = enemyPrefab[Random.Range(0, enemyPrefab.Length)];

                // Instantiate the enemy
                GameObject newEnemy = Instantiate(selectedEnemyPrefab, placePosition, enemyRotation);

                newEnemy.transform.SetParent(enemyNullObj, true);

                //Debug.Log(newEnemy.transform.GetChild(6));
                newEnemy.transform.GetChild(6).GetComponent<FaceCam>().Cam = HealthBarCamera;

                newEnemy.GetComponent<EnemyMove>().target = Player;

                numOfEnemyPlaced++;
            }
        }
    }


    private Vector3 GetPositionInCircle(Vector3 center, float r)
    {
        float r_x = Random.Range(-r, r);
        float xPos = center.x + r_x;
        float zPos = center.z + (r - Mathf.Abs(r_x))*(Random.Range(0, 2) - 1);
        return new Vector3(xPos, center.y ,zPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
