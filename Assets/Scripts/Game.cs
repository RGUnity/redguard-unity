using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Header("Config")]

    [Header("Dynamic Variables")]
    public static bool isGamePaused;
    public static SaveDataContainer WorkingSaveData = new();
    public static EnterSceneModeEnum EnterSceneMode;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        isGamePaused = false;
        
        // Initialize Save System
        if (WorkingSaveData.SceneDataCache == null)
        {
            WorkingSaveData.SceneDataCache = new Dictionary<string, SavableSceneData>();
            print("New SceneDataCache was created");
        }

        var sceneName = SceneManager.GetActiveScene().name;
        
        if (!WorkingSaveData.SceneDataCache.ContainsKey(sceneName))
        {
            var newSceneData = new SavableSceneData();
            WorkingSaveData.SceneDataCache.Add(sceneName, newSceneData);
        }

        if (WorkingSaveData.SceneDataCache[sceneName].ObjectDataDict == null)
        {
            WorkingSaveData.SceneDataCache[sceneName].ObjectDataDict = new Dictionary<string, SavableObjectData>();
        }
        
        if (WorkingSaveData.SavablePlayerData == null)
        {
            WorkingSaveData.SavablePlayerData = new SavablePlayerData();
            print("Created new SavablePlayerData");
        }
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
