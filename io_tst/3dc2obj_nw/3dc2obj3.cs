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
        public struct Mesh_data
        {
            public List<Vector3> vec_lst;
            public List<Vector3> norm_lst;
            public List<Vector2> uv_lst;
            public List<int>[] tri_lst;
            public int texid_cnt;
            public String Name;
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
        private static Mesh_data LoadMesh_3D(RG3DFile file_3d, string name, int frame)
        {

            const int texid_cnt_base = 64;
            const int texid_cnt = texid_cnt_base+2;

    // 1st pass: load verts/normals/faces
            List<Vector3> vec_tmp_lst = new List<Vector3>();
            List<int> tri_tmp_lst = new List<int>();
            List<Vector3> norm_tmp_lst = new List<Vector3>();
            List<Vector2> uv_tmp_lst = new List<Vector2>();
            for(int i=0;i<file_3d.VertexCoordinates.Count;i++)
            {
                Vector3 basev = new Vector3(file_3d.VertexCoordinates[i].x,
                                            file_3d.VertexCoordinates[i].y,
                                            file_3d.VertexCoordinates[i].z);

            /*
                Vector3 basev = new Vector3(file_3d.VertexFrameDeltas[0][i].x,
                                            file_3d.VertexFrameDeltas[0][i].y,
                                            file_3d.VertexFrameDeltas[0][i].z);
            */
                 // big scale down so it fits
                vec_tmp_lst.Add(new Vector3((basev.x-file_3d.VertexFrameDeltas[frame][i].x)/5000.0f,
                                        (basev.y-file_3d.VertexFrameDeltas[frame][i].y)/5000.0f,
                                        (basev.z-file_3d.VertexFrameDeltas[frame][i].z)/5000.0f));

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
            Mesh_data o = new Mesh_data();
            o.vec_lst = vec_lst;
            o.uv_lst = uv_lst;
            o.norm_lst = norm_lst;
            o.tri_lst = tri_lst;
            o.texid_cnt = texid_cnt;
            o.Name = name;
            return o;
        }

        private static void print_obj(Mesh_data mesh, int frame)
        {
            String obj = new String("");
            List<Vector3> vec_lst = mesh.vec_lst;
            List<Vector3> norm_lst = mesh.norm_lst;
            List<Vector2> uv_lst = mesh.uv_lst;
            List<int>[] tri_lst = mesh.tri_lst;
            int texid_cnt = mesh.texid_cnt;

            for(int i=0;i<vec_lst.Count;i++)
            {
                obj += $"v {vec_lst[i].x} {vec_lst[i].y*-1.0f} {vec_lst[i].z}\n";
            }
            for(int i=0;i<uv_lst.Count;i++)
            {
                obj += $"vt {uv_lst[i].x} {uv_lst[i].y}\n";
            }
            for(int i=0;i<norm_lst.Count;i++)
            {
                obj += $"vn {-norm_lst[i].x} {-norm_lst[i].y} {-norm_lst[i].z}\n";
            }
            for(int i=0;i<texid_cnt;i++)
            {
                obj += $"g {i}\n";
                for(int j=0;j<tri_lst[i].Count;j+=3)
                {
                    int h1 = tri_lst[i][j]+1;
                    int h2 = tri_lst[i][j+1]+1;
                    int h3 = tri_lst[i][j+2]+1;
                    obj += $"f {h1}/{h1}/{h1} {h2}/{h2}/{h2} {h3}/{h3}/{h3} \n";
                }
            }

            using (StreamWriter outputFile = new StreamWriter($"./obj/{mesh.Name}_{frame:d3}.obj", false))
            {
                outputFile.WriteLine(obj);
            }
        }
        static void print_stuff(RGFileImport.RG3DFile file_3d )
        {
            Console.WriteLine($"HDR.NumFrames: {file_3d.header.NumFrames}");
            Console.WriteLine($"HDR.NumVertices: {file_3d.header.NumVertices}");
            Console.WriteLine($"HDR.NumFaces: {file_3d.header.NumFaces}");
            Console.WriteLine($"HDR.Section4Count: {file_3d.header.Section4Count}");
            Console.WriteLine($"HDR.Unknown4: {file_3d.header.Unknown4}");

            Console.WriteLine($"HDR.fram: {file_3d.header.OffsetFrameData:X}");
            Console.WriteLine($"HDR.sec4: {file_3d.header.OffsetSection4:X}");
            Console.WriteLine($"HDR.uvof: {file_3d.header.OffsetUVOffsets:X}");
            Console.WriteLine($"HDR.uvda: {file_3d.header.OffsetUVData:X}");
            Console.WriteLine($"HDR.vert: {file_3d.header.OffsetVertexCoords:X}");
            Console.WriteLine($"HDR.norm: {file_3d.header.OffsetFaceNormals:X}");
            Console.WriteLine($"HDR.face: {file_3d.header.OffsetFaceData:X}");
            Console.WriteLine($"face-fram: {file_3d.header.OffsetFaceData-file_3d.header.OffsetFrameData}");
            FrameData fd = new FrameData();
            for(int i=0;i<file_3d.frameData.Count;i++)
            {
                FrameData fd_old = fd;
                fd = file_3d.frameData[i];
                Console.WriteLine($"{i}: {fd.FrameVertexOffset:X4}, {fd.FrameNormalOffset:X4}, {fd.u1:X4}, {fd.u2:X4} || {(fd.FrameNormalOffset-fd.FrameVertexOffset)}");
//                Console.WriteLine($"{i}: {fd.FrameVertexOffset:X4}, {fd.FrameNormalOffset:X4}, {(fd.FrameNormalOffset-fd.FrameVertexOffset)/file_3d.header.NumVertices:X4}:{fd.u2}");
//                Console.WriteLine($"{i}: {fd.u1:D10}, {fd.u2:D10}, {fd.u1:D10}, {fd.u2:D10} || {fd.u1:X8}, {fd.u2:X8}, {fd.u1:X8}, {fd.u2:X8}");
            }

        }

        static void print_verts(RGFileImport.RG3DFile file_3d )
        {
            for(int i=0;i<file_3d.frameData.Count;i++)
            {
                Console.WriteLine($"FRAME {i}");
                for(int j=0;j<file_3d.header.NumVertices;j++)
                {
                    Coord3DInt basec = file_3d.VertexCoordinates[j];
                    Coord3DInt framc = file_3d.VertexFrameDeltas[i][j];
                    string o = new string("");
                    o+= $"{basec.x.ToString("+0000;-0000;+0000")}, {basec.y.ToString("+0000;-0000;+0000")}, {basec.z.ToString("+0000;-0000;+0000")}  + ";
                    o+= $"{framc.x.ToString("+0000;-0000;+0000")}, {framc.y.ToString("+0000;-0000;+0000")}, {framc.z.ToString("+0000;-0000;+0000")}  = ";
                    o+= $"{(basec.x+framc.x).ToString("+0000;-0000;+0000")}, {(basec.y+framc.y).ToString("+0000;-0000;+0000")}, {(basec.z+framc.z).ToString("+0000;-0000;+0000")};";
                    Console.WriteLine(o);
                }
            }
        }

		public static void Main(string[] args)
		{
            List<string> mesh_names = new List<string>();
            for(int i=0;i<4;i++)
                mesh_names.Add(new string($"TROLA{i:d3}"));
            int totframes = 0;
            for(int i=1;i<4;i++)
            {
                RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
                file_3d.LoadFile($"../../game_3dfx/fxart/{mesh_names[i]}.3DC");
                Console.WriteLine($"{mesh_names[i]}:{file_3d.header.NumVertices}");
                totframes += (int)file_3d.header.NumFrames;
                //print_stuff(file_3d );
                //print_verts(file_3d);
                //for(int j=0;j<4;j++)
                for(int j=0;j<file_3d.header.NumFrames;j++)
                {
                    Mesh_data md =  LoadMesh_3D(file_3d, mesh_names[i],j);
                    print_obj(md, j);
                    Console.WriteLine($"AT_{i:d2}:{j:d3}");
                }
            }
            Console.WriteLine($"totframes: {totframes}");
            /*
           RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
            file_3d.LoadFile($"../../game_3dfx/fxart/FLAGA01.3DC");
            print_stuff(file_3d );
            //print_verts(file_3d);
            //for(int j=0;j<4;j++)
            for(int j=0;j<file_3d.header.NumFrames;j++)
            {
                Mesh_data md =  LoadMesh_3D(file_3d, "FLAGA01",j);
                print_obj(md, j);
                Console.WriteLine($"AT_:{j:d3}");
            }
            */

		}
	}
}
