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
    public bool useTextureFiltering;
    public bool playAnimations;
    public bool useFlyMode;

    private void Start()
    {
        // Initialize showUI
        showUI = gui.gameObject.activeSelf;
        ToggleTextureFiltering(false);
        ToggleAnimations(false);
    }

    public void ToggleUI()
    {
        showUI = !showUI;
        gui.gameObject.SetActive(showUI);
    }
    
    public void ToggleTextureFiltering(bool enableFiltering)
    {
        useTextureFiltering = enableFiltering;
        gui.filterToggle.isOn = enableFiltering;
        mv.ApplyTextureFilterSetting();
    }

    public void ToggleAnimations(bool enableAnimations)
    {
        playAnimations = enableAnimations;
        gui.animationToggle.isOn = enableAnimations;
        mv.ApplyAnimationSetting();
    }
    
    public void ToggleFlyMode(bool toggle)
    {
        useFlyMode = toggle;
        gui.flyModeToggle.isOn = toggle;
        mv_camera.useFlyMode = toggle;
    }
    
}
