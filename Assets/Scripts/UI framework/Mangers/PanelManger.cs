using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// panel manager, use stack to store UI
/// </summary>
public class PanelManger 
{
    // store UI in stack
    private Stack<BasePanel> stackPanel;
    // UI manager
    private UIManager uiManager;
    private BasePanel panel;
    
    public PanelManger()
    {
        stackPanel = new Stack<BasePanel>();
        uiManager = new UIManager();
    }

    /// <summary>
    /// Push a ui panel into stack
    /// </summary>
    /// <param name="nextPanel"></param>
    public void Push(BasePanel nextPanel)
    {
        if (stackPanel.Count > 0)
        {
            panel = stackPanel.Peek();
            panel.OnPause();
        }
        stackPanel.Push(nextPanel);
        GameObject panelGameObj = uiManager.GetSingleUI(nextPanel.UIType);
    }

    public void Pop()
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnExit();
            stackPanel.Pop();
        }
            
        if (stackPanel.Count > 0)
            stackPanel.Peek().OnResume();
    }
}
