using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovementController : MonoBehaviour
{
    public List<GameObject> leftArms = new List<GameObject>();
    public List<GameObject> rightArms = new List<GameObject>();

    [SerializeField] private float armSwingAngle = 30.0f;

    private float moveSpeed = 0f;
    private float factor;

    private Quaternion leftArmInitialRotation = Quaternion.identity;
    private Quaternion rightArmInitialRotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!transform.gameObject.GetComponent<PlayerShootingController>().shootingOn){
           
            AnimateArms();
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed * 2;
    }

    public void SetAngleFactor(float fact)
    {
        factor = fact;
    }

    private void AnimateArms()
    {
        // Calculate the angle based on moveSpeed and Time.time for a simple swinging motion
        float angle = Mathf.Sin(Time.time * moveSpeed) * armSwingAngle * factor;

        // Apply rotation to the legs
        foreach (GameObject leftArm in leftArms)
        {
            leftArm.transform.localRotation = leftArmInitialRotation * Quaternion.Euler(0, 0, -angle);
        }
        foreach (GameObject rightArm in rightArms)
        {
            rightArm.transform.localRotation = rightArmInitialRotation * Quaternion.Euler(0, 0, angle);
        }
    }

    public void ResetArms()
    {
        if (!transform.gameObject.GetComponent<PlayerShootingController>().shootingOn)
        {
            foreach (GameObject leftArm in leftArms)
            {
                leftArm.transform.localRotation = leftArmInitialRotation;
            }
            foreach (GameObject rightArm in rightArms)
            {
                rightArm.transform.localRotation = rightArmInitialRotation;
            }
        }
    }
}
