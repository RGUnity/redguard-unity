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
    public void SetModel_wld(string name_wld, string texbsi, string name_col)
    {
        string filename_wld = new string($"./game_3dfx/maps/{name_wld}.WLD");
        RG2Mesh.UnityData_WLD data_WLD = RG2Mesh.WLD2Mesh(filename_wld, name_col);
        GetComponent<MeshFilter>().mesh = data_WLD.mesh;
        GetComponent<MeshRenderer>().SetMaterials(data_WLD.materials);

    }
    public GameObject add3DToScene(string prefix, string name_3d, string name_pal,Vector3 position, Vector3 eulers)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        GameObject spawned = new GameObject($"{prefix}_{name_3d}");
        MeshRenderer meshRenderer = spawned.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = spawned.AddComponent<MeshFilter>();

        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);

        spawned.transform.position = position;
        spawned.transform.Rotate(eulers);
        return spawned;
 
    }
    void LoadRGM(string filename, string name_col)
    {
        List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(filename);
        List<RGRGMStore.RGRGMData> RGM_MPOBs = RGRGMStore.LoadMPOB(filename);
        List<RGRGMStore.RGRGMData> RGM_MPSFs = RGRGMStore.LoadMPSF(filename);
        for(int i=0;i<RGM_MPOBs.Count;i++)
        {
            try
            {
                add3DToScene($"B{i:D3}", RGM_MPOBs[i].name2, name_col, RGM_MPOBs[i].position, RGM_MPOBs[i].rotation);
            }
            catch(Exception ex)
            {
                Debug.Log($"ERR: B{i:D3}: {ex.Message}");
            }
        }


        for(int i=0;i<RGM_MPSOs.Count;i++)
        {
            add3DToScene($"S{i:D3}", RGM_MPSOs[i].name, name_col, RGM_MPSOs[i].position, RGM_MPSOs[i].rotation);
        }
        
        for(int i=0;i<RGM_MPSFs.Count;i++)
        {
            add3DToScene($"F{i:D3}", RGM_MPSFs[i].name, name_col, RGM_MPSFs[i].position, RGM_MPSFs[i].rotation);
        }
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CYRSA_DAT = RG2Mesh.f3D2Mesh("CYRSA001", "ISLAND");
        CYRSA = add3DToScene("0000_CYR","CYRSA001", "ISLAND", new Vector3(0.0f,0.0f,0.0f), new Vector3(0.0f,0.0f,0.0f));
        
        RG3DStore.LoadMeshIntermediatesROB("TAVERN");
        LoadRGM("TAVERN", "ISLAND");
        
        RG3DStore.LoadMeshIntermediatesROB("ISLAND");
        SetModel_wld("ISLAND", "302", "ISLAND");
        LoadRGM("ISLAND", "ISLAND");
        // hell yeah single-line objects
//        add3DToScene("PALMTR03", "ISLAND", new Vector3(100,0,0));

    }
    // quick and dirty anims
    static void AnimateGO(GameObject go, float dt)
    {
        const float FRAMETIME = 0.2f;

        CYRSA_DT -= dt;
        if(CYRSA_DT < 0)
        {
            CYRSA_DT = FRAMETIME;
            CYRSA_CURFRAME++;
            if(CYRSA_CURFRAME >= CYRSA_DAT.framecount)
                CYRSA_CURFRAME = 0;
            CYRSA.GetComponent<MeshFilter>().mesh.SetVertices(CYRSA_DAT.framevertices[CYRSA_CURFRAME]);
            CYRSA.GetComponent<MeshFilter>().mesh.SetNormals(CYRSA_DAT.framenormals[CYRSA_CURFRAME]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimateGO(CYRSA, Time.deltaTime);
        
    }
}
