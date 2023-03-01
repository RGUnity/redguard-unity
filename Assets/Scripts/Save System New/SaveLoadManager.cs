using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameSaver))]
[RequireComponent(typeof(GameLoader))]
public class SaveLoadManager : MonoBehaviour
{
    private void Start()
    {
        switch (Game.EnterSceneMode)
        {
            case EnterSceneModeEnum.None:
                // THis means we are probably starting in the editor
                break;
            case EnterSceneModeEnum.Load:
                // A game was loaded
                GetComponent<GameLoader>().ApplyData();
                break;
            case EnterSceneModeEnum.Door:
                // A door was opened.
                // Clear the NPC data, because that never carries over across scenes
                Game.WorkingSaveData.NPCDataDict.Clear();
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
            GetComponent<GameSaver>().SaveGame();
        }
        
        if (Input.GetButtonDown("Quickload"))
        {
            GetComponent<GameLoader>().LoadGame();
        }
    }
    
    public void DeleteAllSavefileData()
    {
        DataSerializer.DeleteAll();
    }
}
