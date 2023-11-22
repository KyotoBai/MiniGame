using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// store all info of UI. s
/// </summary>
public class UIManager
{
    private Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        dicUI = new Dictionary<UIType, GameObject> ();
    }

    public GameObject GetSingleUI(UIType type)
    {
        GameObject parent = GameObject.Find("Canvas");
        if (!parent)
        {
            Debug.LogError("Canvas not found");
            return null;
        }
        if (dicUI.ContainsKey(type))
        {
            return dicUI[type];
        }
        GameObject ui = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), parent.transform);
        ui.name = type.Name;
        return ui;
    }

    public void DestoryUI(UIType type)
    {
        if (dicUI.ContainsKey(type))
        {
            DestoryUI(type);
            dicUI.Remove(type);
        }
        
    }
}
