using System;
using System.Collections.Generic;
using System.IO;

namespace RGFileImport
{
    public struct FrameData
    {
        public uint FrameVertexOffset;
        public uint FrameNormalOffset;
        public uint u1;
        public uint u2;
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
        public byte U1;
        public uint TextureData;
        public uint U4;
        public List<FaceVertexData> VertexData;

        public bool solid_color;
        public uint TextureId;
        public uint ImageId;
        public byte ColorIndex;
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


    public class RG3DFile
    {
        const uint RedguardHeaderSize = 64;
        const float CoordTranformFactor = 256f;
        const float NormalTransformFactor = 256f;
        const float UVTransformFactor = 4096f;
        /* Some old 3DC files have a different vertex start offset for some unknown reason. Use this to determine whether to try and reload 
         * the vertex data if "invalid" coordinates are found. */
        const int MaxCoordValueForOld3DCReload = 1090519040;

        public RG3DHeader header;
        bool useAltVertexOffset; // Set to true in some old v2.6 3DC files in order to correctly load the vertex data
        bool tryReloadOld3dcFileVertex; // This doesn't seem to always work  (only works with CV_ROPE.3DC and CV_SKUL3.3DC
        uint offsetUVZeroes;
        uint offsetUnknown27;
        Coord3DInt minCoord;
        Coord3DInt maxCoord;
        public List<FrameData> frameData;
        string fullName;
        string name;
        bool is3DCFile;
        public long fileSize;
        uint endFaceDataOffset;
        int totalFaceVertexes;
        public int version;
        Dictionary<int, int> faceVectorSizes;

        public List<FaceData> FaceDataCollection { get; set; }
        public List<Coord3DInt> VertexCoordinates { get; set; }
        public List<Coord3DInt> FaceNormals { get; set; }
        public List<uint> UvOffsets { get; set; }
        public List<Coord3DFloat> UvCoordinates { get; set; }

        public List<List<Coord3DInt>> VertexFrameDeltas { get; set; }

        public void LoadFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException();
                fullName = path;
                name = Path.GetFileName(path);
                is3DCFile = name.ToLower().EndsWith(".3dc");



                byte[] buffer;
                BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
                fileSize = binaryReader.BaseStream.Length;
                buffer = binaryReader.ReadBytes((int)fileSize);
                binaryReader.Close();
                LoadMemory(buffer, is3DCFile);

            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load 3D file {path} with error:\n{ex.Message}");
            }
        }
        public void LoadMemory(byte[] buffer, bool is3DC)
        {
            try
            {
                is3DCFile = is3DC;
                MemoryReader memoryReader = new MemoryReader(buffer);
                header = GetHeader(memoryReader);
                frameData = GetFrameData(memoryReader);
                FaceDataCollection = GetFaceData(memoryReader);
                VertexCoordinates = GetVertexCoordinates(memoryReader);
                VertexFrameDeltas = new List<List<Coord3DInt>>();
                for(int i=0;i<header.NumFrames;i++)
                {
                    VertexFrameDeltas.Add(GetVertexFrameOffsets(memoryReader, i));
                }
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
                        VertexCoordinates = GetVertexCoordinates(memoryReader);
                    }
                    throw new Exception("NOPE");
                }

