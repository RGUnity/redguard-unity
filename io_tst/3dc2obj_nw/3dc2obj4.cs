using System;
using System.Collections.Generic;
using System.IO;
using RGFileImport;
namespace xyz
{
    public class test
	{
		const float MESH_SCALE_FACTOR = 1/5120.0f;
		static public Vector3 MESH_VERT_FLIP = new Vector3(1.0f, -1.0f, 1.0f);
		static public Vector3 MESH_ROT_FLIP = new Vector3(-1.0f, 1.0f, -1.0f);
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
			public void Normalize()
			{
				return;

			}

			public  static Vector3 Scale(Vector3 a, Vector3 s)
			{
				Vector3 o = new Vector3(0,0,0);
				o.x = a.x*s.x;
				o.y = a.y*s.y;
				o.z = a.z*s.z;
				return o;

			}
        }

		public struct Mesh3D_intermediate
		{
			public int subMeshCount;
			public int framecount;
			public List<Vector3> vertices;
			public List<Vector2> uv;
			public List<Vector3> normals;
			public List<Vector3>[] frameDeltaVertices;
			public List<Vector3>[] frameDeltaNormals;

			public Dictionary<string, List<int>> submeshes; // dict key is:
															// texture/imageid
															// -1/colorid
		}


	// intermediate mesh loading
		public struct Face_3DC
		{
			public int vert_cnt;
			public List<Vector3> verts;
			public List<Vector2> uvs;
			public Vector3 norm;
			public List<Vector3>[] frameverts;
			public List<Vector3>[] framenorms;
			public string texid; // key for intermediate mesh submesh dict
		}

