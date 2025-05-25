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
                    SectionSize = ReverseBytes(memoryReader.ReadUInt32());
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
                num_items = memoryReader.ReadUInt32();

                items = new List<RGMMPOBItem>();
                for(int i=0;i<(int)num_items;i++)
                {
                    items.Add(new RGMMPOBItem(memoryReader));
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
			public override string ToString()
			{
				return $@"name: {name} name_2: {name_2}";
			}
		}

		public struct RGMMPSOItem
		{
			public byte[] flags;           //  4 bytes
            public string name;            // 12 bytes
            public ushort posx;            //  2 bytes; increasing moves position east
            public byte[] unknown2;        //  2 bytes always 0?
            public ushort height ;         //  2 bytes increasing moves position up
            public byte[] unknown3;        //  2 bytes 
            public ushort posy;            //  2 bytes increasing moves position north
            public byte[] unknown4;        //  2 bytes always 0
            public int[] rotation_matrix;  // 36 bytes => 3x3 matrix, uses Q4.28 fixed-point
            public byte[] unknown5;        //  2 bytes always 0
                                           // for whoevers keeping track: 66 bytes
                                           // this is not 4-byte aligned, so if
                                           // youre implementing this in c/c++ thats
                                           // why you're getting garbage data

			public RGMMPSOItem(MemoryReader memoryReader)
            {
                flags = memoryReader.ReadBytes(4);
                char[] name_char;
                name_char = memoryReader.ReadChars(12);
                string[] name_strs = new string(name_char).Split('\0');
                name = name_strs[0];
                posx = memoryReader.ReadUInt16();
                unknown2 = memoryReader.ReadBytes(2);
                height = memoryReader.ReadUInt16();
                unknown3 = memoryReader.ReadBytes(2);
                posy = memoryReader.ReadUInt16();
                unknown4 = memoryReader.ReadBytes(2);
                rotation_matrix = new int[9];
                for(int i=0;i<9;i++)
                {
                    rotation_matrix[i] = memoryReader.ReadInt32();
                }
                unknown5 = memoryReader.ReadBytes(2);
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
                num_items = memoryReader.ReadUInt32(); // why the extra 4 bytes? no idea
                items = new List<RGMMPSOItem>();
                for(int i=0;i<(int)num_items;i++)
                {
                    items.Add(new RGMMPSOItem(memoryReader));
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



        // TODO: copy from BSIF
        static protected uint ReverseBytes(uint n)
        {
            var bytes = BitConverter.GetBytes(n);
            Array.Reverse(bytes, 0, bytes.Length);
            return BitConverter.ToUInt32(bytes, 0);
        }

	// data
		public List<RGMSectionHeader> Sections;
        public RGMMPSOSection MPSO;
        public RGMMPOBSection MPOB;
        public long fileSize;

		public void LoadFile(string filename)
        {
            byte[] buffer;
            BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
            fileSize = binaryReader.BaseStream.Length;
            buffer = binaryReader.ReadBytes((int)fileSize);
			binaryReader.Close();
            LoadMemory(buffer);
        }

		public void LoadMemory(byte[] buffer)
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


		public void PrintRGM()
		{
            Console.WriteLine(MPSO);
            Console.WriteLine(MPOB);
		}
	}
}
