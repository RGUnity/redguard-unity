using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RG3DStore
{
    const float MESH_SCALE_FACTOR = 1/5000.0f;
    static public Vector3 MESH_VERT_FLIP = new Vector3(1.0f, -1.0f, 1.0f);
    static public Vector3 MESH_ROT_FLIP = new Vector3(-1.0f, 1.0f, -1.0f);
    public struct Mesh3D_intermediate
    {
        public int subMeshCount;
        public int framecount;
        public List<Vector3>[] vertices;
        public List<Vector2> uv;
        public List<Vector3>[] normals;

        public Dictionary<string, List<int>> submeshes; // dict key is:
                                                        // texture/imageid
                                                        // -1/colorid
    }


    static Dictionary<string, Mesh3D_intermediate> MeshIntermediateDict; // key: meshname

    static string path_to_game = "./game_3dfx";
    static string fxart_path = path_to_game + "/fxart/";


    static RG3DStore()
    {
        MeshIntermediateDict = new Dictionary<string, Mesh3D_intermediate>();
    }

    // for now, assuming we only want to explicitly load 3dc files and that all 3d files are in the ROB files
    public static Mesh3D_intermediate LoadMeshIntermediate3DC(string meshname)
    {
        Mesh3D_intermediate o;
        if(MeshIntermediateDict.TryGetValue(meshname, out o))
        {
            return o;
        }
        else
        {
            string filename = fxart_path + meshname + ".3DC";
            RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
            file_3d.LoadFile(filename);

            MeshIntermediateDict.Add(meshname, LoadMesh_3D_intermediate(file_3d));
            return MeshIntermediateDict[meshname];
        }
    }
    public static void LoadMeshIntermediatesROB(string ROBname)
    {
        string filename = fxart_path + ROBname+ ".ROB";
        RGFileImport.RGROBFile file_rob = new RGFileImport.RGROBFile();
        file_rob.LoadFile(filename);
        for(int i=0;i<file_rob.hdr.NumSegments;i++)
        {
            Mesh3D_intermediate o;
            if(!MeshIntermediateDict.TryGetValue(file_rob.segments[i].SegmentID, out o))
            {
                if(file_rob.segments[i].Size > 0) // probably the 512 thing?
                {
                    RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
                    file_3d.LoadMemory(file_rob.segments[i].Data, false);
                    MeshIntermediateDict.Add(file_rob.segments[i].SegmentID, LoadMesh_3D_intermediate(file_3d));
                }
                else
                {
                    try{
                        LoadMeshIntermediate3DC(file_rob.segments[i].SegmentID);
                    }
                    catch(Exception ex)
                    {
                        Debug.Log($"prolly file not found?");
                    }
                }
            }
        }

    }


// intermediate mesh loading
    public struct Face_3DC
    {
        public int vert_cnt;
        public List<Vector3>[] verts;
        public List<Vector2> uvs;
        public Vector3 norm;
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
            for(int i=0;i<file_3d.VertexCoordinates.Count;i++)
            {
                // big scale down so it fits
                Vector3 vec = new Vector3(file_3d.VertexFrameDeltas[f][i].x*MESH_SCALE_FACTOR,
                                          file_3d.VertexFrameDeltas[f][i].y*MESH_SCALE_FACTOR,
                                          file_3d.VertexFrameDeltas[f][i].z*MESH_SCALE_FACTOR);
                vec.x -= vec_tmp_lst[i].x;
                vec.y -= vec_tmp_lst[i].y;
                vec.z -= vec_tmp_lst[i].z;

                frame_vec_tmp_lst[f].Add(vec);
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
            cur_face.verts = new List<Vector3>[mesh.framecount];
            cur_face.uvs = new List<Vector2>();
            cur_face.norm = Vector3.Scale(norm_tmp_lst[i], MESH_VERT_FLIP);

            if(file_3d.FaceDataCollection[i].solid_color)
            {
                cur_face.texid = $"-1/{file_3d.FaceDataCollection[i].ColorIndex}";
            }
            else
            {
                cur_face.texid = $"{file_3d.FaceDataCollection[i].TextureId}/{file_3d.FaceDataCollection[i].ImageId}";
            }
            // 0th frame = regular verts
            cur_face.verts[0] = new List<Vector3>();
            for(int j=0;j<file_3d.FaceDataCollection[i].VertexData.Count;j++)
            {
                Vector3 vec = vec_tmp_lst[(int)file_3d.FaceDataCollection[i].VertexData[j].VertexIndex];
                vec = Vector3.Scale(vec, MESH_VERT_FLIP);
                cur_face.verts[0].Add(vec);
            }
            for(int f=1;f<mesh.framecount;f++)
            {
                cur_face.verts[f] = new List<Vector3>();
                for(int j=0;j<file_3d.FaceDataCollection[i].VertexData.Count;j++)
                {
                    cur_face.verts[f].Add(frame_vec_tmp_lst[f][(int)file_3d.FaceDataCollection[i].VertexData[j].VertexIndex]);
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
        List<Vector3>[] vec_lst = new List<Vector3>[mesh.framecount];
        List<Vector3>[] norm_lst = new List<Vector3>[mesh.framecount];
        List<Vector2> uv_lst = new List<Vector2>();
        Dictionary<string, List<int>> tri_lst = new Dictionary<string, List<int>>();
        Dictionary<string, Vector2> uv_scale_lst = new Dictionary<string, Vector2>();

        for(int f=0;f<mesh.framecount;f++)
        {
            vec_lst[f] = new List<Vector3>();
            norm_lst[f] = new List<Vector3>();
        }
        for(int i=0;i<face_lst.Count;i++)
        {
            Vector2 v;
            if(!uv_scale_lst.TryGetValue(face_lst[i].texid, out v))
            {
                uv_scale_lst.Add(face_lst[i].texid, new Vector2(0.0f, 0.0f));
            }

			for(int j=0;j<face_lst[i].uvs.Count;j++)
			{
                if(uv_scale_lst[face_lst[i].texid].y < face_lst[i].uvs[j].y)
					uv_scale_lst[face_lst[i].texid] = new Vector2(uv_scale_lst[face_lst[i].texid].x, face_lst[i].uvs[j].y);
                if(uv_scale_lst[face_lst[i].texid].x < face_lst[i].uvs[j].x)
					uv_scale_lst[face_lst[i].texid] = new Vector2(face_lst[i].uvs[j].x, uv_scale_lst[face_lst[i].texid].y);
			}
		}

        int tri_cnt = 0;
        for(int i=0;i<face_lst.Count;i++)
        {
            for(int j=0;j<=face_lst[i].vert_cnt-3;j++)
            {

                int vert_ofs = 1;
                for(int f=0;f<mesh.framecount;f++)
                {
                    vec_lst[f].Add(face_lst[i].verts[f][0]);
                    vec_lst[f].Add(face_lst[i].verts[f][vert_ofs+j]);
                    vec_lst[f].Add(face_lst[i].verts[f][vert_ofs+j+1]);

                    if(f == 0)
                    {
                        norm_lst[f].Add(face_lst[i].norm);
                        norm_lst[f].Add(face_lst[i].norm);
                        norm_lst[f].Add(face_lst[i].norm);
                    }
                    else
                    {
                        // regenerate normals
                        Vector3 p1 = vec_lst[f][vec_lst[f].Count-3];
                        Vector3 p2 = vec_lst[f][vec_lst[f].Count-2];
                        Vector3 p3 = vec_lst[f][vec_lst[f].Count-1];
                        Vector3 norm =  calculateTriNormal(p1, p2, p3);
                        norm_lst[f].Add(face_lst[i].norm);
                        norm_lst[f].Add(face_lst[i].norm);
                        norm_lst[f].Add(face_lst[i].norm);
                    }
                }

                float UV_TRANSFORM_FACTOR_X = uv_scale_lst[face_lst[i].texid].x;
                float UV_TRANSFORM_FACTOR_Y = uv_scale_lst[face_lst[i].texid].y;

                uv_lst.Add(new Vector2(
                                ((face_lst[i].uvs[0].x)/(UV_TRANSFORM_FACTOR_X)),
                                -((UV_TRANSFORM_FACTOR_Y)-face_lst[i].uvs[0].y)/(UV_TRANSFORM_FACTOR_Y)
								));
                uv_lst.Add(new Vector2(
                                ((face_lst[i].uvs[vert_ofs+j].x)/(UV_TRANSFORM_FACTOR_X)),
                                -((UV_TRANSFORM_FACTOR_Y)-face_lst[i].uvs[vert_ofs+j].y)/(UV_TRANSFORM_FACTOR_Y)
								));
                uv_lst.Add(new Vector2(
                                ((face_lst[i].uvs[vert_ofs+j+1].x)/(UV_TRANSFORM_FACTOR_X)),
                                -((UV_TRANSFORM_FACTOR_Y)-face_lst[i].uvs[vert_ofs+j+1].y)/(UV_TRANSFORM_FACTOR_Y)
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

        return mesh;
    }
    static Vector3 calculateTriNormal(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 o = new Vector3();
        Vector3 a = p2-p1;
        Vector3 b = p3-p1;

        o.x = a.y*b.z - a.z*b.y;
        o.y = a.z*b.x - a.z*b.z;
        o.z = a.x*b.y - a.z*b.x;
        return o;
    }
}
