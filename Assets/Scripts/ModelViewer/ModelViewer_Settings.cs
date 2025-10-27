using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ModelViewer_Settings : MonoBehaviour
{
    [SerializeField] private ModelViewer mv;
    [SerializeField] private ModelViewer_GUI gui;
    [SerializeField] private ModelViewer_Camera mv_camera;
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
        mv_camera.useFlyMode = toggle;
        gui.flyModeToggle.isOn = toggle;
    }

    private void ToggleUI()
    {
        showUI = !showUI;
        gui.gameObject.SetActive(showUI);
    }
    
    public void RequestEnableTextureFiltering(bool enableFiltering)
    {
        mv.SwitchTextureFilterMode(enableFiltering ? FilterMode.Bilinear : FilterMode.Point);
        gui.filterToggle.isOn = enableFiltering;
    }

    public void RequestEnableAnimations(bool enableAnimations)
    {
        mv.EnableAnimations(enableAnimations);
        gui.animationToggle.isOn = enableAnimations;
    }
}
