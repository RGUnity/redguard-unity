using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGSFXFile
	{
        public struct SFXSectionHeader
		{
			public string sectionName;
            public uint dataLength;

			public SFXSectionHeader(MemoryReader memoryReader)
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
                    throw new Exception($"Failed to load SFX section header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
SFXHeader
###################################
name: {sectionName}
size: {dataLength:X}
###################################";
			}
		}
		public struct FXHDSection
		{
            public string description;
            public int numSounds;
			public FXHDSection(MemoryReader memoryReader)
            {
                try
                {
                    char[] tchar = memoryReader.ReadChars(32);
                    description = new string(tchar);
                    numSounds = memoryReader.ReadInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load SFX FXHD section with error:\n{ex.Message}");
                }
            }
		}
        public enum AudioType
        {
            audiotype_mono8       = 0,
            audiotype_mono16      = 1,
            audiotype_stereo8     = 2,
            audiotype_stereo16    = 3,
        }
        public enum AudioBitDepth
        {
            audiodepth_8    = 0,
            audiodepth_16   = 1,
        }

        public struct SoundEffect
        {
            public AudioType typeId;
            public AudioBitDepth bitDepth;
            public int sampleRate;
            public char unknown1;
            public char loopFlag;
            public int loopOffset;
            public int loopEnd;
            public int dataLength;
            public char unknown2;
            public byte[] PCMData;
            public SoundEffect(MemoryReader memoryReader)
            {
                try
                {
                    typeId = (AudioType)memoryReader.ReadInt32();
                    bitDepth = (AudioBitDepth)memoryReader.ReadInt32();
                    sampleRate = memoryReader.ReadInt32();
                    unknown1 = memoryReader.ReadChar();
                    loopFlag = memoryReader.ReadChar();
                    loopOffset = memoryReader.ReadInt32();
                    loopEnd = memoryReader.ReadInt32();
                    dataLength = memoryReader.ReadInt32();
                    unknown2 = memoryReader.ReadChar();
                    PCMData = memoryReader.ReadBytes(dataLength*1);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load SFX sound effect with error:\n{ex.Message}");
                }
            }

        }
		public struct FXDTSection
		{
            public List<SoundEffect> soundEffectList;

			public FXDTSection(MemoryReader memoryReader, FXHDSection FXHD)
            {
                try
                {
                    soundEffectList = new List<SoundEffect>();
                    for(int i=0;i<FXHD.numSounds;i++)
                    {
                        soundEffectList.Add(new SoundEffect(memoryReader));
                    }

                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load SFX FXDT section with error:\n{ex.Message}");
                }
            }
		}
        // data
        public long fileSize;
		public List<SFXSectionHeader> Sections;
        public FXHDSection FXHD;
		public FXDTSection FXDT;

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
                throw new Exception($"Failed to load SFX file {filename} with error:\n{ex.Message}");
            }
        }

		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                Sections = new List<SFXSectionHeader>();
                bool end = false;
                while(!end)
                {
                    Sections.Add(new SFXSectionHeader(memoryReader));
                    if(Sections[Sections.Count-1].sectionName == "END ")
                    {
                        end = true;
                    }
                    else if(Sections[Sections.Count-1].sectionName == "FXHD")
                    {
                        FXHD = new FXHDSection(memoryReader);
                    }
                    else if(Sections[Sections.Count-1].sectionName == "FXDT")
                    {
                        FXDT = new FXDTSection(memoryReader, FXHD);
                    }
                    else
                    {
                        memoryReader.Seek(Sections[Sections.Count-1].dataLength, (uint)memoryReader.Position);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load SFX file from memory with error:\n{ex.Message}");
            }
        }
	}
}