		private static Mesh3D_intermediate LoadMesh_3D_intermediate(RGFileImport.RG3DFile file_3d)
		{
			Mesh3D_intermediate mesh = new Mesh3D_intermediate();

	// 1st pass: load verts/normals/faces
			List<Vector3> vec_tmp_lst = new List<Vector3>();
			List<int> tri_tmp_lst = new List<int>();
			List<Vector3> norm_tmp_lst = new List<Vector3>();
			List<Vector2> uv_tmp_lst = new List<Vector2>();

			mesh.framecount = (int)file_3d.header.NumFrames;
			List<Vector3>[] frame_vec_tmp_lst = new List<Vector3>[mesh.framecount];
			List<Vector3>[] frame_norm_tmp_lst = new List<Vector3>[mesh.framecount];

			for(int i=0;i<file_3d.VertexCoordinates.Count;i++)
			{
				// big scale down so it fits
				Vector3 vec = new Vector3(file_3d.VertexCoordinates[i].x*MESH_SCALE_FACTOR,
										  file_3d.VertexCoordinates[i].y*MESH_SCALE_FACTOR,
										  file_3d.VertexCoordinates[i].z*MESH_SCALE_FACTOR);
				vec_tmp_lst.Add(vec);
			}

			for(int f=0;f<mesh.framecount;f++)
			{
				frame_vec_tmp_lst[f] = new List<Vector3>();
				frame_norm_tmp_lst[f] = new List<Vector3>();
				for(int i=0;i<file_3d.VertexCoordinates.Count;i++)
				{
					// big scale down so it fits
					Vector3 vec = new Vector3(-file_3d.VertexFrameDeltas[f][i].x*MESH_SCALE_FACTOR,
											  -file_3d.VertexFrameDeltas[f][i].y*MESH_SCALE_FACTOR,
											  -file_3d.VertexFrameDeltas[f][i].z*MESH_SCALE_FACTOR);
					frame_vec_tmp_lst[f].Add(vec);
					Vector3 norm = new Vector3(0.0f,
											   0.0f,
											   0.0f);
					frame_norm_tmp_lst[f].Add(norm);

				}
			}

			for(int i=0;i<file_3d.FaceNormals.Count;i++)
			{
				Vector3 normal = new Vector3( file_3d.FaceNormals[i].x,
											 file_3d.FaceNormals[i].y,
											  file_3d.FaceNormals[i].z);
				normal.Normalize();
				norm_tmp_lst.Add(normal);
			}
			List<Face_3DC> face_lst = new List<Face_3DC>();
			for(int i=0;i<file_3d.FaceDataCollection.Count;i++)
			{
				Face_3DC cur_face = new Face_3DC();
				cur_face.vert_cnt = file_3d.FaceDataCollection[i].VertexData.Count;
				cur_face.verts = new List<Vector3>();
				cur_face.uvs = new List<Vector2>();
				cur_face.norm = Vector3.Scale(norm_tmp_lst[i], MESH_VERT_FLIP);
				cur_face.frameverts = new List<Vector3>[mesh.framecount];
				cur_face.framenorms = new List<Vector3>[mesh.framecount];

				if(file_3d.FaceDataCollection[i].solid_color)
				{
					cur_face.texid = $"-1/{file_3d.FaceDataCollection[i].ColorIndex}";
				}
				else
				{
					cur_face.texid = $"{file_3d.FaceDataCollection[i].TextureId}/{file_3d.FaceDataCollection[i].ImageId}";
				}
				// regular verts
				for(int j=0;j<file_3d.FaceDataCollection[i].VertexData.Count;j++)
				{
					Vector3 vec = vec_tmp_lst[(int)file_3d.FaceDataCollection[i].VertexData[j].VertexIndex];
					vec = Vector3.Scale(vec, MESH_VERT_FLIP);
					cur_face.verts.Add(vec);
				}
				// frame verts (offsets)
				for(int f=0;f<mesh.framecount;f++)
				{
					cur_face.frameverts[f] = new List<Vector3>();
					cur_face.framenorms[f] = new List<Vector3>();
					for(int j=0;j<file_3d.FaceDataCollection[i].VertexData.Count;j++)
					{
						Vector3 vec = frame_vec_tmp_lst[f][(int)file_3d.FaceDataCollection[i].VertexData[j].VertexIndex];
						vec = Vector3.Scale(vec, MESH_VERT_FLIP);
						cur_face.frameverts[f].Add(vec);
						Vector3 norm = frame_norm_tmp_lst[f][(int)file_3d.FaceDataCollection[i].VertexData[j].VertexIndex];
						norm = Vector3.Scale(vec, MESH_VERT_FLIP);
						cur_face.framenorms[f].Add(norm);
					}
				}

				for(int j=0;j<file_3d.FaceDataCollection[i].VertexData.Count;j++)
				{
					cur_face.uvs.Add(new Vector2(
									file_3d.FaceDataCollection[i].VertexData[j].U,
									file_3d.FaceDataCollection[i].VertexData[j].V
									));
				}
				face_lst.Add(cur_face);
			}
	// 2nd pass: sort faces by texture id and split verts/norms/uvs
			List<Vector3> vec_lst = new List<Vector3>();
			List<Vector3> norm_lst = new List<Vector3>();
			List<Vector3>[] framevec_lst = new List<Vector3>[mesh.framecount];
			List<Vector3>[] framenorm_lst = new List<Vector3>[mesh.framecount];
			List<Vector2> uv_lst = new List<Vector2>();
			Dictionary<string, List<int>> tri_lst = new Dictionary<string, List<int>>();
			Dictionary<string, Vector2> uv_scale_lst = new Dictionary<string, Vector2>();

			for(int f=0;f<mesh.framecount;f++)
			{
				framevec_lst[f] = new List<Vector3>();
				framenorm_lst[f] = new List<Vector3>();
			}
			for(int i=0;i<face_lst.Count;i++)
			{
				Vector2 v;
				if(!uv_scale_lst.TryGetValue(face_lst[i].texid, out v))
				{
					uv_scale_lst.Add(face_lst[i].texid, new Vector2(4069.0f/10.0f, 4069.0f/10.0f));
				}
/*
				for(int j=0;j<face_lst[i].uvs.Count;j++)
				{
					if(uv_scale_lst[face_lst[i].texid].y < face_lst[i].uvs[j].y)
						uv_scale_lst[face_lst[i].texid] = new Vector2(uv_scale_lst[face_lst[i].texid].x, face_lst[i].uvs[j].y);
					if(uv_scale_lst[face_lst[i].texid].x < face_lst[i].uvs[j].x)
						uv_scale_lst[face_lst[i].texid] = new Vector2(face_lst[i].uvs[j].x, uv_scale_lst[face_lst[i].texid].y);
				}
*/
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

					for(int f=0;f<mesh.framecount;f++)
					{
						framevec_lst[f].Add(face_lst[i].frameverts[f][0]);
						framevec_lst[f].Add(face_lst[i].frameverts[f][vert_ofs+j]);
						framevec_lst[f].Add(face_lst[i].frameverts[f][vert_ofs+j+1]);

						framenorm_lst[f].Add(face_lst[i].framenorms[f][0]);
						framenorm_lst[f].Add(face_lst[i].framenorms[f][vert_ofs+j]);
						framenorm_lst[f].Add(face_lst[i].framenorms[f][vert_ofs+j+1]);
					}

					float UV_TRANSFORM_FACTOR_X = uv_scale_lst[face_lst[i].texid].x;
					float UV_TRANSFORM_FACTOR_Y = uv_scale_lst[face_lst[i].texid].y*1.88f;

					uv_lst.Add(new Vector2(
									face_lst[i].uvs[0].x,
									-face_lst[i].uvs[0].y
									));
					uv_lst.Add(new Vector2(
									face_lst[i].uvs[vert_ofs+j].x,
									-face_lst[i].uvs[vert_ofs+j].y
									));
					uv_lst.Add(new Vector2(
									face_lst[i].uvs[vert_ofs+j+1].x,
									-face_lst[i].uvs[vert_ofs+j+1].y
									));


					List<int> l;
					if(!tri_lst.TryGetValue(face_lst[i].texid, out l))
					{
						tri_lst.Add(face_lst[i].texid, new List<int>());
					}
					
					tri_lst[face_lst[i].texid].Add(tri_cnt*3+2);
					tri_lst[face_lst[i].texid].Add(tri_cnt*3+1);
					tri_lst[face_lst[i].texid].Add(tri_cnt*3+0);
					tri_cnt++;
				}
			}

