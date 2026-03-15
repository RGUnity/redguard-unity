using System;
using RGFileImport;
using System.IO;
using System.Collections.Generic;

namespace RGFileImport2
{
    public class RG3DFile
    {
        public struct Coord3DInt
        {
            public int x;
            public int y;
            public int z;

            public Coord3DInt(int X, int Y, int Z)
            {
                x = X;
                y = Y;
                z = Z;
            }
        }

        public struct Coord3DFloat
        {
            public float x;
            public float y;
            public float z;

            public Coord3DFloat(float X, float Y, float Z)
            {
                x = X;
                y = Y;
                z = Z;
            }
        }

        public struct RG3DHeader
        {
            public uint version;            //
            public uint numVertices;        //
            public uint numFaces;           //
            public uint radius;             //
            public uint numFrames;          //
            public uint offsetFrameData;    //
            public uint numUVOffsets;       //
            public uint offsetSection4;     //
            public uint Section4Count;      //
            public uint unknown1;           //
            public uint offsetUVOffsets;    //
            public uint offsetUVData;       //
            public uint offsetVertexCoords; //
            public uint offsetFaceNormals;  //
            public uint numUVOffsets2;      //
            public uint offsetFaceData;     //

            public RG3DHeader(MemoryReader memoryReader)
            {
                try
                {
                    version = memoryReader.ReadUInt32();
                    numVertices = memoryReader.ReadUInt32();
                    numFaces = memoryReader.ReadUInt32();
                    radius = memoryReader.ReadUInt32();
                    numFrames = memoryReader.ReadUInt32();
                    offsetFrameData = memoryReader.ReadUInt32();
                    numUVOffsets = memoryReader.ReadUInt32();
                    offsetSection4 = memoryReader.ReadUInt32();
                    Section4Count = memoryReader.ReadUInt32();
                    unknown1 = memoryReader.ReadUInt32();
                    offsetUVOffsets = memoryReader.ReadUInt32();
                    offsetUVData = memoryReader.ReadUInt32();
                    offsetVertexCoords = memoryReader.ReadUInt32();
                    offsetFaceNormals = memoryReader.ReadUInt32();
                    numUVOffsets2 = memoryReader.ReadUInt32();
                    offsetFaceData = memoryReader.ReadUInt32();

                    switch(version)
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

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) header with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                return $@"###################################
3DHeader
###################################
version: {version}
numVertices: {numVertices}
numFaces: {numFaces}
radius: {radius}
numFrames: {numFrames}
offsetFrameData: {offsetFrameData:X8}
numUVOffsets: {numUVOffsets}
offsetSection4: {offsetSection4:X8}
Section4Count: {Section4Count}
unknown1: {unknown1}
offsetUVOffsets: {offsetUVOffsets:X8}
offsetUVData: {offsetUVData:X8}
offsetVertexCoords: {offsetVertexCoords:X8}
offsetFaceNormals: {offsetFaceNormals:X8}
numUVOffsets2: {numUVOffsets2}
offsetFaceData: {offsetFaceData:X8}
###################################";
            }
        }
        public struct FrameDataItem
        {
            public uint frameVertexOffset;
            public uint frameNormalOffset;
            public uint u1;
            public uint u2;

            public FrameDataItem(MemoryReader memoryReader)
            {
                try
                {
                    frameVertexOffset = memoryReader.ReadUInt32();
                    frameNormalOffset = memoryReader.ReadUInt32();
                    u1 = memoryReader.ReadUInt32();
                    u2 = memoryReader.ReadUInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FrameDataItem with error:\n{ex.Message}");
                }
            }
        }
        public struct FrameDataList
        {
            public List<FrameDataItem> frameData;

