using System;
using TMPro;
using UnityEngine;

public class ModelViewer2_Settings : MonoBehaviour
{
    [SerializeField] private ModelViewer2 mv2;
    [SerializeField] private ModelViewer2_GUI gui;
    [SerializeField] private ModelViewer2_Camera mv2_camera;
    public bool showUI;

    private void Start()
    {
        // Initialize showUI
        showUI = gui.gameObject.activeSelf;
    }

    private void Update()
    {
        // ToggleUi Trigger
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ToggleUI();
        }
    }

    public void ToggleFlyMode(bool toggle)
    {
        mv2_camera.useFlyMode = toggle;
    }

    public void ReloadFiles()
    {
        ModelLoader.RedguardPath = gui.pathInput.text;
        PlayerPrefs.SetString("ViewerRedguardPath", ModelLoader.RedguardPath);
        mv2.ViewerMode_Levels();
    }

    private void ToggleUI()
    {
        showUI = !showUI;
        gui.gameObject.SetActive(showUI);
    }
}
