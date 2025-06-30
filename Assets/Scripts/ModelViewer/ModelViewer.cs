using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using GLTFast;
using GLTFast.Export;
using TMPro;
using UnityEngine.Serialization;

public class ModelViewer : MonoBehaviour
{
    [SerializeField] public ModelViewer_Settings settings;
    [SerializeField] private ModelViewer_GUI gui;
    [SerializeField] private ModelViewer_Camera mvCam;
    [SerializeField] private GameObject objectRoot;
    [SerializeField] private GameObject cameraRoot;
    [SerializeField] private string pathOverride;

    private GameObject _objectRootGenerated;
    private string exportDirectory;
    private List<GameObject> loadedObjects;

    // this should be read in from ini file
    private Dictionary<string, (string,string)> AreaCOLDictionary;


    void Start()
    {
        buildAreaCOLDictionary();

        // if a path override is set, use that
        if (pathOverride.Length > 0)
        {
            ModelLoader.RedguardPath = pathOverride;
            print("Redgaurd Path Override found in Scene. Value: " + ModelLoader.RedguardPath);
        }
        // If there is no override, look for a path in the PlayerPrefs
        else if (PlayerPrefs.HasKey("ViewerRedguardPath"))
        {
            ModelLoader.RedguardPath = PlayerPrefs.GetString("ViewerRedguardPath");
            print("Path found in PlayerPrefs. Value: " + ModelLoader.RedguardPath);
        }
        // Show the default path
        else
        {
            print("Using Default Path. Value: " + ModelLoader.RedguardPath);
        }
        
        // Update Path displayed in UI
        gui.pathInput.text = ModelLoader.RedguardPath;
        
        // Start in Viewer Mode
        ViewerMode_Areas();
        
        // Set default Export path
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        exportDirectory = desktopPath + "/Redguard_Exports/";
        gui.exportPathInput.text = exportDirectory;
    }
    
