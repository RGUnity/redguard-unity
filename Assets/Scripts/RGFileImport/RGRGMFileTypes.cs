using System.Collections.Generic;

namespace RGFileImport
{
    public class RGRGMFile
    {
        public enum ObjectType
        {
            object_unknown = 0,
            object_3d = 1,
            object_audio = 2,
            object_flat = 3,
            object_lightobject = 4,
            object_sound = 5,
            object_light = 6,
        }

        public struct RGMRAHDItem
        {
            public int index;
            public int unknown0;
            public int unknown1;
            public string scriptName;
            public int MPOBCount;
            public int unknown2;
            public int RANMLength;
            public int RANMOffset;
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
            public int RASCThreadStart;
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
        }

        public struct RGMMPOBItem
        {
            public uint id;
            public ObjectType type;
            public byte isActive;
            public string scriptName;
            public string modelName;
            public byte isStatic;
            public short unknown1;
            public int posX;
            public int posY;
            public int posZ;
            public int anglex;
            public int angley;
            public int anglez;
            public byte textureId;
            public byte imageId;
            public int intensity;
            public int radius;
            public short modelId;
            public short worldId;
            public int red;
            public int green;
            public int blue;
        }

        public struct RGMRAHDSection
        {
            public uint num_items;
            public Dictionary<string, RGMRAHDItem> dict;
        }

        public struct RGMRALCItem
        {
            public int offsetX;
            public int offsetY;
            public int offsetZ;
        }

        public struct RGMRALCSection
        {
            public uint num_items;
            public List<RGMRALCItem> items;
        }

        public struct RGMRAANSection
        {
            public byte[] data;
        }

        public struct RGMRAGRSection
        {
            public byte[] data;
        }

        public struct RGMRASTSection
        {
            public char[] text;
        }

        public struct RGMRASBSection
        {
            public int[] offsets;
        }

        public struct RGMRAVASection
        {
            public int[] data;
        }

        public struct RGMRASCSection
        {
            public byte[] scripts;
        }

        public struct RGMRAATSection
        {
            public byte[] attributes;
        }

        public struct RGMRANMSection
        {
            public char[] data;
        }

        public struct RGMRAVCItem
        {
            public byte offsetX;
            public byte offsetY;
            public byte offsetZ;
            public short vertex;
            public int radius;
        }

        public struct RGMRAVCSection
        {
            public uint num_items;
            public List<RGMRAVCItem> items;
        }

        public struct RGMRAHKSection
        {
            public byte[] data;
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
        }

        public struct RGMRAEXSection
        {
            public uint num_items;
            public List<RGMRAEXItem> items;
        }

        public struct RGMMPOBSection
        {
            public uint num_items;
            public List<RGMMPOBItem> items;
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
        }

        public struct RGMMPSZSection
        {
            public uint num_items;
            public List<RGMMPSZItem> items;
        }

        public RGMRAHDSection RAHD;
        public RGMRASTSection RAST;
        public RGMRASBSection RASB;
        public RGMRAVASection RAVA;
        public RGMRASCSection RASC;
        public RGMRAHKSection RAHK;
        public RGMRALCSection RALC;
        public RGMRAEXSection RAEX;
        public RGMRAATSection RAAT;
        public RGMRAANSection RAAN;
        public RGMRAGRSection RAGR;
        public RGMRANMSection RANM;
        public RGMRAVCSection RAVC;
        public RGMMPOBSection MPOB;
        public RGMMPSZSection MPSZ;
        public long fileSize;

        public RGRGMFile()
        {
            RAHD.dict = new Dictionary<string, RGMRAHDItem>();
            RAAT.attributes = new byte[0];
            RAST.text = new char[0];
            RASB.offsets = new int[0];
            RAVA.data = new int[0];
            RASC.scripts = new byte[0];
            RAHK.data = new byte[0];
            RAEX.items = new List<RGMRAEXItem>();
            RAAN.data = new byte[0];
            RAGR.data = new byte[0];
            RANM.data = new char[0];
            RAVC.items = new List<RGMRAVCItem>();
            RALC.items = new List<RGMRALCItem>();
            MPOB.items = new List<RGMMPOBItem>();
            MPSZ.items = new List<RGMMPSZItem>();
        }
    }
}
