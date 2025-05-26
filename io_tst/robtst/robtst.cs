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
            RGFileImport.RG3DFile file_3d = new 3dc2obj_nw/3dc2obj2.cs();
            for(int i=0;i<filerob.hdr.NumSegments;i++)
            {
                Console.WriteLine($"SEG: {i}/{new string(filerob.segments[i].SegmentID)}");
                if(filerob.segments[i].Size > 0)
                {
                    file_3d.LoadMemory(filerob.segments[i].Data, false);

                    Mesh_data mesh = LoadMesh_3D(file_3d, $"{i}");
                    print_obj(mesh);
                 }
            }

            
		}
	}
}
