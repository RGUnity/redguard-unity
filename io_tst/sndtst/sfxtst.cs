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
		}
	}
}
