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

    public struct NativeBuffer
    {
        public IntPtr data;
        public int len;
        public IntPtr handle;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TextureHeader
    {
        public int width;
        public int height;
        public int frameCount;
        public int rgbaSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AllFramesHeader
    {
        public int width;
        public int height;
        public int frameCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RgmdHeader
    {
        public int magic;
        public int version;
        public int submeshCount;
        public int frameCount;
        public int totalVertexCount;
        public int totalIndexCount;
        public uint radius;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RgmdSubmeshHeader
    {
        public byte textured;       // 0 = solid color (color holds resolved RGB), 1 = textured
        public byte colorR;         // resolved RGB red (solid) or 0 (textured)
        public byte colorG;         // resolved RGB green (solid) or 0 (textured)
        public byte colorB;         // resolved RGB blue (solid) or 0 (textured)
        public ushort textureId;    // TEXBSI id (textured) or 0 (solid)
        public byte imageId;        // TEXBSI image (textured) or 0 (solid)
        private byte _pad;
        public int vertexCount;
        public int indexCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RgmdVertex
    {
        public float px;
        public float py;
        public float pz;
        public float nx;
        public float ny;
        public float nz;
        public float u;
        public float v;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RobHeader
    {
        public int segmentCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RobSegmentHeader
    {
        public long segmentName;
        public byte hasModel;
        private byte _pad0;
        private byte _pad1;
        private byte _pad2;
        public int modelDataSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RgplHeader
    {
        public int magic;
        public int placementCount;
        public int lightCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RgplPlacement
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] modelName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] sourceId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] transform;
        public ushort textureId;
        public byte imageId;
        public byte objectType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RgplLight
    {
        public long name0;
        public long name1;
        public long name2;
        public long name3;
        public float r;
        public float g;
        public float b;
        public float posX;
        public float posY;
        public float posZ;
        public float range;
    }

    // ========== Native imports ==========

    [DllImport("rgpre", EntryPoint = "rg_free_buffer", CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeBufferNative(IntPtr buffer);

    [DllImport("rgpre", EntryPoint = "rg_last_error", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr LastErrorNative();

    // --- Scene data (RGMD binary) ---

    [DllImport("rgpre", EntryPoint = "rg_parse_model_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseModelDataNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir);

    [DllImport("rgpre", EntryPoint = "rg_parse_rob_data", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ParseRobDataNative(
        [MarshalAs(UnmanagedType.LPUTF8Str)] string filePath,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string assetsDir);

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

    [DllImport("rgpre", EntryPoint = "rg_get_rtx_subtitle", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr GetRtxSubtitleNative(
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

    public static IntPtr ParseModelData(string filePath, string assetsDir) => ParseModelDataNative(filePath, assetsDir);
    public static IntPtr ParseRobData(string filePath, string assetsDir) => ParseRobDataNative(filePath, assetsDir);
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
    public static IntPtr GetRtxSubtitle(string filePath, int entryIndex) => GetRtxSubtitleNative(filePath, entryIndex);

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

    public static NativeBuffer ReadBuffer(IntPtr bufferPtr)
    {
        if (bufferPtr == IntPtr.Zero)
        {
            return new NativeBuffer { data = IntPtr.Zero, len = 0, handle = IntPtr.Zero };
        }

        ByteBuffer buffer = Marshal.PtrToStructure<ByteBuffer>(bufferPtr);
        return new NativeBuffer { data = buffer.ptr, len = buffer.len, handle = bufferPtr };
    }

    public static void FreeBuffer(IntPtr handle)
    {
        if (handle != IntPtr.Zero)
        {
            FreeBufferNative(handle);
        }
    }

    public static string GetLastErrorMessage()
    {
        IntPtr errPtr = LastErrorNative();
        if (errPtr == IntPtr.Zero)
            return string.Empty;

        byte[] bytes = ExtractBytesAndFree(errPtr);
        return bytes.Length == 0 ? string.Empty : Encoding.UTF8.GetString(bytes);
    }

    // ========== Shared deserialization helpers ==========

    /// <summary>
    /// Validates that <paramref name="bytesNeeded"/> bytes can be read from <paramref name="ptr"/>
    /// without exceeding <paramref name="end"/>. Throws on failure.
    /// </summary>
    public static void EnsureReadable(IntPtr ptr, int bytesNeeded, IntPtr end, string label)
    {
        if (bytesNeeded < 0)
        {
            throw new System.InvalidOperationException($"Negative byte count for {label}: {bytesNeeded}.");
        }

        long current = ptr.ToInt64();
        long final_ = end.ToInt64();
        if (current < 0 || final_ < current || current + bytesNeeded > final_)
        {
            throw new System.InvalidOperationException($"Unexpected end of data while reading {label}.");
        }
    }
}
