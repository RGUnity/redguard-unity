using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class test_settings_manager : MonoBehaviour
{
    //Variables
    public PostProcessLayer localPostProcessLayer;
    public PixelateEffect cameraObject;


    //Function to change pixel size with UI buttons
    public void SetPixelScale(float overridePixelScale)
    {
        cameraObject.pixelScale = overridePixelScale;
        print(overridePixelScale);
    }

    //This Toggles AntiAliasing
    public void ToggleAA(bool antiAliasingEnabled)
    {
        if (antiAliasingEnabled)
        {
            localPostProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        }
        if (!antiAliasingEnabled)
        {
            localPostProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
        }
    }

    //This Toggles Bilinear Filtering for the Rendertexture
    public void ToggleBilinearFiltering(bool overrideBF)
    {
        cameraObject.bilinearFilterEnabled = overrideBF;
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
}