using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private DPIScaler scaler;
    [SerializeField] private string playScene;
    [SerializeField] private string modelViewerScene;
    
    private void Start()
    {
        float systemScale = scaler.GetScaleFactor();
        print("systemScale is " +  systemScale);
        int scaledWidth = (int)(Screen.width * systemScale);
        int scaledHeight = (int)(Screen.height * systemScale);   
        
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // Apply system scale to window size resolution because Windows doesnt do that automatically
            Screen.SetResolution(scaledWidth,  scaledHeight, FullScreenMode.Windowed);
        }
        
        // Move window to the center of screen (because we deleted a PlayerPrefs key) 
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        int width = Display.main.systemWidth/2 - scaledWidth/2;
        int height = Display.main.systemHeight/2 - scaledHeight/2;

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
        
        Screen.SetResolution(Mathf.RoundToInt(Display.main.systemWidth * 0.8f), Mathf.RoundToInt(Display.main.systemHeight * 0.8f), FullScreenMode.MaximizedWindow);
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
    }

    private void OnApplicationQuit()
    {
        DeleteSavedWindowMode();
    }
}
