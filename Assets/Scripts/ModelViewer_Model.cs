using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public class ModelViewer_Model : MonoBehaviour
{
    Mesh mesh;
    MeshRenderer mren;
    Material placeholderMat;
    public void SetModel_wld(/*GameObject target,*/)
    {
        // FILL IN
        string filename = new string("/home/frits/RG_upstream/game/Redguard/maps/ISLAND.WLD");
        RGFileImport.RGWLDFile filewld = new RGFileImport.RGWLDFile();
        filewld.LoadFile(filename);
        filewld.BuildMeshes();

    //Create Unity mesh and map data to it
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().materials[0] = placeholderMat;
        CreateShape_wld(filewld);
        //UpdateMesh();
    }
    void CreateShape_wld(RGFileImport.RGWLDFile filewld)
    {
        mesh.Clear();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.subMeshCount = 64;
        List<Vector3> vec_lst = new List<Vector3>();
        List<Vector2> uv_lst = new List<Vector2>();
        List<int[]> tri_lst = new List<int[]>();
        int tri_ofs = 0;
        for(int i=0;i<mesh.subMeshCount;i++)
        {
            vec_lst.AddRange(filewld.meshes[i].vertices);
            uv_lst.AddRange(filewld.meshes[i].uv);

            int[] tri_tmp = filewld.meshes[i].triangles.ToArray();
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
        mren.material = placeholderMat;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetModel_wld(/*GameObject target,*/);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
