using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FFITextureLoader
{
    private const int PaletteHeaderSize = 8;
    private const int PaletteColorCount = 256;
    private const int PaletteStride = 3;

    private static readonly Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
    private static readonly Dictionary<string, Color32[]> paletteCache = new Dictionary<string, Color32[]>();

    public static void ClearCache()
    {
        textureCache.Clear();
        paletteCache.Clear();
    }

    public static Texture2D DecodeTexture(ushort texbsiId, byte imageId, string paletteName)
    {
        string cacheKey = paletteName + "_" + texbsiId + "_" + imageId;
        if (textureCache.TryGetValue(cacheKey, out var cachedTexture))
            return cachedTexture;

        string assetsDir = Game.pathManager.GetRootFolder();

        IntPtr resultPtr = RgpreBindings.DecodeTexture(assetsDir, texbsiId, imageId);
        if (resultPtr == IntPtr.Zero)
        {
            Debug.LogWarning("[FFI] Failed to decode texture TEXBSI." + texbsiId.ToString("D3") +
                " image " + imageId + ": " + RgpreBindings.GetLastErrorMessage());
            return null;
        }

        byte[] decoded = RgpreBindings.ExtractBytesAndFree(resultPtr);
        if (decoded.Length < 16) return null;

        int width = BitConverter.ToInt32(decoded, 0);
        int height = BitConverter.ToInt32(decoded, 4);
        int rgbaSize = BitConverter.ToInt32(decoded, 12);

        if (width <= 0 || height <= 0 || rgbaSize <= 0 || decoded.Length < 16 + rgbaSize)
            return null;

        byte[] rgbaData = new byte[rgbaSize];
        Buffer.BlockCopy(decoded, 16, rgbaData, 0, rgbaSize);

        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            name = "TEXBSI_" + texbsiId + "_" + imageId,
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Repeat
        };

        texture.LoadRawTextureData(rgbaData);
        texture.Apply();

        textureCache[cacheKey] = texture;
        return texture;
    }

    public static Color GetPaletteColor(byte colorIndex, string paletteName)
    {
        if (TryGetPalette(paletteName, out Color32[] colors) && colorIndex < colors.Length)
            return colors[colorIndex];

        return Color.magenta;
    }

    private static bool TryGetPalette(string paletteName, out Color32[] colors)
    {
        if (paletteCache.TryGetValue(paletteName, out colors))
            return true;

        string artFolder = Game.pathManager.GetArtFolder();
        string palettePath = ResolvePalettePath(artFolder, paletteName);
        if (string.IsNullOrEmpty(palettePath))
        {
            colors = null;
            return false;
        }

        byte[] bytes = File.ReadAllBytes(palettePath);
        int dataOffset = bytes.Length >= PaletteHeaderSize + PaletteColorCount * PaletteStride ? PaletteHeaderSize : 0;
        if (bytes.Length < dataOffset + PaletteColorCount * PaletteStride)
        {
            colors = null;
            return false;
        }

        colors = new Color32[PaletteColorCount];
        for (int i = 0; i < PaletteColorCount; i++)
        {
            int start = dataOffset + i * PaletteStride;
            colors[i] = new Color32(bytes[start], bytes[start + 1], bytes[start + 2], 255);
        }

        paletteCache[paletteName] = colors;
        return true;
    }

    private static string ResolvePalettePath(string artFolder, string paletteName)
    {
        string uppercasePath = Path.Combine(artFolder, paletteName + ".COL");
        if (File.Exists(uppercasePath)) return uppercasePath;

        string lowercasePath = Path.Combine(artFolder, paletteName.ToLowerInvariant() + ".col");
        if (File.Exists(lowercasePath)) return lowercasePath;

        return null;
    }
}
