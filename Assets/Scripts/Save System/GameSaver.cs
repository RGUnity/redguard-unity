using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    public static void SaveGame(string saveFileName)
    {
        // This packs all the important stuff into Game.Data
        PrepareGameDataForSave();
        
        // Finally, write the Game Data into a file
        BayatGames.SaveGameFree.SaveGame.Save("Save/" + saveFileName + ".json", Game.Data);
    }

    public static void QuickSave()
    {
        SaveGame("Quicksave");
    }
    
    public static void RememberObject(GameObject obj)
    {
        {
            var sceneName = SceneManager.GetActiveScene().name;

            
            var objDataDict = Game.Data.Scene[sceneName].ObjectDataDict;

            var itemComponent = obj.GetComponent<Item>();
            var objData = new ObjectData
            {
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                
                isDeleted = itemComponent.isDeleted,
                minAmount = itemComponent.minAmount,
                maxAmount = itemComponent.maxAmount,
                id = itemComponent.id
            };
            
            // Add the objectData
            
            if (!objDataDict.TryAdd(objData.id, objData))
            {
                objDataDict.Remove(objData.id);
                objDataDict.Add(objData.id, objData);
            }
            //print("Saved "+ objData.id + " with isDeleted = "+ objData.isDeleted);

        }
    }

    private static void PrepareGameDataForSave()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        Game.Data.LastSceneName = sceneName;
        
        // ----- Save Player -----

        var player = GameObject.FindGameObjectWithTag("Player");
        var playerData = Game.Data.Player;

        // We subtract -1 on y to get a vector that matches the feet position 
        playerData.PlayerPosition = player.transform.position - new Vector3(0, 1, 0);;;
        playerData.PlayerRotation = player.transform.rotation;




        // ----- Save NPCs -----
        
        // Create new NPCDataList
        Game.Data.NPCDataDict = new Dictionary<string, NPCData>();
        
        foreach (var npc in GameObject.FindGameObjectsWithTag("NPC"))
        {
            var npcComponent = npc.GetComponent<NPC>();
            
            var npcdata = new NPCData
            {
                //Assign values
                id = npcComponent.id,
                config = npcComponent.config,
                actionState = npcComponent.actionState,
                health = npcComponent.health,

                // Assign transform data
                position = npc.transform.position,
                rotation = npc.transform.rotation
            };
            
            // Add NPCData to list in Data
            var npcDataDict = Game.Data.NPCDataDict;
            
            if (!npcDataDict.TryAdd(npcdata.id, npcdata))
            {
                npcDataDict.Remove(npcdata.id);
                npcDataDict.Add(npcdata.id, npcdata);
            }
            
        }
        
        
        // ----- Save Objects -----
        
        // Create new ObjectDataList
        var _sceneData = Game.Data.Scene[sceneName];
        
        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            var itemComponent = item.GetComponent<Item>();
            
            var objData = new ObjectData
            {
                
                position = itemComponent.transform.position,
                rotation = itemComponent.transform.rotation,
                isDeleted = itemComponent.isDeleted,
                minAmount = itemComponent.minAmount,
                maxAmount = itemComponent.maxAmount,
                id = itemComponent.id
            };
            
            // Add ObjectData to list in Data
            var objDataDict = _sceneData.ObjectDataDict;
            
            if (!objDataDict.TryAdd(objData.id, objData))
            {
                objDataDict.Remove(objData.id);
                objDataDict.Add(objData.id, objData);
            }
            
            print("Saved " + objData.id + " with isDeleted = " + objData.isDeleted);

        }

        Game.Data.Timestamp = DateTime.Now;
    }
}
