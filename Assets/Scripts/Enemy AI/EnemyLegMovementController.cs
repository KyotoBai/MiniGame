using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLegMovementController : MonoBehaviour
{
    [SerializeField] private string leftLegObjectName;
    [SerializeField] private string rightLegObjectName;

    [SerializeField] private float legSwingAngle = 30.0f;
    [SerializeField] private float swingAngleFactor = 1f;

    private float moveSpeed = 0f;
    [SerializeField] private float speedFactor = 2f;

    private GameObject leftLeg = null;
    private GameObject rightLeg = null;
    private NavMeshAgent navMeshAgent = null;
    private Quaternion leftLegInitialRotation = Quaternion.identity;
    private Quaternion rightLegInitialRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        leftLeg = FindDeepChild(transform, leftLegObjectName)?.gameObject;
        rightLeg = FindDeepChild(transform, rightLegObjectName)?.gameObject;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (leftLeg == null)
        {
            Debug.Log("Right Leg Not Found!!!");
        }
        if (rightLeg == null)
        {
            Debug.Log("Left Leg Not Found!!!");
        }
        if (navMeshAgent == null)
        {
            Debug.Log("NavMeshAgent Not Found!!!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent != null)
        {
            SetSpeed(navMeshAgent.velocity.magnitude);
        }
        AnimateLegs();
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed * speedFactor;
        Debug.Log("Move Speed set to " + moveSpeed);
    }

    public void SetSpeedFactor(float fact)
    {
        speedFactor = fact;
    }

    public void SetSwingAngleFactor(float fact)
    {
        swingAngleFactor = fact;
    }

    private void AnimateLegs()
    {
        // Calculate the angle based on moveSpeed and Time.time for a simple swinging motion
        float angle = Mathf.Sin(Time.time * moveSpeed) * legSwingAngle * swingAngleFactor;

        // Apply rotation to the legs
        if (leftLeg != null)
        {
            leftLeg.transform.localRotation = leftLegInitialRotation * Quaternion.Euler(0, 0, angle);
        }
        if (rightLeg != null)
        {
            rightLeg.transform.localRotation = rightLegInitialRotation * Quaternion.Euler(0, 0, -angle);
        }


    }

    public void ResetLegs()
    {
        if (leftLeg != null)
        {
            leftLeg.transform.localRotation = leftLegInitialRotation;
        }
        if (rightLeg != null)
        {
            rightLeg.transform.localRotation = rightLegInitialRotation;
        }
    }

    Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }
            Transform found = FindDeepChild(child, name);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
}
