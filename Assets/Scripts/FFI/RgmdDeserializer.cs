using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

public static class RgmdDeserializer
{
    public static List<(string name, Mesh mesh)> DeserializeRob(byte[] robData, out int segmentCount, out int segmentsWithModel)
    {
        var result = new List<(string name, Mesh mesh)>();
        segmentCount = 0;
        segmentsWithModel = 0;

        using (var stream = new MemoryStream(robData))
        using (var reader = new BinaryReader(stream))
        {
            segmentCount = reader.ReadInt32();
            for (int i = 0; i < segmentCount; i++)
            {
                string name = ReadSegmentName(reader);
                byte hasModel = reader.ReadByte();
                if (hasModel == 0)
                {
                    continue;
                }

                segmentsWithModel++;
                int modelSize = reader.ReadInt32();
                if (modelSize <= 0)
                {
                    continue;
                }

                byte[] rgmdData = reader.ReadBytes(modelSize);
                if (rgmdData.Length != modelSize)
                {
                    throw new EndOfStreamException($"ROB segment '{name}' expected {modelSize} bytes but got {rgmdData.Length}.");
                }

                Mesh mesh = ParseRgmdMesh(rgmdData, name);
                if (mesh != null)
                {
                    result.Add((name, mesh));
                }
            }
        }

        return result;
    }

    private static Mesh ParseRgmdMesh(byte[] rgmdData, string segmentName)
    {
        using (var stream = new MemoryStream(rgmdData))
        using (var reader = new BinaryReader(stream))
        {
            string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            if (!string.Equals(magic, "RGMD", StringComparison.Ordinal))
            {
                throw new InvalidDataException($"Segment '{segmentName}' has invalid RGMD magic '{magic}'.");
            }

            reader.ReadBytes(4); // version
            int submeshCount = reader.ReadInt32();
            reader.ReadInt32(); // frame_count
            int totalVertexCount = reader.ReadInt32();
            int totalIndexCount = reader.ReadInt32();
            reader.ReadSingle(); // radius

            if (submeshCount <= 0 || totalVertexCount <= 0 || totalIndexCount <= 0)
            {
                return null;
            }

            var vertices = new List<Vector3>(Math.Max(totalVertexCount, 0));
            var normals = new List<Vector3>(Math.Max(totalVertexCount, 0));
            var uvs = new List<Vector2>(Math.Max(totalVertexCount, 0));
            var allIndices = new List<int>(Math.Max(totalIndexCount, 0));
            var subMeshes = new List<SubMeshDescriptor>(Math.Max(submeshCount, 0));

            for (int submeshIndex = 0; submeshIndex < submeshCount; submeshIndex++)
            {
                byte typeTag = reader.ReadByte();
                if (typeTag == 0)
                {
                    reader.ReadByte();
                    reader.ReadUInt16();
                    reader.ReadByte();
                }
                else if (typeTag == 1)
                {
                    reader.ReadUInt16();
                    reader.ReadByte();
                }
                else
                {
                    throw new InvalidDataException($"Segment '{segmentName}' has unknown submesh type tag {typeTag}.");
                }

                reader.ReadBytes(2); // padding
                reader.ReadByte(); // backface_flag

                int vertexCount = reader.ReadInt32();
                int indexCount = reader.ReadInt32();
                if (vertexCount < 0 || indexCount < 0)
                {
                    throw new InvalidDataException($"Segment '{segmentName}' has negative counts in submesh {submeshIndex}.");
                }

                int submeshVertexStart = vertices.Count;
                for (int v = 0; v < vertexCount; v++)
                {
                    float px = reader.ReadSingle();
                    float py = reader.ReadSingle();
                    float pz = reader.ReadSingle();

                    float nx = reader.ReadSingle();
                    float ny = reader.ReadSingle();
                    float nz = reader.ReadSingle();

                    float u = reader.ReadSingle();
                    float vCoord = reader.ReadSingle();

                    vertices.Add(new Vector3(px, py, pz));
                    normals.Add(new Vector3(nx, ny, nz));
                    uvs.Add(new Vector2(u, vCoord));
                }

                int submeshIndexStart = allIndices.Count;
                for (int idx = 0; idx < indexCount; idx++)
                {
                    uint raw = reader.ReadUInt32();
                    if (raw > int.MaxValue)
                    {
                        throw new InvalidDataException($"Segment '{segmentName}' has index value too large in submesh {submeshIndex}.");
                    }

                    allIndices.Add(submeshVertexStart + (int)raw);
                }

                subMeshes.Add(new SubMeshDescriptor(submeshIndexStart, indexCount, MeshTopology.Triangles));
            }

            var mesh = new Mesh
            {
                name = segmentName
            };

            mesh.indexFormat = vertices.Count > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);

            mesh.SetIndexBufferParams(allIndices.Count, mesh.indexFormat);
            if (mesh.indexFormat == IndexFormat.UInt16)
            {
                var indices16 = new ushort[allIndices.Count];
                for (int i = 0; i < allIndices.Count; i++)
                {
                    indices16[i] = (ushort)allIndices[i];
                }

                mesh.SetIndexBufferData(indices16, 0, 0, indices16.Length);
            }
            else
            {
                mesh.SetIndexBufferData(allIndices, 0, 0, allIndices.Count);
            }

            mesh.subMeshCount = subMeshes.Count;
            for (int i = 0; i < subMeshes.Count; i++)
            {
                mesh.SetSubMesh(i, subMeshes[i], MeshUpdateFlags.DontRecalculateBounds);
            }

            mesh.RecalculateBounds();
            return mesh;
        }
    }

    private static string ReadSegmentName(BinaryReader reader)
    {
        byte[] raw = reader.ReadBytes(8);
        if (raw.Length != 8)
        {
            throw new EndOfStreamException("ROB segment name read failed.");
        }

        int end = Array.IndexOf(raw, (byte)0);
        if (end < 0)
        {
            end = raw.Length;
        }

        return Encoding.ASCII.GetString(raw, 0, end).Trim();
    }
}
