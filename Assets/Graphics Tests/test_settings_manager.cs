using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class test_settings_manager : MonoBehaviour
{
    //Variables
    public PostProcessLayer localPostProcessLayer;
    public PixelateEffect cameraObject;
    //public bool shadowsEnabled

    // void Start()
    // {
    //     ScalableBufferManager.ResizeBuffers(1f, 1f);
    // }


    //Function to change pixel size with UI buttons
    public void SetPixelScale(float overridePixelScale)
    {
        ScalableBufferManager.ResizeBuffers(1 / overridePixelScale, 1 / overridePixelScale);
        cameraObject.pixelScale = overridePixelScale;
        print(overridePixelScale);
    }

    public void ToggleBilinearFiltering(bool overrideBF)
    {
        cameraObject.bilinearFilterEnabled = overrideBF;
    }

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
        //localPostProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
        //print("AA = " + antiAliasingEnabled);
    }

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