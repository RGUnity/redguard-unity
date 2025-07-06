using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using ShadowQuality = UnityEngine.ShadowQuality;


// You can toggle the visibility with F1
public class test_settings_manager : MonoBehaviour
{
    //Variables
    public bool startVisible;
    public Canvas thisCanvas;
    public Dropdown resolutionDropdown;
    public Dropdown fullscreenDropdown;
    public PixelateEffect pixelateEffect;
    public UniversalAdditionalCameraData cameraData;

    Resolution[] resolutions;


    void Start()
    {
        thisCanvas.enabled = startVisible;

        //Add resolutions to dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //print(Screen.fullScreenMode);
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                fullscreenDropdown.SetValueWithoutNotify(0);
                break;
            case FullScreenMode.FullScreenWindow:
                fullscreenDropdown.SetValueWithoutNotify(1);
                break;
            case FullScreenMode.MaximizedWindow:
                fullscreenDropdown.SetValueWithoutNotify(2);
                break;
            case FullScreenMode.Windowed:
                fullscreenDropdown.SetValueWithoutNotify(3);
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            thisCanvas.enabled = !thisCanvas.enabled;
        }
    }

    //set resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //set fullscreen mode
    public void SetFullscreenMode()
    {
        int selection = fullscreenDropdown.value;
        print(selection);
        switch (selection)
        {
            case 0:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen);
                break;
            case 1:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow);
                break;
            case 2:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.MaximizedWindow);
                break;
            case 3:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
                break;
        }
    }

    //Function to change pixel size with UI buttons
    public void SetPixelScale(int overridePixelScale)
    {
        if (pixelateEffect != null)
        {
            pixelateEffect.pixelScale = overridePixelScale;
            print("Set pixelScale to " + overridePixelScale);
        }
        else
        {
            Debug.LogWarning("No Pixelate script linked. Please go to the test_settings_manager and fix that");
        }

    }

    //This Toggles AntiAliasing
    public void ToggleAA(bool antiAliasingEnabled)
    {
        if (antiAliasingEnabled)
        {
            cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else
        {
            cameraData.antialiasing = AntialiasingMode.None;
        }
    }

    //This Toggles Bilinear Filtering for the Rendertexture
    public void ToggleBilinearFiltering(bool overrideBF)
    {
        pixelateEffect.bilinearFilterEnabled = overrideBF;
    }

    //This toggles Shadows
    public void ToggleShadows(bool shadowsEnabled)
    {
        if (shadowsEnabled)
        {
            QualitySettings.shadows = ShadowQuality.All;
        }
        if (!shadowsEnabled)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}