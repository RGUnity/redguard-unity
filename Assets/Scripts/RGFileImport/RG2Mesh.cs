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
//        List<Material> materials = LoadMaterials(filename_texbsi, filename_col);

        UnityData_3D data = new UnityData_3D();
        data.mesh = mesh_3d;
//        data.materials = materials;

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

    private static Mesh LoadMesh_3D(string filename_3d)
    {
        Mesh mesh = new Mesh();
        RGFileImport.RG3DFile file_3d = new RGFileImport.RG3DFile();
        file_3d.LoadFile(filename_3d);

        List<Vector3> vec_lst = new List<Vector3>();
        List<int> tri_lst = new List<int>();

        for(int i=0;i<file_3d.VertexCoordinates.Count;i++)
        {
            // big scale down so it fits
            vec_lst.Add(new Vector3(file_3d.VertexCoordinates[i].x/1000.0f,
                                    file_3d.VertexCoordinates[i].y/1000.0f,
                                    file_3d.VertexCoordinates[i].z/1000.0f));
        }

        for(int i=0;i<file_3d.FaceDataCollection.Count;i++)
        {
            List<int> tris = new List<int>();
            for(int j=0;j<=file_3d.FaceDataCollection[i].VertexData.Count-3;j++)
            {
                int vert_ofs = 1;
                tris.Add((int)file_3d.FaceDataCollection[i].VertexData[0].VertexIndex);
                tris.Add((int)file_3d.FaceDataCollection[i].VertexData[vert_ofs+j].VertexIndex);
                tris.Add((int)file_3d.FaceDataCollection[i].VertexData[vert_ofs+j+1].VertexIndex);
            }
            tri_lst.AddRange(tris);
            Debug.Log($"CUR: {string.Join(", ", tris)}");
        }
        mesh.vertices = vec_lst.ToArray();
        mesh.triangles = tri_lst.ToArray();

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
