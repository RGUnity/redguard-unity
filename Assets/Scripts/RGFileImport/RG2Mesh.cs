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

        List<Material> materials = new List<Material>();
        for(int i=0;i<64;i++)
        {
            materials.Add(RGTexStore.GetMaterial("ISLAND",302,i));
        }

        UnityData_WLD data = new UnityData_WLD();
        data.mesh = mesh_wld;
        data.materials = materials;

        return data;
    }
    public static UnityData_3D f3D2Mesh(string meshname, string palettename)
    {
        RG3DStore.LoadMeshIntermediatesROB("ISLAND");
        RG3DStore.Mesh3D_intermediate mesh_i = RG3DStore.LoadMeshIntermediate3DC(meshname);
        
        List<Material> materials = new List<Material>();
        Mesh mesh_3d = new Mesh();
        mesh_3d.subMeshCount = mesh_i.subMeshCount;
        mesh_3d.vertices = mesh_i.vertices.ToArray();
        mesh_3d.uv = mesh_i.uv.ToArray();
        mesh_3d.normals = mesh_i.normals.ToArray();

        int i = 0;
        foreach(var submesh in mesh_i.submeshes)
        {
            string[] keys = submesh.Key.Split("/");
            materials.Add(RGTexStore.GetMaterial(palettename,Int32.Parse(keys[0]),Int32.Parse(keys[1])));
            List<int> tri_lst = submesh.Value;
            mesh_3d.SetTriangles(tri_lst.ToArray(), i);
            i++;
        } 



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

    // c# has no swap function in its lists apparently
    private static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}
