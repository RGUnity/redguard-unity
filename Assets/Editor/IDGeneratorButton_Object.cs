using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(SavableObject)))]
public class IDGeneratorButton_Object : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SavableObject linkedScript = (SavableObject)target;
        if (GUILayout.Button("Generate ID"))
        {
            linkedScript.GenerateID();
        }
    }
}
