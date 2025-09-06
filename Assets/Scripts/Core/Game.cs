using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    // Managers
    public PathManager localPathManager;
    public ConfigManager localConfigManager;
    public InputManager localInputManager;
    public GUIManager localGUIManager;
    public static PathManager pathManager;
    public static ConfigManager configManager;
    public static InputManager inputManager;
    public static GUIManager guiManager;
    
    // General Variables
    public static GameDataContainer Data = new();
    public static ConfigData Config = new();
    public static InputData Input = new();
    public static bool isPaused;
    public static bool setupIsLoaded;
    
    // Todo: remove this garbage
    public static EnterSceneModeEnum EnterSceneMode;
    public static Menu Menu = new();
    
    
    private void Awake()
    {
        pathManager = localPathManager;
        configManager = localConfigManager;
        inputManager =  localInputManager;
        guiManager =  localGUIManager;
        
        isPaused = false;
        Menu.isLoadedAdditively = false;
    
        // Look for scene data, create new if necessary
        var sceneName = SceneManager.GetActiveScene().name;
    
        if (!Data.Scene.ContainsKey(sceneName))
        {
            var newSceneData = new SceneData();
            Data.Scene.Add(sceneName, newSceneData);
        }
        
        configManager.InitializeConfig();
        if (!pathManager.ValidatePath(Config.redguardPath))
        {
            ShowSetup();
        }
    }

    private static void ShowSetup()
    {
        if (!setupIsLoaded)
        {
            // Start Setup
            SceneManager.LoadScene("Scenes/Setup", LoadSceneMode.Additive);
            setupIsLoaded = true;
        }
    }

    public static void HideSetup()
    {
        if (setupIsLoaded)
        {
            // Exit Setup
            SceneManager.UnloadSceneAsync("Scenes/Setup");
            setupIsLoaded = false;
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
