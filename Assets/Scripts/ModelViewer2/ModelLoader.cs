using System;
using System.Collections.Generic;
using UnityEngine;

public static class ModelLoader
{
    public static string RedguardPath = "C:/Program Files (x86)/GOG Galaxy/Games/Redguard/Redguard";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    static void Start()
    {
        // todo: load RedguardPath from config file
    }
    
    public static GameObject Load3DC(string f3DCname, string colname)
    {
        RGRGMStore.maps_path = RedguardPath +"/maps/";
        RG3DStore.fxart_path = RedguardPath +"/fxart/";
        RGTexStore.fxart_path = RedguardPath +"/fxart/";
        
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(f3DCname, colname);
        GameObject obj = Add3DToScene($"3DC_{f3DCname}",  data_3D, Vector3.zero, Vector3.zero);
        return obj;
    }


    public static List<GameObject> LoadArea(string areaname, string palettename, string wldname)
    {
        List<GameObject> areaObjects;
        RG3DStore.LoadMeshIntermediatesROB(areaname);
        areaObjects = LoadRGM(RedguardPath, areaname, palettename);
        if(!String.IsNullOrEmpty(wldname))
            areaObjects.Add(SetModel_wld(wldname, palettename));
        return areaObjects;
   }

    private static GameObject Add3DToScene(string name, RG2Mesh.UnityData_3D data_3D, Vector3 position, Vector3 eulers)
    {
        // Create new GameObject
        GameObject obj = new GameObject(name);
        
        // Add Mesh Components
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();

        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);

        // Set Position & Rotation
        obj.transform.position = position;
        obj.transform.Rotate(eulers);
        
        return obj;
    }
    
    private static List<GameObject> LoadRGM(string gamepath, string RGMname, string name_col)
    {
        RGRGMStore.maps_path = gamepath+"/maps/";
        RG3DStore.fxart_path = gamepath+"/fxart/";
        RGTexStore.fxart_path = gamepath+"/fxart/";

        List<GameObject> areaObjects = new List<GameObject>();
        List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(RGMname);
        List<RGRGMStore.RGRGMData> RGM_MPOBs = RGRGMStore.LoadMPOB(RGMname);
        List<RGRGMStore.RGRGMData> RGM_MPSFs = RGRGMStore.LoadMPSF(RGMname);
        
        for(int i=0;i<RGM_MPOBs.Count;i++)
        {
			RGFileImport.RGRGMFile filergm = RGRGMStore.GetRGM(RGMname);
            try
            {
                // Create scripted objects
				GameObject spawned = new GameObject($"B_{i:D3}_{filergm.MPOB.items[i].scriptName}");
				spawned.AddComponent<RGScriptedObject>();
				spawned.GetComponent<RGScriptedObject>().instanciateRGScriptedObject(filergm.MPOB.items[i], filergm, name_col);

                areaObjects.Add(spawned);
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: B{i:D3}_{filergm.MPOB.items[i].scriptName}: {ex.Message}");
            }
        }
        for(int i=0;i<RGM_MPSOs.Count;i++)
        {
            try
            {
                // Create static objects
                RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(RGM_MPSOs[i].name, name_col);
                GameObject obj = Add3DToScene($"S{i:D3}_{RGM_MPSOs[i].name}",  data_3D, RGM_MPSOs[i].position, RGM_MPSOs[i].rotation);
               areaObjects.Add(obj);
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: S{i:D3}: {ex.Message}");
            }
        }
        for(int i=0;i<RGM_MPSFs.Count;i++)
        {
            // Create flats
            RG2Mesh.UnityData_3D data_3D = RG2Mesh.FLAT2Mesh(RGM_MPSFs[i].name, name_col);
            GameObject obj = Add3DToScene($"F{i:D3}_{RGM_MPSFs[i].name}",  data_3D, RGM_MPSFs[i].position, RGM_MPSFs[i].rotation);
           areaObjects.Add(obj);
        }


        return areaObjects;
    }
    
    // Spawns the terrain
    private static GameObject SetModel_wld(string name_wld, string name_col)
    {
        // Load the WLD data
        string filename_wld = new string(RedguardPath + $"/maps/{name_wld}.WLD");
        RG2Mesh.UnityData_WLD data_WLD = RG2Mesh.WLD2Mesh(filename_wld, name_col);
        
        // Build the GameObject
        GameObject obj_wld = new GameObject();
        obj_wld.name = "Terrain";
        
        obj_wld.AddComponent<MeshFilter>().mesh = data_WLD.mesh;
        obj_wld.AddComponent<MeshRenderer>().SetMaterials(data_WLD.materials);
        return obj_wld;
    }
}
