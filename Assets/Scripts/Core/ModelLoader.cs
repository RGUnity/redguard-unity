using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public static class ModelLoader
{
    public static Dictionary<uint, RGScriptedObject> scriptedObjects;
    
    public static GameObject Load3DC(string f3DCname, string colname)
    {
        RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d, f3DCname, colname);
        GameObject obj = Add3DToScene($"3DC_{f3DCname}",  data_3D, Vector3.zero, Vector3.zero);
        return obj;
        
        // Todo: namespace cleanup
        //Debug.Log(Game.configData.redguardPath);
    }


    public static List<GameObject> LoadArea(string areaname, string palettename, string wldname)
    {
        List<GameObject> areaObjects;
        RG3DStore.LoadMeshIntermediatesROB(areaname);
        areaObjects = LoadRGM(areaname, palettename);
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
        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();

        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);
        meshCollider.sharedMesh = meshFilter.mesh;

        // Set Position & Rotation
        obj.transform.position = position;
        obj.transform.Rotate(eulers);
        
        return obj;
    }
    
    static readonly ProfilerMarker s_load_RGM = new ProfilerMarker("LoadRGM");
    static readonly ProfilerMarker s_load_MPOB = new ProfilerMarker("LoadMPOB");
    static readonly ProfilerMarker s_load_MPSO = new ProfilerMarker("LoadMPSO");
    static readonly ProfilerMarker s_load_MPSF = new ProfilerMarker("LoadMPSF");
    static readonly ProfilerMarker s_load_MPRP = new ProfilerMarker("LoadMPRP");

    private static List<GameObject> LoadRGM(string RGMname, string name_col)
    {
        scriptedObjects = new Dictionary<uint, RGScriptedObject>();
        List<GameObject> areaObjects = new List<GameObject>();
        List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(RGMname);
        List<RGRGMStore.RGRGMData> RGM_MPSFs = RGRGMStore.LoadMPSF(RGMname);
        List<RGRGMStore.RGRGMRopeData> RGM_MPRPs = RGRGMStore.LoadMPRP(RGMname);
        
        s_load_RGM.Begin();
        RGFileImport.RGRGMFile filergm = RGRGMStore.GetRGM(RGMname);
        s_load_RGM.End();
        RGRGMAnimStore.ReadAnim(filergm);
        RGRGMScriptStore.ReadScript(filergm);

        s_load_MPOB.Begin();
        for(int i=0;i<filergm.MPOB.items.Count;i++)
        {
            try
            {
                // Create scripted objects
				GameObject spawned = new GameObject($"B_{i:D3}_{filergm.MPOB.items[i].scriptName}");
                areaObjects.Add(spawned);
                
				spawned.AddComponent<RGScriptedObject>();
				spawned.GetComponent<RGScriptedObject>().Instanciate(filergm.MPOB.items[i], filergm, name_col);
                scriptedObjects.Add(filergm.MPOB.items[i].id, spawned.GetComponent<RGScriptedObject>());

//                spawned.GetComponent<RGScriptedObject>().SetAnim(20,0);
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: B{i:D3}_{filergm.MPOB.items[i].scriptName}: {ex.Message} : {ex.StackTrace}");
            }
        }
        s_load_MPOB.End();
        s_load_MPSO.Begin();
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
        s_load_MPSO.End();
        s_load_MPSF.Begin();
        for(int i=0;i<RGM_MPSFs.Count;i++)
        {
            // Create flats
            RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_flat, RGM_MPSFs[i].name, name_col);
            GameObject obj = Add3DToScene($"F{i:D3}_{RGM_MPSFs[i].name}",  data_3D, RGM_MPSFs[i].position, RGM_MPSFs[i].rotation);
            areaObjects.Add(obj);
        }
        s_load_MPSF.End();
        s_load_MPRP.Begin();
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
        s_load_MPRP.End();

        return areaObjects;
    }
    
    // Spawns the terrain
    private static GameObject SetModel_wld(string name_wld, string name_col)
    {
        // Load the WLD data
        string filename_wld = new string(Game.pathManager.GetMapsFolder() + $"/{name_wld}.WLD");
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
