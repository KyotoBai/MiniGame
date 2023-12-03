using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PauseMenu : MonoBehaviour
{
    public GameObject blurPanel; 
    public GameObject textPopup; 
    public GameObject oc;
    public List<GameObject> componentList;

    private bool active = false;

    void Start()
    {
        blurPanel.SetActive(false);
        textPopup.SetActive(false);
        foreach (GameObject obj in componentList)
        {
            obj.SetActive(active);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && oc.GetComponent<OpenClose>().isAllStateOff())
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        blurPanel.SetActive(!blurPanel.activeSelf);
        textPopup.SetActive(!textPopup.activeSelf);
        active = !active;
        foreach (GameObject obj in componentList)
        {
            obj.SetActive(active);
        }
        
        Time.timeScale = blurPanel.activeSelf ? 0 : 1;
    }
}
