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
    //[SerializeField] private SceneData _sceneData;
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

    }

    void Update()
    {
        if (Input.GetButtonDown("Quicksave"))
        {
            SaveGame();
        }
        
        if (Input.GetButtonDown("Quickload"))
        {
            LoadGame();
        }
    }

    // Right now this is called by a UI button named "Delete all Savefile Data"
    public void DeleteAllSavefileData()
    {
        DataSerializer.DeleteAll();
    }
    
    private void SaveGame()
    {
        // Save the player inventory
        foreach (InventoryObjectType objType in _inventoryData.objects)
        {
            DataSerializer.Save(objType.name, objType.amount);
            print("Saved " + objType.amount + " " + objType.name);
        }
        
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
    
    
    private void LoadGame()
    {
        
        // This threw an error in a Build one (when entering a new scene) so i'm gonna null check this
        if (_inventoryData != null)
        {
            _inventoryData.objects.Clear();
        }
        
        // Load inventory objects from savefile
        foreach (InventoryObjectType objType in _inventoryData.allowedObjects)
        {
            if (DataSerializer.TryLoad(objType.name, out int loadedAmount))
            {
                //print("Savefile contains " + objType.displayName);
                objType.amount = loadedAmount;
                print("Loaded: " + objType.amount + " " + objType.name);
                if (objType.amount > 0)
                {
                    _inventoryData.objects.Add(objType);
                }
            }

            else
            {
                objType.amount = 0;
            }
        }

        // Once our scriptble objects are set up, scene in the savefile
        if (DataSerializer.TryLoad("CurrentScene", out string loadedSceneName))
        {
            SceneManager.LoadScene(loadedSceneName);
        }
    }
}
