using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(SaveableObject)))]
public class IDGeneratorButton_Object : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SaveableObject linkedScript = (SaveableObject)target;
        if (GUILayout.Button("Generate ID"))
        {
            linkedScript.GenerateID();
        }
    }
}
