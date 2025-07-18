using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static int MoveByAxis(int[] i)
        {
            Console.WriteLine($"MoveByAxis({i[0]}, {i[1]}, {i[2]})");
            return 1;
        }
        public static int Wait(int[] i)
        {
            Console.WriteLine($"Wait({i[0]})");
            return 1;
        }

		public static void Main(string[] args)
		{
			RGRGMFile filergm = new RGRGMFile();
/*
			filergm.LoadFile("../../game_3dfx/maps/TAVERN.RGM");
            RGRGMScriptStore.ReadScript(filergm);
            for(int i=0;i<RGRGMScriptStore.Scripts.Count;i++)
            {
                RGRGMScriptStore.RGMScript cs = RGRGMScriptStore.Scripts.ElementAt(i).Value;
                string o = new string($"{cs.scriptName}: {cs.scriptStrings.Count}:");
                o += $"{cs.objectName}";
                o+= "\\n";
                for(int j=0;j<cs.scriptStrings.Count;j++)
                    o+= $"{cs.scriptStrings[j]},";
                Console.WriteLine(o);
            }
            return;
*/
/*
0x000E: TASK_136(1, 25) pushanimation
0x0017: TASK_271(1) syncwithGroup
*/
            int sdcnt = 400;
            Func<int[], int>[] soupdeffcn = new Func<int[], int>[sdcnt];
            for(int i=0;i<sdcnt;i++)
                soupdeffcn[i] = TASK_NIMPL;
            soupdeffcn[53] = MoveByAxis;
            soupdeffcn[60] = Wait;
            soupdeffcn[136] = PushAnimation;
            soupdeffcn[271] = SyncWithGroup;


			filergm.LoadFile("../../game_3dfx/maps/OBSERVE.RGM");
            RGRGMScriptStore.ReadScript(filergm);
            
            ScriptData sd = new ScriptData("OB_ENG11", soupdeffcn);

            int ticks = 8;
            for(int i=0;i<ticks;i++)
            {
                Console.Write($"{i}:");
                sd.tickScript();
            }

/*
            Console.WriteLine($"{filergm.RAHD.dict.Count}");
            for(int i=0;i<109;i++)
            {
                RGRGMFile.RGMRAHDItem item = filergm.RAHD.dict.ElementAt(i).Value;
                Console.WriteLine($"{i}:{item.scriptName}");
            }
*/
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
