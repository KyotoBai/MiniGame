using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.Agent.SetDestination(this.target.position);
        if (Agent.path.status == NavMeshPathStatus.PathPartial)
        {
            Debug.Log("Partial path found");
        }
    }
}
