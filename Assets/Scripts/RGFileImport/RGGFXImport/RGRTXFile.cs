using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGRTXFile
	{
		public struct RTXItem
		{
            public short hasAudio;
            public int subtitleLength;
            public string subtitle;
            public RGSoundEffect audioData;

			public RTXItem(MemoryReader memoryReader)
            {
                try
                {
                    char[] text_c;
                    hasAudio = memoryReader.ReadInt16();
                    subtitleLength = memoryReader.ReadInt32();
                    text_c = memoryReader.ReadChars(subtitleLength);
                    subtitle = new string(text_c);
                    audioData = new RGSoundEffect(memoryReader, (hasAudio == 256));
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RTX item with error:\n{ex.Message}");
                }
            }
            public bool hasAudioData()
            {
                return (hasAudio == 256);
            }
		}

        // data
        public long fileSize;
		public Dictionary<int, RTXItem> rtxItemDict;

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
                throw new Exception($"Failed to load RTX file {filename} with error:\n{ex.Message}");
            }
        }

		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                rtxItemDict = new Dictionary<int, RTXItem>();
                bool end = false;
                while(!end)
                {
                    // we read the label as an int so we can skip the conversion to string
                    int label = memoryReader.ReadInt32();
                    int size = memoryReader.ReadInt32();
                    if(label == 0x20444E45) // "END " with the space as usual
                        end = true;
                    else
                    {
                        rtxItemDict.Add(label, new RTXItem(memoryReader));
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load RTX file from memory with error:\n{ex.Message}");
            }
        }
	}
}
