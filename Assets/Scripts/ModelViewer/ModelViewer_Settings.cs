using System;
using TMPro;
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
    public float fogDensity = 0.002f;

    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);

    // Lighting
    public bool useCameraLight;
    public bool useSceneLight;
    public float sceneLightIntensity = 1f;

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
        ToggleCameraLight(useCameraLight);
        ToggleSceneLight(useSceneLight);
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
        fogDensity = Mathf.Clamp(density, 0.0001f, 0.1f);
        ApplyFog();
    }

    private void ApplyFog()
    {
        RenderSettings.fog = useFog;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = backgroundColor;
        Camera.main.backgroundColor = backgroundColor;
    }

    public void ToggleCameraLight(bool enable)
    {
        useCameraLight = enable;
        mv.SetCameraLight(enable);
    }

    public void ToggleSceneLight(bool enable)
    {
        useSceneLight = enable;
        mv.SetSceneLight(enable);
    }

    public void SetSceneLightIntensity(float intensity)
    {
        sceneLightIntensity = intensity;
        mv.SetSceneLightIntensity(intensity);
    }

    public void ToggleScriptObjects(bool show)
    {
        showScriptObjects = show;
        mv.ApplyScriptObjectVisibility();
    }
}