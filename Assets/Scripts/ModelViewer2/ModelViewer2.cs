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
    [SerializeField] private Shader shader;
    [SerializeField] private string pathOverride;

    private GameObject _objectRootGenerated;

    void Start()
    {
        RGTexStore.shader = shader;

        // if a path override is set, use that
        if (pathOverride.Length > 0)
        {
            ModelLoader.RedguardPath = pathOverride;
        }

        // Show the default path
        gui.pathInput.text = ModelLoader.RedguardPath;
        
        // Start in Viewer Mode
        ViewerMode_Levels();
    }

    private bool IsPathValid()
    {
        try
        {
            var dirInfo = new DirectoryInfo(ModelLoader.RedguardPath);

            if (dirInfo.Exists)
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
            DirectoryInfo dirInfo = new DirectoryInfo(ModelLoader.RedguardPath + "/fxart");
        
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

    public void Spawn3DC(string filename)
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        Destroy(_objectRootGenerated);
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
        _objectRootGenerated.name = filename + "_" + _objectRootGenerated.GetInstanceID();
        
        // Create the object and parent it under the root
        GameObject obj = ModelLoader.Load3DC(filename);
        obj.transform.SetParent(_objectRootGenerated.transform);

        mv2Cam.FrameObject(_objectRootGenerated);
    }
    
    // Stupid Hardcoded ROB Loading functions
    public void SpawnArea(string filename)
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        Destroy(_objectRootGenerated);
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
        _objectRootGenerated.name = filename + "_" + _objectRootGenerated.GetInstanceID();

        // Create all objects of that area and parent them under the root
        List<GameObject> areaObjects = ModelLoader.LoadArea(filename);
        foreach (var obj in areaObjects)
        {
            obj.transform.SetParent(_objectRootGenerated.transform);
        }
        
        mv2Cam.FrameObject(_objectRootGenerated);
    }
}
