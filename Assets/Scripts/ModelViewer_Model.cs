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
    public void add3DToScene(string name_3d, string name_pal,Vector3 position)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        GameObject spawned = new GameObject(name_3d);
        MeshRenderer meshRenderer = spawned.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = spawned.AddComponent<MeshFilter>();

        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);

        spawned.transform.position = position;
 
    }
    void LoadRGM(string filename)
    {
        RGFileImport.RGRGMFile filergm = new RGFileImport.RGRGMFile();
        filergm.LoadFile(filename);
        const float RGM_SCALE = 10.0f;
        const float RGM_SCALE_HEIGHT = 0.005f;
        for(int i=0;i<filergm.MPSO.items.Count;i++)
        {
            try{
                float posx = (float)filergm.MPSO.items[i].posx*RGM_SCALE;
                float posy = -((float)((int)256-(int)filergm.MPSO.items[i].posy)*RGM_SCALE);
                float posz = (0xFFFF-filergm.MPSO.items[i].height)*RGM_SCALE_HEIGHT;
                add3DToScene(filergm.MPSO.items[i].name, "ISLAND", new Vector3(posx,posz,posy));
            }
            catch(Exception ex)
            {
                Debug.Log($"BROKE {filergm.MPSO.items[i].name}");
            }
        }
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetModel_wld(/*GameObject target,*/);
        LoadRGM("./game_3dfx/maps/ISLAND.RGM");
        // hell yeah single-line objects
        // just need to figure out how to set their position
//        add3DToScene("PALMTR03", "ISLAND", new Vector3(100,0,0));
//        add3DToScene("VILEGARD", "ISLAND", new Vector3(00,00,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
