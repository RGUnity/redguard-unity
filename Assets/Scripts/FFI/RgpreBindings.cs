using System;
using System.Runtime.InteropServices;
using System.Text;

public static class RgpreBindings
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ByteBuffer
    {
        public IntPtr ptr;
        public int len;
    }

    // ========== Native imports ==========

    [DllImport("rgpre", EntryPoint = "rg_free_buffer", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeBufferNative(IntPtr buffer);

    [DllImport("rgpre", EntryPoint = "rg_last_error", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr LastErrorNative();

    // --- Scene data (RGMD binary) ---

    [DllImport("rgpre", EntryPoint = "rg_parse_model_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseModelDataNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport("rgpre", EntryPoint = "rg_parse_rob_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseRobDataNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport("rgpre", EntryPoint = "rg_parse_wld_terrain_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseWldTerrainDataNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport("rgpre", EntryPoint = "rg_parse_rgm_placements", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseRgmPlacementsNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    // --- RGM section access ---

    [DllImport("rgpre", EntryPoint = "rg_rgm_section_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int RgmSectionCountNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string sectionTag);

    [DllImport("rgpre", EntryPoint = "rg_get_rgm_section", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetRgmSectionNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string sectionTag,
        int sectionIndex);

    // --- GLB export ---

    [DllImport("rgpre", EntryPoint = "rg_convert_model_from_path", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertModelFromPathNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir);

    [DllImport("rgpre", EntryPoint = "rg_convert_rgm_from_path", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertRgmFromPathNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir);

    [DllImport("rgpre", EntryPoint = "rg_convert_wld_from_path", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertWldFromPathNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir);

    // --- Texture ---

    [DllImport("rgpre", EntryPoint = "rg_decode_texture", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr DecodeTextureNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir,
        ushort textureId,
        byte imageId);

    [DllImport("rgpre", EntryPoint = "rg_decode_texture_all_frames", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr DecodeTextureAllFramesNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir,
        ushort textureId,
        byte imageId);

    [DllImport("rgpre", EntryPoint = "rg_texbsi_image_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int TexbsiImageCountNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir,
        ushort textureId);

    // --- Audio ---

    [DllImport("rgpre", EntryPoint = "rg_sfx_effect_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int SfxEffectCountNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport("rgpre", EntryPoint = "rg_convert_sfx_to_wav", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertSfxToWavNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        int effectIndex);

    [DllImport("rgpre", EntryPoint = "rg_rtx_entry_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int RtxEntryCountNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport("rgpre", EntryPoint = "rg_convert_rtx_entry_to_wav", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertRtxEntryToWavNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        int entryIndex);

    // --- GXA ---

    [DllImport("rgpre", EntryPoint = "rg_gxa_frame_count", CallingConvention = CallingConvention.Cdecl)]
    private static extern int GxaFrameCountNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    [DllImport("rgpre", EntryPoint = "rg_decode_gxa", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr DecodeGxaNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        int frame);

    // --- Font ---

    [DllImport("rgpre", EntryPoint = "rg_convert_fnt_to_ttf", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ConvertFntToTtfNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath);

    // ========== Public API ==========

    // --- Scene data ---

    public static IntPtr ParseModelData(string filePath) => ParseModelDataNative(filePath);
    public static IntPtr ParseRobData(string filePath) => ParseRobDataNative(filePath);
    public static IntPtr ParseWldTerrainData(string filePath) => ParseWldTerrainDataNative(filePath);
    public static IntPtr ParseRgmPlacements(string filePath) => ParseRgmPlacementsNative(filePath);

    // --- RGM sections ---

    public static int RgmSectionCount(string filePath, string sectionTag) => RgmSectionCountNative(filePath, sectionTag);
    public static IntPtr GetRgmSection(string filePath, string sectionTag, int sectionIndex = 0) => GetRgmSectionNative(filePath, sectionTag, sectionIndex);

    // --- GLB export ---

    public static IntPtr ConvertModelFromPath(string filePath, string assetsDir) => ConvertModelFromPathNative(filePath, assetsDir);
    public static IntPtr ConvertRgmFromPath(string filePath, string assetsDir) => ConvertRgmFromPathNative(filePath, assetsDir);
    public static IntPtr ConvertWldFromPath(string filePath, string assetsDir) => ConvertWldFromPathNative(filePath, assetsDir);

    // --- Texture ---

    public static IntPtr DecodeTexture(string assetsDir, ushort textureId, byte imageId) => DecodeTextureNative(assetsDir, textureId, imageId);
    public static IntPtr DecodeTextureAllFrames(string assetsDir, ushort textureId, byte imageId) => DecodeTextureAllFramesNative(assetsDir, textureId, imageId);
    public static int TexbsiImageCount(string assetsDir, ushort textureId) => TexbsiImageCountNative(assetsDir, textureId);

    // --- Audio ---

    public static int SfxEffectCount(string filePath) => SfxEffectCountNative(filePath);
    public static IntPtr ConvertSfxToWav(string filePath, int effectIndex) => ConvertSfxToWavNative(filePath, effectIndex);
    public static int RtxEntryCount(string filePath) => RtxEntryCountNative(filePath);
    public static IntPtr ConvertRtxEntryToWav(string filePath, int entryIndex) => ConvertRtxEntryToWavNative(filePath, entryIndex);

    // --- GXA ---

    public static int GxaFrameCount(string filePath) => GxaFrameCountNative(filePath);
    public static IntPtr DecodeGxa(string filePath, int frame) => DecodeGxaNative(filePath, frame);

    // --- Font ---

    public static IntPtr ConvertFntToTtf(string filePath) => ConvertFntToTtfNative(filePath);

    // ========== Utilities ==========

    public static byte[] ExtractBytesAndFree(IntPtr bufferPtr)
    {
        if (bufferPtr == IntPtr.Zero)
            return Array.Empty<byte>();

        ByteBuffer buffer = Marshal.PtrToStructure<ByteBuffer>(bufferPtr);
        byte[] managed = buffer.len > 0 ? new byte[buffer.len] : Array.Empty<byte>();
        if (buffer.len > 0 && buffer.ptr != IntPtr.Zero)
            Marshal.Copy(buffer.ptr, managed, 0, buffer.len);

        FreeBufferNative(bufferPtr);
        return managed;
    }

    public static string GetLastErrorMessage()
    {
        IntPtr errPtr = LastErrorNative();
        if (errPtr == IntPtr.Zero)
            return string.Empty;

        byte[] bytes = ExtractBytesAndFree(errPtr);
        return bytes.Length == 0 ? string.Empty : Encoding.UTF8.GetString(bytes);
    }
}
