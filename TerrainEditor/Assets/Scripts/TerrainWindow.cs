using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainWindow : EditorWindow
{
    TerrainEditor te;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/TerrainWindow")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(TerrainWindow));
    }

    void OnFocus()
    {
        te = GameObject.Find("TerrainEditor").GetComponent<TerrainEditor>();
    }

    void OnGUI()
    {     
        GUILayout.Label("Terrain Editor", EditorStyles.boldLabel);

        if (GUILayout.Button("EXPORT"))
        {
            Export();
        }          
    }


    void Export()
    {
        te.Export();
    }
}
