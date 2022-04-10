using System;
using System.Collections.Generic;
using System.IO;

namespace RGFileImport
{
    public class RG3DFile
    {
        public struct FrameDataHeader
        {
            public uint u1;
            public uint u2;
            public uint u3;
            public uint u4;
        }

        public struct FaceVertexData
        {
            public uint VertexIndex;
            public short U;
            public short V;
        }

        public class FaceData
        {
            public byte VertexCount;
            public ushort U1;
            public ushort U2;
            public byte U3;
            public uint U4;
            public List<FaceVertexData> VertexData;
        }

        public class Coord3DInt
        {
            public int x;
            public int y;
            public int z;
        }

        public class Coord3DFloat
        {
            public float x;
            public float y;
            public float z;
        }

        const uint RedguardHeaderSize = 64;
        const float CoordTranformFactor = 256f;
        const float NormalTransformFactor = 256f;
        const float UVTransformFactor = 4096f;
        /* Some old 3DC files have a different vertex start offset for some unknown reason. Use this to determine whether to try and reload 
         * the vertex data if "invalid" coordinates are found. */
        const int MaxCoordValueForOld3DCReload = 1090519040;

        RG3DHeader header;
        bool useAltVertexOffset; // Set to true in some old v2.6 3DC files in order to correctly load the vertex data
        bool tryReloadOld3dcFileVertex; // This doesn't seem to always work  (only works with CV_ROPE.3DC and CV_SKUL3.3DC
        uint offsetUVZeroes;
        uint offsetUnknown27;
        Coord3DInt minCoord;
        Coord3DInt maxCoord;
        FrameDataHeader frameDataHeader;
        string fullName;
        string name;
        bool is3DCFile;
        long fileSize;
        uint endFaceDataOffset;
        int totalFaceVertexes;
        int version;
        Dictionary<int, int> faceVectorSizes;

        public List<FaceData> FaceDataCollection { get; set; }
        public List<Coord3DInt> VertexCoordinates { get; set; }
        public List<Coord3DInt> FaceNormals { get; set; }
        public List<uint> UvOffsets { get; set; }
        public List<Coord3DFloat> UvCoordinates { get; set; }

        public void LoadFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();
            fullName = path;
            name = Path.GetFileName(path);
            is3DCFile = name.ToLower().EndsWith(".3dc");
            using var binaryReader = new BinaryReader(File.OpenRead(path));
            fileSize = binaryReader.BaseStream.Length;
            header = GetHeader(binaryReader);
            frameDataHeader = GetFrameDataHeader(binaryReader);
            FaceDataCollection = GetFaceData(binaryReader);
            VertexCoordinates = GetVertexCoordinates(binaryReader);
            if (is3DCFile && version <= 27 && !useAltVertexOffset && tryReloadOld3dcFileVertex)
            {
                // Reload if coordinates are outside maximums
                if (minCoord.x < -MaxCoordValueForOld3DCReload ||
                    minCoord.y < -MaxCoordValueForOld3DCReload ||
                    minCoord.z < -MaxCoordValueForOld3DCReload ||
                    minCoord.x > MaxCoordValueForOld3DCReload ||
                    minCoord.y > MaxCoordValueForOld3DCReload ||
                    minCoord.z > MaxCoordValueForOld3DCReload)
                {
                    useAltVertexOffset = true;
                    VertexCoordinates = GetVertexCoordinates(binaryReader);
                }
            }

            FaceNormals = GetFaceNormals(binaryReader);
            UvOffsets = GetUVOffsets(binaryReader);
            UvCoordinates = GetUVCoordinates(binaryReader);
        }

        private RG3DHeader GetHeader(BinaryReader binaryReader)
        {
            var header = new RG3DHeader
            {
                Version = binaryReader.ReadUInt32(),
                NumVertices = binaryReader.ReadUInt32(),
                NumFaces = binaryReader.ReadUInt32(),
                Radius = binaryReader.ReadUInt32(),
                NumFrames = binaryReader.ReadUInt32(),
                OffsetFrameData = binaryReader.ReadUInt32(),
                NumUVOffsets = binaryReader.ReadUInt32(),
                OffsetSection4 = binaryReader.ReadUInt32(),
                Section4Count = binaryReader.ReadUInt32(),
                Unknown4 = binaryReader.ReadUInt32(),
                OffsetUVOffsets = binaryReader.ReadUInt32(),
                OffsetUVData = binaryReader.ReadUInt32(),
                OffsetVertexCoords = binaryReader.ReadUInt32(),
                OffsetFaceNormals = binaryReader.ReadUInt32(),
                NumUVOffsets2 = binaryReader.ReadUInt32(),
                OffsetFaceData = binaryReader.ReadUInt32()
            };

            version = header.Version switch
            {
                0x362E3276 => 26, // v2.6
                0x372E3276 => 27, // v2.7
                0x302E3476 => 40, // v4.0
                0x302E3576 => 50, // v5.0
                _ => 0,
            };

            if (version <= 27)
            {
                offsetUVZeroes = header.NumUVOffsets;
                header.NumUVOffsets = 0;
                offsetUnknown27 = header.OffsetUVData;
                header.OffsetUVData = offsetUVZeroes;
            }

            return header;
        }

