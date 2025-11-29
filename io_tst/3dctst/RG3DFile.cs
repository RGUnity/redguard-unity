using System;
using System.IO;
using System.Collections.Generic;
using RGFileImport;

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

        public struct RG3DHeader
        {
            public uint version;            //
            public uint numVertices;        //
            public uint numFaces;           //
            public uint radius;             // unused
            public uint numFrames;          //
            public uint offsetFrameData;    //
            public uint numUVOffsets;       // unused
            public uint offsetSection4;     // unused
            public uint Section4Count;      // unused
            public uint unknown1;           // unused
            public uint offsetUVOffsets;    // unused
            public uint offsetUVData;       // unused; points to numVertices*12 bytes size block
            public uint offsetVertexCoords; //
            public uint offsetFaceNormals;  //
            public uint numUVOffsets2;      // unused
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
//            public List<List<Coord3DInt>> norms;
            public FrameVertexData(MemoryReader memoryReader, RG3DHeader header, FrameDataList frameDataList)
            {
                try
                {
                    coords = new List<List<Coord3DInt>>();
                    for(int i=0;i<header.numFrames;i++)
                    {
                        coords.Add(new List<Coord3DInt>());

                        memoryReader.Seek(frameDataList.frameData[i].frameVertexOffset,0);
                        for(int j=0;j<header.numVertices;j++)
                        {
                            int x = (int)memoryReader.ReadInt16();
                            int y = (int)memoryReader.ReadInt16();
                            int z = (int)memoryReader.ReadInt16();
                            Coord3DInt cur = new Coord3DInt(x, y, z);
                            coords[i].Add(cur);
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
                    for(int i=0;i<coords.Count;i++)
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




        public long fileSize;
        public RG3DHeader header;
        public FrameDataList frameDataList;
        public FaceDataList faceDataList;
        public VertexData vertexData;
        public NormalData normalData;
        public FrameVertexData frameVertexData;

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
                memoryReader.set_usage_marker("HEADER");
                header = new RG3DHeader(memoryReader);

                memoryReader.set_usage_marker("FRAMEDAT");
                frameDataList = new FrameDataList(memoryReader, header);

                memoryReader.set_usage_marker("FVDAT");
                frameVertexData = new FrameVertexData(memoryReader, header, frameDataList);

                memoryReader.set_usage_marker("FACEDAT");
                faceDataList = new FaceDataList(memoryReader, header);

                memoryReader.set_usage_marker("VERTDAT");
                vertexData = new VertexData(memoryReader, header);

                memoryReader.set_usage_marker("NORMDAT");
                normalData = new NormalData(memoryReader, header);

                Console.WriteLine(memoryReader.print_usage());
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load 3D(C) file from memory with error:\n{ex.Message}");
            }
        }

    }
}
