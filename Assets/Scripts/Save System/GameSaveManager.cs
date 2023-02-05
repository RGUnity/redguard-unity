using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ToolBox.Serialization;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
   
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private SceneData _sceneData;
    private GameObject _player;
    public static List<String> deletedObjectCache = new List<String>();

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        
        
        // get a list of savefiles (for later)

        // for (int i = 0; i < 19; i++) 
        // {
        //     var filePath = Application.persistentDataPath + "/Save_" + i + ".data";
        //     
        //     if (File.Exists(filePath))
        //     {
        //         print("found file Save_" + i);
        //     }
        // }
        
        if (_sceneData.nextEntryPoint == null)
        {
            LoadInventory();
        }

    }

    void Update()
    {
        if (Input.GetButtonDown("Quicksave"))
        {
            SaveGame();
        }
        
        if (Input.GetButtonDown("Quickload"))
        {
            // Clear, because we want it to load the deletionStatus from the savefile instead
            deletedObjectCache.Clear();
            
            // Set to null, because we dont want to respawn at a spawnPoint
            _sceneData.nextEntryPoint = null;
            
            LoadInventory();
            LoadScene();
        }
    }

    // Right now this is called by a UI button named "Delete all Savefile Data"
    public void DeleteAllSavefileData()
    {
        DataSerializer.DeleteAll();
    }
    
    private void SaveGame()
    {
        // Save the player inventory objects
        foreach (InventoryObjectType objType in _inventoryData.objects)
        {
            DataSerializer.Save(objType.name, objType.amount);
            print("Saved " + objType.amount + " " + objType.name);
        }
        
        // Save the active inventory object
        DataSerializer.Save("InventoryActiveObject", _inventoryData.activeObject);
        
        // Save the player's position
        DataSerializer.Save("Player_Position", _player.transform.position);
        DataSerializer.Save("Player_Rotation", _player.transform.rotation);
        
        // Save the current scene name
        DataSerializer.Save("CurrentScene", SceneManager.GetActiveScene().name);
        
        // Save the list of deleted objects to disk
        foreach (var obj in deletedObjectCache)
        {
            // Since we only check if the GUID exists as a key, we dont actually need a value.
            // ... the bool is just there because it wants one
            DataSerializer.Save(obj, true); 
        }
    }
    
    private void LoadInventory()
    {
        // This threw an error in a Build one (when entering a new scene) so i'm gonna null check this
        if (_inventoryData != null)
        {
            _inventoryData.objects.Clear();
        }
        
        // Load active object
        if (DataSerializer.TryLoad("InventoryActiveObject", out InventoryObjectType actObj))
        {
            _inventoryData.activeObject = actObj;
            print("Loaded active object: " + actObj);
        }


        // Load inventory objects from savefile
        List<InventoryObjectType> loadedObjects = new List<InventoryObjectType>();
        foreach (InventoryObjectType objType in _inventoryData.allowedObjects)
        {
            if (DataSerializer.TryLoad(objType.name, out int loadedAmount))
            {
                //print("Savefile contains " + objType.displayName);
                objType.amount = loadedAmount;
                //print("Loaded: " + objType.amount + " " + objType.name);
                
                loadedObjects.Add(objType);
            }

            else
            {
                objType.amount = 0;
            }
        }
        
        // If the temp list is not empty, match the lists
        if (loadedObjects.Count > 0)
        {
            _inventoryData.objects = loadedObjects;
        }
        else
        {
            print("No objects loaded");
        }
    }
    
    
    private void LoadScene()
    {
        // Once our scriptble objects are set up, scene in the savefile
        if (DataSerializer.TryLoad("CurrentScene", out string loadedSceneName))
        {
            SceneManager.LoadScene(loadedSceneName);
        }
    }
}
