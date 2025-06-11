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
        
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST
       /* 
        switch (filename)
        {
            case "BELLTOWR":
            
            case "BRENNANS":
                RG3DStore.LoadMeshIntermediatesROB("BRENNANS");
                return LoadRGM(RedguardPath, "BRENNANS", "ISLAND");
            
            case "CARTOGR":
                RG3DStore.LoadMeshIntermediatesROB("CARTOGR");
                return LoadRGM(RedguardPath, "CARTOGR", "ISLAND");
            
            case "CATACOMB":
                RG3DStore.LoadMeshIntermediatesROB("CATACOMB");
                return LoadRGM(RedguardPath, "CATACOMB", "ISLAND");
            
            case "CAVERNS":
                RG3DStore.LoadMeshIntermediatesROB("CAVERNS");
                return LoadRGM(RedguardPath, "CAVERNS", "ISLAND");
            
            case "DRINT":
                RG3DStore.LoadMeshIntermediatesROB("DRINT");
                return LoadRGM(RedguardPath, "DRINT", "ISLAND");
            
            case "EXTPALAC":
                RG3DStore.LoadMeshIntermediatesROB("EXTPALAC");
                areaObjects = LoadRGM(RedguardPath, "EXTPALAC", "ISLAND");
                areaObjects.Add(SetModel_wld("ISLAND", "ISLAND"));
                return areaObjects;
            
            case "GERRICKS":
                RG3DStore.LoadMeshIntermediatesROB("GERRICKS");
                return LoadRGM(RedguardPath, "GERRICKS", "ISLAND");
            
            case "HARBOTWR":
                RG3DStore.LoadMeshIntermediatesROB("HARBTOWR");
                return LoadRGM(RedguardPath, "HARBTOWR", "ISLAND");
            
            case "HIDEINT":
                RG3DStore.LoadMeshIntermediatesROB("HIDEINT");
                areaObjects = LoadRGM(RedguardPath, "HIDEINT", "HIDEOUT");
                areaObjects.Add(SetModel_wld("HIDEOUT", "HIDEOUT"));
                return areaObjects;
            
            case "HIDEOUT":
                RG3DStore.LoadMeshIntermediatesROB("HIDEOUT");
                areaObjects = LoadRGM(RedguardPath, "HIDEOUT", "HIDEOUT");
                areaObjects.Add(SetModel_wld("HIDEOUT", "HIDEOUT"));
                return areaObjects;
            
            case "INVENTRY":
                // RG3DStore.LoadMeshIntermediatesROB("INVENTRY");
                // LoadRGM(_redguardPath + "INVENTRY", "ISLAND");
            
            case "ISLAND":
                RG3DStore.LoadMeshIntermediatesROB("ISLAND");
                areaObjects = LoadRGM(RedguardPath, "ISLAND", "ISLAND");
                areaObjects.Add(SetModel_wld("ISLAND", "ISLAND"));
                return areaObjects;
            
            case "JAILINT":
                RG3DStore.LoadMeshIntermediatesROB("JAILINT");
                return LoadRGM(RedguardPath, "JAILINT", "ISLAND");
            
            case "JFFERS":
                RG3DStore.LoadMeshIntermediatesROB("JFFERS");
                return LoadRGM(RedguardPath, "JFFERS", "ISLAND");

            case "MENU":
                // RG3DStore.LoadMeshIntermediatesROB("MENU");
                // LoadRGM(_redguardPath + "MENU", "ISLAND");

            case "MGUILD":
                RG3DStore.LoadMeshIntermediatesROB("MGUILD");
                return LoadRGM(RedguardPath, "MGUILD", "ISLAND");
            
            case "NECRISLE":
                RG3DStore.LoadMeshIntermediatesROB("NECRISLE");
                areaObjects = LoadRGM(RedguardPath, "NECRISLE", "NECRO");
                areaObjects.Add(SetModel_wld("NECRISLE", "NECRO"));
                return areaObjects;

            case "NECRTOWR":
                RG3DStore.LoadMeshIntermediatesROB("NECRTOWR");
                return LoadRGM(RedguardPath, "NECRTOWR", "NECRO");
            
            case "OBSERVE":
                RG3DStore.LoadMeshIntermediatesROB("OBSERVE");
                return LoadRGM(RedguardPath, "OBSERVE", "ISLAND");

            case "PALACE":
                RG3DStore.LoadMeshIntermediatesROB("PALACE");
                return LoadRGM(RedguardPath, "PALACE", "ISLAND");

            case "PALATEST":
                // RG3DStore.LoadMeshIntermediatesROB("PALATEST");
                // LoadRGM(_redguardPath + "PALATEST", "ISLAND");
            
            case "ROLLOS":
                RG3DStore.LoadMeshIntermediatesROB("ROLLOS");
                return LoadRGM(RedguardPath, "ROLLOS", "ISLAND");

            case "SILVER1":
                RG3DStore.LoadMeshIntermediatesROB("SILVER1");
                return LoadRGM(RedguardPath, "SILVER1", "ISLAND");

            case "SILVER2":
                RG3DStore.LoadMeshIntermediatesROB("SILVER2");
                return LoadRGM(RedguardPath, "SILVER2", "ISLAND");

            case "SMDEN":
                RG3DStore.LoadMeshIntermediatesROB("SMDEN");
                return LoadRGM(RedguardPath, "SMDEN", "ISLAND");

            case "START":
                RG3DStore.LoadMeshIntermediatesROB("START");
                return LoadRGM(RedguardPath, "START", "ISLAND");

            case "TAVERN":
                RG3DStore.LoadMeshIntermediatesROB("TAVERN");
                return LoadRGM(RedguardPath, "TAVERN", "ISLAND");

            case "TEMPLE":
                RG3DStore.LoadMeshIntermediatesROB("TEMPLE");
                return LoadRGM(RedguardPath, "TEMPLE", "ISLAND");

            case "TEMPTEST":
                // RG3DStore.LoadMeshIntermediatesROB("TEMPTEST");
                // LoadRGM(_redguardPath + "TEMPTEST", "ISLAND");

            case "VILE":
                RG3DStore.LoadMeshIntermediatesROB("VILE");
                return LoadRGM(RedguardPath, "VILE", "ISLAND");
        }
        */

        return null;
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

        List<GameObject> areaObjects = new List<GameObject>();
        List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(RGMname);
        List<RGRGMStore.RGRGMData> RGM_MPOBs = RGRGMStore.LoadMPOB(RGMname);
        List<RGRGMStore.RGRGMData> RGM_MPSFs = RGRGMStore.LoadMPSF(RGMname);
        
        for(int i=0;i<RGM_MPOBs.Count;i++)
        {
            try
            {
                // Create static objects
                RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(RGM_MPOBs[i].name2, name_col);
                GameObject obj = Add3DToScene($"B{i:D3}_{RGM_MPOBs[i].name2}",  data_3D, RGM_MPOBs[i].position, RGM_MPOBs[i].rotation);
                areaObjects.Add(obj);
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: B{i:D3}: {ex.Message}");
            }
        }
        for(int i=0;i<RGM_MPSOs.Count;i++)
        {
            try
            {
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
