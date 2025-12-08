using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;

namespace xyz
{
    public class test
	{

		public static void Main(string[] args)
		{
			RGFileImport.RGSFXFile filesfx = new RGFileImport.RGSFXFile();
			filesfx.LoadFile("../../game_3dfx/sound/MAIN.SFX");
            Console.WriteLine($"descr: {filesfx.FXHD.description}");
            Console.WriteLine($"numSounds: {filesfx.FXHD.numSounds}");
            dmpsfx(filesfx, 19);

		}
        private static void dmpsfx(RGFileImport.RGSFXFile sfxFile, int id)
        {
            RGSFXFile.SoundEffect sfx = sfxFile.FXDT.soundEffectList[id];
            float[] PCM8 = PCM8ToFloat(sfx.PCMData, sfx.dataLength);
            for(int i=0;i<sfx.PCMData.Length;i++)
            {
                Console.WriteLine($"{id:D3}: {sfx.PCMData[i]:D5}");
                Console.WriteLine($"F{id:D3}: {PCM8[i]}");
            }

        }
        private static float[] PCM8ToFloat(byte[] datain, int dataLength)
        {
            float[] o = new float[dataLength];
            for(int i=0;i<dataLength;i++)
            {
                o[i] = (((float)datain[i])/128.0f)-1.0f;
            }
            return o;
        }
	}
}
