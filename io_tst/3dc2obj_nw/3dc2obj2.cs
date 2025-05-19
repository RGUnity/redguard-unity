using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
namespace xyz
{
    public class test
	{
	public class Vector2
	{
		public float x;
		public float y;
		public Vector2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public class Vector3
	{
		public float x;
		public float y;
		public float z;
		public Vector3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

    public struct Face_3DC
    {
        public int vert_cnt;
        public List<Vector3> verts;
        public List<Vector2> uvs;
        public Vector3 norm;
        public int texid;
    }


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
    private static void LoadMesh_3D(string filename_3d)
    {
float[] aspect_ratios = {
49.0f/26.0f,
42.0f/35.0f,
8.0f/8.0f,
8.0f/16.0f,
16.0f/17.0f,
9.0f/16.0f,
23.0f/90.0f,
64.0f/92.0f,
37.0f/67.0f,
49.0f/26.0f,
14.0f/28.0f,
64.0f/92.0f,
32.0f/32.0f,
32.0f/32.0f,
32.0f/32.0f,
32.0f/32.0f,
64.0f/16.0f,
64.0f/32.0f,
64.0f/16.0f,
64.0f/32.0f,
32.0f/16.0f,
32.0f/32.0f,
32.0f/16.0f,
32.0f/32.0f,
16.0f/64.0f,
16.0f/64.0f,
16.0f/32.0f,
128.0f/8.0f,
128.0f/4.0f,
16.0f/8.0f,
16.0f/32.0f,
16.0f/16.0f,
16.0f/8.0f,
16.0f/32.0f,
16.0f/16.0f,
16.0f/32.0f,
16.0f/16.0f,
16.0f/32.0f,
16.0f/16.0f,
8.0f/32.0f,
16.0f/16.0f,
8.0f/32.0f,
16.0f/16.0f,
16.0f/32.0f,
16.0f/16.0f,
16.0f/32.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
32.0f/32.0f,
32.0f/32.0f,
32.0f/8.0f,
16.0f/32.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f,
16.0f/16.0f };

        const int texid_cnt_base = 64;
        const int texid_cnt = texid_cnt_base+2;
        RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
        file_3d.LoadFile(filename_3d);

// 1st pass: load verts/normals/faces
        List<Vector3> vec_tmp_lst = new List<Vector3>();
        List<int> tri_tmp_lst = new List<int>();
        List<Vector3> norm_tmp_lst = new List<Vector3>();
        List<Vector2> uv_tmp_lst = new List<Vector2>();
        for(int i=0;i<file_3d.VertexCoordinates.Count;i++)
        {
            // big scale down so it fits
            vec_tmp_lst.Add(new Vector3(file_3d.VertexCoordinates[i].x/500.0f,
                                    file_3d.VertexCoordinates[i].y/500.0f,
                                    file_3d.VertexCoordinates[i].z/500.0f));
        }
        for(int i=0;i<file_3d.FaceNormals.Count;i++)
        {
            norm_tmp_lst.Add(new Vector3(file_3d.FaceNormals[i].x,
                                     file_3d.FaceNormals[i].y,
                                     file_3d.FaceNormals[i].z));
        }
        List<Face_3DC> face_lst = new List<Face_3DC>();
        for(int i=0;i<file_3d.FaceDataCollection.Count;i++)
        {
            Face_3DC cur_face = new Face_3DC();
            cur_face.vert_cnt = file_3d.FaceDataCollection[i].VertexData.Count;
            cur_face.verts = new List<Vector3>();
            cur_face.uvs = new List<Vector2>();
            cur_face.norm = norm_tmp_lst[i];
            // TODO: how to deal with solid colors?
            if(file_3d.FaceDataCollection[i].solid_color)
                cur_face.texid = texid_cnt_base + 1;
            else
                cur_face.texid = (int)file_3d.FaceDataCollection[i].ImageId;

            for(int j=0;j<file_3d.FaceDataCollection[i].VertexData.Count;j++)
            {
                cur_face.verts.Add(vec_tmp_lst[(int)file_3d.FaceDataCollection[i].VertexData[j].VertexIndex]);
                cur_face.uvs.Add(new Vector2(
                                file_3d.FaceDataCollection[i].VertexData[j].U,
                                file_3d.FaceDataCollection[i].VertexData[j].V
								));
				if(cur_face.texid == 0)
				{
					Console.WriteLine($"# UV OG: {file_3d.FaceDataCollection[i].VertexData[j].U} {file_3d.FaceDataCollection[i].VertexData[j].V}");
				}
            }
            face_lst.Add(cur_face);
        }
// 2nd pass: sort faces by texture id and split verts/norms/uvs
        List<Vector3> vec_lst = new List<Vector3>();
        List<Vector3> norm_lst = new List<Vector3>();
        List<Vector2> uv_lst = new List<Vector2>();
        List<int>[] tri_lst = new List<int>[texid_cnt];
        for(int i=0;i<texid_cnt;i++)
            tri_lst[i] = new List<int>();

        float[] uv_transforms = new float[texid_cnt];
        float[] uv_transforms_y = new float[texid_cnt];
        for(int i=0;i<texid_cnt;i++)
		{
			uv_transforms[i] = 0.0f;
			uv_transforms_y[i] = 0.0f;
		}
        for(int i=0;i<face_lst.Count;i++)
        {
			for(int j=0;j<face_lst[i].uvs.Count;j++)
			{
				if(face_lst[i].uvs[j].y > uv_transforms_y[face_lst[i].texid])
					uv_transforms_y[face_lst[i].texid] = face_lst[i].uvs[j].y;

				if(face_lst[i].uvs[j].x > uv_transforms[face_lst[i].texid])
					uv_transforms[face_lst[i].texid] = face_lst[i].uvs[j].x;
			}
			
		}

        int tri_cnt = 0;
        for(int i=0;i<face_lst.Count;i++)
        {
            for(int j=0;j<=face_lst[i].vert_cnt-3;j++)
            {
                int vert_ofs = 1;
                vec_lst.Add(face_lst[i].verts[0]);
                vec_lst.Add(face_lst[i].verts[vert_ofs+j]);
                vec_lst.Add(face_lst[i].verts[vert_ofs+j+1]);

                norm_lst.Add(face_lst[i].norm);
                norm_lst.Add(face_lst[i].norm);
                norm_lst.Add(face_lst[i].norm);

                float UV_TRANSFORM_FACTOR_X = uv_transforms[face_lst[i].texid];
                float UV_TRANSFORM_FACTOR_Y = uv_transforms_y[face_lst[i].texid];

                uv_lst.Add(new Vector2(
                                ((face_lst[i].uvs[0].x)/(UV_TRANSFORM_FACTOR_X)),
                                ((UV_TRANSFORM_FACTOR_Y)-face_lst[i].uvs[0].y)/(UV_TRANSFORM_FACTOR_Y)
								));
                uv_lst.Add(new Vector2(
                                ((face_lst[i].uvs[vert_ofs+j].x)/(UV_TRANSFORM_FACTOR_X)),
                                ((UV_TRANSFORM_FACTOR_Y)-face_lst[i].uvs[vert_ofs+j].y)/(UV_TRANSFORM_FACTOR_Y)
								));
                uv_lst.Add(new Vector2(
                                ((face_lst[i].uvs[vert_ofs+j+1].x)/(UV_TRANSFORM_FACTOR_X)),
                                ((UV_TRANSFORM_FACTOR_Y)-face_lst[i].uvs[vert_ofs+j+1].y)/(UV_TRANSFORM_FACTOR_Y)
								));


                tri_lst[face_lst[i].texid].Add(tri_cnt*3);
                tri_lst[face_lst[i].texid].Add(tri_cnt*3+1);
                tri_lst[face_lst[i].texid].Add(tri_cnt*3+2);
                tri_cnt++;
            }
        }
		Console.WriteLine("# 3DC file");
        for(int i=0;i<vec_lst.Count;i++)
		{
			Console.WriteLine($"v {vec_lst[i].x} {vec_lst[i].y} {vec_lst[i].z}");
		}
        for(int i=0;i<uv_lst.Count;i++)
		{
			Console.WriteLine($"vt {uv_lst[i].x} {uv_lst[i].y}");
		}
        for(int i=0;i<norm_lst.Count;i++)
		{
			Console.WriteLine($"vn {norm_lst[i].x} {norm_lst[i].y} {norm_lst[i].z}");
		}
        for(int i=0;i<texid_cnt;i++)
		{
			Console.WriteLine($"g {i}");
			for(int j=0;j<tri_lst[i].Count;j+=3)
			{
				int h1 = tri_lst[i][j]+1;
				int h2 = tri_lst[i][j+1]+1;
				int h3 = tri_lst[i][j+2]+1;
				Console.WriteLine($"f {h1}/{h1}/{h1} {h2}/{h2}/{h2} {h3}/{h3}/{h3} ");
			}
		}


/*
        mesh.subMeshCount = texid_cnt;
        mesh.vertices = vec_lst.ToArray();
        mesh.uv = uv_lst.ToArray();
        mesh.normals = norm_lst.ToArray();
        
        // TODO: clean up unused submeshes and deal with solid colors
        for(int i=0;i<mesh.subMeshCount;i++)
            mesh.SetTriangles(tri_lst[i].ToArray(),i);

        return mesh;
*/
    }


		 public static void Main(string[] args)
		{
			LoadMesh_3D("../../game_3dfx/fxart/CYRSA001.3DC");
		}
	}
}
