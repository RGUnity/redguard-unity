using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void LoadGame()
    {
        if (DataSerializer.TryLoad("SaveData", out GameDataContainer loadedSaveData))
        {
            Game.Data = loadedSaveData;
            Game.EnterSceneMode = EnterSceneModeEnum.Load;
            
            print("Loaded GameData with " + Game.Data.Player.Inventory.activeObject.type);
            
            SceneManager.LoadScene(Game.Data.LastSceneName);
        }
    }

    public void ApplyData()
    {
        SetPlayer();
        SetNPCs();
        SetObjects();
    }

    public void SetObjects()
    {

        Dictionary<string, GameObject> sceneObjectDict = new();

        foreach (var obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            var objectID = obj.GetComponent<Item>().id;
            sceneObjectDict.Add(objectID, obj);
        }
        
        string sceneName = SceneManager.GetActiveScene().name;
        Dictionary<string, ObjectData> loadedObjDict = Game.Data.Scene[sceneName].ObjectDataDict;
        
        foreach (var entry in loadedObjDict)
        {
            var loadedObj = entry.Value;
            //print("loadedObjDict contains " + loadedObj.id + "with isDeleted = " + loadedObj.isDeleted);
            
            if (sceneObjectDict.ContainsKey(loadedObj.id))
            {
                // Object is already in scene
                var sceneObj = sceneObjectDict[loadedObj.id];
                
                
                if (loadedObj.isDeleted)
                {
                    // If it exists and is Deleted, simply hide it.
                    sceneObj.SetActive(false);
                    print(sceneObj.gameObject.name + " was marked as deleted and will be hidden");
                }
                else
                {
                    // If it exists but is not deleted, apply the data
                    sceneObj.transform.position = loadedObj.position;
                    sceneObj.transform.rotation = loadedObj.rotation;
                    
                    Item itemComponent = sceneObj.GetComponent<Item>();
                    itemComponent.maxAmount = loadedObj.maxAmount;
                    itemComponent.minAmount = loadedObj.minAmount;
                }
            }
            else
            {
                if (loadedObj.isDeleted)
                {
                    // Ignore it. I dont see why we need to respawn objects that have been deleted.
                }
                else
                {
                    // Respawn the object and apply the data
                    var newSceneObj = Instantiate(loadedObj.objectType.worldObject, loadedObj.position, loadedObj.rotation);

                    Item itemComponent = newSceneObj.GetComponent<Item>();
                    itemComponent.maxAmount = loadedObj.maxAmount;
                    itemComponent.minAmount = loadedObj.minAmount;
                }
            }
        }
    }

    public void SetNPCs()
    {
        Dictionary<string, GameObject> sceneNPCDict = new();

        foreach (var npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            var objectID = npc.GetComponent<NPC>().id;
            sceneNPCDict.Add(objectID, npc);
        }
        
        string sceneName = SceneManager.GetActiveScene().name;
        Dictionary<string, NPCData> loadedNPCDict = Game.Data.NPCDataDict;
        
        foreach (var entry in loadedNPCDict)
        {
            var loadedNPC = entry.Value;
            //print("loadedNPCDict contains " + loadedNPC.id);

            if (sceneNPCDict.ContainsKey(loadedNPC.id))
            {
                // Object is already in scene
                var sceneNPC = sceneNPCDict[loadedNPC.id];

                // Load transform data
                sceneNPC.transform.position = loadedNPC.position;
                sceneNPC.transform.rotation = loadedNPC.rotation;

                // Load values
                var npcComponent = sceneNPC.GetComponent<NPC>();
                npcComponent.actionState = loadedNPC.actionState;
                npcComponent.health = loadedNPC.health;
            }

            else
            {
                // Respawn the NPC and apply the data
                var newSceneNPC = Instantiate(loadedNPC.config.worldPrefab, loadedNPC.position, loadedNPC.rotation);
                
                // Load values
                var npcComponent = newSceneNPC.GetComponent<NPC>();
                npcComponent.actionState = loadedNPC.actionState;
                npcComponent.health = loadedNPC.health;
                npcComponent.id = loadedNPC.id;
            }
        }
    }

    public void SetPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerData = Game.Data.Player;
        
        
        // Just use those always?
        var playerController = player.GetComponent<CharacterController>();
        playerController.enabled = false;
        
        // We add the y +1 vector again because otherwise we fall through the floor
        player.transform.position = playerData.PlayerPosition + new Vector3(0, 1, 0);;
        player.transform.rotation = playerData.PlayerRotation;
        playerController.enabled = true;

        
        //inventoryData.activeObject = playerData.InventoryActiveObject;
        //inventoryData.objects = playerData.InventoryObjects;

    }
}
