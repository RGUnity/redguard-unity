using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RG2Mesh
{
    public static Mesh WLD2Mesh(string filename)
    {
        const int texid_cnt = 64;
        Mesh mesh = new Mesh();
        RGFileImport.RGWLDFile file = new RGFileImport.RGWLDFile();

        // load file and build internal mesh
        file.LoadFile(filename);
        file.BuildMeshes();

        // RG format to mesh

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
        for(int i=0;i<mesh.subMeshCount;i++)
            mesh.SetTriangles(tri_lst[i],i);
        mesh.RecalculateNormals();
        return mesh;
    }
    
// WLD
}
