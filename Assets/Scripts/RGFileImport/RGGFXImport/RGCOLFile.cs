using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGCOLFile
	{
        public struct RGColor
        {
            public byte r;
            public byte g;
            public byte b;

            public RGColor(byte[] input)
            {
                r = input[0];
                g = input[1];
                b = input[2];
            }
        }
        void ReadColors(MemoryReader memoryReader)
        {
            colors = new List<RGColor>();
            try
            {
                for(int i=0;i<256;i++)
                {
                    colors.Add(new RGColor(memoryReader.ReadBytes(3)));
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load Colors with error:\n{ex.Message}");
            }
        }
        // data
        public long fileSize;
		public List<RGColor> colors;
        private bool fromFile = false; // if we load COL from memory, we skip 2x4bytes

		public void LoadFile(string filename)
        {
            try
            {
                fromFile = true;
                byte[] buffer;
                BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
                fileSize = binaryReader.BaseStream.Length;
                buffer = binaryReader.ReadBytes((int)fileSize);
                binaryReader.Close();
                LoadMemory(buffer);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load COL file {filename} with error:\n{ex.Message}");
            }
        }
		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                if(fromFile)
                {
                    memoryReader.ReadUInt32(); // filesize
                    memoryReader.ReadUInt32(); // unknown
                }
                ReadColors(memoryReader);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load COL file from memory with error:\n{ex.Message}");
            }
        }
	}
}