                FaceNormals = GetFaceNormals(memoryReader);
                UvOffsets = GetUVOffsets(memoryReader);
                UvCoordinates = GetUVCoordinates(memoryReader);

            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load 3D file from memory:\n{ex.Message}");
            }
        }

        private RG3DHeader GetHeader(MemoryReader memoryReader)
        {
            var header = new RG3DHeader
            {
                Version = memoryReader.ReadUInt32(),
                NumVertices = memoryReader.ReadUInt32(),
                NumFaces = memoryReader.ReadUInt32(),
                Radius = memoryReader.ReadUInt32(),
                NumFrames = memoryReader.ReadUInt32(),
                OffsetFrameData = memoryReader.ReadUInt32(),
                NumUVOffsets = memoryReader.ReadUInt32(),
                OffsetSection4 = memoryReader.ReadUInt32(),
                Section4Count = memoryReader.ReadUInt32(),
                Unknown4 = memoryReader.ReadUInt32(),
                OffsetUVOffsets = memoryReader.ReadUInt32(),
                OffsetUVData = memoryReader.ReadUInt32(),
                OffsetVertexCoords = memoryReader.ReadUInt32(),
                OffsetFaceNormals = memoryReader.ReadUInt32(),
                NumUVOffsets2 = memoryReader.ReadUInt32(),
                OffsetFaceData = memoryReader.ReadUInt32()
            };

            version = 0;
			switch(header.Version)
            {
                case 0x362E3276: 
					version = 26; // v2.6
					break;
                case 0x372E3276:
					version = 27; // v2.7
					break;
                case 0x302E3476:
					version = 40; // v4.0
					break;
                case 0x302E3576:
					version = 50; // v5.0
					break;
				default:
					version = 0;
					break;
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

        private List<FrameData> GetFrameData(MemoryReader memoryReader)
        {
            List<FrameData> fd_lst = new List<FrameData>();
            if (header.OffsetFrameData == 0 || header.OffsetFrameData >= memoryReader.Length)
                return fd_lst;
            if (header.OffsetFrameData >= memoryReader.Length)
                throw new EndOfStreamException("OffsetFrameData is beyond stream length.");
            memoryReader.Seek(header.OffsetFrameData, 0);
            for(int i=0;i<header.NumFrames;i++)
            {
                FrameData cur = new FrameData();
                cur.FrameVertexOffset = memoryReader.ReadUInt32();
                cur.FrameNormalOffset = memoryReader.ReadUInt32();
                cur.u1 = memoryReader.ReadUInt32();
                cur.u2 = memoryReader.ReadUInt32();
                fd_lst.Add(cur);
            }
            return fd_lst;
        }

        private List<FaceData> GetFaceData(MemoryReader memoryReader)
        {
            var fileLength = (uint)memoryReader.Length;
            uint dataSize = FindNextOffsetAfter(header.OffsetFaceData, fileLength, header) - header.OffsetFaceData;
            var faceDatas = new List<FaceData>();
            if (header.NumFaces == 0)
                return faceDatas;
            if (header.OffsetFaceData > fileLength)
                throw new EndOfStreamException("OffsetFaceData is beyond stream length.");
            faceVectorSizes = new Dictionary<int, int>();
            memoryReader.Seek(header.OffsetFaceData, 0);
            for (var i = 0; i < header.NumFaces; ++i)
            {
                faceDatas.Add(new FaceData());
                var faceData = faceDatas[i];
                faceData.VertexCount = memoryReader.ReadByte();

				faceData.U1 = memoryReader.ReadByte();
                if (version > 27)
					faceData.TextureData = (uint)memoryReader.ReadUInt32();
                else
					faceData.TextureData = (uint)memoryReader.ReadUInt16();

                if(version > 27)
                {
                    if((faceData.TextureData >> 20) == 0x0FFF)
                    {
                        faceData.ColorIndex = (byte)(faceData.TextureData>>8);
                        faceData.solid_color = true;
                    }
                    else
                    {
                        uint tmp = (faceData.TextureData >>8)-4000000;
                        uint one = (tmp/250)%40;
                        uint ten = ((tmp-(one*250))/1000)%100;
                        uint hundred = (tmp-(one*250)-(ten*1000))/4000;
                        faceData.TextureId = one+ten+hundred;

                        one = (faceData.TextureData& 0xFF)%10;
                        ten = ((faceData.TextureData& 0xFF)/40)*10;
                        faceData.ImageId = one+ten;
                        faceData.solid_color = false;
                    }
                }
                else
                {
                    faceData.TextureId = (faceData.TextureData >> 7);
                    if(faceData.TextureId < 2)
                    {
                        faceData.ColorIndex = (byte)(faceData.TextureData);
                        faceData.solid_color = true;
                    }
                    else
                    {
                        faceData.ImageId = (byte)(faceData.TextureData & 0x7f);
                        faceData.solid_color = false;
                    }
                }

                faceData.U4 = memoryReader.ReadUInt32();
                if (!faceVectorSizes.ContainsKey(faceData.VertexCount))
                    faceVectorSizes[faceData.VertexCount] = 0;
                faceVectorSizes[faceData.VertexCount]++;
                totalFaceVertexes += faceData.VertexCount;
                short u = 0;
                short v = 0;
                faceData.VertexData = new List<FaceVertexData>();
                for (var j = 0; j < faceData.VertexCount; ++j)
                {
                    var vertexIndex = memoryReader.ReadUInt32();
                    faceData.VertexData.Add(new FaceVertexData()
                    {
                        VertexIndex = version <= 27 ? vertexIndex / 12 : vertexIndex,
                        // why the +u/v?
                        U = (short)(memoryReader.ReadInt16() + u),
                        V = (short)(memoryReader.ReadInt16() + v)
                    });

                    u = faceData.VertexData[j].U;
                    v = faceData.VertexData[j].V;
                }
            }

            var endOffset = (uint)memoryReader.Position;
            var readSize = endOffset - header.OffsetFaceData;
            endFaceDataOffset = endOffset;
            if (readSize != dataSize && (!is3DCFile || version > 27))
                throw new EndOfStreamException("Failed to read full face data section.");
            return faceDatas;
        }
        private List<Coord3DInt> GetVertexCoordinates(MemoryReader memoryReader)
        {
            uint dataSize = FindNextOffsetAfter(header.OffsetVertexCoords, (uint)memoryReader.Length, header) - header.OffsetVertexCoords;
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
                    offset += frameData[0].u1;
                if (offset >= fileSize)
                    throw new EndOfStreamException("Vertex coordinates are beyond end of file.");
                memoryReader.Seek(offset, 0); 
            }
            else
                memoryReader.Seek(header.OffsetVertexCoords, 0); 
            for (var i = 0; i < header.NumVertices; ++i)
            {
                var coord = new Coord3DInt()
                {
                    x = memoryReader.ReadInt32(),
                    y = memoryReader.ReadInt32(),
                    z = memoryReader.ReadInt32()
                };

                coords.Add(new Coord3DInt()
                {
                    x = coord.x,
                    y = coord.y,
                    z = coord.z
                });
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
                    if (maxCoord.x < coord.x) maxCoord.x = coord.x;
                    if (maxCoord.y < coord.y) maxCoord.y = coord.y;
                    if (maxCoord.z < coord.z) maxCoord.z = coord.z;
                }
            }

            if (memoryReader.Position - header.OffsetVertexCoords != dataSize && (!is3DCFile || version > 27))
                throw new Exception("Did not read all bytes from 3d file vertex coordinates section.");

            return coords;
        }
        private List<Coord3DInt> GetVertexFrameOffsets(MemoryReader memoryReader, int frame)
        {
            var coords = new List<Coord3DInt>();
            if (header.NumVertices <= 0)
                return coords;
            memoryReader.Seek(frameData[frame].FrameVertexOffset, 0); 
            for (var i = 0; i < header.NumVertices; ++i)
            {
                var coord = new Coord3DInt()
                {
                    x = (int)memoryReader.ReadInt16(),
                    y = (int)memoryReader.ReadInt16(),
                    z = (int)memoryReader.ReadInt16()
                };

                coords.Add(new Coord3DInt()
                {
                    x = coord.x,
                    y = coord.y,
                    z = coord.z
                });
            }

            return coords;
        }



        private List<Coord3DInt> GetFaceNormals(MemoryReader memoryReader)
        {
            var fileLength = (uint)memoryReader.Length;
            var dataSize = FindNextOffsetAfter(header.OffsetFaceNormals, fileLength, header) - header.OffsetFaceNormals;
            memoryReader.Seek(header.OffsetFaceNormals, 0);
            var faceNormals = new List<Coord3DInt>();
            for (var i = 0; i < header.NumFaces; ++i)
            {
                faceNormals.Add(new Coord3DInt()
                {
                    x = memoryReader.ReadInt32(),
                    y = memoryReader.ReadInt32(),
                    z = memoryReader.ReadInt32()
                });
            }

            var readSize = memoryReader.Position - header.OffsetFaceNormals;
            if (readSize != dataSize && (!is3DCFile || version > 27))
                throw new Exception("Did not read all bytes from 3D file face normal section.");
            return faceNormals;
        }

        private List<uint> GetUVOffsets(MemoryReader memoryReader)
        {
            var uvOffsets = new List<uint>();
            if (header.OffsetUVOffsets == 0)
                return uvOffsets;
            var fileLength = (uint)memoryReader.Length;
            var dataSize = FindNextOffsetAfter(header.OffsetUVOffsets, fileLength, header) - header.OffsetUVOffsets;
            if (header.NumUVOffsets == 0)
                return uvOffsets;
            memoryReader.Seek(header.OffsetUVOffsets, 0);
            for (var i = 0; i < header.NumUVOffsets; ++i)
                uvOffsets.Add(memoryReader.ReadUInt32());
            if (memoryReader.Position - header.OffsetUVOffsets != dataSize)
                throw new Exception("Did not read all bytes from 3d file UV offset data.");

            return uvOffsets;
        }

        private List<Coord3DFloat> GetUVCoordinates(MemoryReader memoryReader)
        {
            var fileLength = (uint)memoryReader.Length;
            var uvCoordinates = new List<Coord3DFloat>();
            if (header.OffsetUVData == 0 || header.OffsetUVData >= fileSize)
                return uvCoordinates;
            var dataSize = FindNextOffsetAfter(header.OffsetUVData, fileLength, header) - header.OffsetUVData;
            var numUVCoordinates = dataSize / 12;
            if (numUVCoordinates == 0)
                return uvCoordinates;
            memoryReader.Seek(header.OffsetUVData, 0);
            for (var i = 0; i < numUVCoordinates; ++i)
            {
                uvCoordinates.Add(new Coord3DFloat()
                {
                    // why single and not int?
                    x = memoryReader.ReadSingle(),
                    y = memoryReader.ReadSingle(),
                    z = memoryReader.ReadSingle()
                });
            }

            if (memoryReader.Position - header.OffsetUVData != dataSize)
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
