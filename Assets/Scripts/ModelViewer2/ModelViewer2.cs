using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ModelViewer2 : MonoBehaviour
{
    [SerializeField] public ModelViewer2_Settings settings;
    [SerializeField] private ModelViewer2_GUI gui;
    [SerializeField] private ModelViewer2_Camera mv2Cam;
    [SerializeField] private GameObject objectRoot;
    [SerializeField] private GameObject cameraRoot;
    [SerializeField] private float scrollSpeed = 5;
    [SerializeField] private Shader shader;

    private GameObject objectRootGenerated;

    void Start()
    {
        RGTexStore.shader = shader;
        ViewerMode_Levels();
    }

    private bool IsPathValid()
    {
        try
        {
            var dirInfo = new DirectoryInfo(settings.redguardPath);

            if (dirInfo.Exists)
            {
                print("Using Folder " + settings.redguardPath);
                RG3DStore.path_to_game = settings.redguardPath;
                RGTexStore.path_to_game = settings.redguardPath;

                // Switch the GUI to level mode
                gui.PathErrorMode(false);

                return true;
            }
            else
            {
                Debug.LogWarning("Folder does not exist: " + settings.redguardPath);
                gui.ClearButtonList();
                gui.PathErrorMode(true);
                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            gui.PathErrorMode(true);
            throw;
        }

        return false;
    }
    
    // Mode to for viewing full levels
    public void ViewerMode_Levels()
    {
        if (IsPathValid())
        {
            // Switch the GUI to level mode
            gui.UpdateUI_Levels();
        }
    }
    
    // Mode to viewing individual Models
    public void ViewerMode_Models()
    {
        if (IsPathValid())
        {
            DirectoryInfo dirInfo = new DirectoryInfo(settings.redguardPath + "/fxart");
        
            // Switch the GUI to model mode
            gui.UpdateUI_Models(dirInfo.GetFiles("*.3DC"));
        }
    }
    
    // Mode to viewing textures
    public void ViewerMode_Textures()
    {
        if (IsPathValid())
        {
            // Switch the GUI to texture mode
            gui.UpdateUI_Textures();
        }
    }
    
    // Spawns the terrain
    public void SetModel_wld(string name_wld, string texbsi, string name_col)
    {
        // Load the WLD data
        string filename_wld = new string(settings.redguardPath + $"/maps/{name_wld}.WLD");
        RG2Mesh.UnityData_WLD data_WLD = RG2Mesh.WLD2Mesh(filename_wld, name_col);
        
        // Build the GameObject
        GameObject obj_wld = new GameObject();
        obj_wld.name = "Terrain";
        obj_wld.transform.SetParent(objectRootGenerated.transform);
        
        obj_wld.AddComponent<MeshFilter>().mesh = data_WLD.mesh;
        obj_wld.AddComponent<MeshRenderer>().SetMaterials(data_WLD.materials);
    }
    
    // Spawns 3D or ROB objects
    public void add3DToScene(string prefix, string name_3d, string name_pal,Vector3 position, Vector3 eulers)
    {
        RG2Mesh.UnityData_3D data_3D = RG2Mesh.f3D2Mesh(name_3d, name_pal);

        GameObject spawned = new GameObject($"{prefix}_{name_3d}");
        
        objectRootGenerated.transform.SetParent(objectRoot.transform);
        objectRootGenerated.name = "Root_Generated_" + name_3d;
        spawned.transform.SetParent(objectRootGenerated.transform);
        
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
                Debug.LogWarning($"ERR: B{i:D3}: {ex.Message}");
            }
        }
        for(int i=0;i<RGM_MPSOs.Count;i++)
        {
            add3DToScene($"S{i:D3}", RGM_MPSOs[i].name, name_col, RGM_MPSOs[i].position, RGM_MPSOs[i].rotation);
        }
    }

    public void Load3DC(string filename)
    {
        Destroy(objectRootGenerated);
        
        objectRootGenerated = new GameObject();
        
        RG3DStore.LoadMeshIntermediate3DC(filename);
        add3DToScene(filename +"_", filename, "OBSERVAT", Vector3.zero, Vector3.zero);

        mv2Cam.FrameObject(objectRootGenerated);
    }
    
    // Stupid Hardcoded ROB Loading functions
    public void LoadROB(string filename)
    {
        // Destroy last objects
        Destroy(objectRootGenerated);
        
        objectRootGenerated = new GameObject();
        
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST
        
        switch (filename)
        {
            default:
                break;

            case "BELLTOWR":
                RG3DStore.LoadMeshIntermediatesROB("BELLTOWR");
                LoadRGM(settings.redguardPath + "/maps/BELLTOWR.RGM", "ISLAND");
                break;

            case "BRENNANS":
                RG3DStore.LoadMeshIntermediatesROB("BRENNANS");
                LoadRGM(settings.redguardPath + "/maps/BRENNANS.RGM", "ISLAND");
                break;

            case "CARTOGR":
                RG3DStore.LoadMeshIntermediatesROB("CARTOGR");
                LoadRGM(settings.redguardPath + "/maps/CARTOGR.RGM", "ISLAND");
                break;

            case "CATACOMB":
                RG3DStore.LoadMeshIntermediatesROB("CATACOMB");
                LoadRGM(settings.redguardPath + "/maps/CATACOMB.RGM", "ISLAND");
                break;

            case "CAVERNS":
                RG3DStore.LoadMeshIntermediatesROB("CAVERNS");
                LoadRGM(settings.redguardPath + "/maps/CAVERNS.RGM", "ISLAND");
                break;

            case "DRINT":
                RG3DStore.LoadMeshIntermediatesROB("DRINT");
                LoadRGM(settings.redguardPath + "/maps/DRINT.RGM", "ISLAND");
                break;

            case "EXTPALAC":
                RG3DStore.LoadMeshIntermediatesROB("EXTPALAC");
                SetModel_wld("ISLAND", "302", "ISLAND");
                LoadRGM(settings.redguardPath + "/maps/EXTPALAC.RGM", "ISLAND");
                break;

            case "GERRICKS":
                RG3DStore.LoadMeshIntermediatesROB("GERRICKS");
                LoadRGM(settings.redguardPath + "/maps/GERRICKS.RGM", "ISLAND");
                break;

            case "HARBOTWR":
                RG3DStore.LoadMeshIntermediatesROB("HARBTOWR");
                LoadRGM(settings.redguardPath + "/maps/HARBTOWR.RGM", "ISLAND");
                break;

            case "HIDEINT":
                RG3DStore.LoadMeshIntermediatesROB("HIDEINT");
                SetModel_wld("HIDEOUT", "302", "HIDEOUT");
                LoadRGM(settings.redguardPath + "/maps/HIDEINT.RGM", "HIDEOUT");
                break;

            case "HIDEOUT":
                RG3DStore.LoadMeshIntermediatesROB("HIDEOUT");
                SetModel_wld("HIDEOUT", "302", "HIDEOUT");
                LoadRGM(settings.redguardPath + "/maps/HIDEOUT.RGM", "HIDEOUT");
                break;

            case "INVENTRY":
                // RG3DStore.LoadMeshIntermediatesROB("INVENTRY");
                // LoadRGM(settings.redguardPath + "/maps/INVENTRY.RGM", "ISLAND");
                break;

            case "ISLAND":
                RG3DStore.LoadMeshIntermediatesROB("ISLAND");
                SetModel_wld("ISLAND", "302", "ISLAND");
                LoadRGM(settings.redguardPath + "/maps/ISLAND.RGM", "ISLAND");
                break;

            case "JAILINT":
                RG3DStore.LoadMeshIntermediatesROB("JAILINT");
                LoadRGM(settings.redguardPath + "/maps/JAILINT.RGM", "ISLAND");
                break;

            case "JFFERS":
                RG3DStore.LoadMeshIntermediatesROB("JFFERS");
                LoadRGM(settings.redguardPath + "/maps/JFFERS.RGM", "ISLAND");
                break;

            case "MENU":
                // RG3DStore.LoadMeshIntermediatesROB("MENU");
                // LoadRGM(settings.redguardPath + "/maps/MENU.RGM", "ISLAND");
                break;

            case "MGUILD":
                RG3DStore.LoadMeshIntermediatesROB("MGUILD");
                LoadRGM(settings.redguardPath + "/maps/MGUILD.RGM", "ISLAND");
                break;

            case "NECRISLE":
                RG3DStore.LoadMeshIntermediatesROB("NECRISLE");
                SetModel_wld("NECRISLE", "302", "NECRO");
                LoadRGM(settings.redguardPath + "/maps/NECRISLE.RGM", "NECRO");
                break;

            case "NECRTOWR":
                RG3DStore.LoadMeshIntermediatesROB("NECRTOWR");
                LoadRGM(settings.redguardPath + "/maps/NECRTOWR.RGM", "NECRO");
                break;

            case "OBSERVE":
                RG3DStore.LoadMeshIntermediatesROB("OBSERVE");
                LoadRGM(settings.redguardPath + "/maps/OBSERVE.RGM", "ISLAND");
                break;

            case "PALACE":
                RG3DStore.LoadMeshIntermediatesROB("PALACE");
                LoadRGM(settings.redguardPath + "/maps/PALACE.RGM", "ISLAND");
                break;

            case "PALATEST":
                // RG3DStore.LoadMeshIntermediatesROB("PALATEST");
                // LoadRGM(settings.redguardPath + "/maps/PALATEST.RGM", "ISLAND");
                break;

            case "ROLLOS":
                RG3DStore.LoadMeshIntermediatesROB("ROLLOS");
                LoadRGM(settings.redguardPath + "/maps/ROLLOS.RGM", "ISLAND");
                break;

            case "SILVER1":
                RG3DStore.LoadMeshIntermediatesROB("SILVER1");
                LoadRGM(settings.redguardPath + "/maps/SILVER1.RGM", "ISLAND");
                break;

            case "SILVER2":
                RG3DStore.LoadMeshIntermediatesROB("SILVER2");
                LoadRGM(settings.redguardPath + "/maps/SILVER2.RGM", "ISLAND");
                break;

            case "SMDEN":
                RG3DStore.LoadMeshIntermediatesROB("SMDEN");
                LoadRGM(settings.redguardPath + "/maps/SMDEN.RGM", "ISLAND");
                break;

            case "START":
                RG3DStore.LoadMeshIntermediatesROB("START");
                LoadRGM(settings.redguardPath + "/maps/START.RGM", "ISLAND");
                break;

            case "TAVERN":
                RG3DStore.LoadMeshIntermediatesROB("TAVERN");
                LoadRGM(settings.redguardPath + "/maps/TAVERN.RGM", "ISLAND");
                break;

            case "TEMPLE":
                RG3DStore.LoadMeshIntermediatesROB("TEMPLE");
                LoadRGM(settings.redguardPath + "/maps/TEMPLE.RGM", "ISLAND");
                break;

            case "TEMPTEST":
                // RG3DStore.LoadMeshIntermediatesROB("TEMPTEST");
                // LoadRGM(settings.redguardPath + "/maps/TEMPTEST.RGM", "ISLAND");
                break;

            case "VILE":
                RG3DStore.LoadMeshIntermediatesROB("VILE");
                LoadRGM(settings.redguardPath + "/maps/VILE.RGM", "ISLAND");
                break;
        }
        
        mv2Cam.FrameObject(objectRootGenerated);
    }
}
