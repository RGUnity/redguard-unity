using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGRTXFile
	{
        public struct RTXAudioData
        {
            int unknown1;
            int unknown2;
            int sampleRate;
            int unknown3;
            short unknown4;
            int unknown5;
            int audioLength;
            byte unknown6;
            byte[] audioData;


			public RTXAudioData(MemoryReader memoryReader, bool shouldRead)
            {
                try
                {
                    if(shouldRead)
                    {
                        unknown1 = memoryReader.ReadInt32();
                        unknown2 = memoryReader.ReadInt32();
                        sampleRate = memoryReader.ReadInt32();
                        unknown3 = memoryReader.ReadInt32();
                        unknown4 = memoryReader.ReadInt16();
                        unknown5 = memoryReader.ReadInt32();
                        audioLength = memoryReader.ReadInt32();
                        unknown6 = memoryReader.ReadByte();
                        audioData = memoryReader.ReadBytes(audioLength);
                    }
                    else
                    {
                        unknown1 = 0;
                        unknown2 = 0;
                        sampleRate = 0;
                        unknown3 = 0;
                        unknown4 = 0;
                        unknown5 = 0;
                        audioLength = 0;
                        unknown6 = 0;
                        audioData = new byte[1];

                    }

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RTX audio data with error:\n{ex.Message}");
                }
            }
		}
		public struct RTXItem
		{
            public short hasAudio;
            public int subtitleLength;
            public string subtitle;
            public RTXAudioData audioData;

			public RTXItem(MemoryReader memoryReader)
            {
                try
                {
                    char[] text_c;
                    hasAudio = memoryReader.ReadInt16();
                    subtitleLength = memoryReader.ReadInt32();
                    text_c = memoryReader.ReadChars(subtitleLength);
                    subtitle = new string(text_c);

                    audioData = new RTXAudioData(memoryReader, (hasAudio == 256));

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load RTX item with error:\n{ex.Message}");
                }
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
