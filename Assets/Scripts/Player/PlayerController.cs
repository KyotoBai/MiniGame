using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 720.0f; // Degrees per second
    public float acceleration = 1.0f;
    public float deceleration = 1.0f;
    public float stopAngle = 5.0f; // Angle threshold to start moving

    private Vector3 currentVelocity;
    private Vector3 targetDirection;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool isMovingInput = moveHorizontal != 0 || moveVertical != 0;

        // Determine the target direction based on input
        if (isMovingInput)
        {
            targetDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        }
        else
        {
            targetDirection = Vector3.zero;
        }

        // Rotate towards the target direction
        Vector3 adjustedDirection = new Vector3(-targetDirection.z, 0.0f, targetDirection.x);
        if (targetDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Determine the target velocity
        Vector3 targetVelocity = isMovingInput && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(new Vector3(-targetDirection.z, 0.0f, targetDirection.x))) < stopAngle ? targetDirection * speed : Vector3.zero;

        // Smoothly interpolate the current velocity towards the target velocity
        //currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity,
        //    (targetVelocity.magnitude > 0.01f ? acceleration : deceleration) * Time.deltaTime);


        float currentSpeed = currentVelocity.magnitude;
        float targetSpeed = targetVelocity.magnitude;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed,
            (targetSpeed > 0.01f ? acceleration : deceleration) * Time.deltaTime);

        // Set the direction of current velocity to match the character's forward direction
        currentVelocity = targetDirection * currentSpeed;


        // Move the character
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);
    }
}