        private FrameDataHeader GetFrameDataHeader(BinaryReader binaryReader)
        {
            if (header.OffsetFrameData == 0 || header.OffsetFrameData >= binaryReader.BaseStream.Length)
                return new FrameDataHeader();
            if (header.OffsetFrameData >= binaryReader.BaseStream.Length)
                throw new EndOfStreamException("OffsetFrameData is beyond stream length.");
            binaryReader.BaseStream.Seek(header.OffsetFrameData, SeekOrigin.Begin);
            return new FrameDataHeader
            {
                u1 = binaryReader.ReadUInt32(),
                u2 = binaryReader.ReadUInt32(),
                u3 = binaryReader.ReadUInt32(),
                u4 = binaryReader.ReadUInt32()
            };
        }

        private List<FaceData> GetFaceData(BinaryReader binaryReader)
        {
            var fileLength = (uint)binaryReader.BaseStream.Length;
            uint dataSize = FindNextOffsetAfter(header.OffsetFaceData, fileLength, header) - header.OffsetFaceData;
            var faceDatas = new List<FaceData>();
            if (header.NumFaces == 0)
                return faceDatas;
            if (header.OffsetFaceData > fileLength)
                throw new EndOfStreamException("OffsetFaceData is beyond stream length.");
            faceVectorSizes = new Dictionary<int, int>();
            binaryReader.BaseStream.Seek(header.OffsetFaceData, SeekOrigin.Begin);
            for (var i = 0; i < header.NumFaces; ++i)
            {
                faceDatas.Add(new FaceData());
                var faceData = faceDatas[i];
                faceData.VertexCount = binaryReader.ReadByte();
                if (version > 27)
                    faceData.U1 = binaryReader.ReadUInt16();
                else
                    faceData.U1 = binaryReader.ReadByte();
                faceData.U2 = binaryReader.ReadUInt16();
                if (version > 27)
                    faceData.U3 = binaryReader.ReadByte();
                else
                    faceData.U3 = 0;
                faceData.U4 = binaryReader.ReadUInt32();
                if (!faceVectorSizes.ContainsKey(faceData.VertexCount))
                    faceVectorSizes[faceData.VertexCount] = 0;
                faceVectorSizes[faceData.VertexCount]++;
                totalFaceVertexes += faceData.VertexCount;
                short u = 0;
                short v = 0;
                faceData.VertexData = new List<FaceVertexData>();
                for (var j = 0; j < faceData.VertexCount; ++j)
                {
                    var vertexIndex = binaryReader.ReadUInt32();
                    faceData.VertexData.Add(new FaceVertexData()
                    {
                        VertexIndex = version <= 27 ? vertexIndex / 12 : vertexIndex,
                        U = (short)(binaryReader.ReadInt16() + u),
                        V = (short)(binaryReader.ReadInt16() + v)
                    });

                    u = faceData.VertexData[j].U;
                    v = faceData.VertexData[j].V;
                }
            }

            var endOffset = (uint)binaryReader.BaseStream.Position;
            var readSize = endOffset - header.OffsetFaceData;
            endFaceDataOffset = endOffset;
            if (readSize != dataSize && (!is3DCFile || version > 27))
                throw new EndOfStreamException("Failed to read full face data section.");
            return faceDatas;
        }

        private List<Coord3DInt> GetVertexCoordinates(BinaryReader binaryReader)
        {
            uint dataSize = FindNextOffsetAfter(header.OffsetVertexCoords, (uint)binaryReader.BaseStream.Length, header) - header.OffsetVertexCoords;
            var coords = new List<Coord3DInt>();
            if (header.NumVertices <= 0)
                return coords;
            if (is3DCFile && version <= 27)
            {
                if (header.OffsetFrameData == 0)
                    throw new Exception("No frame data header found in old 3DC file data.");
                if (endFaceDataOffset == 0)
                    throw new Exception("No vertex data offset found for old 3DC file data.");
                var offset = endFaceDataOffset;
                if (!useAltVertexOffset)
                    offset += frameDataHeader.u3;
                if (offset >= fileSize)
                    throw new EndOfStreamException("Vertex coordinates are beyond end of file.");
                binaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
            }
            else
                binaryReader.BaseStream.Seek(header.OffsetVertexCoords, SeekOrigin.Begin);
            for (var i = 0; i < header.NumVertices; ++i)
            {
                var coord = new Coord3DInt()
                {
                    x = binaryReader.ReadInt32(),
                    y = binaryReader.ReadInt32(),
                    z = binaryReader.ReadInt32()
                };

                coords.Add(coord);
                if (i == 0)
                {
                    minCoord = coord;
                    maxCoord = coord;
                }
                else
                {
                    if (minCoord.x > coord.x) minCoord.x = coord.x;
                    if (minCoord.y > coord.y) minCoord.y = coord.y;
                    if (minCoord.z > coord.z) minCoord.z = coord.z;
                    if (maxCoord.x < coord.x) minCoord.x = coord.x;
                    if (maxCoord.y < coord.y) minCoord.y = coord.y;
                    if (maxCoord.z < coord.z) minCoord.z = coord.z;
                }
            }

            if (binaryReader.BaseStream.Position - header.OffsetVertexCoords != dataSize && (!is3DCFile || version > 27))
                throw new Exception("Did not read all bytes from 3d file vertex coordinates section.");

            return coords;
        }

