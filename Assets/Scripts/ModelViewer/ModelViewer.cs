using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModelViewer : MonoBehaviour
{
    [SerializeField] public ModelViewer_Settings settings;
    [SerializeField] private ModelViewer_GUI gui;
    [SerializeField] private ModelViewer_Camera mvCam;
    [SerializeField] public GLTFExporter glTFExporter;
    [SerializeField] private GameObject objectRoot;
    [SerializeField] private GameObject cameraRoot;
    [SerializeField] private string pathOverride;

    public GameObject _objectRootGenerated;
    public string exportDirectory;
    private List<GameObject> loadedObjects;

    void Start()
    {
        // if a path override is set, use that
        if (pathOverride.Length > 0)
        {
            Game.pathManager.SetPath(pathOverride);
            print("Redguard Path Override found in Scene. Value: " + Game.pathManager.GetRootFolder());
        }
        
        // Start in Viewer Mode
        ViewerMode_Areas();
        
        // Set default Export path
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        exportDirectory = desktopPath + "/Redguard_Exports/";
        gui.exportPathInput.text = exportDirectory;
    }
    
    // Mode to for viewing full levels
    public void ViewerMode_Areas()
    {
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
        
        // Switch the GUI to level mode
        gui.UpdateUI_Levels();
        gui.objectDropDown.interactable = false;
        gui.overlays_AreaMode.SetActive(true);
        gui.ClearIsolationDropdown();
    }
    
    // Mode to viewing individual Models
    public void ViewerMode_Models()
    {
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
    
        // Switch the GUI to model mode
        gui.UpdateUI_Models();
        gui.overlays_AreaMode.SetActive(false);
    }
    
    // Mode to viewing textures
    public void ViewerMode_Textures()
    {
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
        
        // Switch the GUI to texture mode
        gui.UpdateUI_Textures();
        gui.overlays_AreaMode.SetActive(false);
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
        settings.RequestEnableAnimations(false);
        
        print("Loaded object: " + f3DCname);

        //SwitchTextureFilterMode(FilterMode.Point);
    }
    
    public void SpawnArea(string RGM, string WLD, string COL)
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        Destroy(_objectRootGenerated);
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
        _objectRootGenerated.name = RGM;

        // Create all objects of that area and parent them under the root
        loadedObjects = ModelLoader.LoadArea(RGM, COL, WLD);
        
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
        
        print("Loaded area: " + RGM);
        RGMeshStore.DumpDict();
        RG3DStore.DumpDict();
        RGRGMStore.DumpDict();
        RGTexStore.DumpDict();
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
