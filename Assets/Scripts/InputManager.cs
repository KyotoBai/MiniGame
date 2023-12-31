using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public event Action OnClicked, OnExit;

    [SerializeField]
    private Camera sceneCam;

    [SerializeField]
    private LayerMask placementLayermask;

    private Vector3 lastPos;

    private float placeCooldown = 0.03f;
    public float lastPlaceTime = 0.0f;

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

    public bool IsPointerOverUI()
       => EventSystem.current.IsPointerOverGameObject();

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time - lastPlaceTime >= placeCooldown)
            {
                lastPlaceTime = Time.time;
                OnClicked?.Invoke();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            OnExit?.Invoke();
        }
    }
}