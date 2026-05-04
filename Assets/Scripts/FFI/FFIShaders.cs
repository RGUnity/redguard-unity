using UnityEngine;

/// <summary>
/// Shared shader resolution for FFI loaders. All FFI-loaded geometry uses URP Simple Lit.
/// </summary>
public static class FFIShaders
{
    private const string DefaultLitShaderName = "Universal Render Pipeline/Simple Lit";

    private static Shader defaultLitShader;

    public static Shader GetDefaultLit()
    {
        if (defaultLitShader == null)
        {
            defaultLitShader = Shader.Find(DefaultLitShaderName);
            if (defaultLitShader == null)
            {
                Debug.LogError("[FFI] Shader not found: " + DefaultLitShaderName + ". URP must be active.");
            }
        }

        return defaultLitShader;
    }
}
