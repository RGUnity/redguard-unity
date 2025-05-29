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
    public void SetModel_wld(string name_wld, string texbsi, string name_col)
    {
        string filename_wld = new string($"./game_3dfx/maps/{name_wld}.WLD");
        RG2Mesh.UnityData_WLD data_WLD = RG2Mesh.WLD2Mesh(filename_wld, name_col);
        GetComponent<MeshFilter>().mesh = data_WLD.mesh;
        GetComponent<MeshRenderer>().SetMaterials(data_WLD.materials);

    }
    public void add3DToScene(string prefix, string name_3d, string name_pal,Vector3 position, Vector3 eulers)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        GameObject spawned = new GameObject($"{prefix}_{name_3d}");
        MeshRenderer meshRenderer = spawned.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = spawned.AddComponent<MeshFilter>();

        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);

        spawned.transform.position = position;
        spawned.transform.Rotate(eulers);
 
    }
    void LoadRGM(string filename, string name_col)
    {
        List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(filename);
        List<RGRGMStore.RGRGMData> RGM_MPOBs = RGRGMStore.LoadMPOB(filename);
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
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        RG3DStore.LoadMeshIntermediatesROB("TAVERN");
        LoadRGM("./game_3dfx/maps/TAVERN.RGM", "ISLAND");
        */
        RG3DStore.LoadMeshIntermediatesROB("ISLAND");
        SetModel_wld("ISLAND", "302", "ISLAND");
        LoadRGM("./game_3dfx/maps/ISLAND.RGM", "ISLAND");
        // hell yeah single-line objects
//        add3DToScene("PALMTR03", "ISLAND", new Vector3(100,0,0));
//        add3DToScene("VILEGARD", "ISLAND", new Vector3(00,00,0));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
