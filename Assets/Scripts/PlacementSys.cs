using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSys : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GameObject mouseIndicator;

    private void Updata()
    {
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Debug.Log("mouse Pos: " + mousePos);
        mouseIndicator.transform.position = mousePos;
    }
}