    void buildAreaCOLDictionary()
    {
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST

        AreaCOLDictionary = new Dictionary<string, (string,string)>();
        //                     area name    COL      WLD (can be empty)
        AreaCOLDictionary.Add("BELLTOWR", ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("BRENNANS", ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("CARTOGR",  ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("CATACOMB", ("CATACOMB", ""));
        AreaCOLDictionary.Add("CAVERNS",  ("CAVETEST", ""));
        AreaCOLDictionary.Add("DRINT",    ("OBSERVAT", ""));
        AreaCOLDictionary.Add("EXTPALAC", ("ISLE3DFX", "ISLAND"));
        AreaCOLDictionary.Add("GERRICKS", ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("HARBTOWR", ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("HIDEINT",  ("HIDEOUT","HIDEOUT"));
        AreaCOLDictionary.Add("HIDEOUT",  ("HIDEOUT","HIDEOUT"));
        AreaCOLDictionary.Add("ISLAND",   ("ISLE3DFX", "ISLAND"));
        AreaCOLDictionary.Add("JAILINT",  ("REDCAVE", ""));
        AreaCOLDictionary.Add("JFFERS",   ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("MGUILD",   ("MGUILD", ""));
        AreaCOLDictionary.Add("NECRISLE", ("NECRO",  "NECRISLE"));
        AreaCOLDictionary.Add("NECRTOWR", ("NECRO",  ""));
        AreaCOLDictionary.Add("OBSERVE",  ("OBSERVAT", ""));
        AreaCOLDictionary.Add("PALACE",   ("PALACE00", ""));
        AreaCOLDictionary.Add("ROLLOS",   ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("SILVER1",  ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("SILVER2",  ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("SMDEN",    ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("START",    ("ISLE3DFX", "HIDEOUT"));
        AreaCOLDictionary.Add("TAVERN",   ("TAVERN", ""));
        AreaCOLDictionary.Add("TEMPLE",   ("ISLE3DFX", ""));
        AreaCOLDictionary.Add("VILE",     ("ISLE3DFX", ""));
    }
    
    private bool IsPathValid()
    {
        try
        {
            if (File.Exists(ModelLoader.RedguardPath + "/REDGUARD.EXE"))
            {
                print("Using Folder " + ModelLoader.RedguardPath);
                RG3DStore.path_to_game = ModelLoader.RedguardPath;
                RGTexStore.path_to_game = ModelLoader.RedguardPath;

                // Switch the GUI to level mode
                gui.PathErrorMode(false);

                return true;
            }
            else
            {
                Debug.LogWarning("Folder does not exist: " + ModelLoader.RedguardPath);
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
    public void ViewerMode_Areas()
    {
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
        
        if (IsPathValid())
        {
            // Switch the GUI to level mode
            gui.UpdateUI_Levels();
            gui.objectDropDown.interactable = false;
            gui.overlays_AreaMode.SetActive(true);
            gui.ClearIsolationDropdown();
        }
    }
    
    // Mode to viewing individual Models
    public void ViewerMode_Models()
    {
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
        
        if (IsPathValid())
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ModelLoader.RedguardPath + "/fxart");
        
            // Switch the GUI to model mode
            gui.UpdateUI_Models(dirInfo.GetFiles("*.3DC"));
            gui.overlays_AreaMode.SetActive(false);
        }
    }
    
    // Mode to viewing textures
    public void ViewerMode_Textures()
    {
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
        
        if (IsPathValid())
        {
            // Switch the GUI to texture mode
            gui.UpdateUI_Textures();
            gui.overlays_AreaMode.SetActive(false);
        }
    }

    public void Spawn3DC(string f3DCname, string colname)
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        Destroy(_objectRootGenerated);
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
        _objectRootGenerated.name = f3DCname;
        
        // Create the object and parent it under the root
        GameObject obj = ModelLoader.Load3DC(f3DCname, colname);
        obj.transform.SetParent(_objectRootGenerated.transform);
        
        loadedObjects = new List<GameObject>();
        loadedObjects.Add(obj);

        mvCam.useFlyMode = false;
        mvCam.FrameObject(_objectRootGenerated);
        
        settings.ToggleFlyMode(false);
        settings.RequestEnableTextureFiltering(true);
        settings.RequestEnableAnimations(true);

        
        print("Loaded object: " + f3DCname);

        //SwitchTextureFilterMode(FilterMode.Point);
    }
    
    public void SpawnArea(string areaname)
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        Destroy(_objectRootGenerated);
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
        _objectRootGenerated.name = areaname;

        // Create all objects of that area and parent them under the root
        string colname = AreaCOLDictionary[areaname].Item1;
        string wldname = AreaCOLDictionary[areaname].Item2;
        
        loadedObjects = ModelLoader.LoadArea(areaname, colname, wldname);
        
        foreach (var obj in loadedObjects)
        {
            obj.transform.SetParent(_objectRootGenerated.transform);
        }
        
        settings.ToggleFlyMode(false);
        settings.RequestEnableTextureFiltering(true);
        settings.RequestEnableAnimations(true);
        
        mvCam.useFlyMode = false;
        mvCam.FrameObject(_objectRootGenerated);
        
        gui.objectDropDown.interactable = true;
        gui.PopulateIsolationDropdown(loadedObjects);
        
        print("Loaded area: " + areaname);
        RGMeshStore.DumpDict();
        RG3DStore.DumpDict();
        RGRGMStore.DumpDict();
        RGTexStore.DumpDict();

    }
    
    public async Task ExportGLTF()
    {
        // Create Subfolder for Object
        var exportDirWithSubfolder = exportDirectory + _objectRootGenerated.name;
        var fullGLTFPath = exportDirWithSubfolder + "/" + _objectRootGenerated.name + ".gltf";

        // If missing, create the target folder
        if (!Directory.Exists(exportDirWithSubfolder))
        {
            Directory.CreateDirectory(exportDirWithSubfolder);
        }
        
        // Define objects to export
        var objectsToExport = new GameObject[] {_objectRootGenerated};
        
        var export = new GameObjectExport();
        export.AddScene(objectsToExport);
        
        // Async glTF export
        var success = await export.SaveToFileAndDispose(fullGLTFPath);

        if (success)
        {
            print("Exported " + fullGLTFPath);
        }
        else
        {
            Debug.LogError("Something went wrong trying to export the model." + fullGLTFPath);
        }
    }

    public void IsolateObject(string selection)
    {
        if (selection == "None")
        {
            // Show all objects
            foreach (var obj in loadedObjects)
            {
                obj.gameObject.SetActive(true);
            }
        }
        else
        {
            // show the selected object
            foreach (var obj in loadedObjects)
            {
                if (obj.name == selection)
                {
                    obj.gameObject.SetActive(true);
                }
                else
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }

        mvCam.useFlyMode = false;
        mvCam.FrameObject(objectRoot);
    }
    
    public void SwitchTextureFilterMode(FilterMode mode)
    {
        foreach (var mat in RGTexStore.MaterialDict)
        {
            mat.Value.mainTexture.filterMode = mode;
        }
    }

    public void EnableAnimations(bool enableAnimations)
    {
        foreach (var obj in loadedObjects)
        {
            if (obj.TryGetComponent(out RGScriptedObject rgso))
            {
                if (rgso.type == RGScriptedObject.ScriptedObjectType.scriptedobject_animated)
                {
                    rgso.allowAnimation = enableAnimations;
                }
            }
        }
    }
}
