using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ToolBox.Serialization;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    [SerializeField] private SceneData _sceneData;
    [SerializeField] private InventoryData _inventoryData;

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
            //PlayerPrefs.DeleteKey("EnterThroughDoor");
            
            SaveGame();
        }
        
        if (Input.GetButtonDown("Quickload"))
        {
            // Clear, because we want it to load the deletionStatus from the savefile instead
            deletedObjectCache.Clear();
            
            // TODO get the scene that we are trying to reload into, and take its SceneData!
            if (DataSerializer.TryLoad("SceneData", out SceneData _loadedSceneData))
            {
                PlayerPrefs.DeleteKey("EnterThroughDoor");
                PlayerPrefs.SetString("EnterThroughLoad", SceneManager.GetActiveScene().name);
            
                //LoadInventory();
                LoadScene();
            }
            else
            {
                print("No SceneData found. LoadScene aborted");
            }

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
        
        DataSerializer.Save("SceneData", _sceneData);
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


    private void LoadScene()
    {
        // Once our scriptble objects are set up, scene in the savefile
        if (DataSerializer.TryLoad("CurrentScene", out string loadedSceneName))
        {
            SceneManager.LoadScene(loadedSceneName);
        }
    }
}
