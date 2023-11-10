using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSys : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject mouseIndicator;

    private void Update()
    {
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePos;
    }
}