			mesh.subMeshCount = tri_lst.Count;
			mesh.vertices = vec_lst;
			mesh.uv = uv_lst;
			mesh.normals = norm_lst;
			mesh.submeshes = tri_lst;
			mesh.frameDeltaVertices = framevec_lst;
			mesh.frameDeltaNormals = framenorm_lst;

			return mesh;
		}

        private static void print_obj(Mesh3D_intermediate mesh, string Name)
        {
            String obj = new String("");
            List<Vector3> vertices = mesh.vertices;
            List<Vector2> uv = mesh.uv;
            List<Vector3> normals = mesh.normals;

            for(int i=0;i<vertices.Count;i++)
            {
                obj += $"v {vertices[i].x} {vertices[i].y} {vertices[i].z}\n";
            }
            for(int i=0;i<uv.Count;i++)
            {
			/*
				uint x = (uint)uv[i].x;
				uint y = (uint)uv[i].y;
                obj += $"vt {x:X8} {y:X8}\n";
			*/
				float x = uv[i].x;
				float y = uv[i].y;
				x /= 1024.0f;
				y /= 1024.0f;
                obj += $"vt {x} {y}\n";
//				x /= (4069.0f/10.0f);
//				y /= (4069.0f/10.0f);
            }
            for(int i=0;i<normals.Count;i++)
            {
                obj += $"vn {normals[i].x} {normals[i].y} {normals[i].z}\n";
            }
			int h=0;
			foreach(var it in mesh.submeshes)
            {
                obj += $"g {it.Key}\n";
				h++;
                for(int j=0;j<it.Value.Count;j+=3)
                {
                    int h1 = it.Value[j]+1;
                    int h2 = it.Value[j+1]+1;
                    int h3 = it.Value[j+2]+1;
                    obj += $"f {h1}/{h1}/{h1} {h2}/{h2}/{h2} {h3}/{h3}/{h3} \n";
                }
            }

            using (StreamWriter outputFile = new StreamWriter($"./obj/{Name}.obj", false))
            {
                outputFile.WriteLine(obj);
            }
        }

		public static void Main(string[] args)
		{
			RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
			RGFileImport.RGROBFile file_rob = new RGFileImport.RGROBFile();
			Mesh3D_intermediate  imm;
            file_rob.LoadFile("../../game_3dfx/fxart/GERRICKS.ROB");
			for(int i=0;i<file_rob.hdr.NumSegments;i++)
			{
				if(file_rob.segments[i].Size > 0)
				{
					Console.WriteLine($"{i}: {file_rob.segments[i].SegmentID}");
					file_3d.LoadMemory(file_rob.segments[i].Data, false);
					imm = LoadMesh_3D_intermediate(file_3d);
					print_obj(imm, file_rob.segments[i].SegmentID);
				}
			}

            file_3d.LoadFile("../../game_3dfx/fxart/CYRSA001.3DC");
            imm = LoadMesh_3D_intermediate(file_3d);
			print_obj(imm, "test");
		}
	}
}
