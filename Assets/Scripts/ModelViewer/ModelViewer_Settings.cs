using Unity.Mathematics;
using UnityEngine;

public class ModelViewer_Settings : MonoBehaviour
{
    [SerializeField] private ModelViewer mv;
    [SerializeField] private ModelViewer_GUI gui;
    [SerializeField] private ModelViewer_Camera mv_camera;

    public void Initialize()
    {
        Camera.main.fieldOfView = SettingsData.fieldOfView;
        ApplyFog();
        
        ToggleTextureFiltering(SettingsData.useTextureFiltering);
        ToggleAnimations(SettingsData.playAnimations);
        ToggleFlyMode(SettingsData.useFlyMode);
    }

    public void ToggleUI()
    {
        SettingsData.showUI = !SettingsData.showUI;
        gui.gameObject.SetActive(SettingsData.showUI);
    }

    public void ToggleTextureFiltering(bool enableFiltering)
    {
        SettingsData.useTextureFiltering = enableFiltering;
        if (gui.filterToggle != null) gui.filterToggle.SetIsOnWithoutNotify(enableFiltering);
        mv.ApplyTextureFilterSetting();
    }

    public void ToggleAnimations(bool enableAnimations)
    {
        SettingsData.playAnimations = enableAnimations;
        if (gui.animationToggle != null) gui.animationToggle.SetIsOnWithoutNotify(enableAnimations);
        mv.ApplyAnimationSetting();
    }

    public void ToggleFlyMode(bool toggle)
    {
        SettingsData.useFlyMode = toggle;
        if (gui.flyModeToggle != null) gui.flyModeToggle.SetIsOnWithoutNotify(toggle);
        mv_camera.useFlyMode = toggle;
    }

    public void SetFieldOfView(float fov)
    {
        SettingsData.fieldOfView = Mathf.Clamp(fov, 30f, 120f);
        Camera.main.fieldOfView = SettingsData.fieldOfView;
    }

    public void SetFlySpeedMultiplier(float multiplier)
    {
        flySpeedMultiplier = Mathf.Clamp(multiplier, 0.1f, 20f);
        if (gui.flySpeedSlider != null) gui.flySpeedSlider.SetValueWithoutNotify(flySpeedMultiplier);
        mv_camera.flySpeedMultiplier = flySpeedMultiplier;
    }

    public void ToggleFog(bool enable)
    {
        SettingsData.useFog = enable;
        ApplyFog();
    }

    public void SetFogDensity(float density)
    {
        SettingsData.fogDensity = density;
        ApplyFog();
    }

    private void ApplyFog()
    {
        RenderSettings.fog = SettingsData.useFog;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = math.square(SettingsData.fogDensity) / 100;
        RenderSettings.fogColor = SettingsData.fogColor;
    }
}