        private List<Coord3DInt> GetFaceNormals(BinaryReader binaryReader)
        {
            var fileLength = (uint)binaryReader.BaseStream.Length;
            var dataSize = FindNextOffsetAfter(header.OffsetFaceNormals, fileLength, header) - header.OffsetFaceNormals;
            binaryReader.BaseStream.Seek(header.OffsetFaceNormals, SeekOrigin.Begin);
            var faceNormals = new List<Coord3DInt>();
            for (var i = 0; i < header.NumFaces; ++i)
            {
                faceNormals.Add(new Coord3DInt()
                {
                    x = binaryReader.ReadInt32(),
                    y = binaryReader.ReadInt32(),
                    z = binaryReader.ReadInt32()
                });
            }

            var readSize = binaryReader.BaseStream.Position - header.OffsetFaceNormals;
            if (readSize != dataSize && (!is3DCFile || version > 27))
                throw new Exception("Did not read all bytes from 3D file face normal section.");
            return faceNormals;
        }

        private List<uint> GetUVOffsets(BinaryReader binaryReader)
        {
            var uvOffsets = new List<uint>();
            if (header.OffsetUVOffsets == 0)
                return uvOffsets;
            var fileLength = (uint)binaryReader.BaseStream.Length;
            var dataSize = FindNextOffsetAfter(header.OffsetUVOffsets, fileLength, header) - header.OffsetUVOffsets;
            if (header.NumUVOffsets == 0)
                return uvOffsets;
            binaryReader.BaseStream.Seek(header.OffsetUVOffsets, SeekOrigin.Begin);
            for (var i = 0; i < header.NumUVOffsets; ++i)
                uvOffsets.Add(binaryReader.ReadUInt32());
            if (binaryReader.BaseStream.Position - header.OffsetUVOffsets != dataSize)
                throw new Exception("Did not read all bytes from 3d file UV offset data.");

            return uvOffsets;
        }

        private List<Coord3DFloat> GetUVCoordinates(BinaryReader binaryReader)
        {
            var fileLength = (uint)binaryReader.BaseStream.Length;
            var uvCoordinates = new List<Coord3DFloat>();
            if (header.OffsetUVData == 0 || header.OffsetUVData >= fileSize)
                return uvCoordinates;
            var dataSize = FindNextOffsetAfter(header.OffsetUVData, fileLength, header) - header.OffsetUVData;
            var numUVCoordinates = dataSize / 12;
            if (numUVCoordinates == 0)
                return uvCoordinates;
            binaryReader.BaseStream.Seek(header.OffsetUVData, SeekOrigin.Begin);
            for (var i = 0; i < numUVCoordinates; ++i)
            {
                uvCoordinates.Add(new Coord3DFloat()
                {
                    x = binaryReader.ReadSingle(),
                    y = binaryReader.ReadSingle(),
                    z = binaryReader.ReadSingle()
                });
            }

            if (binaryReader.BaseStream.Position - header.OffsetUVData != dataSize)
                throw new Exception("Did not read all bytes from 3d file UV coordinate data.");

            return uvCoordinates;
        }

        private static uint FindNextOffsetAfter(uint offset, uint fileEnd, RG3DHeader header)
        {
            uint minOffset = fileEnd;
            if (offset >= fileEnd)
                return fileEnd;
            if (header.OffsetFaceData > offset && header.OffsetFaceData < minOffset)
                minOffset = header.OffsetFaceData;
            if (header.OffsetFaceNormals > offset && header.OffsetFaceNormals < minOffset)
                minOffset = header.OffsetFaceNormals;
            if (header.OffsetFrameData > offset && header.OffsetFrameData < minOffset)
                minOffset = header.OffsetFrameData;
            if (header.OffsetSection4 > offset && header.OffsetSection4 < minOffset)
                minOffset = header.OffsetSection4;
            if (header.OffsetUVData > offset && header.OffsetUVData < minOffset)
                minOffset = header.OffsetUVData;
            if (header.OffsetUVOffsets > offset && header.OffsetUVOffsets < minOffset)
                minOffset = header.OffsetUVOffsets;
            if (header.OffsetVertexCoords > offset && header.OffsetVertexCoords < minOffset)
                minOffset = header.OffsetVertexCoords;
            return minOffset;
        }
    }
}
