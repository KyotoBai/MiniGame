using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{
    public GameObject subPanel;
    private uint i = 0;

    public void Start()
    {
        subPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            subPanel.SetActive(false);
            i = 0;
        }
    }

    public void OnButtomClick()
    {
        i++;
        if (i < 2)
        {
        }
        else
        {
            i = 0;
        }

        if (i == 1)
        {
            subPanel.SetActive(true);

        }
        else
        {
            subPanel.SetActive(false);
        }

    }

}
