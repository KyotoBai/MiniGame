using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// parent class of all UI panel
/// </summary>
public abstract class BasePanel
{
    // UI info
    public UIType UIType { get; private set; }

    // Only once
    public virtual void OnEnter() { }
    // pause
    public virtual void OnPause() { }
    // resume
    public virtual void OnResume() { }
    // exit
    public virtual void OnExit() { }
}
