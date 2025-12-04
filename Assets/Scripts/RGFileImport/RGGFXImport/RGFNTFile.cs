using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGFNTFile
	{
        public struct FNTSectionHeader
		{
			public string sectionName;
            public uint dataLength;

			public FNTSectionHeader(MemoryReader memoryReader)
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
                    throw new Exception($"Failed to load FNT section header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
FNTHeader
###################################
name: {sectionName}
size: {dataLength:X}
###################################";
			}
		}
		public struct FNHDSection
		{

            public string description;
            public short unknown1;
            public short hasRDAT;
            public short unknown2;
            public short unknown3;
            public short unknown4;
            public short maxWidth;
            public short lineHeight;
            public short firstCharacter;
            public short numCharacters;
            public short unknown5;
            public short unknown6;
            public short unknown7;
			public FNHDSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    char[] tchar = memoryReader.ReadChars(32);
                    description = new string(tchar);
                    unknown1 = memoryReader.ReadInt16();
                    hasRDAT = memoryReader.ReadInt16();
                    unknown2 = memoryReader.ReadInt16();
                    unknown3 = memoryReader.ReadInt16();
                    unknown4 = memoryReader.ReadInt16();
                    maxWidth = memoryReader.ReadInt16();
                    lineHeight = memoryReader.ReadInt16();
                    firstCharacter = memoryReader.ReadInt16();
                    numCharacters = memoryReader.ReadInt16();
                    unknown5 = memoryReader.ReadInt16();
                    unknown6 = memoryReader.ReadInt16();
                    unknown7 = memoryReader.ReadInt16();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load FNT FNHD section with error:\n{ex.Message}");
                }
            }
		}
		public struct CharacterData
		{

            public short enabled;
            public short offsetLeft;
            public short offsetTop;
            public short width;
            public short height;
            public byte[] data;
			public CharacterData(MemoryReader memoryReader)
            {
                try
                {
                    enabled = memoryReader.ReadInt16();
                    offsetLeft = memoryReader.ReadInt16();
                    offsetTop = memoryReader.ReadInt16();
                    width = memoryReader.ReadInt16();
                    height = memoryReader.ReadInt16();
                    data = memoryReader.ReadBytes(width*height);

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load FNT character data with error:\n{ex.Message}");
                }
            }
		}

		public struct FBMPSection
		{

            public List<CharacterData> characters;
			public FBMPSection(MemoryReader memoryReader, FNHDSection FNHD)
            {
                try
                {
                    characters = new List<CharacterData>();
                    for(int i=0;i<FNHD.numCharacters;i++)
                    {
                        characters.Add(new CharacterData(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load FNT FBMP section with error:\n{ex.Message}");
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
                    throw new Exception($"Failed to load FNT BPAL section with error:\n{ex.Message}");
                }
            }
		}
		public struct RDATSection
		{
            // quick and dirty since we dont use it, real format:
            // char[136] description
            // int[10?] unknown
            public byte[] unknown;

			public RDATSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    unknown = memoryReader.ReadBytes((int)size);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load FNT RDAT section with error:\n{ex.Message}");
                }
            }
		}

        // data
        public long fileSize;
		public List<FNTSectionHeader> Sections;
        public BPALSection BPAL;
		public FNHDSection FNHD;
		public FBMPSection FBMP;
		public RDATSection RDAT;

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
                throw new Exception($"Failed to load FNT file {filename} with error:\n{ex.Message}");
            }
        }

		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                Sections = new List<FNTSectionHeader>();
                bool end = false;
                while(!end)
                {
                    Sections.Add(new FNTSectionHeader(memoryReader));
                    if(Sections[Sections.Count-1].sectionName == "END ")
                    {
                        end = true;
                    }
                    else if(Sections[Sections.Count-1].sectionName == "FNHD")
                    {
                        FNHD = new FNHDSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    // BPAL/FPAL are identical
                    else if(Sections[Sections.Count-1].sectionName == "BPAL" ||
                            Sections[Sections.Count-1].sectionName == "FPAL")
                    {
                        BPAL = new BPALSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "FBMP")
                    {
                        FBMP = new FBMPSection(memoryReader, FNHD);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RDAT")
                    {
                        RDAT = new RDATSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else
                    {
                        Console.WriteLine($"SEC: {Sections[Sections.Count-1].sectionName}");
                        memoryReader.Seek(Sections[Sections.Count-1].dataLength, (uint)memoryReader.Position);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load FNT file from memory with error:\n{ex.Message}");
            }
        }
	}
}
