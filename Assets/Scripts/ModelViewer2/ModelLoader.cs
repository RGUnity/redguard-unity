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
    
    public static GameObject Load3DC(string filename)
    {
        RG3DStore.LoadMeshIntermediate3DC(filename);
        GameObject obj = Add3DToScene("3DC_", filename, "OBSERVAT", Vector3.zero, Vector3.zero);
        return obj;
    }
    
    public static List<GameObject> LoadArea(string filename)
    {
        List<GameObject> areaObjects;
        
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST
        
        switch (filename)
        {
            case "BELLTOWR":
                RG3DStore.LoadMeshIntermediatesROB("BELLTOWR");
                return LoadRGM(RedguardPath + "/maps/BELLTOWR.RGM", "ISLAND");
            
            case "BRENNANS":
                RG3DStore.LoadMeshIntermediatesROB("BRENNANS");
                return LoadRGM(RedguardPath + "/maps/BRENNANS.RGM", "ISLAND");
            
            case "CARTOGR":
                RG3DStore.LoadMeshIntermediatesROB("CARTOGR");
                return LoadRGM(RedguardPath + "/maps/CARTOGR.RGM", "ISLAND");
            
            case "CATACOMB":
                RG3DStore.LoadMeshIntermediatesROB("CATACOMB");
                return LoadRGM(RedguardPath + "/maps/CATACOMB.RGM", "ISLAND");
            
            case "CAVERNS":
                RG3DStore.LoadMeshIntermediatesROB("CAVERNS");
                return LoadRGM(RedguardPath + "/maps/CAVERNS.RGM", "ISLAND");
            
            case "DRINT":
                RG3DStore.LoadMeshIntermediatesROB("DRINT");
                return LoadRGM(RedguardPath + "/maps/DRINT.RGM", "ISLAND");
            
            case "EXTPALAC":
                RG3DStore.LoadMeshIntermediatesROB("EXTPALAC");
                areaObjects = LoadRGM(RedguardPath + "/maps/EXTPALAC.RGM", "ISLAND");
                areaObjects.Add(SetModel_wld("ISLAND", "ISLAND"));
                return areaObjects;
            
            case "GERRICKS":
                RG3DStore.LoadMeshIntermediatesROB("GERRICKS");
                return LoadRGM(RedguardPath + "/maps/GERRICKS.RGM", "ISLAND");
            
            case "HARBOTWR":
                RG3DStore.LoadMeshIntermediatesROB("HARBTOWR");
                return LoadRGM(RedguardPath + "/maps/HARBTOWR.RGM", "ISLAND");
            
            case "HIDEINT":
                RG3DStore.LoadMeshIntermediatesROB("HIDEINT");
                areaObjects = LoadRGM(RedguardPath + "/maps/HIDEINT.RGM", "HIDEOUT");
                areaObjects.Add(SetModel_wld("HIDEOUT", "HIDEOUT"));
                return areaObjects;
            
            case "HIDEOUT":
                RG3DStore.LoadMeshIntermediatesROB("HIDEOUT");
                areaObjects = LoadRGM(RedguardPath + "/maps/HIDEOUT.RGM", "HIDEOUT");
                areaObjects.Add(SetModel_wld("HIDEOUT", "HIDEOUT"));
                return areaObjects;
            
            case "INVENTRY":
                // RG3DStore.LoadMeshIntermediatesROB("INVENTRY");
                // LoadRGM(_redguardPath + "/maps/INVENTRY.RGM", "ISLAND");
            
            case "ISLAND":
                RG3DStore.LoadMeshIntermediatesROB("ISLAND");
                areaObjects = LoadRGM(RedguardPath + "/maps/ISLAND.RGM", "ISLAND");
                areaObjects.Add(SetModel_wld("ISLAND", "ISLAND"));
                return areaObjects;
            
            case "JAILINT":
                RG3DStore.LoadMeshIntermediatesROB("JAILINT");
                return LoadRGM(RedguardPath + "/maps/JAILINT.RGM", "ISLAND");
            
            case "JFFERS":
                RG3DStore.LoadMeshIntermediatesROB("JFFERS");
                return LoadRGM(RedguardPath + "/maps/JFFERS.RGM", "ISLAND");

            case "MENU":
                // RG3DStore.LoadMeshIntermediatesROB("MENU");
                // LoadRGM(_redguardPath + "/maps/MENU.RGM", "ISLAND");

            case "MGUILD":
                RG3DStore.LoadMeshIntermediatesROB("MGUILD");
                return LoadRGM(RedguardPath + "/maps/MGUILD.RGM", "ISLAND");
            
            case "NECRISLE":
                RG3DStore.LoadMeshIntermediatesROB("NECRISLE");
                areaObjects = LoadRGM(RedguardPath + "/maps/NECRISLE.RGM", "NECRO");
                areaObjects.Add(SetModel_wld("NECRISLE", "NECRO"));
                return areaObjects;

            case "NECRTOWR":
                RG3DStore.LoadMeshIntermediatesROB("NECRTOWR");
                return LoadRGM(RedguardPath + "/maps/NECRTOWR.RGM", "NECRO");
            
            case "OBSERVE":
                RG3DStore.LoadMeshIntermediatesROB("OBSERVE");
                return LoadRGM(RedguardPath + "/maps/OBSERVE.RGM", "ISLAND");

            case "PALACE":
                RG3DStore.LoadMeshIntermediatesROB("PALACE");
                return LoadRGM(RedguardPath + "/maps/PALACE.RGM", "ISLAND");

            case "PALATEST":
                // RG3DStore.LoadMeshIntermediatesROB("PALATEST");
                // LoadRGM(_redguardPath + "/maps/PALATEST.RGM", "ISLAND");
            
            case "ROLLOS":
                RG3DStore.LoadMeshIntermediatesROB("ROLLOS");
                return LoadRGM(RedguardPath + "/maps/ROLLOS.RGM", "ISLAND");

            case "SILVER1":
                RG3DStore.LoadMeshIntermediatesROB("SILVER1");
                return LoadRGM(RedguardPath + "/maps/SILVER1.RGM", "ISLAND");

            case "SILVER2":
                RG3DStore.LoadMeshIntermediatesROB("SILVER2");
                return LoadRGM(RedguardPath + "/maps/SILVER2.RGM", "ISLAND");

            case "SMDEN":
                RG3DStore.LoadMeshIntermediatesROB("SMDEN");
                return LoadRGM(RedguardPath + "/maps/SMDEN.RGM", "ISLAND");

            case "START":
                RG3DStore.LoadMeshIntermediatesROB("START");
                return LoadRGM(RedguardPath + "/maps/START.RGM", "ISLAND");

            case "TAVERN":
                RG3DStore.LoadMeshIntermediatesROB("TAVERN");
                return LoadRGM(RedguardPath + "/maps/TAVERN.RGM", "ISLAND");

            case "TEMPLE":
                RG3DStore.LoadMeshIntermediatesROB("TEMPLE");
                return LoadRGM(RedguardPath + "/maps/TEMPLE.RGM", "ISLAND");

            case "TEMPTEST":
                // RG3DStore.LoadMeshIntermediatesROB("TEMPTEST");
                // LoadRGM(_redguardPath + "/maps/TEMPTEST.RGM", "ISLAND");

            case "VILE":
                RG3DStore.LoadMeshIntermediatesROB("VILE");
                return LoadRGM(RedguardPath + "/maps/VILE.RGM", "ISLAND");
        }

        return null;
    }

    private static GameObject Add3DToScene(string prefix, string name_3d, string name_pal,Vector3 position, Vector3 eulers)
    {
        // Create Unity Mesh Data
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        // Create new GameObject
        GameObject obj = new GameObject($"{prefix}_{name_3d}");
        
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
    
    private static List<GameObject> LoadRGM(string filename, string name_col)
    {
        List<GameObject> areaObjects = new List<GameObject>();
        List<RGRGMStore.RGRGMData> RGM_MPSOs = RGRGMStore.LoadMPSO(filename);
        List<RGRGMStore.RGRGMData> RGM_MPOBs = RGRGMStore.LoadMPOB(filename);
        
        for(int i=0;i<RGM_MPOBs.Count;i++)
        {
            try
            {
                // Create static objects
                GameObject obj = Add3DToScene($"B{i:D3}", RGM_MPOBs[i].name2, name_col, RGM_MPOBs[i].position, RGM_MPOBs[i].rotation);
                areaObjects.Add(obj);
            }
            catch(Exception ex)
            {
                Debug.LogWarning($"ERR: B{i:D3}: {ex.Message}");
            }
        }
        for(int i=0;i<RGM_MPSOs.Count;i++)
        {
            // Create dynamic objects
           GameObject obj = Add3DToScene($"S{i:D3}", RGM_MPSOs[i].name, name_col, RGM_MPSOs[i].position, RGM_MPSOs[i].rotation);
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
