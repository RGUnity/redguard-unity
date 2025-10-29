using System;
using System.Collections.Generic;
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

    public string loadedFileName;
    public string minimalLoadedFileName;
    public GameObject _objectRootGenerated;
    private string exportDirectory;
    public List<GameObject> loadedObjects;

    void Start()
    {
        // if a path override is set, use that
        if (pathOverride.Length > 0)
        {
            Game.pathManager.SetPath(pathOverride);
            print("Redguard Path Override found in Scene. Value: " + Game.pathManager.GetRootFolder());
        }
        
        // Start in Area Mode
        SwitchViewerMode(ViewerModes.Areas);
        
        // Set default Export path
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        exportDirectory = desktopPath + "/Redguard_Exports/";
        gui.exportPathInput.text = exportDirectory;
    }

    public void SwitchViewerMode(ViewerModes mode)
    {
        gui.UpdateGUI(mode);
    }
    
    public void SpawnModel(string f3Dname, ModelFileType fileType, string colname)
    {
        PrepareLoad();
        
        loadedObjects = new List<GameObject>();
        switch (fileType)
        {
            case ModelFileType.file3D:
                loadedObjects.Add(ModelLoader.Load3D(f3Dname, colname));
                loadedFileName = "/Redguard/fxart/" + f3Dname + ".3D";
                break;
            case ModelFileType.file3DC:
                loadedObjects.Add(ModelLoader.Load3DC(f3Dname, colname));
                loadedFileName = "/Redguard/fxart/" + f3Dname + ".3DC";
                break;
            case ModelFileType.fileROB:
                loadedObjects = ModelLoader.LoadROB(f3Dname, colname);
                loadedFileName = "/Redguard/fxart/" + f3Dname + ".ROB";
                break;
        }

        minimalLoadedFileName = f3Dname;
        
        SpreadObjects(loadedObjects);
        
        FinalizeLoad();
        print("Loaded object: " + loadedFileName);
    }
    
    public void SpawnArea(string RGM, string WLD, string COL)
    {
        PrepareLoad();
        
        // Load Area objects
        loadedObjects = ModelLoader.LoadArea(RGM, COL, WLD);
        loadedFileName = RGM;
        minimalLoadedFileName = RGM;
        
        FinalizeLoad();
        print("Loaded area: " + loadedFileName);
    }
    
    private void PrepareLoad()
    {
        DeleteLoadedObject();
    }

    private void FinalizeLoad()
    {
        _objectRootGenerated.name = loadedFileName;
        ParentListObjects(loadedObjects, _objectRootGenerated);
        mvCam.FrameObject(_objectRootGenerated);
        
        settings.ToggleFlyMode(false);
        settings.RequestEnableTextureFiltering(true);
        settings.RequestEnableAnimations(true);
        
        gui.UpdateOverlays();
        
        RGMeshStore.DumpDict();
        RG3DStore.DumpDict();
        RGRGMStore.DumpDict();
        RGTexStore.DumpDict();
    }
    
    // Spread the loaded objects in negative X direction
    private void SpreadObjects(List<GameObject> objects)
    {
        float occupiedDistance = 0;
        
        foreach (var obj in objects)
        {
            var component = obj.GetComponent<Renderer>();
            float objectWidth = component.bounds.size.x;
            float originOffset =  obj.transform.position.x + component.bounds.center.x;
            
            float xPosition = (occupiedDistance + objectWidth / 2 + originOffset) * -1;
            obj.transform.position = new Vector3(xPosition, 0, 0);
            
            float spacing = 0.5f;
            occupiedDistance += objectWidth + spacing;
        }
    }

    private void ParentListObjects(List<GameObject> objects, GameObject parent)
    {
        foreach (var obj in objects)
        {
            obj.transform.SetParent(parent.transform);
        }
    }

    private void DeleteLoadedObject()
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
        }
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
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
        if (RGTexStore.MaterialDict != null)
        {
            foreach (var mat in RGTexStore.MaterialDict)
            {
                mat.Value.mainTexture.filterMode = mode;
            }
        }
    }

    public void EnableAnimations(bool enableAnimations)
    {
        if (loadedObjects != null)
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

    public string GetExportDirectory()
    {
        exportDirectory = gui.exportPathInput.text;
        return exportDirectory;
    }
}
