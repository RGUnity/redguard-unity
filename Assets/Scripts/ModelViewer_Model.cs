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
        string filename_3d = new string("CYRSA001");
        string filename_col = new String("ISLAND");
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(filename_3d, filename_col);
        GetComponent<MeshFilter>().mesh = data_3D.mesh;
        GetComponent<MeshRenderer>().SetMaterials(data_3D.materials);
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SetModel_wld(/*GameObject target,*/);
        SetModel_3D(/*GameObject target,*/);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
