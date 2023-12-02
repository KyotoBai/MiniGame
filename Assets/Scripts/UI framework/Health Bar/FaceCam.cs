using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCam : MonoBehaviour
{
    public Transform Cam;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Cam.forward);
    }
}
