using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(DeletableObject)))]
public class GUIDGeneratorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        DeletableObject linkedScript = (DeletableObject)target;
        if (GUILayout.Button("Generate GUID"))
        {
            linkedScript.GenerateGUID();
        }
    }
}
