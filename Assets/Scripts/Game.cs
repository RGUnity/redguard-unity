using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Header("Config")]

    [Header("Dynamic Variables")]
    public static bool isGamePaused;
    public static GameDataContainer Data = new();
    public static EnterSceneModeEnum EnterSceneMode;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        isGamePaused = false;
        
        // Initialize Save System
        var sceneName = SceneManager.GetActiveScene().name;
        
        if (!Data.Scene.ContainsKey(sceneName))
        {
            var newSceneData = new SceneData();
            Data.Scene.Add(sceneName, newSceneData);
        }
        
        
        // These below are probably obsolete because mose fields should now have "new()" behind them

        // if (Data.Scene == null)
        // {
        //     Data.Scene = new Dictionary<string, SceneData>();
        //     print("New SceneData was created for this scene");
        // }
        //

        //
        // if (Data.Scene[sceneName].ObjectDataDict == null)
        // {
        //     Data.Scene[sceneName].ObjectDataDict = new Dictionary<string, ObjectData>();
        // }
        //
        // if (Data.Player == null)
        // {
        //     Data.Player = new PlayerData();
        //     print("Created new PlayerData");
        // }
        //
        // if (Data.Player.Inventory == null)
        // {
        //     Data.Player.Inventory = new InventoryData();
        //     print("Created new InventoryData");
        // }
    }

    public static void PauseGame()
    {
        isGamePaused = true;
        //print("Game Paused");
    }
    
    public static void ContinueGame()
    {
        isGamePaused = false;
        //print("Game Continues");
    }
}
