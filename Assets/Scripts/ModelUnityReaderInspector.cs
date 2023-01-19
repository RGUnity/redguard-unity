#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ModelUnityReader))]
public class ModelUnityReaderInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ModelUnityReader ModelViewer = (ModelUnityReader)target;
        if(GUILayout.Button("Update Model Now"))
        if(target != null&&Application.isPlaying) ModelViewer.SetModel((RGFiles)ModelViewer.CurrentFile);
        // if(EditorGUI.Foldout(FilterType))
        // if(target != null&&Application.isPlaying) MusicPlayer.SetMixerSnapshot();
        ;
    }
}
#endif
