using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private string playScene;
    [SerializeField] private string modelviewerScene;
    
    private void Start()
    {
        // Move window to the center of screen (because we deleted a PlayerPrefs key) 
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        
        int width = Display.main.systemWidth/2 - Screen.width/2;
        int height = Display.main.systemHeight/2 - Screen.height/2;
        
        Screen.MoveMainWindowTo(displays[0], new Vector2Int(width, height));
    }

    public void Button_StartGame()
    {
        mainPanel.SetActive(false);
        
        //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);

        // This ensures that the game always starts in windowed mode for the launcher
        // I tried setting the PlayerPrefs, but that didnt work so no we're simply deleting it...
        PlayerPrefs.DeleteKey("Screenmanager Fullscreen mode Default_h401710285");
        
        print("Todo: Load menu scene or whatever");
        //SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene(playScene);
    }

    public void Button_StartModelViewer()
    {
        mainPanel.SetActive(false);
        
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.MaximizedWindow);
        PlayerPrefs.DeleteKey("Screenmanager Fullscreen mode Default_h401710285");
        
        //SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene(modelviewerScene);
    }
    
    public void Button_EditPath()
    {
        Game.ShowSetup();
    }
}
