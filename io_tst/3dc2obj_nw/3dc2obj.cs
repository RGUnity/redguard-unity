using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
namespace xyz
{
    public class test
	{
        public static string FILETOLOAD = "HUNDING";

        static void print_FaceVertexData(FaceVertexData i)
        {
			Console.WriteLine($"VertexIndex: {i.VertexIndex}");
			Console.WriteLine($"U: {i.U}");
			Console.WriteLine($"V: {i.V}");
        }

        static void print_FaceData(FaceData i)
        {
			Console.WriteLine($"VertexCount: {i.VertexCount}");
			Console.WriteLine($"U1: {i.U1}");
			Console.WriteLine($"TextureData: {i.TextureData}");
			Console.WriteLine($"U4: {i.U4}");
            //public uint U4;
			for(int j=0;j<i.VertexData.Count;j++)
				print_FaceVertexData(i.VertexData[j]);
        }

        static void print_Coord3DInt(Coord3DInt i)
        {
			Console.WriteLine($"X Y Z: {i.x} {i.y} {i.z}");
        }

        static void print_Coord3DFloat(Coord3DFloat i)
        {
			Console.WriteLine($"X Y Z: {i.x:0.000000} {i.y:0.000000} {i.z:0.000000}");
        }

        static void print_FaceData_ob(FaceData i, int ni)
        {
			for(int j=0;j<=i.VertexData.Count-3;j++)
			{
				List<int> face_cur = new List<int>();
				int vert_ofs = 1;
				face_cur.Add((int)i.VertexData[0].VertexIndex+1);
				face_cur.Add((int)i.VertexData[vert_ofs+j].VertexIndex+1);
				face_cur.Add((int)i.VertexData[vert_ofs+j+1].VertexIndex+1);
				Console.WriteLine($"f {String.Join($"/1/{ni} ",face_cur)}/1/{ni}");
				//Console.WriteLine($"f {String.Join(" ",face_cur)}");
			}
        }

        static void print_Coord3DInt_vert_obj(Coord3DInt i)
        {
			Console.WriteLine($"v {i.x/1000.0f} {i.y/1000.0f} {i.z/1000.0f}");
        }
        static void print_Coord3DInt_norm_obj(Coord3DInt i)
        {
			Console.WriteLine($"vn {i.x} {i.y} {i.z}");
        }

		static void print_TextureId(FaceData i, int version)
		{
			if(version > 27)
			{
				if((i.TextureData >> 20) == 0x0FFF)
				{
					byte ColorIndex = (byte)(i.TextureData>>8);
					Console.WriteLine($"COLOR: {ColorIndex}");
				}
				else
				{
					uint tmp = (i.TextureData >>8)-4000000;
					uint one = (tmp/250)%40;
					uint ten = ((tmp-(one*250))/1000)%100;
					uint hundred = (tmp-(one*250)-(ten*1000))/4000;
					uint TextureId = one+ten+hundred;

					one = (i.TextureData& 0xFF)%10;
					ten = ((i.TextureData& 0xFF)/40)*10;
					uint ImageId = one+ten;
					Console.WriteLine($"TEX: {TextureId} IMG: {ImageId}");
				}
			}
			else
			{
				uint TextureId = (i.TextureData >> 7);
				if(TextureId < 2)
				{
					byte ColorIndex = (byte)(i.TextureData);
					Console.WriteLine($"COLOR: {ColorIndex}");
				}
				else
				{
					byte ImageId = (byte)(i.TextureData & 0x7f);
					Console.WriteLine($"TEX: {TextureId} IMG: {ImageId}");
				}
			}
		}


