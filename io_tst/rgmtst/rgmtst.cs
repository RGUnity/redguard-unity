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
			RGRGMFile filergm = new RGRGMFile();
			filergm.LoadFile("../../game_3dfx/maps/ISLAND.RGM");
            filergm.PrintRGM();
            
		}
	}
}
