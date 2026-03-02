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

            foreach(KeyValuePair<int, RGRTXFile.RTXItem> entry in filertx.rtxItemDict)
            {
            Console.WriteLine($"{entry.Key:X8}: {entry.Value.subtitle}");
            // do something with entry.Value or entry.Key
            //    RTXEntry newRTX = new RTXEntry(entry.Value.subtitle, null);
            //    RTXDict.Add(entry.Key, newRTX);
            }

            /*
            Console.WriteLine($"descr: {filertx.FXHD.description}");
            Console.WriteLine($"numSounds: {filertx.FXHD.numSounds}");
            dmprtx(filertx, 19);
            */

		}
	}
}
