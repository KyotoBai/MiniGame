using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovementController : MonoBehaviour
{
    public List<GameObject> leftLegs = new List<GameObject>();
    public List<GameObject> rightLegs = new List<GameObject>();

    [SerializeField] private float legSwingAngle = 30.0f;

    private float moveSpeed = 0f;
    private float factor;

    private Quaternion leftLegInitialRotation = Quaternion.identity;
    private Quaternion rightLegInitialRotation = Quaternion.identity;
   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AnimateLegs();
    }

    public void SetSpeed(float speed )
    {
        moveSpeed = speed * 2;
    }

    public void SetAngleFactor(float fact)
    {
        factor = fact;
    }

    private void AnimateLegs()
    {
        // Calculate the angle based on moveSpeed and Time.time for a simple swinging motion
        float angle = Mathf.Sin(Time.time * moveSpeed) * legSwingAngle * factor;

        // Apply rotation to the legs
        foreach ( GameObject leftLeg in leftLegs )
        {
            leftLeg.transform.localRotation = leftLegInitialRotation * Quaternion.Euler(0, 0, angle);
        }
        foreach (GameObject rightLeg in rightLegs )
        {
            rightLeg.transform.localRotation = rightLegInitialRotation * Quaternion.Euler(0, 0, -angle);
        }
        
        
    }

    public void ResetLegs()
    {
        foreach (GameObject leftLeg in leftLegs)
        {
            leftLeg.transform.localRotation = leftLegInitialRotation;
        }
        foreach (GameObject rightLeg in rightLegs)
        {
            rightLeg.transform.localRotation = rightLegInitialRotation;
        }
    }
}
