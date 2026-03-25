using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

public static class RgmdDeserializer
{
    private const int RgmdMagic = 0x444D4752; // "RGMD"
    private static readonly int RobHeaderSize = Marshal.SizeOf<RgpreBindings.RobHeader>();
    private static readonly int RobSegmentHeaderSize = Marshal.SizeOf<RgpreBindings.RobSegmentHeader>();
    private static readonly int RgmdHeaderSize = Marshal.SizeOf<RgpreBindings.RgmdHeader>();
    private static readonly int RgmdSubmeshHeaderSize = Marshal.SizeOf<RgpreBindings.RgmdSubmeshHeader>();
    private static readonly int RgmdVertexSize = Marshal.SizeOf<RgpreBindings.RgmdVertex>();

    public struct SubmeshMaterialInfo
    {
        public bool isSolidColor;
        public Color32 solidColor;  // resolved RGB from native (solid colors only)
        public ushort textureId;
        public byte imageId;
    }

    public static List<(string name, Mesh mesh)> DeserializeRob(IntPtr bufferHandle, out int segmentCount, out int segmentsWithModel)
    {
        var data = DeserializeRobWithMaterials(bufferHandle, out segmentCount, out segmentsWithModel);
        var result = new List<(string name, Mesh mesh)>(data.Count);
        for (int i = 0; i < data.Count; i++)
        {
            result.Add((data[i].name, data[i].mesh));
        }

        return result;
    }

    public static List<(string name, Mesh mesh, SubmeshMaterialInfo[] materials)> DeserializeRobWithMaterials(IntPtr bufferHandle, out int segmentCount, out int segmentsWithModel)
    {
        var result = new List<(string name, Mesh mesh, SubmeshMaterialInfo[] materials)>();
        segmentCount = 0;
        segmentsWithModel = 0;

        RgpreBindings.NativeBuffer buffer = RgpreBindings.ReadBuffer(bufferHandle);
        try
        {
            if (buffer.data == IntPtr.Zero || buffer.len < RobHeaderSize)
            {
                return result;
            }

            IntPtr ptr = buffer.data;
            IntPtr end = buffer.data + buffer.len;
            var robHeader = Marshal.PtrToStructure<RgpreBindings.RobHeader>(ptr);
            segmentCount = robHeader.segmentCount;
            ptr += RobHeaderSize;

            for (int i = 0; i < segmentCount; i++)
            {
                EnsureReadable(ptr, RobSegmentHeaderSize, end, "ROB segment header");
                var segment = Marshal.PtrToStructure<RgpreBindings.RobSegmentHeader>(ptr);
                ptr += RobSegmentHeaderSize;

                string name = DecodeRobSegmentName(segment.segmentName);
                int modelSize = segment.modelDataSize;

                if (segment.hasModel == 0 || modelSize <= 0)
                {
                    continue;
                }

                segmentsWithModel++;
                EnsureReadable(ptr, modelSize, end, $"ROB segment '{name}' RGMD payload");

                var (mesh, materials, _) = ParseRgmdMesh(ptr, ptr + modelSize, name);
                ptr += modelSize;
                if (mesh != null)
                {
                    result.Add((name, mesh, materials));
                }
            }
        }
        finally
        {
            RgpreBindings.FreeBuffer(buffer.handle);
        }

        return result;
    }

    public static (Mesh mesh, SubmeshMaterialInfo[] materials, int frameCount) DeserializeModel(IntPtr bufferHandle, string name)
    {
        RgpreBindings.NativeBuffer buffer = RgpreBindings.ReadBuffer(bufferHandle);
        try
        {
            if (buffer.data == IntPtr.Zero || buffer.len < RgmdHeaderSize)
            {
                return (null, Array.Empty<SubmeshMaterialInfo>(), 0);
            }

            return ParseRgmdMesh(buffer.data, buffer.data + buffer.len, name);
        }
        finally
        {
            RgpreBindings.FreeBuffer(buffer.handle);
        }
    }

