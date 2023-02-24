using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ToolBox.Serialization;
using UnityEditor.VersionControl;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;

    private GameObject _player;

    public static Dictionary<string, GameObject> saveableObjects = new();
    public static Dictionary<string, GameObject> sceneNPCDict = new ();

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

            
        // Has to be onStart, because onAwake, the NPCs add themselves to the sceneNPCDict
        if (PlayerPrefs.HasKey("EnterThroughLoad"))
        {
            GetComponent<LoadSavedNPCs>().LoadNPCs();
        }

        
        // Delete Entry keys
        PlayerPrefs.DeleteKey("EnterThroughDoor");
        PlayerPrefs.DeleteKey("EnterThroughLoad");
        
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

        Dictionary<string, GameObject> updatedDict = new();
        
        foreach (var entry in sceneNPCDict)
        {
            if (entry.Value != null)
            {
                updatedDict.Add(entry.Key, entry.Value);
            }
        }

        sceneNPCDict = updatedDict;
    }

    void Update()
    {
        if (Input.GetButtonDown("Quicksave"))
        {
            SaveGame();
        }
        
        if (Input.GetButtonDown("Quickload"))
        {
            PlayerPrefs.DeleteKey("EnterThroughDoor");
            PlayerPrefs.SetString("EnterThroughLoad", SceneManager.GetActiveScene().name);
            
            // sceneNPCDict must be cleared, because otherwise it will be full with dead object references next time
            sceneNPCDict.Clear();
            
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
            //print("Saved " + objType.amount + " " + objType.name);
        }
        print("Saved Inventory");
        
        // Save the active inventory object
        DataSerializer.Save("InventoryActiveObject", _inventoryData.activeObject);
        
        // Save the player's position
        DataSerializer.Save("Player_Position", _player.transform.position);
        DataSerializer.Save("Player_Rotation", _player.transform.rotation);
        
        //DataSerializer.Save("SceneData", _sceneData);
        // Save the current scene name
        DataSerializer.Save("CurrentScene", SceneManager.GetActiveScene().name);
        
        

        
        // Save the NPCs

        // Primary List for NPC data
        List<NPCData> NPCDataList = new();

        // To remember which IDs already have been added
        List <string> idCache = new();
        
        // Here we loop through the list of NPCs and create a NPCData for each one
        foreach (var entry in sceneNPCDict)
        {
            var id = entry.Key;
            var npc = entry.Value;
            
            // If the ID has already been added, throw a warning
            if (idCache.Contains(id))
            {
                Debug.LogWarning("NPC " + npc + " has no unique ID! Please generate a new one.");
            }
            else
            {
                idCache.Add(id);
            }

            
            // Create new NPCData
            var npcdata = new NPCData
            {
                // Assign values
                id = id,
                config = npc.GetComponent<NPC>().config,
                actionState = npc.GetComponent<NPC>().actionState,
                health = npc.GetComponent<NPC>().health,

                // Assign transform data
                position = npc.transform.position,
                rotation = npc.transform.rotation
            };

            // Add the NPCData to the list
            NPCDataList.Add(npcdata);
            // print("added " + npcdata.id + "to NPCDataList" );
            // print("NPCDataList count is " + NPCDataList.Count);
        }
        // Clear id cache. we dont need this anymore.
        idCache.Clear();
        
        // Finally, save the List to the savefile
        DataSerializer.Save("NPCDataList", NPCDataList);
        print(NPCDataList.Count + " NPCs stored in Savefile, the scene did contain " + sceneNPCDict.Count);
    }

   
    private void LoadScene()
    {
        // Once our scriptable objects are set up, scene in the savefile
        if (DataSerializer.TryLoad("CurrentScene", out string loadedSceneName))
        {
            SceneManager.LoadScene(loadedSceneName);
        }
    }
}
