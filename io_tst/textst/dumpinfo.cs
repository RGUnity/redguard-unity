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
"cloud512.gxa",
"grey256.gxa",
"gui.gxa",
"necrosky.gxa",
"nightsky.gxa",
"paper.gxa",
"pickblob.gxa",
"pickbox.gxa",
"rgmap.gxa",
"sky61.gxa",
"snuff.gxa",
"square.gxa",
"CATACOMB.GXA",
"CAVERNS.GXA",
"COMPASS2.GXA",
"CROSS.GXA",
"DRINT.GXA",
"DUSK.GXA",
"GXICONS.GXA",
"INVARROW.GXA",
"INVBACK.GXA",
"INVBORDR.GXA",
"INVMASK.GXA",
"INVSEL.GXA",
"ISLAND1.GXA",
"ISLAND.GXA",
"LOGPAPER.GXA",
"MAPMAP.GXA",
"MINIMENU.GXA",
"MIRROR1.GXA",
"MIRROR2.GXA",
"MIRROR3.GXA",
"MM_CHECK.GXA",
"MM_MOVIE.GXA",
"MM_SCRSZ.GXA",
"MM_SLIDE.GXA",
"NECRISLE.GXA",
"NECRTOWR.GXA",
"OBSERVE.GXA",
"PICKUPS.GXA",
"PICKUPSS.GXA",
"POWERUP.GXA",
"SCROLL.GXA",
"STARTUP2.GXA",
"STMAP01.GXA",
"STMAP02.GXA",
"STMAP03.GXA",
"STMAP04.GXA",
"STMAP05.GXA",
"STMAP06.GXA",
"STMAP07.GXA",
"STMAP08.GXA",
"STMAP09.GXA",
"STMAP10.GXA",
"STMAP11.GXA",
"STMAP12.GXA",
"STMAP13.GXA",
"STMAP14.GXA",
"STMAP15.GXA",
"STMAP16.GXA",
"STMAP17.GXA",
"SUNSET.GXA",
"TAVERN.GXA",
"TEMPLE.GXA",
"VIEWBACK.GXA",
            };
            
            for(int i=0;i<files.Count;i++)
            {
                RGFileImport.RGGXAFile filegxa = new RGFileImport.RGGXAFile();
                try
                {
                    filegxa.LoadFile($"../../game_3dfx/system/{files[i]}");
                    Console.WriteLine($"{files[i]}");
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
                catch(Exception ex)
                {
                    Console.WriteLine($"BROKE: {files[i]}");
                }
            }
        }
	}
}
