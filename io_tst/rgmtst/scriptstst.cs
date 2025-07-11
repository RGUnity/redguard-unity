using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
namespace xyz
{
    public class test
	{
        public static int TASK_NIMPL(int[] i)
        {
            Console.WriteLine($"TASK_NIMP({string.Join(",",i)})");
            return 1;
        }

        public static int PushAnimation(int[] i)
        {
            Console.WriteLine($"PushAnimation({i[0]}, {i[1]})");
            return 1;
        }
        public static int SyncWithGroup(int[] i)
        {
            Console.WriteLine($"SyncWithGroup({i[0]})");
            return 1;
        }
		public static void Main(string[] args)
		{
			RGRGMFile filergm = new RGRGMFile();
			filergm.LoadFile("../../game_3dfx/maps/ISLAND.RGM");
            RGRGMScriptStore.ReadScript(filergm);
/*
0x000E: TASK_136(1, 25) pushanimation
0x0017: TASK_271(1) syncwithGroup
*/
            int sdcnt = 400;
            Func<int[], int>[] soupdeffcn = new Func<int[], int>[sdcnt];
            for(int i=0;i<sdcnt;i++)
                soupdeffcn[i] = TASK_NIMPL;
            soupdeffcn[136] = PushAnimation;
            soupdeffcn[271] = SyncWithGroup;


            
            ScriptData sd = new ScriptData("IHEALTH", soupdeffcn);
            int runs      = 1;
            for(int i=0;i<runs;i++)
                sd.runScript();

/*
            int cnt = RGRGMScriptStore.scripts.Count;
            int runs      = 1;
            for(int j=0;j<cnt;j++)
            {
                RGRGMScriptStore.scripts[j].TST_IFRET = true;
                RGRGMScriptStore.scripts[j].TST_IFFLIP = true;
                for(int i=0;i<runs;i++)
                    RGRGMScriptStore.scripts[j].runScript();
            }
*/

        }
	}
}
