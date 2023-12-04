using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool stopMovement = false;
    public bool weaponOn = false;
    [Header("Player Moving Properties")]
    [Tooltip("vetcor.x is moving speed when weapon OFF, vetcor.y is moving speed when weapon ON")]
    public Vector2 speedTwoType = new Vector2(3f, 1.5f);
    public float rotationSpeed = 720.0f; // Degrees per second
    public Vector2 accelerationTwoType = new Vector2(1f, 1.2f);
    public Vector2 decelerationTwoType = new Vector2(0.6f, 0.8f);
    public float stopAngle = 5.0f; // Angle threshold to start moving

    public PlayerPrefabControll playerPrefabControl;
    public LegMovementController LegMovementController;

    private Vector3 currentVelocity;
    private Vector3 targetDirection;
    private float speed;
    private float acceleration;
    private float deceleration;
    private float drag;

    void Update()
    {
        if (stopMovement)
        {
            return;
        }

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
        Quaternion targetRotation = adjustedDirection == Vector3.zero? Quaternion.identity : Quaternion.LookRotation(adjustedDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (!weaponOn && targetDirection.magnitude > 0.01f)
        {
            transform.rotation = rotation;
        }

        if (weaponOn)
        {
            drag = playerPrefabControl.getWeaponMovingDrag();

            rotation = targetRotation;
            speed = speedTwoType.y * drag;
            acceleration = accelerationTwoType.y * drag;
            deceleration = decelerationTwoType.y * drag;
        }
        else
        {
            speed = speedTwoType.x;
            acceleration = accelerationTwoType.x;
            deceleration = decelerationTwoType.x;
        }

        // Determine the target velocity
        Vector3 targetVelocity = isMovingInput && Quaternion.Angle(rotation, Quaternion.LookRotation(new Vector3(-targetDirection.z, 0.0f, targetDirection.x))) < stopAngle ? targetDirection * speed : Vector3.zero;

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

        if (isMovingInput)
        {
            LegMovementController.SetAngleFactor(Mathf.Abs(currentSpeed)/speed);
            LegMovementController.SetSpeed(speed);
        } /*else
        {
            LegMovementController.ResetLegs();
        }*/
    }
}
