using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGBSIFile
	{
		public struct BSISectionHeader
		{
			public string sectionName;
            public int sectionSize;

			public BSISectionHeader(MemoryReader memoryReader)
            {
                try
                {
                    sectionName = new string(memoryReader.ReadChars(4));
                    if(sectionName == "END ") // space at the end!
                    {
                        sectionSize = 0;
                    }
                    else
                    {
                        sectionSize = (int)MemoryReader.ReverseBytes(memoryReader.ReadUInt32());
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load BSISectionHeader with error:\n{ex.Message}");
                }
            }
		}

		public struct IFHDSegment
		{
			public byte[] data;

			public IFHDSegment(MemoryReader memoryReader)
            {
                try
                {
                    data = memoryReader.ReadBytes(44);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load IFHDSegment with error:\n{ex.Message}");
                }
            }
		}
		public struct BSIFSegment
		{
            // nothing here :)
			public BSIFSegment(MemoryReader memoryReader)
            {
            }
		}
		public struct BHDRSegment
		{
			public short xOffset;
			public short yOffset;
            public short width;
            public short height;
            public byte unknown1;
            public byte unknown2;
            public short unknown3;
            public short unknown4;
            public short frameCount;
            public short unknown5;
            public short unknown6;
            public short unknown7;
            public byte unknown8;
            public byte unknown9;
            public short unknown10;

			public BHDRSegment(MemoryReader memoryReader)
            {
                try
                {
                    xOffset = memoryReader.ReadInt16();
                    yOffset = memoryReader.ReadInt16();
                    width = memoryReader.ReadInt16();
                    height = memoryReader.ReadInt16();
                    unknown1 = memoryReader.ReadByte();
                    unknown2 = memoryReader.ReadByte();
                    unknown3 = memoryReader.ReadInt16();
                    unknown4 = memoryReader.ReadInt16();
                    frameCount = memoryReader.ReadInt16();
                    unknown5 = memoryReader.ReadInt16();
                    unknown6 = memoryReader.ReadInt16();
                    unknown7 = memoryReader.ReadInt16();
                    unknown8 = memoryReader.ReadByte();
                    unknown9 = memoryReader.ReadByte();
                    unknown10 = memoryReader.ReadInt16();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load BHDRSegment with error:\n{ex.Message}");
                }
            }
		}
        public struct CMAPSegment
		{
			public byte[] data;

			public CMAPSegment(MemoryReader memoryReader)
            {
                try
                {
                    data = memoryReader.ReadBytes(768);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load CMAPSegment with error:\n{ex.Message}");
                }
            }
		}
        public struct DATASegment
		{
			public int[] frameOffsets;
			public List<byte[]> data;

			public DATASegment(BHDRSegment BHDR,MemoryReader memoryReader)
            {
                try
                {
                    data = new List<byte[]>();
                    if(BHDR.frameCount == 1)
                    {
                        frameOffsets = new int[1];
                        data.Add(memoryReader.ReadBytes(BHDR.width*BHDR.height));
                    }
                    else
                    {
                        int frameOffsetSize = BHDR.height*BHDR.frameCount;
                        frameOffsets = new int[frameOffsetSize];
                        int baseOffset = memoryReader.Position;
                        for(int i=0;i<frameOffsetSize;i++)
                            frameOffsets[i] = memoryReader.ReadInt32();

                        for(int f=0;f<BHDR.frameCount;f++)
                        {
                            List<byte> data_f = new List<byte>();
                            for(int y=0;y<BHDR.height;y++)
                            {
                                int offsetIndex = BHDR.height*f+y;
                                int offset = frameOffsets[offsetIndex];
                                memoryReader.Seek((uint)offset, (uint) baseOffset);
                                data_f.AddRange(memoryReader.ReadBytes(BHDR.width));
                            }
                            data.Add(data_f.ToArray());
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load DATASegment with error:\n{ex.Message}");
                }
            }
		}

        // data
        public long fileSize;

		public IFHDSegment IFHD;
		public BSIFSegment BSIF;
		public BHDRSegment BHDR;
        public CMAPSegment CMAP;
        public DATASegment DATA;

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
                throw new Exception($"Failed to load BSI file {filename} with error:\n{ex.Message}");
            }
        }
		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                List<BSISectionHeader> sections = new List<BSISectionHeader>();
                bool end = false;
                while(!end)
                {
                    sections.Add(new BSISectionHeader(memoryReader));
                    if(sections[sections.Count-1].sectionName == "END ")
                    {
                        end = true;
                    }
                    else if(sections[sections.Count-1].sectionName == "IFHD")
                    {
                        IFHD = new IFHDSegment(memoryReader);
                    }
                    else if(sections[sections.Count-1].sectionName == "BSIF")
                    {
                        BSIF = new BSIFSegment(memoryReader);
                    }
                    else if(sections[sections.Count-1].sectionName == "BHDR")
                    {
                        BHDR = new BHDRSegment(memoryReader);
                    }
                    else if(sections[sections.Count-1].sectionName == "CMAP")
                    {
                        CMAP = new CMAPSegment(memoryReader);
                    }
                    else if(sections[sections.Count-1].sectionName == "DATA")
                    {
                        int ofs = memoryReader.Position;
                        DATA = new DATASegment(BHDR, memoryReader);
                        // not sure why we have to seek here
                        memoryReader.Seek((uint)ofs, (uint)sections[sections.Count-1].sectionSize);
                    }
                    else
                    {
                        throw new Exception($"Unknown BSI segment name: \"{sections[sections.Count-1].sectionName}\"");
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load BSI file from memory with error:\n{ex.Message}");
            }
        }
	}
}
