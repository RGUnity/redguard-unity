using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(SpawnPointUpdater)))]
public class SpawnPointUpdaterButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SpawnPointUpdater linkedScript = (SpawnPointUpdater)target;
        if (GUILayout.Button("Update SpawnPoint Asset"))
        {
            linkedScript.SetTransformData();
        }
    }
}
