using UnityEngine;

public class FloatingCannonController : MonoBehaviour
{
    [SerializeField] private Transform target; // Reference to the player character
    [SerializeField] private float smoothTime = 0.3f; // Time for the movement to smooth out
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0); // Offset to keep the cannon above the player

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        // Check if the target exists
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            // Smoothly move the cannon towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
