using System;
using System.Runtime.InteropServices;
using System.Text;

public static class RgpreBindings
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ByteBuffer
    {
        public IntPtr ptr;
        public int length;
        public int capacity;
    }

    [DllImport("rgpre", EntryPoint = "rg_parse_model_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseModelDataNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_parse_rob_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseRobDataNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_parse_wld_terrain_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseWldTerrainDataNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_parse_rgm_placements", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseRgmPlacementsNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_free_buffer", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeBufferNative(IntPtr buffer);

    [DllImport("rgpre", EntryPoint = "rg_last_error", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr LastErrorNative();

    [DllImport("rgpre", EntryPoint = "rg_texture_cache_create", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr CreateTextureCacheNative(
        IntPtr palette_data, int palette_len,
        [In] ushort[] texbsi_ids,
        [In] IntPtr[] texbsi_datas,
        [In] int[] texbsi_lens,
        int texbsi_count
    );

    [DllImport("rgpre", EntryPoint = "rg_texture_cache_free", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeTextureCacheNative(IntPtr cache);

    [DllImport("rgpre", EntryPoint = "rg_convert_model_to_glb", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertModelToGlbNative(IntPtr data, int len, IntPtr texture_cache);

    [DllImport("rgpre", EntryPoint = "rg_convert_rob_to_glb", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertRobToGlbNative(IntPtr data, int len, IntPtr texture_cache);

    [DllImport("rgpre", EntryPoint = "rg_convert_rgm_to_glb", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertRgmToGlbNative(
        IntPtr rgm_data, int rgm_len,
        IntPtr texture_cache,
        [In] IntPtr[] model_names,
        [In] IntPtr[] model_datas,
        [In] int[] model_lens,
        [In] byte[] model_types,
        int model_count
    );

    [DllImport("rgpre", EntryPoint = "rg_get_rgm_metadata", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetRgmMetadataNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_convert_wld_to_glb", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertWldToGlbNative(
        IntPtr wld_data, int wld_len,
        IntPtr texture_cache,
        IntPtr rgm_data, int rgm_len,
        [In] IntPtr[] model_names,
        [In] IntPtr[] model_datas,
        [In] int[] model_lens,
        [In] byte[] model_types,
        int model_count
    );

    // --- scene.rs: texture decoding ---

    [DllImport("rgpre", EntryPoint = "rg_decode_texture", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr DecodeTextureNative(IntPtr texture_cache, ushort texture_id, byte image_id);

    [DllImport("rgpre", EntryPoint = "rg_decode_texture_all_frames", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr DecodeTextureAllFramesNative(IntPtr texture_cache, ushort texture_id, byte image_id);

    [DllImport("rgpre", EntryPoint = "rg_texbsi_image_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int TexbsiImageCountNative(IntPtr texture_cache, ushort texture_id);

    // --- scene.rs: audio ---

    [DllImport("rgpre", EntryPoint = "rg_sfx_effect_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int SfxEffectCountNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_convert_sfx_to_wav", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertSfxToWavNative(IntPtr data, int len, int effect_index);

    [DllImport("rgpre", EntryPoint = "rg_rtx_entry_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int RtxEntryCountNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_convert_rtx_entry_to_wav", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertRtxEntryToWavNative(IntPtr data, int len, int entry_index);

    [DllImport("rgpre", EntryPoint = "rg_rtx_metadata", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr RtxMetadataNative(IntPtr data, int len);

    // --- scene.rs: misc formats ---

    [DllImport("rgpre", EntryPoint = "rg_parse_palette", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParsePaletteNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_convert_pvo_to_json", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertPvoToJsonNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_convert_cht_to_json", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertChtToJsonNative(IntPtr data, int len);

    [DllImport("rgpre", EntryPoint = "rg_convert_fnt_to_ttf", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertFntToTtfNative(IntPtr data, int len);

    // ========== Public API ==========

    // --- Raw mesh parsing ---

    public static IntPtr ParseModelData(IntPtr data, int len)
    {
        return ParseModelDataNative(data, len);
    }

    public static IntPtr ParseRobData(IntPtr data, int len)
    {
        return ParseRobDataNative(data, len);
    }

    public static IntPtr ParseWldTerrainData(IntPtr data, int len)
    {
        return ParseWldTerrainDataNative(data, len);
    }

    public static IntPtr ParseRgmPlacements(IntPtr data, int len)
    {
        return ParseRgmPlacementsNative(data, len);
    }

    public static IntPtr CreateTextureCache(
        IntPtr paletteData,
        int paletteLen,
        ushort[] texbsiIds,
        IntPtr[] texbsiDatas,
        int[] texbsiLens,
        int texbsiCount)
    {
        return CreateTextureCacheNative(paletteData, paletteLen, texbsiIds, texbsiDatas, texbsiLens, texbsiCount);
    }

    public static void FreeTextureCache(IntPtr cache)
    {
        FreeTextureCacheNative(cache);
    }

    public static IntPtr ConvertWldToGlb(
        IntPtr wldData, int wldLen, IntPtr textureCache,
        IntPtr rgmData, int rgmLen,
        IntPtr[] modelNames, IntPtr[] modelDatas, int[] modelLens, byte[] modelTypes, int modelCount)
    {
        return ConvertWldToGlbNative(wldData, wldLen, textureCache, rgmData, rgmLen,
            modelNames, modelDatas, modelLens, modelTypes, modelCount);
    }

    // --- GLB conversion: single model ---

    public static IntPtr ConvertModelToGlb(IntPtr data, int len, IntPtr textureCache)
    {
        return ConvertModelToGlbNative(data, len, textureCache);
    }

    // --- GLB conversion: ROB archive ---

    public static IntPtr ConvertRobToGlb(IntPtr data, int len, IntPtr textureCache)
    {
        return ConvertRobToGlbNative(data, len, textureCache);
    }

    // --- GLB conversion: RGM scene ---

    public static IntPtr ConvertRgmToGlb(
        IntPtr rgmData, int rgmLen, IntPtr textureCache,
        IntPtr[] modelNames, IntPtr[] modelDatas, int[] modelLens, byte[] modelTypes, int modelCount)
    {
        return ConvertRgmToGlbNative(rgmData, rgmLen, textureCache,
            modelNames, modelDatas, modelLens, modelTypes, modelCount);
    }

    // --- RGM metadata (JSON) ---

    public static IntPtr GetRgmMetadata(IntPtr data, int len)
    {
        return GetRgmMetadataNative(data, len);
    }

    // --- Texture decoding ---

    public static IntPtr DecodeTexture(IntPtr textureCache, ushort textureId, byte imageId)
    {
        return DecodeTextureNative(textureCache, textureId, imageId);
    }

    public static IntPtr DecodeTextureAllFrames(IntPtr textureCache, ushort textureId, byte imageId)
    {
        return DecodeTextureAllFramesNative(textureCache, textureId, imageId);
    }

    public static int TexbsiImageCount(IntPtr textureCache, ushort textureId)
    {
        return TexbsiImageCountNative(textureCache, textureId);
    }

    // --- Audio: SFX ---

    public static int SfxEffectCount(IntPtr data, int len)
    {
        return SfxEffectCountNative(data, len);
    }

    public static IntPtr ConvertSfxToWav(IntPtr data, int len, int effectIndex)
    {
        return ConvertSfxToWavNative(data, len, effectIndex);
    }

    // --- Audio: RTX dialogue ---

    public static int RtxEntryCount(IntPtr data, int len)
    {
        return RtxEntryCountNative(data, len);
    }

    public static IntPtr ConvertRtxEntryToWav(IntPtr data, int len, int entryIndex)
    {
        return ConvertRtxEntryToWavNative(data, len, entryIndex);
    }

    public static IntPtr RtxMetadata(IntPtr data, int len)
    {
        return RtxMetadataNative(data, len);
    }

    // --- Misc formats ---

    public static IntPtr ParsePalette(IntPtr data, int len)
    {
        return ParsePaletteNative(data, len);
    }

    public static IntPtr ConvertPvoToJson(IntPtr data, int len)
    {
        return ConvertPvoToJsonNative(data, len);
    }

    public static IntPtr ConvertChtToJson(IntPtr data, int len)
    {
        return ConvertChtToJsonNative(data, len);
    }

    public static IntPtr ConvertFntToTtf(IntPtr data, int len)
    {
        return ConvertFntToTtfNative(data, len);
    }

    // ========== Utilities ==========

    public static byte[] ExtractBytesAndFree(IntPtr bufferPtr)
    {
        if (bufferPtr == IntPtr.Zero)
        {
            return Array.Empty<byte>();
        }

        ByteBuffer buffer = Marshal.PtrToStructure<ByteBuffer>(bufferPtr);
        byte[] managed = buffer.length > 0 ? new byte[buffer.length] : Array.Empty<byte>();
        if (buffer.length > 0 && buffer.ptr != IntPtr.Zero)
        {
            Marshal.Copy(buffer.ptr, managed, 0, buffer.length);
        }

        FreeBufferNative(bufferPtr);
        return managed;
    }

    public static string GetLastErrorMessage()
    {
        IntPtr errPtr = LastErrorNative();
        if (errPtr == IntPtr.Zero)
        {
            return string.Empty;
        }

        byte[] bytes = ExtractBytesAndFree(errPtr);
        return bytes.Length == 0 ? string.Empty : Encoding.UTF8.GetString(bytes);
    }
}
