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
            public uint version;                //
            public uint numVertices;            //
            public uint numFaces;               //
            public uint radius;                 // unused
            public uint numFrames;              //
            public uint offsetFrameData;        //
            public uint numUnknownOffsets;      // 
            public uint offsetSection4;         // unused
            public uint Section4Count;          // unused
            public uint unknown1;               // unused
            public uint offsetUnknownOffsets;   //
            public uint offsetUnknownData;      // unknown, per-vertex data presumeably normals?
            public uint offsetVertexCoords;     //
            public uint offsetFaceNormals;      //
            public uint numUnknownOffsets2;          // unused
            public uint offsetFaceData;         //

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
                    numUnknownOffsets = memoryReader.ReadUInt32();
                    offsetSection4 = memoryReader.ReadUInt32();
                    Section4Count = memoryReader.ReadUInt32();
                    unknown1 = memoryReader.ReadUInt32();
                    offsetUnknownOffsets = memoryReader.ReadUInt32();
                    offsetUnknownData = memoryReader.ReadUInt32();
                    offsetVertexCoords = memoryReader.ReadUInt32();
                    offsetFaceNormals = memoryReader.ReadUInt32();
                    numUnknownOffsets2 = memoryReader.ReadUInt32();
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
numUnknownOffsets: {numUnknownOffsets}
offsetSection4: {offsetSection4:X8}
Section4Count: {Section4Count}
unknown1: {unknown1}
offsetUnknownOffsets: {offsetUnknownOffsets:X8}
offsetUnknownData: {offsetUnknownData:X8}
offsetVertexCoords: {offsetVertexCoords:X8}
offsetFaceNormals: {offsetFaceNormals:X8}
numUnknownOffsets2: {numUnknownOffsets2}
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
            /* frameVertexOffset works mostly
             * frameNormalOffset might not be normals, it seems to be controlled by u2:
             * if u2 == 2, it contains 4 bytes per face
             * if u2 == 4, UNKNOWN, but not face-related
             * if u2 == 8, UNKNOWN
            */

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
                    // calculated values
                    o += $"{faceData[i].textureId:X8} ";
                    o += $"{faceData[i].imageId:X8} ";
                    o += $"{faceData[i].solidColor:X8} ";
                    o += $"{faceData[i].colorIndex:X8}\n";
                    // vertexdata
                    for(int j=0;j<faceData[i].vertexData.Count;j++)
                    {
                        o += $"{faceData[i].vertexData[j].vertexIndex:D3} ";
                        o += $"{faceData[i].vertexData[j].u:X3} ({faceData[i].vertexData[j].u}) ";
                        o += $"{faceData[i].vertexData[j].v:X3} ({faceData[i].vertexData[j].v})\n";
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
        public UnknownData unknownData;

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

                memoryReader.set_usage_marker("FACEDAT");
                faceDataList = new FaceDataList(memoryReader, header);

                memoryReader.set_usage_marker("VERTDAT");
                vertexData = new VertexData(memoryReader, header);

                memoryReader.set_usage_marker("NORMDAT");
                normalData = new NormalData(memoryReader, header);
/*
                memoryReader.set_usage_marker("FVERDAT");
                frameVertexData = new FrameVertexData(memoryReader, header, frameDataList);
*/

                unknownData = new UnknownData(memoryReader, header);
/*
*/
                Console.WriteLine(memoryReader.print_usage());
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load 3D(C) file from memory with error:\n{ex.Message}");
            }
        }
        public struct UnknownData
        {
            // currently unknown data, 3*int per vertex, MKCRATE looks like normals?
            public List<Coord3DInt> unknownData;

            public UnknownData(MemoryReader memoryReader, RG3DHeader header)
            {
                try
                {
                    // if the offsetUnknowOffsets is not set, this data is 12 bytes per vertex
                    // found at the offsetUnknownData location
                    if(header.offsetUnknownOffsets == 0)
                    {
                        memoryReader.set_usage_marker("UKNDAT");
                        unknownData = new List<Coord3DInt>();
                        memoryReader.Seek(header.offsetUnknownData,0);

                        for(int i=0;i<header.numVertices;i++)
                        {
                            int x = memoryReader.ReadInt32();
                            int y = memoryReader.ReadInt32();
                            int z = memoryReader.ReadInt32();
                            unknownData.Add(new Coord3DInt(x, y, z));
                        }

                    }
                    else
                    {
                        memoryReader.set_usage_marker("UKNOFS");
                        List<uint> unknownDataOffsets = new List<uint>();

                        memoryReader.Seek(header.offsetUnknownOffsets,0);
                        for(int i=0;i<header.numUnknownOffsets;i++)
                        {
                            uint cur = memoryReader.ReadUInt32();
                            unknownDataOffsets.Add(cur);
                        }

                        memoryReader.set_usage_marker("UKNDAT");
                        unknownData = new List<Coord3DInt>();
                        for(int i=0;i<unknownDataOffsets.Count;i++)
                        {
                            memoryReader.Seek(unknownDataOffsets[i],0);
                            int x = memoryReader.ReadInt32();
                            int y = memoryReader.ReadInt32();
                            int z = memoryReader.ReadInt32();
                            unknownData.Add(new Coord3DInt(x, y, z));

                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load 3D(C) UnknownData with error:\n{ex.Message}");
                }
            }
            public override string ToString()
            {
                string o = new String($@"###################################
UnknownDataOffsets
###################################");
                o += "\n";
                for(int i=0;i<unknownData.Count;i++)
                {
                    o += $"{i:D3}: {unknownData[i].x:D5},{unknownData[i].y:D5},{unknownData[i].z:D5}\n";
                }
                o += "###################################";
                return o;
                
            }

        }



    }
}
