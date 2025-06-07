using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGTEXBSIFile
	{
		public struct ImageSection
		{
			public string imageName;
            public int imageSize;
            public RGBSIFile imageData;

			public ImageSection(MemoryReader memoryReader)
            {
                try
                {
                    imageData = new RGBSIFile();

                    char[] ImageName_char;
                    ImageName_char = memoryReader.ReadChars(9);
                    string[] name_strs = new string(ImageName_char).Split('\0');
                    imageName = name_strs[0];
                    if(imageName.Length == 0) // END basically
                    {
                        imageSize = 0;
                        // no actual image
                    }
                    else
                    {
                        imageSize = memoryReader.ReadInt32();
                        imageData.LoadMemory(memoryReader.ReadBytes(imageSize));
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load ImageSection with error:\n{ex.Message}");
                }
            }
		}

        // data
        public long fileSize;
		public List<ImageSection> images;

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
                throw new Exception($"Failed to load TEXBSI file {filename} with error:\n{ex.Message}");
            }
        }
		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                images = new List<ImageSection>();
                bool end = false;
                while(!end)
                {
                    ImageSection cur = new ImageSection(memoryReader);
                    if(cur.imageName.Length == 0)
                    {
                        end = true;
                    }
                    else
                    {
                        images.Add(cur);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load TEXBSI file from memory with error:\n{ex.Message}");
            }
        }
	}
}