    private static (Mesh mesh, SubmeshMaterialInfo[] materials, int frameCount) ParseRgmdMesh(IntPtr ptr, IntPtr end, string segmentName)
    {
        EnsureReadable(ptr, RgmdHeaderSize, end, $"RGMD header for '{segmentName}'");
        var rgmdHeader = Marshal.PtrToStructure<RgpreBindings.RgmdHeader>(ptr);
        if (rgmdHeader.magic != RgmdMagic)
        {
            string magicText = Encoding.ASCII.GetString(BitConverter.GetBytes(rgmdHeader.magic));
            throw new InvalidOperationException($"Segment '{segmentName}' has invalid RGMD magic '{magicText}'.");
        }

        int submeshCount = rgmdHeader.submeshCount;
        int frameCount = rgmdHeader.frameCount;
        int totalVertexCount = rgmdHeader.totalVertexCount;
        int totalIndexCount = rgmdHeader.totalIndexCount;

        if (submeshCount <= 0 || totalVertexCount <= 0 || totalIndexCount <= 0)
        {
            return (null, Array.Empty<SubmeshMaterialInfo>(), frameCount);
        }

        var vertices = new List<Vector3>(Math.Max(totalVertexCount, 0));
        var normals = new List<Vector3>(Math.Max(totalVertexCount, 0));
        var uvs = new List<Vector2>(Math.Max(totalVertexCount, 0));
        var allIndices = new List<int>(Math.Max(totalIndexCount, 0));
        var subMeshes = new List<SubMeshDescriptor>(Math.Max(submeshCount, 0));
        var submeshMaterials = new SubmeshMaterialInfo[submeshCount];

        ptr += RgmdHeaderSize;
        for (int submeshIndex = 0; submeshIndex < submeshCount; submeshIndex++)
        {
            EnsureReadable(ptr, RgmdSubmeshHeaderSize, end, $"submesh header {submeshIndex} in '{segmentName}'");
            var submeshHeader = Marshal.PtrToStructure<RgpreBindings.RgmdSubmeshHeader>(ptr);
            ptr += RgmdSubmeshHeaderSize;

            submeshMaterials[submeshIndex] = new SubmeshMaterialInfo
            {
                isSolidColor = submeshHeader.textured == 0,
                solidColor = new Color32(submeshHeader.colorR, submeshHeader.colorG, submeshHeader.colorB, 255),
                textureId = submeshHeader.textureId,
                imageId = submeshHeader.imageId
            };

            int vertexCount = submeshHeader.vertexCount;
            int indexCount = submeshHeader.indexCount;
            if (vertexCount < 0 || indexCount < 0)
            {
                throw new InvalidOperationException($"Segment '{segmentName}' has negative counts in submesh {submeshIndex}.");
            }

            int submeshVertexStart = vertices.Count;
            for (int v = 0; v < vertexCount; v++)
            {
                EnsureReadable(ptr, RgmdVertexSize, end, $"vertex {v} in submesh {submeshIndex} of '{segmentName}'");
                var vertex = Marshal.PtrToStructure<RgpreBindings.RgmdVertex>(ptr);
                ptr += RgmdVertexSize;

                vertices.Add(new Vector3(vertex.px, vertex.py, vertex.pz));
                normals.Add(new Vector3(vertex.nx, vertex.ny, vertex.nz));
                uvs.Add(new Vector2(vertex.u, vertex.v));
            }

            int submeshIndexStart = allIndices.Count;
            if (indexCount > 0)
            {
                EnsureReadable(ptr, indexCount * sizeof(uint), end, $"indices in submesh {submeshIndex} of '{segmentName}'");
                var indexWords = new int[indexCount];
                Marshal.Copy(ptr, indexWords, 0, indexCount);
                ptr += indexCount * sizeof(uint);

                for (int idx = 0; idx < indexCount; idx++)
                {
                    uint raw = unchecked((uint)indexWords[idx]);
                    if (raw > int.MaxValue)
                    {
                        throw new InvalidOperationException($"Segment '{segmentName}' has index value too large in submesh {submeshIndex}.");
                    }

                    allIndices.Add(submeshVertexStart + (int)raw);
                }
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
        return (mesh, submeshMaterials, frameCount);
    }

    private static string DecodeRobSegmentName(long rawName)
    {
        byte[] nameBytes = BitConverter.GetBytes(rawName);
        return Encoding.ASCII.GetString(nameBytes).TrimEnd('\0').Trim();
    }

    private static void EnsureReadable(IntPtr ptr, int bytesNeeded, IntPtr end, string label)
        => RgpreBindings.EnsureReadable(ptr, bytesNeeded, end, label);
}
