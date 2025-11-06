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

        DeleteSavedWindowMode();
        
        print("Todo: Load menu scene or whatever");
        //SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene(playScene);
    }

    public void Button_StartModelViewer()
    {
        mainPanel.SetActive(false);
        
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.MaximizedWindow);
        DeleteSavedWindowMode();
        
        //SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene(modelviewerScene);
    }
    
    public void Button_EditPath()
    {
        Game.ShowSetup();
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
