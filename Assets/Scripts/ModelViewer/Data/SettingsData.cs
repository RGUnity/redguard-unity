using UnityEngine;

public class SettingsData
{
    // System
    public static bool showUI = true;
    
    // Camera
    public static bool useFlyMode = false;
    public static float fieldOfView = 60f;

    // Rendering
    public static bool useTextureFiltering = false;
    public static bool useFog;
    public static float fogDensity = 0.5f;
    public static Color fogColor = new(0.682353f, 0.7960784f, 1f, 1f);
    
    // Objects
    public static bool playAnimations = false;
}