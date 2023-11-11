using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject mainCamera;

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Move the player
        transform.Translate(movement * speed * Time.deltaTime);

        // Move the camera to follow the player
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z - 10);
        }
    }
}
