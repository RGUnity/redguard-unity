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
			public List<RGMRAHDItem> items;
			public RGMRAHDSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();
                    items = new List<RGMRAHDItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMRAHDItem(memoryReader));
                    }
                    memoryReader.ReadUInt32(); // no idea what this one is?
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAHD section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRAHDItem
		{

            public int unknown0;
            public int unknown1;
            public string scriptName;
            public int MPOBCount;
            public int unknown2;

            public int RAMNLength;
            public int RAMNOffset;

            public int RAATOffset;

            public int RAANCount;
            public int RAANLength;
            public int RAANOffset;

            public int RAGRMaxGroup;
            public int RAGROffset;

            public int unknown3;
            public int unknown4;
            public int unknown5;

            public int RASBCount;
            public int RASBLength;
            public int RASBOffset;

            public int RASCLength;
            public int RASCOffset;
            public int RASCStartAt;

            public int RAHKLength;
            public int RAHKOffset;

            public int RALCCount;
            public int RALCLength;
            public int RALCOffset;

            public int RAEXLength;
            public int RAEXOffset;

            public int RAVACount;
            public int RAVALength;
            public int RAVAOffset;

            public int unknown6;
            public int frameCount;

            public int MPSZNormalId;
            public int MPSZCombatId;
            public int MPSZDeadId;

            public short unknown7;
            public short unknown8;
            public short unknown9;
            public short textureId;
            public int RAVCOffset;

			public RGMRAHDItem(MemoryReader memoryReader)
            {
                try
                {
                    unknown0 = memoryReader.ReadInt32();
                    unknown1 = memoryReader.ReadInt32();

                    char[] name_char = memoryReader.ReadChars(9);
                    string[] name_strs = new string(name_char).Split('\0');
                    scriptName = name_strs[0];
                    MPOBCount = memoryReader.ReadInt32();
                    unknown2 = memoryReader.ReadInt32();

                    RAMNLength = memoryReader.ReadInt32();
                    RAMNOffset = memoryReader.ReadInt32();

                    RAATOffset = memoryReader.ReadInt32();

                    RAANCount = memoryReader.ReadInt32();
                    RAANLength = memoryReader.ReadInt32();
                    RAANOffset = memoryReader.ReadInt32();

                    RAGRMaxGroup = memoryReader.ReadInt32();
                    RAGROffset = memoryReader.ReadInt32();

                    unknown3 = memoryReader.ReadInt32();
                    unknown4 = memoryReader.ReadInt32();
                    unknown5 = memoryReader.ReadInt32();

                    RASBCount = memoryReader.ReadInt32();
                    RASBLength = memoryReader.ReadInt32();
                    RASBOffset = memoryReader.ReadInt32();

                    RASCLength = memoryReader.ReadInt32();
                    RASCOffset = memoryReader.ReadInt32();
                    RASCStartAt = memoryReader.ReadInt32();

                    RAHKLength = memoryReader.ReadInt32();
                    RAHKOffset = memoryReader.ReadInt32();

                    RALCCount = memoryReader.ReadInt32();
                    RALCLength = memoryReader.ReadInt32();
                    RALCOffset = memoryReader.ReadInt32();

                    RAEXLength = memoryReader.ReadInt32();
                    RAEXOffset = memoryReader.ReadInt32();

                    RAVACount = memoryReader.ReadInt32();
                    RAVALength = memoryReader.ReadInt32();
                    RAVAOffset = memoryReader.ReadInt32();

                    unknown6 = memoryReader.ReadInt32();
                    frameCount = memoryReader.ReadInt32();

                    MPSZNormalId = memoryReader.ReadInt32();
                    MPSZCombatId = memoryReader.ReadInt32();
                    MPSZDeadId = memoryReader.ReadInt32();

                    unknown7 = memoryReader.ReadInt16();
                    unknown8 = memoryReader.ReadInt16();
                    unknown9 = memoryReader.ReadInt16();
                    textureId = memoryReader.ReadInt16();
                    RAVCOffset = memoryReader.ReadInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAHD item with error:\n{ex.Message}");
                }
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
			public int id;           // 4 bytes 0
			public byte typeId;      // 1 byte  4
			public byte isActive;    // 1 byte  5
            public string name;      // 9 bytes 6
            public string name2;     // 9 bytes 15
            public byte isStatic;    // 1 byte  24
            public short unknown1;    // 2 byte 26
            public int  posx;        // 4 bytes 27
            public int  posy;        // 4 bytes
            public int  posz;        // 4 bytes
            public uint  anglex;     // 4 bytes
            public uint  angley;     // 4 bytes
            public uint  anglez;     // 4 bytes
            public byte textureId;
            public byte imageId;
            public short intensity;
            public short radius;
            public short modelId;
            public byte[] unknown2;  // 8 bytes
			public RGMMPOBItem(MemoryReader memoryReader)
            {
                try
                {
                    id = memoryReader.ReadInt32();
                    typeId =  memoryReader.ReadByte();
                    isActive =  memoryReader.ReadByte();

                    char[] name_char = memoryReader.ReadChars(9);
                    string[] name_strs = new string(name_char).Split('\0');
                    name = name_strs[0];
                    name_char = memoryReader.ReadChars(9);
                    name_strs = new string(name_char).Split('\0');
                    name2 = name_strs[0];

                    isStatic = memoryReader.ReadByte();
                    unknown1 = memoryReader.ReadInt16();
                    posx = memoryReader.ReadInt24();
                    memoryReader.ReadByte();
                    posy = memoryReader.ReadInt24();
                    memoryReader.ReadByte();
                    posz = memoryReader.ReadInt24();
                    anglex = memoryReader.ReadUInt32();
                    angley = memoryReader.ReadUInt32();
                    anglez = memoryReader.ReadUInt32();
                    short textureData = memoryReader.ReadInt16();
                    textureId = (byte)(textureData >> 7);
                    imageId = (byte)(textureData&127);
                    intensity = memoryReader.ReadInt16();
                    radius = memoryReader.ReadInt16();
                    modelId = memoryReader.ReadInt16();
                    unknown2 = memoryReader.ReadBytes(8);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPOB item with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"{name},{String.Join(",", unknown2)}";
			}
		}

		public struct RGMMPSOItem
		{
			public int id;           //  4 bytes
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
                    id = memoryReader.ReadInt32();
                    char[] name_char;
                    name_char = memoryReader.ReadChars(12);
                    string[] name_strs = new string(name_char).Split('\0');
                    name = name_strs[0];
                    posx = memoryReader.ReadInt24();
                    memoryReader.ReadByte(); // 4 bytes for that s24 above
                    posy = memoryReader.ReadInt24();
                    memoryReader.ReadByte();
                    posz = memoryReader.ReadInt24();
                    memoryReader.ReadByte();
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
		public struct RGMMPSZItem
		{
			public byte unknown;
            public int sizeX;
            public int sizeY;
            public int sizeZ;
            public int posX;
            public int posY;
            public int posZ;
            public int pSizeX;
            public int pSizeY;
            public int pSizeZ;
            public int mSizeX;
            public int mSizeY;
            public int mSizeZ;

			public RGMMPSZItem(MemoryReader memoryReader)
            {
                try
                {
                    unknown = memoryReader.ReadByte();
                    sizeX = memoryReader.ReadInt32();
                    sizeY = memoryReader.ReadInt32();
                    sizeZ = memoryReader.ReadInt32();
                    posX = memoryReader.ReadInt32();
                    posY = memoryReader.ReadInt32();
                    posZ = memoryReader.ReadInt32();
                    pSizeX = memoryReader.ReadInt32();
                    pSizeY = memoryReader.ReadInt32();
                    pSizeZ = memoryReader.ReadInt32();
                    mSizeX = memoryReader.ReadInt32();
                    mSizeY = memoryReader.ReadInt32();
                    mSizeZ = memoryReader.ReadInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSZ item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPSZSection
		{

            public uint num_items;
			public List<RGMMPSZItem> items;
            const int MPSZItem_size = 49; // heres that magic number again
			public RGMMPSZSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = size/MPSZItem_size;
                    items = new List<RGMMPSZItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPSZItem(memoryReader));
                    }

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSZ section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPMKItem
		{
            public int posX;
            public int posY;
            public int posZ;

			public RGMMPMKItem(MemoryReader memoryReader)
            {
                try
                {
                    posX = memoryReader.ReadInt32();
                    posY = memoryReader.ReadInt32();
                    posZ = memoryReader.ReadInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPMK item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPMKSection
		{

            public uint num_items;
			public List<RGMMPMKItem> items;
			public RGMMPMKSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();
                    items = new List<RGMMPMKItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPMKItem(memoryReader));
                    }
                    // unused flags?
                    memoryReader.ReadBytes((int)num_items);

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPMK section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPSFItem
		{
            public int id;
            public int unknown0;
            public int posX;
            public int posY;
            public int posZ;
            public short textureId;
            public short imageId;
            public short unknown1;

			public RGMMPSFItem(MemoryReader memoryReader)
            {
                try
                {
                    id = memoryReader.ReadInt32();
                    unknown0 = memoryReader.ReadInt32();

                    posX = memoryReader.ReadInt24();
                    memoryReader.ReadByte();
                    posY = memoryReader.ReadInt24();
                    memoryReader.ReadByte();
                    posZ = memoryReader.ReadInt24();
                    memoryReader.ReadByte();

                    ushort textureData = memoryReader.ReadUInt16();
                    textureId = (short)(textureData >> 7);
                    imageId = (short)(textureData&127);
                    unknown1 = memoryReader.ReadInt16();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSF item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPSFSection
		{

            public uint num_items;
			public List<RGMMPSFItem> items;
			public RGMMPSFSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();
                    items = new List<RGMMPSFItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPSFItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSF section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPSLItem
		{
            public int id;
            public int worldId;
            public int posX;
            public int posY;
            public int posZ;
            public short radius;
            public short intensity;
            public short red;
            public short green;
            public short blue;
            public byte[] unknown; // 12 bytes

			public RGMMPSLItem(MemoryReader memoryReader)
            {
                try
                {
                    id = memoryReader.ReadInt32();
                    worldId = memoryReader.ReadInt32();
                    posX = memoryReader.ReadInt32();
                    posY = memoryReader.ReadInt32();
                    posZ = memoryReader.ReadInt32();
                    radius = memoryReader.ReadInt16();
                    intensity= memoryReader.ReadInt16();
                    red = memoryReader.ReadInt16();
                    green = memoryReader.ReadInt16();
                    blue = memoryReader.ReadInt16();
                    unknown = memoryReader.ReadBytes(12);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSL item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPSLSection
		{

            public uint num_items;
			public List<RGMMPSLItem> items;
			public RGMMPSLSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();
                    items = new List<RGMMPSLItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPSLItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPSL section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPRPItem
		{
            public int id;
            public byte unknown0;
            public int posX;
            public int posY;
            public int posZ;
            public int angleY;
            public int type;
            public int swing;
            public int speed;
            public short length;
            public string staticModel;
            public string ropeModel;
            public int[] unknown1; // 28 bytes

			public RGMMPRPItem(MemoryReader memoryReader)
            {
                try
                {
                    id = memoryReader.ReadInt32();
                    unknown0 = memoryReader.ReadByte();
                    posX = memoryReader.ReadInt32();
                    posY = memoryReader.ReadInt32();
                    posZ = memoryReader.ReadInt24();
                    angleY = memoryReader.ReadInt32();
                    type = memoryReader.ReadInt32();
                    swing = memoryReader.ReadInt32();
                    speed = memoryReader.ReadInt32();
                    length = memoryReader.ReadInt16();

                    char[] name_char;
                    name_char = memoryReader.ReadChars(9);
                    string[] name_strs = new string(name_char).Split('\0');
                    staticModel = new string(name_strs[0]);

                    name_char = memoryReader.ReadChars(9);
                    name_strs = new string(name_char).Split('\0');
                    ropeModel = new string(name_strs[0]);

                    unknown1 = memoryReader.ReadInt32s(7);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPRP item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMMPRPSection
		{

            public uint num_items;
			public List<RGMMPRPItem> items;
			public RGMMPRPSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = memoryReader.ReadUInt32();
                    items = new List<RGMMPRPItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMMPRPItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM MPRP section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRALCItem
		{
            public int offsetX;
            public int offsetY;
            public int offsetZ;

			public RGMRALCItem(MemoryReader memoryReader)
            {
                try
                {
                    offsetX = memoryReader.ReadInt32();
                    offsetY = memoryReader.ReadInt32();
                    offsetZ = memoryReader.ReadInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RALC item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRALCSection
		{

            public uint num_items;
			public List<RGMRALCItem> items;
			public RGMRALCSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = size/12;
                    items = new List<RGMRALCItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMRALCItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RALC section with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRAEXItem
		{
            public short grip0;
            public short grip1;
            public short scabbard0;
            public short scabbard1;
            public short unknown0;
            public short textureId;
            public short vVertex;
            public short vSize;
            public short tauntId;
            public short unknown1;
            public short unknown2;
            public short unknown3;
            public short rangeMin;
            public short rangeIdeal;
            public short rangeMax;

			public RGMRAEXItem(MemoryReader memoryReader)
            {
                try
                {
                    grip0 = memoryReader.ReadInt16();
                    grip1 = memoryReader.ReadInt16();
                    scabbard0 = memoryReader.ReadInt16();
                    scabbard1 = memoryReader.ReadInt16();
                    unknown0 = memoryReader.ReadInt16();
                    textureId = memoryReader.ReadInt16();
                    vVertex = memoryReader.ReadInt16();
                    vSize = memoryReader.ReadInt16();
                    tauntId = memoryReader.ReadInt16();
                    unknown1 = memoryReader.ReadInt16();
                    unknown2 = memoryReader.ReadInt16();
                    unknown3 = memoryReader.ReadInt16();
                    rangeMin = memoryReader.ReadInt16();
                    rangeIdeal = memoryReader.ReadInt16();
                    rangeMax = memoryReader.ReadInt16();


                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAEX item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRAEXSection
		{

            public uint num_items;
			public List<RGMRAEXItem> items;
			public RGMRAEXSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = size/30;
                    items = new List<RGMRAEXItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMRAEXItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAEX section with error:\n{ex.Message}");
                }
            }
		}
        /*
		public struct RGMRAANItem
		{
            public int faceCount;
            public byte frameCount;
            public byte unknown0;
            public string modelFile;

			public RGMRAANItem(MemoryReader memoryReader)
            {
                try
                {
                    faceCount = memoryReader.ReadInt32();
                    frameCount = memoryReader.ReadByte();
                    unknown0 = memoryReader.ReadByte();
                    modelFile = new string("");
                    byte curc = 0x00;
                    do
                    {
                        curc = memoryReader.ReadByte();
                        modelFile += (char)curc;
                    }
                    while(curc != 0x00);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAAN item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRAANSection
		{

            public uint num_items;
			public List<RGMRAANItem> items;
			public RGMRAANSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = size/12;
                    items = new List<RGMRAANItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMRAANItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAAN section with error:\n{ex.Message}");
                }
            }
		}
        */
		public struct RGMRAVCItem
		{
            public byte offsetX;
            public byte offsetY;
            public byte offsetZ;
            public short vertex;
            public int radius;

			public RGMRAVCItem(MemoryReader memoryReader)
            {
                try
                {
                    offsetX = memoryReader.ReadByte();
                    offsetY = memoryReader.ReadByte();
                    offsetZ = memoryReader.ReadByte();
                    vertex = memoryReader.ReadInt16();
                    radius = memoryReader.ReadInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAVC item with error:\n{ex.Message}");
                }
            }
		}
		public struct RGMRAVCSection
		{

            public uint num_items;
			public List<RGMRAVCItem> items;
			public RGMRAVCSection(MemoryReader memoryReader, uint size)
            {
                try
                {
                    num_items = size/9;
                    items = new List<RGMRAVCItem>();
                    for(int i=0;i<(int)num_items;i++)
                    {
                        items.Add(new RGMRAVCItem(memoryReader));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RGM RAVC section with error:\n{ex.Message}");
                }
            }
		}




	// data
		public List<RGMSectionHeader> Sections;
        public RGMRAHDSection RAHD;
        public RGMRASTSection RAST;
        public RGMRASBSection RASB;
        public RGMRAVASection RAVA;
        public RGMRASCSection RASC;
        //public RGMRAHKSection RAHK;
        public RGMRALCSection RALC;
        public RGMRAEXSection RAEX;
        public RGMRAATSection RAAT;
        //public RGMRAANSection RAAN;
        //public RGMRAGRSection RAGR;
        //public RGMRANMSection RANM;
        public RGMRAVCSection RAVC;
        public RGMMPOBSection MPOB;
        public RGMMPRPSection MPRP;
        public RGMMPSOSection MPSO;
        public RGMMPSLSection MPSL;
        public RGMMPSFSection MPSF;
        public RGMMPMKSection MPMK;
        public RGMMPSZSection MPSZ;
        //public RGWDNMCSection WDNM;
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
                        Console.WriteLine($"AT: {memoryReader.Position:X}");
                        RAHD = new RGMRAHDSection(memoryReader, Sections[Sections.Count-1].dataLength);
                        Console.WriteLine($"END: {memoryReader.Position:X}");
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
                    else if(Sections[Sections.Count-1].sectionName == "MPSZ")
                    {
                        MPSZ = new RGMMPSZSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPMK")
                    {
                        MPMK = new RGMMPMKSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPSF")
                    {
                        MPSF = new RGMMPSFSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPSL")
                    {
                        MPSL = new RGMMPSLSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "MPRP")
                    {
                        MPRP = new RGMMPRPSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RALC")
                    {
                        RALC = new RGMRALCSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "RAEX")
                    {
                        RAEX = new RGMRAEXSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    /*
                    else if(Sections[Sections.Count-1].sectionName == "RAAN")
                    {
                        RAAN = new RGMRAANSection(memoryReader, Sections[Sections.Count-1].dataLength);
                    }
                    */
                    else if(Sections[Sections.Count-1].sectionName == "RAVC")
                    {
                        RAVC = new RGMRAVCSection(memoryReader, Sections[Sections.Count-1].dataLength);
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