		 public static void Main(string[] args)
		{
            RGROBFile filerob = new RGROBFile();
            filerob.LoadFile("../../game_3dfx/fxart/ISLAND.ROB");
            RGFileImport.RG3DFile file3d = new RGFileImport.RG3DFile();
            for(int i=0;i<filerob.hdr.NumSegments;i++)
            {
                if(filerob.segments[i].SegmentID == FILETOLOAD)
                {
                    Console.WriteLine(filerob.segments[i].SegmentID);
                    file3d.LoadMemory(filerob.segments[i].Data, false);
                }
            }

			for(int i=0;i<file3d.FaceDataCollection.Count;i++)
			{
//				print_TextureId(file3d.FaceDataCollection[i], file3d.version);
				Console.WriteLine(file3d.FaceDataCollection[i].TextureData & 0xFFFFFF00);
			}
/*
			for(int i=0;i<file3d.FaceDataCollection.Count;i++)
			{
				for(int j=0;j<file3d.FaceDataCollection[i].VertexCount;j++)
				{
					uint vert = file3d.FaceDataCollection[i].VertexData[j].VertexIndex;
					if(vert == 6)
					{
						float UV_TRANSFORM_FACTOR = 4096.0f;
						float u = (UV_TRANSFORM_FACTOR-file3d.FaceDataCollection[i].VertexData[j].U)/UV_TRANSFORM_FACTOR;
						float v = (UV_TRANSFORM_FACTOR-file3d.FaceDataCollection[i].VertexData[j].V)/UV_TRANSFORM_FACTOR;
						Console.WriteLine($"f uv: {u} {v}");

						u = file3d.UvCoordinates[i].x;
						v = file3d.UvCoordinates[i].y;
						float w  = file3d.UvCoordinates[i].z;
						Console.WriteLine($"v uvw: {u} {v} {w}");
				print_TextureId(file3d.FaceDataCollection[i], file3d.version);
					}
				}
			}
			Console.WriteLine($"o FACE: {file3d.FaceDataCollection.Count} UV: {file3d.UvCoordinates.Count} VERT: {file3d.VertexCoordinates.Count}");
*/
			/*
			Console.WriteLine($"o CYR");
			for(int i=0;i<file3d.VertexCoordinates.Count;i++)
				print_Coord3DInt_vert_obj(file3d.VertexCoordinates[i]);
			for(int i=0;i<file3d.FaceNormals.Count;i++)
				print_Coord3DInt_norm_obj(file3d.FaceNormals[i]);

			Console.WriteLine($"vt 1.0 0.0 0.0");
			Console.WriteLine($"s 1");

			for(int i=0;i<file3d.FaceDataCollection.Count;i++)
			{
				print_FaceData_ob(file3d.FaceDataCollection[i], i+1);
			}


			Console.WriteLine($"#{file3d.FaceNormals.Count},{file3d.FaceDataCollection.Count}");
			*/
			/*
			for(int i=0;i<file3d.FaceDataCollection.Count;i++)
			{
				for(int j=0;j<file3d.FaceDataCollection[i].VertexData.Count;j++)
				{
					Console.WriteLine($"{file3d.FaceDataCollection[i].VertexData[j].VertexIndex}");
				}
			}
			*/
				/*
			for(int i=0;i<file3d.FaceDataCollection.Count;i++)
				print_FaceData(file3d.FaceDataCollection[i]);
			for(int i=0;i<file3d.VertexCoordinates.Count;i++)
				print_Coord3DInt(file3d.VertexCoordinates[i]);
			for(int i=0;i<file3d.FaceNormals.Count;i++)
				print_Coord3DInt(file3d.FaceNormals[i]);
			for(int i=0;i<file3d.UvCoordinates.Count;i++)
				print_Coord3DFloat(file3d.UvCoordinates[i]);
				*/
/*
        public List<FaceData> FaceDataCollection { get; set; }
        public List<Coord3DInt> VertexCoordinates { get; set; }
        public List<Coord3DInt> FaceNormals { get; set; }
        public List<uint> UvOffsets { get; set; }
        public List<Coord3DFloat> UvCoordinates { get; set; }
*/
		}
	}
}
