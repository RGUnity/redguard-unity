using System;
using System.Collections.Generic;
using System.IO;

namespace RGFileImport
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }


    public class tst
    {
		public static void Main(string[] args)
		{
            RGINIFile inifile = new RGINIFile();
			inifile.LoadFile("../../game_3dfx/ITEM.INI");
            Console.WriteLine($"tmo: {string.Join(",",inifile.itemData.start_item_list)}");
            //for(int i=0;i<inifile.worldData.test_map_order.Count;i++)
            //test_map_order;
		}
	}
}
