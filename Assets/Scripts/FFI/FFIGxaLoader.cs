using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FFIGxaLoader
{
    private static readonly Dictionary<string, Material> materialCache = new Dictionary<string, Material>();
    private static Shader gxaShader;

    public static Material GetMaterial_GXA(string gxaName, int frame)
    {
        string upperName = string.IsNullOrEmpty(gxaName) ? string.Empty : gxaName.ToUpperInvariant();
        string key = upperName + "/" + frame.ToString("D3");
        if (materialCache.TryGetValue(key, out Material cachedMaterial))
        {
            return cachedMaterial;
        }

        string path = ResolveGxaPath(upperName);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("[FFI] GXA file not found for " + gxaName);
            return null;
        }

        IntPtr decodedPtr = RgpreBindings.DecodeGxa(path, frame);
        if (decodedPtr == IntPtr.Zero)
        {
            Debug.LogWarning("[FFI] rg_decode_gxa failed for " + gxaName + ": " + RgpreBindings.GetLastErrorMessage());
            return null;
        }

        byte[] decoded = RgpreBindings.ExtractBytesAndFree(decodedPtr);
        if (decoded.Length < 16)
        {
            return null;
        }

        int width = BitConverter.ToInt32(decoded, 0);
        int height = BitConverter.ToInt32(decoded, 4);
        int rgbaSize = BitConverter.ToInt32(decoded, 12);
        if (width <= 0 || height <= 0 || rgbaSize <= 0 || decoded.Length < 16 + rgbaSize)
        {
            return null;
        }

        byte[] rgba = new byte[rgbaSize];
        Buffer.BlockCopy(decoded, 16, rgba, 0, rgbaSize);

        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            name = "GXA_" + upperName + "_" + frame,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };
        texture.LoadRawTextureData(rgba);
        texture.Apply();

        var material = new Material(GetShader());
        material.mainTexture = texture;
        materialCache[key] = material;
        return material;
    }

    public static void ClearCache()
    {
        materialCache.Clear();
    }

    private static string ResolveGxaPath(string gxaName)
    {
        string systemFolder = Game.pathManager.GetSystemFolder();
        string uppercase = Path.Combine(systemFolder, gxaName + ".GXA");
        if (File.Exists(uppercase))
        {
            return uppercase;
        }

        string lowercase = Path.Combine(systemFolder, gxaName.ToLowerInvariant() + ".gxa");
        if (File.Exists(lowercase))
        {
            return lowercase;
        }

        return null;
    }

    private static Shader GetShader()
    {
        if (gxaShader == null)
        {
            gxaShader = Shader.Find("Universal Render Pipeline/Simple Lit");
        }

        return gxaShader;
    }
}
