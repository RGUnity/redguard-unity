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

    private void Start()
    {
        // Initialize showUI
        showUI = gui.gameObject.activeSelf;
        ToggleTextureFiltering(false);
        ToggleAnimations(false);
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
}
