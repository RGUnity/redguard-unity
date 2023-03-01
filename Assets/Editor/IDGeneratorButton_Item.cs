using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor((typeof(Item)))]
public class IDGeneratorButton_Item : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Item linkedScript = (Item)target;
        if (GUILayout.Button("Generate ID"))
        {
            linkedScript.GenerateID();
            Undo.RecordObject(linkedScript, "Generate Identifier");
            PrefabUtility.RecordPrefabInstancePropertyModifications(linkedScript);
        }
    }
}
