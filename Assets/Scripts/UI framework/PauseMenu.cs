using UnityEngine;
using UnityEngine.UI; 

public class PauseMenu : MonoBehaviour
{
    public GameObject blurPanel; 
    public GameObject textPopup; 
    public GameObject oc;

    void Start()
    {
        blurPanel.SetActive(false);
        textPopup.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && oc.GetComponent<OpenClose>().isAllStateOff())
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {      
        blurPanel.SetActive(!blurPanel.activeSelf);
        textPopup.SetActive(!textPopup.activeSelf);
        
        Time.timeScale = blurPanel.activeSelf ? 0 : 1;
    }
}
