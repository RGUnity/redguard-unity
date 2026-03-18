using UnityEngine;
using System;

namespace RGFileImport
{
    // Audio is common between SFX and RTX files
    public enum RGAudioType
    {
        audiotype_mono8       = 0,
        audiotype_mono16      = 1,
        audiotype_stereo8     = 2,
        audiotype_stereo16    = 3,
    }
    public enum RGAudioBitDepth
    {
        audiodepth_8    = 0,
        audiodepth_16   = 1,
    }
    public struct RGSoundEffect
    {
        public RGAudioType typeId;
        public RGAudioBitDepth bitDepth;
        public int sampleRate;
        public char unknown1;
        public char loopFlag;
        public int loopOffset;
        public int loopEnd;
        public int dataLength;
        public char unknown2;
        public byte[] PCMData;
        public RGSoundEffect(MemoryReader memoryReader, bool shouldRead=true)
        {
            if(!shouldRead)
            {
                typeId = 0;
                bitDepth = 0;
                sampleRate = 0;
                unknown1 = (char)0;
                loopFlag = (char)0;
                loopOffset = 0;
                loopEnd = 0;
                dataLength = 0;
                unknown2 = (char)0;
                PCMData = new byte[1];
            }
            else
            {
                try
                {
                    typeId = (RGAudioType)memoryReader.ReadInt32();
                    bitDepth = (RGAudioBitDepth)memoryReader.ReadInt32();
                    sampleRate = memoryReader.ReadInt32();
                    unknown1 = memoryReader.ReadChar();
                    loopFlag = memoryReader.ReadChar();
                    loopOffset = memoryReader.ReadInt32();
                    loopEnd = memoryReader.ReadInt32();
                    dataLength = memoryReader.ReadInt32();
                    unknown2 = memoryReader.ReadChar();

                    PCMData = memoryReader.ReadBytes(dataLength);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load SFX sound effect with error:\n{ex.Message}");
                }
            }
        }

    }
}
