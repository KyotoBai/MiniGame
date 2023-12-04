using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManual : MonoBehaviour
{
    public GameObject userManualPanel;
    public GameObject mainButton;
    private bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        userManualPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        userManualPanel.SetActive(isOn);
    }

    public void OnClickUserManual()
    {
        
        mainButton.SetActive(false);
        userManualPanel.SetActive(true);
        isOn = true;
    }

    public void BackClickUserManual()
    {

        mainButton.SetActive(true);
        userManualPanel.SetActive(false);
        isOn = false;
    }

    public bool isUserManualOn()
    {
        return isOn;
    }
}
