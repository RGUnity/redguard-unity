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
			RGROBFile filerob = new RGROBFile();
			filerob.LoadFile("../../game_3dfx/fxart/ISLAND.ROB");
            filerob.PrintROB();
            
		}
	}
}
