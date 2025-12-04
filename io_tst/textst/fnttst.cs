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
            List<string> files = new List<string>(){
"ARIALBG.FNT",
"ARIALMD.FNT",
"ARIALSB.FNT",
"ARIALSM.FNT",
"ARIALVB.FNT",
"ARIALVS.FNT",
"FONTNORM.FNT",
"FONTNORS.FNT",
"FONTPALE.FNT",
"FONTRED.FNT",
"FONTSEL.FNT",
"FONTSELS.FNT",
"HIDONE.FNT",
"HINORMAL.FNT",
"HISELECT.FNT",
"LODONE.FNT",
"LONORMAL.FNT",
"LOSELECT.FNT",
"LOWFONT.FNT",
"REDDARK.FNT",
"REDGUARD.FNT",
"REDLOW.FNT",
"REDNORM.FNT",
"REDONE.FNT",
"REDRED.FNT",
"REDSEL.FNT",
"SREDDARK.FNT",
"SREDNORM.FNT",
"SREDSEL.FNT",
                };

            for(int i=0;i<files.Count;i++)
            {
                RGFileImport.RGFNTFile filefnt = new RGFileImport.RGFNTFile();
                filefnt.LoadFile($"../../game_3dfx/fonts/{files[i]}");
                Console.WriteLine($"tit {files[i]}:  {filefnt.FNHD.description}");
            }

/*
            Console.WriteLine($"dat:  {filefnt.BMHD.numImages}");
            Console.WriteLine($"images: {filefnt.BBMP.BBMPItems.Count}");
            for(int j=0;j<filefnt.BBMP.BBMPItems.Count;j++)
            {
                Console.WriteLine($"unknown: {filefnt.BBMP.BBMPItems[j].unknown}");
                Console.WriteLine($"width: {filefnt.BBMP.BBMPItems[j].width}");
                Console.WriteLine($"height: {filefnt.BBMP.BBMPItems[j].height}");
                for(int h=0;h<filefnt.BBMP.BBMPItems[j].unknown2.Length;h++)
                {
                    Console.WriteLine($"unknown2[{h}]: {filefnt.BBMP.BBMPItems[j].unknown2[h]}");
                }
            }

*/
		}
	}
}
