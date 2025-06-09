using System;
using TMPro;
using UnityEngine;

public class ModelViewer2_Settings : MonoBehaviour
{
    [SerializeField] private ModelViewer2 mv2;
    [SerializeField] private ModelViewer2_GUI gui;
    [SerializeField] private ModelViewer2_Camera mv2_camera;

    public void ToggleFlyMode(bool toggle)
    {
        mv2_camera.useFlyMode = toggle;
    }

    public void ReloadFiles()
    {
        ModelLoader.RedguardPath = gui.pathInput.text;
        mv2.ViewerMode_Levels();
    }
}
