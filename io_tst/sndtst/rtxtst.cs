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
			RGFileImport.RGRTXFile filertx = new RGFileImport.RGRTXFile();
			filertx.LoadFile("../../game_3dfx/ENGLISH.RTX");
            /*
            Console.WriteLine($"descr: {filertx.FXHD.description}");
            Console.WriteLine($"numSounds: {filertx.FXHD.numSounds}");
            dmprtx(filertx, 19);
            */

		}
	}
}
