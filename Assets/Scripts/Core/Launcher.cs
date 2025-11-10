using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private DPIScaler scaler;
    [SerializeField] private string playScene;
    [SerializeField] private string modelViewerScene;
    [SerializeField] private int2 baseWindowSize;
    [SerializeField] private int2 scaledWindowSize;
    
    private void Start()
    {
        float systemScale = scaler.GetScaleFactor();
        print("systemScale is " +  systemScale);
        scaledWindowSize.x = Mathf.RoundToInt(baseWindowSize.x * systemScale);
        scaledWindowSize.y = Mathf.RoundToInt(baseWindowSize.y * systemScale);   

        // Apply system scale to window size
        Screen.SetResolution(scaledWindowSize.x,  scaledWindowSize.y, FullScreenMode.Windowed);
    
        // Move window to the center of screen (because we deleted a PlayerPrefs key) 
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        int width = Display.main.systemWidth/2 - scaledWindowSize.x/2;
        int height = Display.main.systemHeight/2 - scaledWindowSize.y/2;

        Screen.MoveMainWindowTo(displays[0], new Vector2Int(width, height));
    }

    public void Button_StartGame()
    {
        mainPanel.SetActive(false);
        
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);
        DeleteSavedWindowMode();
        
        print("Todo: Load menu scene or whatever");
        //SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene(playScene);
    }

    public void Button_StartModelViewer()
    {
        mainPanel.SetActive(false);

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Screen.SetResolution(Mathf.RoundToInt(Display.main.systemWidth * 0.8f), Mathf.RoundToInt(Display.main.systemHeight * 0.8f), FullScreenMode.MaximizedWindow);
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.Windowed);
        }
        DeleteSavedWindowMode();
        
        //SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene(modelViewerScene);
    }
    
    public void Button_EditPath()
    {
        Game.ShowSetup();
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
    
    private void DeleteSavedWindowMode()
    {
        // This ensures that the program starts using the window mode as it is defined in the project settings
        PlayerPrefs.DeleteKey("Screenmanager Fullscreen mode Default_h401710285");
        PlayerPrefs.DeleteKey("Screenmanager Fullscreen mode Default");
    }

    private void OnApplicationQuit()
    {
        DeleteSavedWindowMode();
    }
}
