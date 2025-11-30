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
			RGFileImport.RGGXAFile filegxa = new RGFileImport.RGGXAFile();
			filegxa.LoadFile("../../game_3dfx/system/gui.gxa");

            Console.WriteLine($"dat:  {filegxa.BMHD.numImages}");
            Console.WriteLine($"images: {filegxa.BBMP.BBMPItems.Count}");
            for(int j=0;j<filegxa.BBMP.BBMPItems.Count;j++)
            {
                Console.WriteLine($"unknown: {filegxa.BBMP.BBMPItems[j].unknown}");
                Console.WriteLine($"width: {filegxa.BBMP.BBMPItems[j].width}");
                Console.WriteLine($"height: {filegxa.BBMP.BBMPItems[j].height}");
                for(int h=0;h<filegxa.BBMP.BBMPItems[j].unknown2.Length;h++)
                {
                    Console.WriteLine($"unknown2[{h}]: {filegxa.BBMP.BBMPItems[j].unknown2[h]}");
                }
            }

		}
	}
}
