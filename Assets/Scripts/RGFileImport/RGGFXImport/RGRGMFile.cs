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
		public struct RGMMPSOItem
		{
			public byte[] flags;    //  8 bytes
            public string name;     // 12 bytes
            public byte[] unknown1; //  1 byte
            public byte posx;       //  1 byte
            public int  height ;    //  4 bytes
            public byte[] unknown2; //  3 bytes
            public byte posy;       //  1 byte
            public byte[] unknown4; // 36 bytes
                                    // for whoevers keeping track: 66 bytes
                                    // this is not 4-byte aligned, so if
                                    // youre implementing this in c/c++ thats
                                    // why you're getting garbage data

			public RGMMPSOItem(MemoryReader memoryReader)
            {
                flags = memoryReader.ReadBytes(8);
                char[] name_char;
                name_char = memoryReader.ReadChars(12);
                string[] name_strs = new string(name_char).Split('\0');
                name = name_strs[0];
                unknown1 = memoryReader.ReadBytes(1);
                posx = memoryReader.ReadByte();
                height = (int)ReverseBytes(memoryReader.ReadUInt32());
                unknown2 = memoryReader.ReadBytes(3);
                posy = memoryReader.ReadByte();
                unknown4 = memoryReader.ReadBytes(36);
            }
			public override string ToString()
			{
				return $@"name: {name} x: {posx} y: {posy}";
			}
		}
		public struct RGMMPSOSection
		{

			public List<RGMMPSOItem> items;
            const int MPSOItem_size = 66; // heres that magic number again
			public RGMMPSOSection(MemoryReader memoryReader, uint size)
            {
                items = new List<RGMMPSOItem>();
                uint ptr = 0;
                while(ptr<size-4)
                {
                    items.Add(new RGMMPSOItem(memoryReader));
                    ptr += 66;
                }
                memoryReader.ReadUInt32(); // why the extra 4 bytes? no idea
            }
			public override string ToString()
			{
                string o = new String($@"###################################\nMPSO Data\n###################################");
                for(int i=0;i<items.Count;i++)
                {
                    o += $"\n{items[i]}";
                }
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
		}
	}
}
