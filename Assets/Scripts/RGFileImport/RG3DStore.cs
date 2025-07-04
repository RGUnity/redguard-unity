using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RG3DStore
{
    const float MESH_SCALE_FACTOR = 1/5120.0f;
    static public Vector3 MESH_VERT_FLIP = new Vector3(1.0f, -1.0f, 1.0f);
    static public Vector3 MESH_ROT_FLIP = new Vector3(-1.0f, 1.0f, -1.0f);
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


    static Dictionary<string, Mesh3D_intermediate> MeshIntermediateDict; // key: meshname

    public static string path_to_game = "./game_3dfx";
    public static string fxart_path = path_to_game + "/fxart/";


    static RG3DStore()
    {
        MeshIntermediateDict = new Dictionary<string, Mesh3D_intermediate>();
    }
    public static void DumpDict()
    {
        Debug.Log($"INTERS:");
        List<string> keys = new List<string>(MeshIntermediateDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
    }


    public static Mesh3D_intermediate LoadMeshIntermediateFlat(string flatDesc)
    {
       Mesh3D_intermediate o;
        if(MeshIntermediateDict.TryGetValue(flatDesc, out o))
        {
            return o;
        }
        else
        {
            o = new Mesh3D_intermediate();
            o.subMeshCount = 1;
            o.framecount = 0;
            o.vertices = new List<Vector3>();
            o.uv = new List<Vector2>();
            o.normals = new List<Vector3>();
            o.submeshes = new Dictionary<string, List<int>>();

            o.vertices.Add(new Vector3(0.0f,0.0f,0.0f));
            o.vertices.Add(new Vector3(1.0f,0.0f,0.0f));
            o.vertices.Add(new Vector3(1.0f,1.0f,0.0f));
            o.vertices.Add(new Vector3(0.0f,1.0f,0.0f));

            o.uv.Add(new Vector2(1.0f,1.0f));
            o.uv.Add(new Vector2(0.0f,1.0f));
            o.uv.Add(new Vector2(0.0f,0.0f));
            o.uv.Add(new Vector2(1.0f,0.0f));
            
            o.normals = new List<Vector3>();
            o.normals.Add(new Vector3(0.0f,0.0f,-1.0f));
            o.normals.Add(new Vector3(0.0f,0.0f,-1.0f));
            o.normals.Add(new Vector3(0.0f,0.0f,-1.0f));
            o.normals.Add(new Vector3(0.0f,0.0f,-1.0f));

            List<int> tris = new List<int>();
            tris.Add(0);
            tris.Add(1);
            tris.Add(2);

            tris.Add(2);
            tris.Add(3);
            tris.Add(0);

            o.submeshes.Add(flatDesc, tris);

            MeshIntermediateDict.Add(flatDesc, o);
            return MeshIntermediateDict[flatDesc];
        }

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
            string filename = path_to_game + "/fxart/" + meshname + ".3DC";
            
            RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
            file_3d.LoadFile(filename);

            MeshIntermediateDict.Add(meshname, LoadMesh_3D_intermediate(file_3d));
            return MeshIntermediateDict[meshname];
        }
    }
    public static void LoadMeshIntermediatesROB(string ROBname)
    {
        string filename = path_to_game + "/fxart/" + ROBname+ ".ROB";
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
                        Debug.Log($"Failed to load 3DC file from rob with error {ex.Message}");
                    }
                }
            }
        }

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

        for(int f=0;f<mesh.framecount;f++)
        {
            framevec_lst[f] = new List<Vector3>();
            framenorm_lst[f] = new List<Vector3>();
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

                uv_lst.Add(new Vector2(
                                face_lst[i].uvs[0].x,
                                face_lst[i].uvs[0].y
								));
                uv_lst.Add(new Vector2(
                                face_lst[i].uvs[vert_ofs+j].x,
                                face_lst[i].uvs[vert_ofs+j].y
								));
                uv_lst.Add(new Vector2(
                                face_lst[i].uvs[vert_ofs+j+1].x,
                                face_lst[i].uvs[vert_ofs+j+1].y
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
