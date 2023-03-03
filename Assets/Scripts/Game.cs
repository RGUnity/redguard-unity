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
        isGamePaused = true;
        //print("Game Paused");
    }
    
    public static void ContinueGame()
    {
        isGamePaused = false;
        //print("Game Continues");
    }
}
