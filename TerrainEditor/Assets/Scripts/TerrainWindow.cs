using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainWindow : EditorWindow
{
    TerrainEditor te;


    [MenuItem("Window/TerrainWindow")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TerrainWindow));
    }


    void OnGUI()
    {
        if (!te)
        {
            te = GameObject.Find("TerrainEditor").GetComponent<TerrainEditor>();
            
            if (!te)
                return;
        }

        GUILayout.Label("Terrain Editor", EditorStyles.boldLabel);

        if (te.selected >= 0)
        {
            // there is a polygon already selected, show its information and edit tools
            int transformTool = 0;
            string[] content = { "Translate", "Rotate", "Scale" };
            transformTool = GUI.SelectionGrid(new Rect(0, 0, 196, 32), transformTool, content, 3);
        }

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
