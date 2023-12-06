using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private float pathUpdateInterval = 0.5f; // Time in seconds to update the path
    private NavMeshPath path;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        StartCoroutine(UpdatePathRoutine());
    }

    IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            UpdatePathToTarget();
            //transform.gameObject.GetComponent<EnemyLegMovementController>().AnimateLegs();
            yield return new WaitForSeconds(pathUpdateInterval);
        }
    }

    void UpdatePathToTarget()
    {
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetPath(path);
        }
        else if (path.status == NavMeshPathStatus.PathPartial)
        {
            MoveToLastReachablePoint();
            
        }
    }

    private void MoveToLastReachablePoint()
    {
        if (path.corners.Length > 1)
        {
            Vector3 lastReachablePoint = path.corners[path.corners.Length - 2];
            NavMeshPath partialPath = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, lastReachablePoint, NavMesh.AllAreas, partialPath);
            if (partialPath.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(partialPath);
            }
        }
    }
}
