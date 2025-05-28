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
            public uint SectionSize;

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
                        SectionSize = 0;
                    }
                    else
                    {
                        SectionSize = MemoryReader.ReverseBytes(memoryReader.ReadUInt32());
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
size: {SectionSize:X}
###################################";
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
            // copied from MPSO, no idea if it fits
			public byte[] flags;     // 6 bytes
            public string name;      // 9 bytes
            public string name_2;    // 12 bytes
            public byte[] unknown;  // 39 bytes

			public RGMMPOBItem(MemoryReader memoryReader)
            {
                try
                {
                    flags = memoryReader.ReadBytes(6);
                    char[] name_char;
                    name_char = memoryReader.ReadChars(9);
                    string[] name_strs = new string(name_char).Split('\0');
                    name = name_strs[0];
                    name_char = memoryReader.ReadChars(12);
                    name_strs = new string(name_char).Split('\0');
                    name_2 = name_strs[0];
                    
                    unknown = memoryReader.ReadBytes(39);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPOB item with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"name: {name} name_2: {name_2}";
			}
		}

		public struct RGMMPSOItem
		{
			public byte[] flags;           //  4 bytes
            public string name;            // 12 bytes
            public uint posx;              //  4 bytes; increasing moves position east
            public uint height ;           //  4 bytes increasing moves position up
            public uint posy;              //  4 bytes increasing moves position north
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
                    posx = memoryReader.ReadUInt32();
                    height = memoryReader.ReadUInt32();
                    posy = memoryReader.ReadUInt32();
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
                    else if(Sections[Sections.Count-1].sectionName == "MPOB")
                    {
                        MPOB = new RGMMPOBSection(memoryReader, Sections[Sections.Count-1].SectionSize);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPSO")
                    {
                        MPSO = new RGMMPSOSection(memoryReader, Sections[Sections.Count-1].SectionSize);
                    }
                    else
                    {
                        memoryReader.Seek(Sections[Sections.Count-1].SectionSize, (uint)memoryReader.Position);
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
