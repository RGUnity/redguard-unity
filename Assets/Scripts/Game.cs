using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Header("Config")]

    [Header("Dynamic Variables")]
    public static bool isPaused;
    public static GameDataContainer Data = new();
    public static EnterSceneModeEnum EnterSceneMode;
    public static Menu Menu = new();
    
    
    // Start is called before the first frame update
    void Awake()
    {
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
        Game.Menu.isLoadedAdditively = true;
        print("Game Paused.");
    }
    
    public static void ContinueGame()
    {
        isPaused = false;
        Game.Menu.isLoadedAdditively = false;
        print("Game Continues.");
    }
}
