using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGRGMFile
	{
		public struct RGMSectionHeader
		{
			public string sectionName;
            public uint dataLength;

			public RGMSectionHeader(MemoryReader memoryReader)
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
                    throw new Exception($"Failed to load RGM section header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
RGMHeader
###################################
name: {sectionName}
size: {dataLength:X}
###################################";
			}
		}
		public struct RGMRAHDSection
        {
            public uint num_items;  // 4 bytes
            public byte[] Unknown1 ;  // 3 bytes
			public List<RGMRAHDItem> items;
			public RGMRAHDSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();
                    Unknown1 = memoryReader.ReadBytes(4);

                    items = new List<RGMRAHDItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMRAHDItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAHD section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nRAHD Data\n###################################");
                for(int i=0;i<items.Count;i++)
                {
                    o += $"\n{items[i]}";
                }
                o += $"\nNUM_ITEMS:: {num_items}\n";
                o += $"\n###################################";
				return o;
			}
		}
		public struct RGMRAHDItem
		{
            public byte[] tmp;
            public byte[] Unknown1;      // 4 bytes
            public string name;          // 9 bytes
            public short instances;      // 2 bytes
            public byte[] Unknown2;      // 50 bytes
            public int stringCount;      // 4 bytes
            public byte[] Unknown3;      // 4 bytes
            public int stringOffsetIndex;// 4 bytes
            public int scriptLength;     // 4 bytes
            public int scriptDataOffset; // 4 bytes
            public int scriptPC;         // 4 bytes
            public byte[] Unknown4;      // 28 bytes
            public int variableCount;    // 4 bytes
            public byte[] Unknown5;      // 4 bytes
            public int variableOffset;   // 4 bytes
            public byte[] Unknown6;      // 37 bytes
                                         // total: 165 bytes
			public RGMRAHDItem(MemoryReader memoryReader)
            {
                try
                {
                    int pos = memoryReader.Position;
                    tmp = memoryReader.ReadBytes(165);
                    memoryReader.Seek((uint)pos ,0);

                    Unknown1 = memoryReader.ReadBytes(4);
                    char[] name_char = memoryReader.ReadChars(9);
                    string[] name_strs = new string(name_char).Split('\0');
                    name = name_strs[0];
                    instances = memoryReader.ReadInt16();
                    Unknown2 = memoryReader.ReadBytes(50);
                    stringCount = memoryReader.ReadInt32();
                    Unknown3 = memoryReader.ReadBytes(4);
                    stringOffsetIndex = memoryReader.ReadInt32();
                    scriptLength = memoryReader.ReadInt32();
                    scriptDataOffset = memoryReader.ReadInt32();
                    scriptPC = memoryReader.ReadInt32();
                    Unknown4 = memoryReader.ReadBytes(28);
                    variableCount = memoryReader.ReadInt32();
                    Unknown5 = memoryReader.ReadBytes(4);
                    variableOffset = memoryReader.ReadInt32();
                    Unknown6 = memoryReader.ReadBytes(36);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAHD item with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o1 = new string("");
                for(int i=0;i<tmp.Length;i++)
                {
                    o1 += $"{tmp[i]:X2},";
                }
                Console.WriteLine(o1);

                string o = new string("");
                for(int i=0;i<Unknown1.Length;i++)
                {
                    o += $"{Unknown1[i]:X2},";
                }
                o += $"{name},";
                for(int i=0;i<Unknown2.Length;i++)
                {
                    o += $"{Unknown2[i]:X2},";
                }
                o += $"{stringCount},";
                for(int i=0;i<Unknown3.Length;i++)
                {
                    o += $"{Unknown3[i]:X2},";
                }
                o += $"{stringOffsetIndex},";
                for(int i=0;i<Unknown4.Length;i++)
                {
                    o += $"{Unknown4[i]:X2},";
                }
                o += $"{instances:D4},";
                o += $"{scriptLength:D4},";
                o += $"{scriptDataOffset:D4},";
                o += $"{scriptPC:D4},";
                for(int i=0;i<Unknown5.Length;i++)
                {
                    o += $"{Unknown5[i]:X2},";
                }
                o += $"{variableCount:D4},";
                for(int i=0;i<Unknown6.Length;i++)
                {
                    o += $"{Unknown6[i]:X2},";
                }
                o += $"{variableOffset:D4},";
				return o;
			}
		}
		public struct RGMRASTSection
        {
            public char[] text;     // its one big string
			public RGMRASTSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    text = memoryReader.ReadChars((int)size);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAST section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nRAST Data\n###################################");
                for(int i=0;i<text.Length;i++)
                {
                    o += $"{text[i]}";
                }
                o += $"\n###################################";
				return o;
			}
		}
		public struct RGMRASBSection
        {
            public int[] offsets;     // its one big array of ints
			public RGMRASBSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    offsets = memoryReader.ReadInt32s((int)size/4);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RASB section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nRASB Data\n###################################");
                for(int i=0;i<offsets.Length;i++)
                {
                    o += $"{offsets[i]}";
                }
                o += $"\n###################################";
				return o;
			}
		}
		public struct RGMRAVASection
        {
            public int[] data;     // its one big array of ints
			public RGMRAVASection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    data = memoryReader.ReadInt32s((int)size/4);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAVA section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nRAVA Data\n###################################");
                for(int i=0;i<data.Length;i++)
                {
                    o += $"{data[i]}";
                }
                o += $"\n###################################";
				return o;
			}
		}
		public struct RGMRASCSection
        {
            public byte[] scripts;     // We read the scripts in one go, then process them later
			public RGMRASCSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    scripts = memoryReader.ReadBytes((int)size);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RASC section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nRASC Data\n###################################");
                for(int i=0;i<scripts.Length;i++)
                {
                    o += $"{scripts[i]:X2}";
                }
                o += $"\n###################################";
				return o;
			}
		}
		public struct RGMRAATSection
        {
            public byte[] attributes;     // size bytes
			public RGMRAATSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    attributes = memoryReader.ReadBytes((int)size);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAAT section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nRAAT Data\n###################################");
                for(int i=0;i<attributes.Length;i++)
                {
                    o += $"{attributes[i]:X2}";
                }
                o += $"\n###################################";
				return o;
			}
		}


		public struct RGMMPOBSection
		{
            public uint num_items;  // 4 bytes
            // theres 4*66 bytes at the front with no names, not sure if this is header or empty data
            // if i dont read em the num_items is not correct
			public List<RGMMPOBItem> items;
            const int MPOBItem_size = 66; // heres that magic number again
			public RGMMPOBSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();

                    items = new List<RGMMPOBItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPOBItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPOB section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nMPOB Data\n###################################");
                for(int i=0;i<items.Count;i++)
                {
                    o += $"\n{items[i]}";
                }
                o += $"\nNUM_ITEMS:: {num_items}\n";
                o += $"\n###################################";
				return o;
			}
		}
		public struct RGMMPOBItem
		{
			public byte[] flags;     // 6 bytes
            public string name;      // beginning until 00
            public string name2;     // from the end until 00
                                     // 18 bytes combined
            public byte hasmodel;    // 1 byte
            public byte unknown1;    // 1 byte
            public int  posx;        // 4 bytes
            public int  posy;        // 4 bytes
            public int  posz;        // 4 bytes
            public uint  anglex;     // 4 bytes
            public uint  angley;     // 4 bytes
            public uint  anglez;     // 4 bytes
            public byte[] unknown2;  // 16 bytes
			public RGMMPOBItem(MemoryReader memoryReader)
            {
                try
                {
                    flags = memoryReader.ReadBytes(6);

                    char[] name_char = memoryReader.ReadChars(18);
                    string[] name_strs = new string(name_char).Split('\0');
                    name = name_strs[0];
                    name2 = name_strs[name_strs.Length-1];

                    hasmodel = memoryReader.ReadByte();
                    unknown1 = memoryReader.ReadByte();
                    posx = memoryReader.ReadInt32();
                    posy = memoryReader.ReadInt32();
                    posz = memoryReader.ReadInt32();
                    anglex = memoryReader.ReadUInt32();
                    angley = memoryReader.ReadUInt32();
                    anglez = memoryReader.ReadUInt32();
                    unknown2 = memoryReader.ReadBytes(16);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPOB item with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"{String.Join(",", flags)},{name},{String.Join(",", unknown2)}";
			}
		}

		public struct RGMMPSOItem
		{
			public byte[] flags;           //  4 bytes
            public string name;            // 12 bytes
            public int posx;              //  4 bytes; increasing moves position east
            public int posy;           //  4 bytes increasing moves position up
            public int posz;              //  4 bytes increasing moves position north
            public int[] rotation_matrix;  // 36 bytes => 3x3 matrix, uses Q4.28 fixed-point
            public byte[] unknown;        //  2 bytes always 0
                                           // for whoevers keeping track: 66 bytes
                                           // this is not 4-byte aligned, so if
                                           // youre implementing this in c/c++ thats
                                           // why you're getting garbage data

			public RGMMPSOItem(MemoryReader memoryReader)
            {
                try
                {
                    flags = memoryReader.ReadBytes(4);
                    char[] name_char;
                    name_char = memoryReader.ReadChars(12);
                    string[] name_strs = new string(name_char).Split('\0');
                    name = name_strs[0];
                    posx = memoryReader.ReadInt32();
                    posy = memoryReader.ReadInt32();
                    posz = memoryReader.ReadInt32();
                    rotation_matrix = new int[9];
                    for(int i=0;i<9;i++)
                    {
                        rotation_matrix[i] = memoryReader.ReadInt32();
                    }
                    unknown = memoryReader.ReadBytes(2);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSO item with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new string($@"name: {name} ");
                for(int i=0;i<9;i++)
                {
                    o+= $" {rotation_matrix[i]},";
                }
				return o;
			}
		}
		public struct RGMMPSOSection
		{

            public uint num_items;
			public List<RGMMPSOItem> items;
            const int MPSOItem_size = 66; // heres that magic number again
			public RGMMPSOSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32(); // why the extra 4 bytes? no idea
                    items = new List<RGMMPSOItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPSOItem(memoryReader));
                    }

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSO section with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
                string o = new String($"###################################\nMPSO Data\n###################################");
                for(int i=0;i<items.Count;i++)
                {
                    o += $"\n{items[i]}";
                }
                o += $"\nNUM_ITEMS:: {num_items}\n";
                o += $"\n###################################";
				return o;
			}
		}



	// data
		public List<RGMSectionHeader> Sections;
        public RGMRAHDSection RAHD;
        public RGMRASTSection RAST;
        public RGMRASBSection RASB;
        public RGMRAVASection RAVA;
        public RGMRASCSection RASC;
        public RGMRAATSection RAAT;
        public RGMMPSOSection MPSO;
        public RGMMPOBSection MPOB;
        public long fileSize;

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
                throw new Exception($"Failed to load RGM file {filename} with error:\n{ex.Message}");
            }
        }

		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                Sections = new List<RGMSectionHeader>();
                bool end = false;
                while(!end)
                {
                    Sections.Add(new RGMSectionHeader(memoryReader));
                    if(Sections[Sections.Count-1].sectionName == "END ")
                    {
                        end = true;
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RAHD")
                    {
                        RAHD = new RGMRAHDSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RAST")
                    {
                        RAST = new RGMRASTSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RASB")
                    {
                        RASB = new RGMRASBSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RAVA")
                    {
                        RAVA = new RGMRAVASection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RASC")
                    {
                        RASC = new RGMRASCSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RAAT")
                    {
                        RAAT = new RGMRAATSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPOB")
                    {
                        MPOB = new RGMMPOBSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPSO")
                    {
                        MPSO = new RGMMPSOSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else
                    {
                        memoryReader.Seek(Sections[Sections.Count-1].dataLength, (uint)memoryReader.Position);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load RGM file from memory with error:\n{ex.Message}");
            }
        }


		public void PrintRGM()
		{
            Console.WriteLine(MPSO);
            Console.WriteLine(MPOB);
		}
	}
}
