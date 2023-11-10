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
        Debug.Log("start!");
        Vector3 mousePos = Input.mousePosition;
        Debug.Log("mouse Pos: " + mousePos);

        mousePos.z = sceneCam.nearClipPlane;

        Ray ray = sceneCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPos = hit.point;
        }
        return lastPos;
    }
}