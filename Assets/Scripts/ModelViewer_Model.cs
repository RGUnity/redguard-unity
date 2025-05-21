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
    public void SetModel_wld(/*GameObject target,*/)
    {
        // 6-line WLD loading, not bad
        string filename_wld = new string("./game_3dfx/maps/ISLAND.WLD");
        string filename_texbsi = new string("./game_3dfx/fxart/TEXBSI.302");
        string filename_col = new String("./game_3dfx/fxart/ISLAND.COL");
        RG2Mesh.UnityData_WLD data_WLD = RG2Mesh.WLD2Mesh(filename_wld, filename_texbsi, filename_col);
        GetComponent<MeshFilter>().mesh = data_WLD.mesh;
        GetComponent<MeshRenderer>().SetMaterials(data_WLD.materials);

    }
    public void SetModel_3D(/*GameObject target,*/)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh("CYRSA001", "ISLAND");
        GetComponent<MeshFilter>().mesh = data_3D.mesh;
        GetComponent<MeshRenderer>().SetMaterials(data_3D.materials);
    }
    public void add3DToScene(string name_3d, string name_pal)
    {
        GameObject spawned = new GameObject(name_3d);
        MeshRenderer meshRenderer = spawned.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = spawned.AddComponent<MeshFilter>();

        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);
        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);
 
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetModel_3D(/*GameObject target,*/);

        // hell yeah single-line objects
        // just need to figure out how to set their position
        add3DToScene("GYPTENT", "ISLAND");
        add3DToScene("GYPCART", "ISLAND");
        add3DToScene("TOBIAS", "ISLAND");
        add3DToScene("NFARA002", "ISLAND");
        add3DToScene("AVIKA001", "ISLAND");
        add3DToScene("NIDAA001", "ISLAND");
        add3DToScene("VILEGARD", "ISLAND");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
