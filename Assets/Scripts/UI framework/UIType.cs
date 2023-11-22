using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// store all UI information, including name and path
/// </summary>
public class UIType
{
    // UI name
    public string Name { get; private set; }
    // UI path
    public string Path { get; private set; }

    public UIType(string path)
    {
        Path = path;
        Name = path.Substring(path.LastIndexOf('/') + 1);
    }
}
