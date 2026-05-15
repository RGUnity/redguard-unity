using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
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

    // Camera
    public float fieldOfView = 60f;

    // Rendering
    public bool useFog;
    public float fogDensity = 0.5f;
    public Color fogColor = new(0.682353f, 0.7960784f, 1f, 1f);

    // Objects
    public bool showScriptObjects = true;

    public void Initialize()
    {
        Camera.main.fieldOfView = fieldOfView;
        ApplyFog();

        showUI = gui.gameObject.activeSelf;
        ToggleTextureFiltering(useTextureFiltering);
        ToggleAnimations(playAnimations);
        ToggleFlyMode(useFlyMode);
        ToggleScriptObjects(showScriptObjects);
    }

    public void ToggleUI()
    {
        showUI = !showUI;
        gui.gameObject.SetActive(showUI);
    }

    public void ToggleTextureFiltering(bool enableFiltering)
    {
        useTextureFiltering = enableFiltering;
        if (gui.filterToggle != null) gui.filterToggle.SetIsOnWithoutNotify(enableFiltering);
        mv.ApplyTextureFilterSetting();
    }

    public void ToggleAnimations(bool enableAnimations)
    {
        playAnimations = enableAnimations;
        if (gui.animationToggle != null) gui.animationToggle.SetIsOnWithoutNotify(enableAnimations);
        mv.ApplyAnimationSetting();
    }

    public void ToggleFlyMode(bool toggle)
    {
        useFlyMode = toggle;
        if (gui.flyModeToggle != null) gui.flyModeToggle.SetIsOnWithoutNotify(toggle);
        mv_camera.useFlyMode = toggle;
    }

    public void SetFieldOfView(float fov)
    {
        fieldOfView = Mathf.Clamp(fov, 30f, 120f);
        Camera.main.fieldOfView = fieldOfView;
    }

    public void ToggleFog(bool enable)
    {
        useFog = enable;
        ApplyFog();
    }

    public void SetFogDensity(float density)
    {
        fogDensity = density;
        ApplyFog();
    }

    private void ApplyFog()
    {
        RenderSettings.fog = useFog;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = math.square(fogDensity) / 100;
        RenderSettings.fogColor = fogColor;
    }

    public void ToggleScriptObjects(bool show)
    {
        showScriptObjects = show;
        mv.ApplyScriptObjectVisibility();
    }
}