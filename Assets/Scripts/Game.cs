using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private ConfigManager localConfigManager;
    public static ConfigManager configManager;
    public static ConfigManager.ConfigData configData;

    public static bool isPaused;
    public static GameDataContainer Data = new();
    public static EnterSceneModeEnum EnterSceneMode;
    public static Menu Menu = new();


    // Start is called before the first frame update
    void Awake()
    {
        configManager = localConfigManager;
    
    
        isPaused = false;
        Menu.isLoadedAdditively = false;
    
        // Look for scene data, create new if necessary
        var sceneName = SceneManager.GetActiveScene().name;
    
        if (!Data.Scene.ContainsKey(sceneName))
        {
            var newSceneData = new SceneData();
            Data.Scene.Add(sceneName, newSceneData);
        }
    }

    public static void PauseGame()
    {
        isPaused = true;
        print("Game Paused.");
    
        // Attention: Player char controller checks for "if (!Game.isPaused)"
    }

    public static void ContinueGame()
    {
        isPaused = false;
        print("Game Continues.");
    }
}
