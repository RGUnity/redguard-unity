using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private GameObject setupPanel;
    [SerializeField] private GameObject mainPanel;
    
    private void Awake()
    {
        // Move window to the center of screen (because we deleted a PlayerPrefs key) 
        List<DisplayInfo> displays = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displays);
        
        int width = Display.main.systemWidth/2 - Screen.width/2;
        int height = Display.main.systemHeight/2 - Screen.height/2;
        
        Screen.MoveMainWindowTo(displays[0], new Vector2Int(width, height));

        if (PathManager.CheckPaths())
        {
            // Show Launcher
            setupPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        else
        {
            // Show Setup
            setupPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
    }

    public void Button_Continue()
    {
        mainPanel.SetActive(true);
        setupPanel.SetActive(false);
    }

    public void Button_StartGame()
    {
        
        //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow);

        // This ensures that the game always starts in windowed mode for the launcher
        // I tried setting the PlayerPrefs, but that didnt work so no we're simply deleting it...
        PlayerPrefs.DeleteKey("Screenmanager Fullscreen mode Default_h401710285");
    }

    public void Button_StartModelViewer()
    {
        SceneManager.UnloadSceneAsync("Scenes/Launcher");
        SceneManager.LoadScene("Scenes/ModelViewer");
    }
}
