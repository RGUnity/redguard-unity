using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
namespace xyz
{
    public class test
	{
/*
        public static void print_MPSO(RGRGMFile filergm)
        {
            for(int i=0;i<filergm.MPSO.num_items;i++)
            {
                string o = new string("");
                RGRGMFile.RGMMPSOItem it = filergm.MPSO.items[i];
                for(int j=0;j<it.flags.Length;j++)
                {
                    o += $"{it.flags[j]:X2},";
                }
                o += $"{i}_{it.name},";
                o += $"{it.posx:X8},";
                o += $"{it.posy:X8},";
                o += $"{it.posz:X8},";
                for(int j=0;j<it.unknown.Length;j++)
                {
                    o += $"{it.unknown[j]:X2},";
                }
                Console.WriteLine($"{o}");
            }
        }
        public static void print_MPOB(RGRGMFile filergm)
        {
            for(int i=0;i<filergm.MPOB.num_items;i++)
            {
                string o = new string("");
                RGRGMFile.RGMMPOBItem it = filergm.MPOB.items[i];
                for(int j=0;j<it.flags.Length;j++)
                {
                    o += $"{it.flags[j]:X2},";
                }
                string name_o = new String(it.name);
                while(name_o.Length < 10)
                    name_o += "_";
                string name_o2 = new String(it.name2);
                while(name_o2.Length < 10)
                    name_o2 += "_";
                o += $"{i:D4}_{name_o},";
                o += $"{name_o2},";
                o += $"{it.unknown1:X2},";
                o += $"{it.posx:X2},";
                o += $"{it.posy:X2},";
                o += $"{it.posz:X2},";
                o += $"{it.anglex:X2},";
                o += $"{it.angley:X2},";
                o += $"{it.anglez:X2},";
                for(int j=0;j<it.unknown2.Length;j++)
                {
                    o += $"{it.unknown2[j]:X2},";
                }
                Console.WriteLine($"{o}");
            }
        }

*/
		public static void Main(string[] args)
		{
			RGRGMFile filergm = new RGRGMFile();
			filergm.LoadFile("../../game_3dfx/maps/TEMPLE.RGM");

            /*
            foreach(var entry in filergm.RAHD.dict)
            {
                Console.WriteLine($"{entry.Value.scriptName}: {entry.Value.RANMLength}");
                MemoryReader RANMReader = new MemoryReader(filergm.RANM.data);
                RANMReader.Seek((uint)entry.Value.RANMOffset, 0);

                char[] curc = RANMReader.ReadChars(entry.Value.RANMLength -1);
                string modelname = new string(curc);

                Console.WriteLine($"NAM: {modelname}");
 
            }
            */
            /*
            foreach(var entry in filergm.RAHD.dict)
            {
                Console.WriteLine($"{entry.Value.scriptName}: {entry.Value.RALCCount}/{entry.Value.RALCLength}/{entry.Value.RALCOffset}");
                int RALC_LOC_CNT = entry.Value.RALCLength/12;
                int RALC_LOC_OFS = entry.Value.RALCOffset/12;
                for(int i=0;i<entry.Value.RALCCount;i++)
                {
                    RGRGMFile.RGMRALCItem it = filergm.RALC.items[RALC_LOC_OFS+i];
                    Console.WriteLine($"LOC {i}: {it.offsetX},{it.offsetY},{it.offsetZ}");
                }
            }
            */
            for(int i=0;i<filergm.MPOB.num_items;i++)
            {
                RGRGMFile.RGMMPOBItem it = filergm.MPOB.items[i];
                //Console.WriteLine($"//\t\tModelLoader.scriptedObjects[0x{it.id:X}].allowScripting = true; // {it.scriptName}");
                Console.WriteLine($"{it.id:X}; {it.scriptName}");
 
            }
            /*
            MemoryReader memoryReader = new MemoryReader(filergm.RAAN.data);
            foreach(var entry in filergm.RAHD.dict)
            {
                Console.WriteLine($"{entry.Value.scriptName}:");
                memoryReader.Seek((uint)entry.Value.RAANOffset, 0);

                char[] curc = memoryReader.ReadChars(entry.Value.RAANLength);
                string modelname = new string(curc);

                Console.WriteLine($"NAM: {modelname}");
            }
            */
//            Console.WriteLine(filergm.RAHD);
            //filergm.PrintRGM();
//            print_MPOB(filergm);
//            print_MPSO(filergm);
//Q4_28 q1 = Q4_28.FromFloat(0.5f);
/*
float[,] rotationMatrix = new float[3, 3]
{
    {0.8660254037844387f, -0.5f, 0f},
    {0.5f, 0.8660254037844387f, 0f},
    {0f, 0f, 1f}
};

RotationMatrix rm = new RotationMatrix(rotationMatrix);
Vector3 rotationAxes = rm.GetRotationAxes();

Console.WriteLine("X: " + rotationAxes.X);
Console.WriteLine("Y: " + rotationAxes.Y);
Console.WriteLine("Z: " + rotationAxes.Z);

                   
*/
		}
	}
}
