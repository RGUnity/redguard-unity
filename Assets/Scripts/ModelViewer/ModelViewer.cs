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
    }

    private void Update()
    {
        // Input Event: Toggle UI
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            settings.ToggleUI();
        }

        // Input Event: Focus object
        if (Input.GetKeyUp(KeyCode.F))
        {
            mvCam.FrameObject(_objectRootGenerated);
        }
    }

    public void SwitchViewerMode(ViewerModes mode)
    {
        gui.SwitchViewerModeGUI(mode);
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
    
    public void SpawnArea(string RGM, string WLD, string COL, string prettyAreaName)
    {
        PrepareLoad();
        
        // Load Area objects
        loadedObjects = ModelLoader.LoadArea(RGM, COL, WLD);
        minimalLoadedFileName = RGM;

        if (WLD.Equals(string.Empty))
        {
            loadedFileName = prettyAreaName + " (" + RGM + ".RGM, " + COL + ".COL)";
        }
        else
        {
            loadedFileName = prettyAreaName + " (" + RGM + ".RGM, " + COL + ".COL, " + WLD + ".WLD)";
        }
        
        
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

        if (!settings.useFlyMode)
        {
            mvCam.FrameObject(_objectRootGenerated);
        }
        
        gui.UpdateOverlays();
        gui.UpdateExportButton();

        ApplyTextureFilterSetting();
        ApplyAnimationSetting();
        
        // RGMeshStore.DumpDict();
        // RG3DStore.DumpDict();
        // RGRGMStore.DumpDict();
        // RGTexStore.DumpDict();
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
    
    public void ApplyTextureFilterSetting()
    {
        if (RGTexStore.MaterialDict == null)
        {
            return;
        }

        foreach (var mat in RGTexStore.MaterialDict)
        {
            if (settings.useTextureFiltering)
            {
                mat.Value.mainTexture.filterMode = FilterMode.Bilinear;
            }
            else
            {
                mat.Value.mainTexture.filterMode = FilterMode.Point;
            }
        }
    }

    public void ApplyAnimationSetting()
    {
        if (loadedObjects == null)
        {
            return;
        }

        foreach (var obj in loadedObjects)
        {
            if (obj.TryGetComponent(out RGScriptedObject rgso))
            {
                if (rgso.type == RGScriptedObject.ScriptedObjectType.scriptedobject_animated)
                {
                    rgso.allowAnimation = settings.playAnimations;
                }
            }
        }
    }
}
