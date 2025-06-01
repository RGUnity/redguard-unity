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
            RGRGMScriptStore.ReadScript(filergm);

            int cnt = RGRGMScriptStore.scripts.Count;
            int runs      = 1;
            for(int j=0;j<cnt;j++)
            {
                RGRGMScriptStore.scripts[j].TST_IFRET = true;
                RGRGMScriptStore.scripts[j].TST_IFFLIP = true;
                for(int i=0;i<runs;i++)
                    RGRGMScriptStore.scripts[j].runScript();
            }

        }
	}
}
