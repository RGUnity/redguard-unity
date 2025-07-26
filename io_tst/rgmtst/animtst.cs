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
            RGRGMAnimStore.ReadAnim(filergm);
            /*
            foreach(var entry in RGRGMAnimStore.Anims)
                Console.WriteLine(entry.Key);
            */
            Console.WriteLine(RGRGMAnimStore.Anims["CYRUS"]);
/*
            int cnt = RGRGMAnimStore.Anims.Count;
            for(int j=22;j<23;j++)
            {
                Console.WriteLine("######################################");
                Console.WriteLine($"############### {j}::{filergm.RAHD.items[j].scriptName} ###############");
                Console.WriteLine(RGRGMAnimStore.Anims[j]);
            }
*/
            /*
            RGRGMAnimStore.Anims["FLAG"].PushAnimation(RGRGMAnimStore.AnimGroup.anim_walk_forward,0);
            for(int i=0;i<50;i++)
            {
                Console.WriteLine(RGRGMAnimStore.Anims["GUARD01"].NextFrame());
            }
            */

        }
	}
}
