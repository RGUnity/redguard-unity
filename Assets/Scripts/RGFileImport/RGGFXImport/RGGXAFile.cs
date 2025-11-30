using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGGXAFile
	{
        public struct GXASectionHeader
		{
			public string sectionName;
            public uint dataLength;

			public GXASectionHeader(MemoryReader memoryReader)
            {
                try
                {
                    char[] sectionName_char;
                    sectionName_char = memoryReader.ReadChars(4);
                    string[] name_strs = new string(sectionName_char).Split('\0');
                    sectionName = name_strs[0];

                    if(sectionName == "END ") // yeah theres a space there
                    {
                        dataLength = 0;
                    }
                    else
                    {
                        dataLength = MemoryReader.ReverseBytes(memoryReader.ReadUInt32());
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load GXA section header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
GXAHeader
###################################
name: {sectionName}
size: {dataLength:X}
###################################";
			}
		}

		public struct BMHDSection
		{

            public string title;
            public byte[] unknown1;
            public short numImages;
			public BMHDSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    title = new string(memoryReader.ReadChars(22));
                    unknown1 = memoryReader.ReadBytes(10);
                    numImages = memoryReader.ReadInt16();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load GXA BMHD section with error:\n{ex.Message}");
                }
            }
		}
		public struct BPALSection
		{
            public struct BPALColor
            {
                public byte r;
                public byte g;
                public byte b;
                public BPALColor(byte[] input)
                {
                    r = input[0];
                    g = input[1];
                    b = input[2];
                }
            }

            public List<BPALColor> colors;

			public BPALSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    colors = new List<BPALColor>();
                    for(int i=0;i<256;i++)
                    {
                        colors.Add(new BPALColor(memoryReader.ReadBytes(3)));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load GXA BPAL section with error:\n{ex.Message}");
                }
            }
		}
		public struct BBMPItem
        {
            public short unknown;
            // width and height are flipped down the line in GraphicsConverter
            public short width;
            public short height;
            public short[] unknown2;
            public byte[] data;

			public BBMPItem(MemoryReader memoryReader)
            {
                try
                {
                    unknown = memoryReader.ReadInt16();
                    width = memoryReader.ReadInt16();
                    height = memoryReader.ReadInt16();
                    unknown2 = memoryReader.ReadInt16s(6);
                    data = memoryReader.ReadBytes(width*height);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load GXA BBMP item with error:\n{ex.Message}");
                }
            }

        }

		public struct BBMPSection
		{

            public List<BBMPItem> BBMPItems;

			public BBMPSection(MemoryReader memoryReader, BMHDSection BMHD)
            {
                try
                {
                    BBMPItems = new List<BBMPItem>();
                    for(int i=0;i<BMHD.numImages;i++)
                    {
                        BBMPItems.Add(new BBMPItem(memoryReader));
                    }

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load GXA BBMP section with error:\n{ex.Message}");
                }
            }
		}

	// data
        public long fileSize;
		public List<GXASectionHeader> Sections;
		public BMHDSection BMHD;
		public BPALSection BPAL;
		public BBMPSection BBMP;

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
                throw new Exception($"Failed to load GXA file {filename} with error:\n{ex.Message}");
            }
        }

		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                Sections = new List<GXASectionHeader>();
                bool end = false;
                while(!end)
                {
                    Sections.Add(new GXASectionHeader(memoryReader));
                    if(Sections[Sections.Count-1].sectionName == "END ")
                    {
                        end = true;
                    }
                    else if(Sections[Sections.Count-1].sectionName == "BMHD")
                    {
                        BMHD = new BMHDSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "BPAL")
                    {
                        BPAL = new BPALSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "BBMP")
                    {
                        BBMP = new BBMPSection(memoryReader, BMHD);
                    }
                    else
                    {
                        memoryReader.Seek(Sections[Sections.Count-1].dataLength, (uint)memoryReader.Position);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load GXA file from memory with error:\n{ex.Message}");
            }
        }
	}
}
