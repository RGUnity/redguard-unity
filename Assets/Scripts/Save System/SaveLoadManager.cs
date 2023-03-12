using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameSaver))]
[RequireComponent(typeof(GameLoader))]
[RequireComponent(typeof(InventoryManager))]
public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    
    // Right now this happens on Start, because we initialize the data types on Awake
    private void Awake()
    {
        switch (Game.EnterSceneMode)
        {
            case EnterSceneModeEnum.None:
                // This means we are probably starting in the editor
                inventoryManager.LoadDefaultInventory();
                break;
            case EnterSceneModeEnum.Load:
                // A game was loaded
                GetComponent<GameLoader>().ApplyData();
                break;
            case EnterSceneModeEnum.Door:
                // A door was opened.
                // Clear the NPC data, because that never carries over across scenes
                Game.Data.NPCDataDict.Clear();
                GetComponent<GameLoader>().ApplyData();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        
        // Reset EnterSceneMode
        Game.EnterSceneMode = EnterSceneModeEnum.None;

    }

    void Update()
    {
        if (Input.GetButtonDown("Quicksave"))
        {
            //GetComponent<GameSaver>().SaveGame();
        }
        
        if (Input.GetButtonDown("Quickload"))
        {
            //GameLoader.LoadGame();
        }
    }
    
    public void DeleteAllSavefileData()
    {

    }
}
