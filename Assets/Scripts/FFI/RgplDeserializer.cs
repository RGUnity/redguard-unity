using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class RgplDeserializer
{
    private const int RgplMagic = 0x4C504752; // "RGPL"
    private static readonly int RgplHeaderSize = Marshal.SizeOf<RgpreBindings.RgplHeader>();
    private static readonly int RgplPlacementSize = Marshal.SizeOf<RgpreBindings.RgplPlacement>();
    private static readonly int RgplLightSize = Marshal.SizeOf<RgpreBindings.RgplLight>();

    public struct Placement
    {
        public string modelName;
        public string sourceId;
        public Matrix4x4 transform;
        public byte type;
        public ushort textureId;
        public byte imageId;
    }

    public struct LightData
    {
        public string name;
        public Color color;
        public Vector3 position;
        public float range;
    }

    public struct RgplData
    {
        public List<Placement> placements;
        public List<LightData> lights;
    }

    public static RgplData Deserialize(IntPtr bufferHandle)
    {
        var result = new RgplData();
        result.placements = new List<Placement>();
        result.lights = new List<LightData>();

        RgpreBindings.NativeBuffer buffer = RgpreBindings.ReadBuffer(bufferHandle);
        try
        {
            if (buffer.data == IntPtr.Zero || buffer.len < RgplHeaderSize)
            {
                return result;
            }

            IntPtr ptr = buffer.data;
            IntPtr end = buffer.data + buffer.len;

            var header = Marshal.PtrToStructure<RgpreBindings.RgplHeader>(ptr);
            if (header.magic != RgplMagic)
            {
                string magicText = Encoding.ASCII.GetString(BitConverter.GetBytes(header.magic));
                throw new InvalidOperationException("Invalid RGPL magic: " + magicText);
            }

            int placementCount = header.placementCount;
            int lightCount = header.lightCount;
            ptr += RgplHeaderSize;

            for (int i = 0; i < placementCount; i++)
            {
                EnsureReadable(ptr, RgplPlacementSize, end, "RGPL placement");
                var src = Marshal.PtrToStructure<RgpreBindings.RgplPlacement>(ptr);
                ptr += RgplPlacementSize;

                result.placements.Add(new Placement
                {
                    modelName = ReadFixedString(src.modelName),
                    sourceId = ReadFixedString(src.sourceId),
                    transform = ReadTransform(src.transform),
                    textureId = src.textureId,
                    imageId = src.imageId,
                    type = src.objectType
                });
            }

            for (int i = 0; i < lightCount; i++)
            {
                EnsureReadable(ptr, RgplLightSize, end, "RGPL light");
                var src = Marshal.PtrToStructure<RgpreBindings.RgplLight>(ptr);
                ptr += RgplLightSize;

                result.lights.Add(new LightData
                {
                    name = ReadFixedString(ReadLightNameBytes(src)),
                    color = new Color(src.r, src.g, src.b),
                    position = new Vector3(src.posX, src.posY, src.posZ),
                    range = src.range
                });
            }
        }
        finally
        {
            RgpreBindings.FreeBuffer(buffer.handle);
        }

        return result;
    }

    private static string ReadFixedString(byte[] raw)
    {
        if (raw == null || raw.Length == 0)
        {
            return string.Empty;
        }

        int end = Array.IndexOf(raw, (byte)0);
        if (end < 0)
        {
            end = raw.Length;
        }

        return Encoding.ASCII.GetString(raw, 0, end).Trim();
    }

    private static Matrix4x4 ReadTransform(float[] values)
    {
        var matrix = new Matrix4x4();
        if (values == null || values.Length < 16)
        {
            return matrix;
        }

        int index = 0;
        for (int c = 0; c < 4; c++)
        {
            for (int r = 0; r < 4; r++)
            {
                matrix[r, c] = values[index++];
            }
        }

        return matrix;
    }

    private static byte[] ReadLightNameBytes(RgpreBindings.RgplLight light)
    {
        byte[] name = new byte[32];
        Buffer.BlockCopy(BitConverter.GetBytes(light.name0), 0, name, 0, 8);
        Buffer.BlockCopy(BitConverter.GetBytes(light.name1), 0, name, 8, 8);
        Buffer.BlockCopy(BitConverter.GetBytes(light.name2), 0, name, 16, 8);
        Buffer.BlockCopy(BitConverter.GetBytes(light.name3), 0, name, 24, 8);
        return name;
    }

    private static void EnsureReadable(IntPtr ptr, int bytesNeeded, IntPtr end, string label)
    {
        if (bytesNeeded < 0)
        {
            throw new InvalidOperationException($"Negative byte count for {label}: {bytesNeeded}.");
        }

        long current = ptr.ToInt64();
        long final = end.ToInt64();
        if (current < 0 || final < current || current + bytesNeeded > final)
        {
            throw new InvalidOperationException($"Unexpected end of data while reading {label}.");
        }
    }
}
