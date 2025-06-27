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
        
        RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d, f3DCname, colname);
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

    private static GameObject Add3DToScene(string name, RGMeshStore.UnityData_3D data_3D, Vector3 position, Vector3 eulers)
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
        List<RGRGMStore.RGRGMData> RGM_MPSFs = RGRGMStore.LoadMPSF(RGMname);
        List<RGRGMStore.RGRGMRopeData> RGM_MPRPs = RGRGMStore.LoadMPRP(RGMname);
        
        RGFileImport.RGRGMFile filergm = RGRGMStore.GetRGM(RGMname);
        for(int i=0;i<filergm.MPOB.items.Count;i++)
        {
            try
            {
                // Create scripted objects
				GameObject spawned = new GameObject($"B_{i:D3}_{filergm.MPOB.items[i].scriptName}");
                areaObjects.Add(spawned);
                
				spawned.AddComponent<RGScriptedObject>();
				spawned.GetComponent<RGScriptedObject>().Instanciate(filergm.MPOB.items[i], filergm, name_col);
                spawned.GetComponent<RGScriptedObject>().SetAnim(20);
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: B{i:D3}_{filergm.MPOB.items[i].scriptName}: {ex.Message} : {ex.StackTrace}");
            }
        }
        for(int i=0;i<RGM_MPSOs.Count;i++)
        {
            try
            {
                // Create static objects
                RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d, RGM_MPSOs[i].name, name_col);
                GameObject obj = Add3DToScene($"S{i:D3}_{RGM_MPSOs[i].name}",  data_3D, RGM_MPSOs[i].position, RGM_MPSOs[i].rotation);
                obj.isStatic = true;
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
            RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_flat, RGM_MPSFs[i].name, name_col);
            GameObject obj = Add3DToScene($"F{i:D3}_{RGM_MPSFs[i].name}",  data_3D, RGM_MPSFs[i].position, RGM_MPSFs[i].rotation);
           areaObjects.Add(obj);
        }
        for(int i=0;i<RGM_MPRPs.Count;i++)
        {
            try
            {
                // Create static objects
                RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d, RGM_MPRPs[i].ropeModel, name_col);
                Vector3 pos = RGM_MPRPs[i].position; 
                int j = 0;
                for(j=0;j<RGM_MPRPs[i].count;j++)
                {
                    pos.y -= 0.8f; // TODO: is this always correct?
                    GameObject obj = Add3DToScene($"R{i:D3}_{j:D3}_{RGM_MPRPs[i].ropeModel}",  data_3D, pos, new Vector3(0.0f,0.0f,0.0f));
                   areaObjects.Add(obj);
                }
                if(RGM_MPRPs[i].staticModel != null)
                {
                    pos.y -= 0.8f; // TODO: is this always correct?
                    GameObject obj = Add3DToScene($"R{i:D3}_{j:D3}_{RGM_MPRPs[i].staticModel}",  data_3D, pos, new Vector3(0.0f,0.0f,0.0f));
                   areaObjects.Add(obj);
                }
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: R{i:D3}: {ex.Message}");
            }
        }

        return areaObjects;
    }
    
    // Spawns the terrain
    private static GameObject SetModel_wld(string name_wld, string name_col)
    {
        // Load the WLD data
        string filename_wld = new string(RedguardPath + $"/maps/{name_wld}.WLD");
        RGMeshStore.UnityData_WLD data_WLD = RGMeshStore.WLD2Mesh(filename_wld, name_col);
        
        // Build the GameObject
        GameObject obj_wld = new GameObject();
        obj_wld.isStatic = true;
        obj_wld.name = "Terrain";
        
        obj_wld.AddComponent<MeshFilter>().mesh = data_WLD.mesh;
        obj_wld.AddComponent<MeshRenderer>().SetMaterials(data_WLD.materials);
        return obj_wld;
    }
}
