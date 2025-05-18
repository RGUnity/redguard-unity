using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RG2Mesh
{
    public struct UnityData_WLD
    {
        public Mesh mesh;
        public List<Material> materials;
    }
    public struct UnityData_3D
    {
        public Mesh mesh;
        public List<Material> materials;
    }

    public static UnityData_WLD WLD2Mesh(string filename_wld, string filename_texbsi, string filename_col)
    {
        Mesh mesh_wld = LoadMesh_WLD(filename_wld);
        List<Material> materials = LoadMaterials(filename_texbsi, filename_col);

        UnityData_WLD data = new UnityData_WLD();
        data.mesh = mesh_wld;
        data.materials = materials;

        return data;
    }
    public static UnityData_3D f3D2Mesh(string filename_3d, string filename_texbsi, string filename_col)
    {
        Mesh mesh_3d = LoadMesh_3D(filename_3d);
        List<Material> materials = LoadMaterials(filename_texbsi, filename_col);

        UnityData_3D data = new UnityData_3D();
        data.mesh = mesh_3d;
        data.materials = materials;

        return data;
    }


    private static Mesh LoadMesh_WLD(string filename_wld)
    {
        const int texid_cnt = 64;
        Mesh mesh = new Mesh();
        RGFileImport.RGWLDFile file = new RGFileImport.RGWLDFile();

        // load file and build internal mesh
        file.LoadFile(filename_wld);
        file.BuildMeshes();

        mesh.indexFormat = IndexFormat.UInt32;
        mesh.subMeshCount = texid_cnt;

        List<Vector3> vec_lst = new List<Vector3>();
        List<Vector2> uv_lst  = new List<Vector2>();
        List<int[]>   tri_lst = new List<int[]>();
        int tri_ofs = 0;
        for(int i=0;i<mesh.subMeshCount;i++)
        {
            vec_lst.AddRange(file.meshes[i].vertices);
            uv_lst.AddRange(file.meshes[i].uv);

            int[] tri_tmp = file.meshes[i].triangles.ToArray();
            int j = 0;
            for(j=0;j<tri_tmp.Length;j++)
            {
                tri_tmp[j] += tri_ofs;
            }
            tri_ofs += j;
            tri_lst.Add(tri_tmp);
        }

        mesh.vertices = vec_lst.ToArray();
        mesh.uv = uv_lst.ToArray();
        for(int i=0;i<mesh.subMeshCount;i++)
            mesh.SetTriangles(tri_lst[i],i);
        mesh.RecalculateNormals();
        return mesh;
    }
    private static List<Material> LoadMaterials(string filename_texbsi, string filename_col)
    {
        // TEXBSI.302
        List<Material> materials = new List<Material>();
        List<Texture2D> textures = LoadTexture(filename_texbsi, filename_col);

        for(int i =0;i<textures.Count;i++)
        {
            materials.Add(new Material(Shader.Find("Legacy Shaders/Diffuse Fast")));
            materials[materials.Count-1].mainTexture = textures[i];
        }
        return materials;
    }

    private static List<Texture2D> LoadTexture(string filename_texbsi, string filename_col)
    {
        List<Texture2D> tex_lst = new List<Texture2D>();
        Texture2D[] tex_lst_sorted;
        RGPaletteFile.RGColor[] palette = RGPaletteFile.Load(filename_col);
        RGTextureBSIFile bsif = new RGTextureBSIFile(palette);

        bsif.LoadFile(filename_texbsi);

        tex_lst_sorted = new Texture2D[bsif.Images.Count];
        for(int i =0;i<bsif.Images.Count;i++)
        {
            Texture2D cur_tex = GraphicsConverter.RGTextureToTexture2D(bsif.Images[i])[0];
            tex_lst_sorted[Int32.Parse(bsif.Images[i].Name.Substring(3))] = cur_tex;
        }

        return new List<Texture2D>(tex_lst_sorted);
    }

    public struct Face_3DC
    {
        public int vert_cnt;
        public List<Vector3> verts;
        public List<Vector2> uvs;
        public Vector3 norm;
        public int texid;
    }

    private static Mesh LoadMesh_3D(string filename_3d)
    {
        const int texid_cnt_base = 64;
        const int texid_cnt = texid_cnt_base+2;
        Mesh mesh = new Mesh();
        RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
        file_3d.LoadFile(filename_3d);

        const float UV_TRANSFORM_FACTOR = 4069.0f;
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
                                (UV_TRANSFORM_FACTOR-file_3d.FaceDataCollection[i].VertexData[j].U)/UV_TRANSFORM_FACTOR,
                                (UV_TRANSFORM_FACTOR-file_3d.FaceDataCollection[i].VertexData[j].V)/UV_TRANSFORM_FACTOR));
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

                uv_lst.Add(face_lst[i].uvs[0]);
                uv_lst.Add(face_lst[i].uvs[vert_ofs+j]);
                uv_lst.Add(face_lst[i].uvs[vert_ofs+j+1]);
                
                tri_lst[face_lst[i].texid].Add(tri_cnt*3);
                tri_lst[face_lst[i].texid].Add(tri_cnt*3+1);
                tri_lst[face_lst[i].texid].Add(tri_cnt*3+2);
                tri_cnt++;
            }
        }

        mesh.subMeshCount = texid_cnt;
        mesh.vertices = vec_lst.ToArray();
        mesh.uv = uv_lst.ToArray();
        mesh.normals = norm_lst.ToArray();
        
        // TODO: clean up unused submeshes and deal with solid colors
        for(int i=0;i<mesh.subMeshCount;i++)
            mesh.SetTriangles(tri_lst[i].ToArray(),i);

        return mesh;
    }
    // c# has no swap function in its lists apparently
    private static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}
