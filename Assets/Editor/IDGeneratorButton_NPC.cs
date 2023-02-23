using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(NPC)))]
public class IDGeneratorButton_NPC : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        NPC linkedScript = (NPC)target;
        if (GUILayout.Button("Generate GUID"))
        {
            linkedScript.GenerateID();
            Undo.RecordObject(linkedScript, "Generate Identifier");
            PrefabUtility.RecordPrefabInstancePropertyModifications(linkedScript);
        }
    }
}
