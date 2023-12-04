using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class OpenClose : MonoBehaviour
{
    public GameObject parentPanel;
    public AudioSource audioSource;
    private GameObject[] SubPanels;
    private bool[] panelStatus;
    

    public void Start()
    {
        SubPanels = new GameObject[parentPanel.transform.childCount];
        panelStatus = new bool[parentPanel.transform.childCount];

        for (int i = 0; i < parentPanel.transform.childCount; i++)
        {
            SubPanels[i] = parentPanel.transform.GetChild(i).gameObject;
            panelStatus[i] = false;
            SubPanels[i].SetActive(false);
        } 
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < panelStatus.Length; i++)
            {
                SubPanels[i].SetActive(false);
                panelStatus[i] = false;
            }
        }
    }

    public void OnButtomClick(int i)
    {
        // Debug.Log("Panel " + i + " curr: " + panelStatus[i]);
        // close panel if curr panel status is ture
        if (panelStatus[i])
        {
            SubPanels[i].SetActive(false);
            foreach (GameObject panel in SubPanels)
            {
                panel.SetActive(false);
            }
            for (int j = 0; j < panelStatus.Length; j++)
            {
                panelStatus[j] = false;
            }
            audioSource.Play();
        }
        else
        {
            SubPanels[i].SetActive(true);
            panelStatus[i] = true;

            for (int j = 0; j < panelStatus.Length; j++)
            {
                if (j != i)
                {
                    panelStatus[j] = false;
                    SubPanels[j].SetActive(false);
                }
            }

            audioSource.Play();
        }
    }

    public bool isAllStateOff()
    {
        for (int i = 0; i < panelStatus.Length; i++)
        {
            if (panelStatus[i])
            {
                return false;
            }
        }
        return true;
    }
}
