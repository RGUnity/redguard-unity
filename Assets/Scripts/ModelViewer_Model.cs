using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport;

using Assets.Scripts.RGFileImport.RGGFXImport;
public class ModelViewer_Model : MonoBehaviour
{
    static GameObject CYRSA;
    static RG2Mesh.UnityData_3D CYRSA_DAT;
    static float CYRSA_DT = 1.0f;
    static int CYRSA_CURFRAME = 0;
    static int CYRSA_NXTFRAME = 1;
    public void SetModel_wld(string name_wld, string texbsi, string name_col)
    {
        string filename_wld = new string($"./game_3dfx/maps/{name_wld}.WLD");
        RG2Mesh.UnityData_WLD data_WLD = RG2Mesh.WLD2Mesh(filename_wld, name_col);
        GetComponent<MeshFilter>().mesh = data_WLD.mesh;
        GetComponent<MeshCollider>().sharedMesh = data_WLD.mesh;
        GetComponent<MeshRenderer>().SetMaterials(data_WLD.materials);

    }
    public GameObject add3DToScene(string prefix, string name_3d, string name_pal,Vector3 position, Vector3 eulers)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        GameObject spawned = new GameObject($"{prefix}_{name_3d}");
        MeshRenderer meshRenderer = spawned.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = spawned.AddComponent<MeshFilter>();
        MeshCollider meshCollider = spawned.AddComponent<MeshCollider>();

        meshFilter.mesh = data_3D.mesh;
        meshCollider.sharedMesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);

        spawned.transform.position = position;
        spawned.transform.Rotate(eulers);
        return spawned;
 
    }
    public GameObject addFlatToScene(string prefix, string name_flat, string name_pal,Vector3 position, Vector3 eulers)
    {
        RG2Mesh.UnityData_3D data_FLAT = RG2Mesh.FLAT2Mesh(name_flat, name_pal);

        GameObject spawned = new GameObject($"{prefix}_{name_flat}");
        MeshRenderer meshRenderer = spawned.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = spawned.AddComponent<MeshFilter>();

        meshFilter.mesh = data_FLAT.mesh;
        meshRenderer.SetMaterials(data_FLAT.materials);

        spawned.transform.position = position;
        spawned.transform.Rotate(eulers);
        return spawned;
 
    }

    void LoadRGM(string filename, string name_col)
    {


        RGFileImport.RGWLDFile wld = new RGFileImport.RGWLDFile();
        wld.LoadFile($"./game_3dfx/maps/ISLAND.WLD");
        List<RGRGMStore.RGRGMData> RGM_WDNMs = RGRGMStore.LoadWDNM(filename);
        Dictionary<int, List<int>> zdict = new Dictionary<int, List<int>> ();
        int tot = 0;
        for(int i=0;i<RGM_WDNMs.Count;i++)
        {
            Vector3 pos = RGM_WDNMs[i].position;
            /*
            pos.y += 200.0f;
            RaycastHit hit;
            if(Physics.Raycast(new Ray(pos,new Vector3(0,-1,0)), out hit))
            {
                if(hit.collider == GetComponent<MeshCollider>())
                {
                    Vector3 p1 = pos;
                    p1.y -= 200.0f;
                    Vector3 p2 = hit.point;
                           tot++;
                    Debug.Log($"POINT,({p2.y},{p1.y})");
                }
            }
            */
    
            add3DToScene(RGM_WDNMs[i].name2, RGM_WDNMs[i].name, name_col, pos, RGM_WDNMs[i].rotation);
        }
       List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(filename);
        for(int j=0;j<RGM_MPSOs.Count;j++)
        {
            add3DToScene($"S{j:D3}", RGM_MPSOs[j].name, name_col, RGM_MPSOs[j].position, RGM_MPSOs[j].rotation);
        }
    }

    public GameObject addAnimToScene(string prefix, string name_3d, string name_pal,Vector3 position, Vector3 eulers)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        GameObject spawned = new GameObject($"{prefix}_{name_3d}");
        SkinnedMeshRenderer skinnedMeshRenderer = spawned.AddComponent<SkinnedMeshRenderer>();
        Mesh skinnedMesh = spawned.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        //skinnedMesh = data_3D.mesh;
        spawned.GetComponent<SkinnedMeshRenderer>().sharedMesh = data_3D.mesh;
        skinnedMeshRenderer.SetMaterials(data_3D.materials);

        spawned.transform.position = position;
        spawned.transform.Rotate(eulers);
        return spawned;
 
    }


    void Start()
    {
        CYRSA_DAT = RG2Mesh.f3D2Mesh("WOMEA001", "ISLAND");
        CYRSA = addAnimToScene("0000_CYR","WOMEA001", "ISLAND", new Vector3(0.0f,0.0f,0.0f), new Vector3(0.0f,0.0f,0.0f));
       /* 
        RG3DStore.LoadMeshIntermediatesROB("TAVERN");
        LoadRGM("TAVERN", "ISLAND");
        
        // hell yeah single-line objects
//        add3DToScene("PALMTR03", "ISLAND", new Vector3(100,0,0));
       */ 

        RG3DStore.LoadMeshIntermediatesROB("ISLAND");
        SetModel_wld("ISLAND", "302", "ISLAND");
        LoadRGM("ISLAND", "ISLAND");
    }
    // quick and dirty anims
    static void AnimateGO(GameObject go, float dt)
    {
        const float FRAMETIME = 2.0f;

        CYRSA_DT -= dt;
        if(CYRSA_DT < 0)
        {
            CYRSA.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight (CYRSA_CURFRAME, 0.0f);
            CYRSA.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight (CYRSA_NXTFRAME, 100.0f);
            CYRSA_DT = FRAMETIME;
            CYRSA_CURFRAME = CYRSA_NXTFRAME;
            CYRSA_NXTFRAME++;
            if(CYRSA_NXTFRAME >= CYRSA_DAT.framecount)
                CYRSA_NXTFRAME = 0;
        }

        float BLEND1 = (CYRSA_DT/FRAMETIME)*100.0f;
        float BLEND2 = 100-BLEND1;
        Debug.Log($"B1: {BLEND1} <> {BLEND2}");
        CYRSA.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight (CYRSA_CURFRAME, BLEND1);
        CYRSA.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight (CYRSA_NXTFRAME, BLEND2);
    }

    // Update is called once per frame
    void Update()
    {
//        AnimateGO(CYRSA, Time.deltaTime);
        
    }
}
