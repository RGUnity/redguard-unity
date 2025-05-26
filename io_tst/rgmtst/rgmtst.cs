using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
namespace xyz
{
    public class test
	{

        public static void print_MPSO(RGRGMFile filergm)
        {
            for(int i=0;i<filergm.MPSO.num_items;i++)
            {
                string o = new string("");
                RGRGMFile.RGMMPSOItem it = filergm.MPSO.items[i];
                for(int j=0;j<it.flags.Length;j++)
                {
                    o += $"{it.flags[j]:X},";
                }
                o += $"{i}_{it.name},";
                o += $"{it.posx},";
                for(int j=0;j<it.unknown2.Length;j++)
                {
                    o += $"{it.unknown2[j]:X},";
                }
                o += $"{it.height},";
                for(int j=0;j<it.unknown3.Length;j++)
                {
                    o += $"{it.unknown3[j]:X},";
                }
                o += $"{it.posy},";
                for(int j=0;j<it.unknown4.Length;j++)
                {
                    o += $"{it.unknown4[j]:X},";
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
                    o += $"{it.flags[j]:X},";
                }
                o += $"{i}_{it.name},";
                o += $"{it.name_2},";
                for(int j=0;j<it.unknown.Length;j++)
                {
                    o += $"{it.unknown[j]:X},";
                }
                Console.WriteLine($"{o}");
            }
        }

		public static void Main(string[] args)
		{
			RGRGMFile filergm = new RGRGMFile();
			filergm.LoadFile("../../game_3dfx/maps/ISLAND.RGM");
            //filergm.PrintRGM();
            for(int i=0;i<filergm.MPSO.items.Count;i++)
            {
                Q4_28[] qs = new Q4_28[9];
                Console.WriteLine($"{i}_{filergm.MPSO.items[i].name}:");
                for(int j=0;j<filergm.MPSO.items[i].rotation_matrix.Length;j++)
                {
                    qs[j] = new Q4_28(filergm.MPSO.items[i].rotation_matrix[j]);
                    Console.WriteLine($"{(uint)filergm.MPSO.items[i].rotation_matrix[j]:X}:{qs[j]}");
                }

                float[,] rotationMatrix = new float[3, 3]
                {

                    {qs[0].ToFloat(), qs[1].ToFloat(), qs[2].ToFloat()},
                    {qs[3].ToFloat(), qs[4].ToFloat(), qs[5].ToFloat()},
                    {qs[6].ToFloat(), qs[7].ToFloat(), qs[8].ToFloat()}
                };

                RotationMatrix rm = new RotationMatrix(rotationMatrix);
                rm.Normalize();
//                Vector3 rotationAxes = rm.GetRotationAxes();
//                Console.WriteLine($"{filergm.MPSO.items[i].name},{rotationAxes.X},{rotationAxes.Y},{rotationAxes.Z}");
//                Console.WriteLine($"{filergm.MPSO.items[i].name},{String.Join(",",qs)}");

            }
            //print_MPOB(filergm);
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
