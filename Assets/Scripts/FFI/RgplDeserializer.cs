using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class RgplDeserializer
{
    public struct Placement
    {
        public string modelName;
        public string sourceId;
        public Matrix4x4 transform;
        public byte type;
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

    public static RgplData Deserialize(byte[] data)
    {
        var result = new RgplData();
        result.placements = new List<Placement>();
        result.lights = new List<LightData>();

        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            if (magic != "RGPL")
            {
                throw new InvalidDataException("Invalid RGPL magic: " + magic);
            }

            int placementCount = reader.ReadInt32();
            int lightCount = reader.ReadInt32();

            for (int i = 0; i < placementCount; i++)
            {
                var p = new Placement();
                p.modelName = ReadFixedString(reader, 32);
                p.sourceId = ReadFixedString(reader, 32);

                var m = new Matrix4x4();
                for (int c = 0; c < 4; c++)
                {
                    for (int r = 0; r < 4; r++)
                    {
                        m[r, c] = reader.ReadSingle();
                    }
                }

                p.transform = m;
                p.type = reader.ReadByte();
                reader.ReadBytes(3);

                result.placements.Add(p);
            }

            for (int i = 0; i < lightCount; i++)
            {
                var l = new LightData();
                l.name = ReadFixedString(reader, 32);
                l.color = new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                l.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                l.range = reader.ReadSingle();
                result.lights.Add(l);
            }
        }

        return result;
    }

    private static string ReadFixedString(BinaryReader reader, int length)
    {
        byte[] raw = reader.ReadBytes(length);
        int end = Array.IndexOf(raw, (byte)0);
        if (end < 0)
        {
            end = raw.Length;
        }

        return Encoding.UTF8.GetString(raw, 0, end).Trim();
    }
}
