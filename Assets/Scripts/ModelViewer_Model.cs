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
    void LoadRGM(string filename)
    {
        RGFileImport.RGRGMFile filergm = new RGFileImport.RGRGMFile();
        filergm.LoadFile(filename);
        const float RGM_SCALE = 1.0f/20.0f;
        const float RGM_SCALE_HEIGHT = 1.0f/21.0f;
        const float RGM_OFS_HEIGHT = 0.0f;
        const float RGM_X_OFS = 29.8f;
        const float RGM_Y_OFS = -22.8f;

        Vector3 eulers_from_RGM_data(RGFileImport.RGRGMFile.RGMMPSOItem i)
        {
            Matrix4x4 m = new Matrix4x4();
            m[0,0] = (new RGFileImport.Q4_28(i.rotation_matrix[0])).ToFloat();
            m[0,1] = (new RGFileImport.Q4_28(i.rotation_matrix[1])).ToFloat();
            m[0,2] = (new RGFileImport.Q4_28(i.rotation_matrix[2])).ToFloat();
            m[1,0] = (new RGFileImport.Q4_28(i.rotation_matrix[3])).ToFloat();
            m[1,1] = (new RGFileImport.Q4_28(i.rotation_matrix[4])).ToFloat();
            m[1,2] = (new RGFileImport.Q4_28(i.rotation_matrix[5])).ToFloat();
            m[2,0] = (new RGFileImport.Q4_28(i.rotation_matrix[6])).ToFloat();
            m[2,1] = (new RGFileImport.Q4_28(i.rotation_matrix[7])).ToFloat();
            m[2,2] = (new RGFileImport.Q4_28(i.rotation_matrix[8])).ToFloat();
            m[3,3] = 1;
            return m.rotation.eulerAngles;
        }

        for(int i=0;i<filergm.MPSO.items.Count;i++)
        {
            try{
                float posx =  ((float)(filergm.MPSO.items[i].posx)*RGM_SCALE);
                float posy = -((float)((int)0xFFFF-(int)filergm.MPSO.items[i].posy)*RGM_SCALE);
                float posz = ((float)(int)0xFFFF-(int)filergm.MPSO.items[i].height)*RGM_SCALE_HEIGHT;
                posx += RGM_X_OFS;
                posy += RGM_Y_OFS;
                posz += RGM_OFS_HEIGHT;

                Vector3 eulers = eulers_from_RGM_data(filergm.MPSO.items[i]);
                add3DToScene($"{i}", filergm.MPSO.items[i].name, "ISLAND", new Vector3(posx,posz,posy), eulers);
            }
            catch(Exception ex)
            {
                Debug.Log($"BROKE {filergm.MPSO.items[i].name}: {ex}");
            }
        }
    }

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RG3DStore.LoadMeshIntermediatesROB("ISLAND");
        SetModel_wld(/*GameObject target,*/);
        LoadRGM("./game_3dfx/maps/ISLAND.RGM");
        // hell yeah single-line objects
//        add3DToScene("PALMTR03", "ISLAND", new Vector3(100,0,0));
//        add3DToScene("VILEGARD", "ISLAND", new Vector3(00,00,0));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
