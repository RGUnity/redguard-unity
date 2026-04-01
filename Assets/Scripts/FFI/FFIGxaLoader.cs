using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        RgpreBindings.NativeBuffer buffer = RgpreBindings.ReadBuffer(decodedPtr);
        try
        {
            int headerSize = Marshal.SizeOf<RgpreBindings.TextureHeader>();
            if (buffer.data == IntPtr.Zero || buffer.len < headerSize)
            {
                return null;
            }

            var header = Marshal.PtrToStructure<RgpreBindings.TextureHeader>(buffer.data);
            if (header.width <= 0 || header.height <= 0 || header.rgbaSize <= 0 || buffer.len < headerSize + header.rgbaSize)
            {
                return null;
            }

            byte[] rgba = new byte[header.rgbaSize];
            Marshal.Copy(buffer.data + headerSize, rgba, 0, header.rgbaSize);

            var texture = new Texture2D(header.width, header.height, TextureFormat.RGBA32, false)
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
        finally
        {
            RgpreBindings.FreeBuffer(buffer.handle);
        }
    }

    public static void ClearCache()
    {
        foreach (var mat in materialCache.Values)
        {
            if (mat != null)
            {
                if (mat.mainTexture != null)
                    UnityEngine.Object.Destroy(mat.mainTexture);
                UnityEngine.Object.Destroy(mat);
            }
        }
        materialCache.Clear();
    }

    private static string ResolveGxaPath(string gxaName)
        => FFIPathUtils.ResolveFile(Game.pathManager.GetSystemFolder(), gxaName, ".GXA");

    private static Shader GetShader()
    {
        if (gxaShader == null)
        {
            gxaShader = Shader.Find("Universal Render Pipeline/Simple Lit");
        }

        return gxaShader;
    }
}