            public FrameDataList(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    frameData = new List<FrameDataItem>();

                    memoryReader.Seek(header.offsetFrameData,0);
                    for(int i=0;i<header.numFrames;i++)
                    {
                        FrameDataItem cur = new FrameDataItem(memoryReader);
                        frameData.Add(cur);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FrameDataList with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                string o = new String($@"###################################
FrameDataList
###################################");
                o += "\n";
                for(int i=0;i<frameData.Count;i++)
                {
                    o += $"{frameData[i].frameVertexOffset:X8} {frameData[i].frameNormalOffset:X8} {frameData[i].u1:X8} {frameData[i].u2:X8}\n";
                }
                o += "###################################";
                return o;
                
            }

        }

        public struct FaceVertexItem
        {
            public uint vertexIndex;
            public short u;
            public short v;

            public FaceVertexItem(MemoryReader memoryReader, RG3DHeader header, short u_total, short v_total)
            {
                try
                {
                    vertexIndex = memoryReader.ReadUInt32();
                    u = memoryReader.ReadInt16();
                    v = memoryReader.ReadInt16();
                    if(header.version <= 27)
                        vertexIndex /= 12;
                    u += u_total;
                    v += v_total;
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FaceVertexItem with error:\n{ex.Message}");
                }
            }
        }
        public struct FaceDataItem
        {
            public byte vertexCount;
            public byte u1;
            public uint textureData;
            public uint u2;
            public List<FaceVertexItem> vertexData;

            // calculated values
            public uint textureId;
            public uint imageId;
            public bool solidColor;
            public byte colorIndex;

            public FaceDataItem(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    vertexCount = memoryReader.ReadByte();
                    u1 = memoryReader.ReadByte();
                    if(header.version > 27)
                        textureData = memoryReader.ReadUInt32();
                    else
                        textureData = memoryReader.ReadUInt16();
                    u2 = memoryReader.ReadUInt32();
            // vertexdata
                    vertexData = new List<FaceVertexItem>();                    
                    short v_total = 0;
                    short u_total = 0;
                    for(int i=0;i<vertexCount;i++)
                    {
                        vertexData.Add(new FaceVertexItem(memoryReader, header, u_total, v_total));
                        u_total = vertexData[i].u;
                        v_total = vertexData[i].v;
                    }

                    // calculated values
                    if(header.version > 27)
                    {
                        if((textureData >> 20) == 0x0FFF)
                        {
                            textureId = 0;
                            imageId = 0;
                            colorIndex = (byte)(textureData >> 8);
                            solidColor = true;
                        }
                        else
                        {
                            uint tmp = (textureData >> 8) - 4000000;
                            uint t_one = (tmp/250)%40;
                            uint t_ten = ((tmp-(t_one*250))/1000)%100;
                            uint t_hundred = (tmp-(t_one*250)-(t_ten*1000))/4000;

                            uint i_one = (textureData & 0xFF)%10;
                            uint i_ten = ((textureData & 0xFF)/40)*10;

                            textureId = t_one+t_ten+t_hundred;
                            imageId = i_one+i_ten;
                            colorIndex = 0;
                            solidColor = false;

                        }
                    }
                    else
                    {
                        textureId = (textureData >> 7);
                        if(textureId < 2)
                        {
                            imageId = 0;
                            colorIndex = (byte)(textureData);
                            solidColor = true;
                        }
                        else
                        {
                            imageId = (byte)(textureData & 0x7F);
                            colorIndex = 0;
                            solidColor = false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FaceDataItem with error:\n{ex.Message}");
                }
            }
        }
        public struct FaceDataList
        {
            public List<FaceDataItem> faceData;

            public FaceDataList(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    faceData = new List<FaceDataItem>();
                    memoryReader.Seek(header.offsetFaceData,0);
                    for(int i=0;i<header.numFaces;i++)
                    {
                        FaceDataItem cur = new FaceDataItem(memoryReader, header);
                        faceData.Add(cur);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FaceDataList with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                string o = new String($@"###################################
FaceDataList
###################################");
                o += "\n";
                for(int i=0;i<faceData.Count;i++)
                {
                    o += $"{i:D3} ";
                    o += $"{faceData[i].vertexCount:X8} ";
                    o += $"{faceData[i].u1:X8} ";
                    o += $"{faceData[i].textureData:X8} ";
                    o += $"{faceData[i].u2:X8} ";
                    // vertexdata

                    // calculated values
                    o += $"{faceData[i].textureId:X8} ";
                    o += $"{faceData[i].imageId:X8} ";
                    o += $"{faceData[i].solidColor:X8} ";
                    o += $"{faceData[i].colorIndex:X8}\n";
                    for(int j=0;j<faceData[i].vertexData.Count;j++)
                    {
                        o += $"{faceData[i].vertexData[j].vertexIndex:X3} ";
                        o += $"{faceData[i].vertexData[j].u:X3} ";
                        o += $"{faceData[i].vertexData[j].v:X3}\n";
                    }
                }
                o += "###################################";
                return o;
                
            }

        }

        public struct VertexData
        {
            public List<Coord3DInt> coords;
            public VertexData(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    coords = new List<Coord3DInt>();
                    memoryReader.Seek(header.offsetVertexCoords,0);
                    for(int i=0;i<header.numVertices;i++)
                    {
                        int x = memoryReader.ReadInt32();
                        int y = memoryReader.ReadInt32();
                        int z = memoryReader.ReadInt32();
                        Coord3DInt cur = new Coord3DInt(x, y, z);
                        coords.Add(cur);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) VertexData with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                string o = new String($@"###################################
VertexDataList
###################################");
                o += "\n";
                for(int i=0;i<coords.Count;i++)
                {
                    o += $"{coords[i].x},";
                    o += $"{coords[i].y},";
                    o += $"{coords[i].z}\n";
                }
                o += "###################################";
                return o;
                
            }

        }

        public struct NormalData
        {
            public List<Coord3DInt> coords;
            public NormalData(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    coords = new List<Coord3DInt>();
                    memoryReader.Seek(header.offsetFaceNormals,0);
                    for(int i=0;i<header.numFaces;i++)
                    {
                        int x = memoryReader.ReadInt32();
                        int y = memoryReader.ReadInt32();
                        int z = memoryReader.ReadInt32();
                        Coord3DInt cur = new Coord3DInt(x, y, z);
                        coords.Add(cur);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) NormalData with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                string o = new String($@"###################################
NormalDataList
###################################");
                o += "\n";
                for(int i=0;i<coords.Count;i++)
                {
                    o += $"{coords[i].x},";
                    o += $"{coords[i].y},";
                    o += $"{coords[i].z}\n";
                }
                o += "###################################";
                return o;
                
            }

        }


        public struct FrameVertexData
        {
            public List<List<Coord3DInt>> coords;
            public FrameVertexData(MemoryReader memoryReader, RG3DHeader header, FrameDataList frameDataList)
            {
                try
                {
                    coords = new List<List<Coord3DInt>>();
                    bool useInt32 = header.numFrames > 0 && frameDataList.frameData[0].u2 == 4;

                    for(int i=0;i<header.numFrames;i++)
                    {
                        coords.Add(new List<Coord3DInt>());

                        memoryReader.Seek(frameDataList.frameData[i].frameVertexOffset,0);
                        if(i == 0 || useInt32)
                        {
                            for(int j=0;j<header.numVertices;j++)
                            {
                                int x = memoryReader.ReadInt32();
                                int y = memoryReader.ReadInt32();
                                int z = memoryReader.ReadInt32();
                                coords[i].Add(new Coord3DInt(x, y, z));
                            }
                        }
                        else
                        {
                            for(int j=0;j<header.numVertices;j++)
                            {
                                int x = (int)memoryReader.ReadInt16();
                                int y = (int)memoryReader.ReadInt16();
                                int z = (int)memoryReader.ReadInt16();
                                coords[i].Add(new Coord3DInt(x, y, z));
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FrameVertexData with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                string o = new String($@"###################################
FrameVertexDataList
###################################");
                o += "\n";
                for(int f=0;f<coords.Count;f++)
                {
                    o += $"FRAME {f}:\n";
                    for(int i=0;i<coords[f].Count;i++)
                    {
                        o += $"{coords[f][i].x},";
                        o += $"{coords[f][i].y},";
                        o += $"{coords[f][i].z}\n";
                    }
                }
                o += "###################################";
                return o;

            }

        }

        public struct FrameNormalData
        {
            public List<List<Coord3DFloat>> faceNormals; // [frame][face]

            public static Coord3DFloat Decode101010_2(uint packed)
            {
                int nx = (int)(packed & 0x3FF); if(nx >= 512) nx -= 1024;
                int ny = (int)((packed >> 10) & 0x3FF); if(ny >= 512) ny -= 1024;
                int nz = (int)((packed >> 20) & 0x3FF); if(nz >= 512) nz -= 1024;
                return new Coord3DFloat(nx / 256.0f, ny / 256.0f, nz / 256.0f);
            }

            public FrameNormalData(MemoryReader memoryReader, RG3DHeader header, FrameDataList frameDataList)
            {
                try
                {
                    faceNormals = new List<List<Coord3DFloat>>();

                    for(int i=0;i<header.numFrames;i++)
                    {
                        faceNormals.Add(new List<Coord3DFloat>());

                        if(frameDataList.frameData[i].frameNormalOffset == 0)
                            continue;

                        memoryReader.Seek(frameDataList.frameData[i].frameNormalOffset,0);

                        if(i == 0)
                            continue;
                        for(int j=0;j<header.numFaces;j++)
                        {
                            faceNormals[i].Add(Decode101010_2(memoryReader.ReadUInt32()));
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) FrameNormalData with error:\n{ex.Message}");
                }
            }
        }




        public struct VertexNormalData
        {
            public List<Coord3DFloat> normals;

            public VertexNormalData(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    normals = new List<Coord3DFloat>();

                    if(header.offsetUVData == 0)
                        return;

                    if(header.offsetUVOffsets == 0)
                    {
                        memoryReader.Seek(header.offsetUVData,0);
                        for(int i=0;i<header.numVertices;i++)
                        {
                            float x = memoryReader.ReadSingle();
                            float y = memoryReader.ReadSingle();
                            float z = memoryReader.ReadSingle();
                            normals.Add(new Coord3DFloat(x, y, z));
                        }
                    }
                    else
                    {
                        List<uint> offsets = new List<uint>();
                        memoryReader.Seek(header.offsetUVOffsets,0);
                        for(int i=0;i<header.numUVOffsets;i++)
                        {
                            offsets.Add(memoryReader.ReadUInt32());
                        }
                        for(int i=0;i<offsets.Count;i++)
                        {
                            memoryReader.Seek(offsets[i],0);
                            float x = memoryReader.ReadSingle();
                            float y = memoryReader.ReadSingle();
                            float z = memoryReader.ReadSingle();
                            normals.Add(new Coord3DFloat(x, y, z));
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) VertexNormalData with error:\n{ex.Message}");
                }
            }
        }

        public struct SubObjectFaceRef
        {
            public uint faceDataOffset;
            public int faceIndex;

            public SubObjectFaceRef(MemoryReader memoryReader)
            {
                faceDataOffset = memoryReader.ReadUInt32();
                faceIndex = (int)(memoryReader.ReadUInt16() / 4);
            }
        }

        public struct SubObjectEntry
        {
            public int centerX, centerY, centerZ;
            public uint radius;
            public Coord3DFloat extents;
            public List<SubObjectFaceRef> faceRefs;

            public SubObjectEntry(MemoryReader memoryReader)
            {
                centerX = memoryReader.ReadInt32();
                centerY = memoryReader.ReadInt32();
                centerZ = memoryReader.ReadInt32();
                radius = memoryReader.ReadUInt32();
                ushort faceCount = memoryReader.ReadUInt16();
                float ex = memoryReader.ReadSingle();
                float ey = memoryReader.ReadSingle();
                float ez = memoryReader.ReadSingle();
                extents = new Coord3DFloat(ex, ey, ez);
                faceRefs = new List<SubObjectFaceRef>();
                for(int i = 0; i < faceCount; i++)
                    faceRefs.Add(new SubObjectFaceRef(memoryReader));
            }
        }

        public struct SubObjectData
        {
            public List<SubObjectEntry> entries;

            public SubObjectData(MemoryReader memoryReader, RG3DHeader header)
            {
                entries = new List<SubObjectEntry>();
                if(header.offsetSection4 == 0 || header.Section4Count == 0)
                    return;
                try
                {
                    memoryReader.Seek(header.offsetSection4, 0);
                    for(int i = 0; i < header.Section4Count; i++)
                        entries.Add(new SubObjectEntry(memoryReader));
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) SubObjectData with error:\n{ex.Message}");
                }
            }
        }

        public long fileSize;
        public RG3DHeader header;
        public FrameDataList frameDataList;
        public FaceDataList faceDataList;
        public VertexData vertexData;
        public NormalData normalData;
        public FrameVertexData frameVertexData;
        public FrameNormalData frameNormalData;
        public VertexNormalData vertexNormalData;
        public SubObjectData subObjectData;

        public void LoadFile(string filename)
        {
            try
            {
                byte[] buffer;
                BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
                fileSize = binaryReader.BaseStream.Length;
                buffer = binaryReader.ReadBytes((int)fileSize);
                binaryReader.Close();
                LoadMemory(buffer);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load 3D(C) file {filename} with error:\n{ex.Message}");
            }
        }
        public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer, true);
                memoryReader.set_usage_marker("HDR");
                header = new RG3DHeader(memoryReader);
                memoryReader.set_usage_marker("FDL");
                frameDataList = new FrameDataList(memoryReader, header);
                memoryReader.set_usage_marker("FVD");
                frameVertexData = new FrameVertexData(memoryReader, header, frameDataList);
                memoryReader.set_usage_marker("FND");
                frameNormalData = new FrameNormalData(memoryReader, header, frameDataList);
                memoryReader.set_usage_marker("fDL");
                faceDataList = new FaceDataList(memoryReader, header);
                memoryReader.set_usage_marker("VD");
                vertexData = new VertexData(memoryReader, header);
                memoryReader.set_usage_marker("ND");
                normalData = new NormalData(memoryReader, header);
                memoryReader.set_usage_marker("VND");
                vertexNormalData = new VertexNormalData(memoryReader, header);
                memoryReader.set_usage_marker("SOD");
                subObjectData = new SubObjectData(memoryReader, header);
                memoryReader.set_usage_marker("OTHER");
                Console.WriteLine(memoryReader.print_usage());
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load 3D(C) file from memory with error:\n{ex.Message}");
            }
        }

    }
}
