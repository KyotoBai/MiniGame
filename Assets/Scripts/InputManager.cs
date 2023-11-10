using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCam;

    [SerializeField]
    private LayerMask placementLayermask;

    private Vector3 lastPos;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos.z = sceneCam.nearClipPlane;

        //check if we pointing on plain
        Ray ray = sceneCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 300, placementLayermask))
        {
            lastPos = hit.point;
        }
        return lastPos;
    }
}