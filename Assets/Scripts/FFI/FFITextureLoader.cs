using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class FFITextureLoader
{
    private const int PaletteHeaderSize = 8;
    private const int PaletteColorCount = 256;
    private const int PaletteStride = 3;

    private static readonly Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
    private static readonly Dictionary<string, Color32[]> paletteCache = new Dictionary<string, Color32[]>();

    // Rust-side texture cache handle (created once, reused for all decode calls)
    private static IntPtr nativeTextureCache = IntPtr.Zero;
    private static string cachedPaletteName;

    public static void ClearCache()
    {
        textureCache.Clear();
        paletteCache.Clear();
        FreeNativeCache();
    }

    private static void FreeNativeCache()
    {
        if (nativeTextureCache != IntPtr.Zero)
        {
            RgpreBindings.FreeTextureCache(nativeTextureCache);
            nativeTextureCache = IntPtr.Zero;
            cachedPaletteName = null;
        }
    }

    /// Ensure the native TextureCache is created for the given palette.
    /// Loads all TEXBSI banks from the art folder into one Rust-side cache.
    public static IntPtr EnsureNativeCache(string paletteName)
    {
        if (nativeTextureCache != IntPtr.Zero && cachedPaletteName == paletteName)
            return nativeTextureCache;

        FreeNativeCache();

        string artFolder = Game.pathManager.GetArtFolder();
        string palettePath = ResolvePalettePath(artFolder, paletteName);

        byte[] paletteBytes = null;
        GCHandle palettePin = default;

        if (!string.IsNullOrEmpty(palettePath))
        {
            paletteBytes = File.ReadAllBytes(palettePath);
            palettePin = GCHandle.Alloc(paletteBytes, GCHandleType.Pinned);
        }

        // Scan for all TEXBSI.### files
        var texbsiIds = new List<ushort>();
        var texbsiDataList = new List<byte[]>();
        var texbsiPins = new List<GCHandle>();

        foreach (string path in Directory.GetFiles(artFolder, "TEXBSI.*"))
        {
            string ext = Path.GetExtension(path).TrimStart('.');
            if (int.TryParse(ext, out int id) && id >= 0 && id <= ushort.MaxValue)
            {
                texbsiIds.Add((ushort)id);
                texbsiDataList.Add(File.ReadAllBytes(path));
            }
        }

        IntPtr[] texbsiDatas = new IntPtr[texbsiDataList.Count];
        int[] texbsiLens = new int[texbsiDataList.Count];
        for (int i = 0; i < texbsiDataList.Count; i++)
        {
            var pin = GCHandle.Alloc(texbsiDataList[i], GCHandleType.Pinned);
            texbsiPins.Add(pin);
            texbsiDatas[i] = pin.AddrOfPinnedObject();
            texbsiLens[i] = texbsiDataList[i].Length;
        }

        try
        {
            nativeTextureCache = RgpreBindings.CreateTextureCache(
                paletteBytes != null ? palettePin.AddrOfPinnedObject() : IntPtr.Zero,
                paletteBytes?.Length ?? 0,
                texbsiIds.ToArray(), texbsiDatas, texbsiLens, texbsiDataList.Count);

            cachedPaletteName = paletteName;
            return nativeTextureCache;
        }
        finally
        {
            if (palettePin.IsAllocated) palettePin.Free();
            foreach (var pin in texbsiPins)
                if (pin.IsAllocated) pin.Free();
        }
    }

    public static Texture2D DecodeTexture(ushort texbsiId, byte imageId, string paletteName)
    {
        string cacheKey = texbsiId + "_" + imageId;
        if (textureCache.TryGetValue(cacheKey, out var cachedTexture))
            return cachedTexture;

        IntPtr cache = EnsureNativeCache(paletteName);
        if (cache == IntPtr.Zero) return null;

        IntPtr resultPtr = RgpreBindings.DecodeTexture(cache, texbsiId, imageId);
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
