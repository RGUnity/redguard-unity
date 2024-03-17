using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu()
    {
        // Todo: Game.Pause() or something


        // If the Menu Scene is already loaded, hide it
        if (Game.Menu.isLoadedAdditively)
        {
            //print("isLoadedAdditively = " + Game.Menu.isLoadedAdditively);
            HideMainMenu();

            //print("isLoadedAdditively was set to false, is now = " + Game.Menu.isLoadedAdditively);
        }
        // If not, show it
        else
        {
            //print("isLoadedAdditively = " + Game.Menu.isLoadedAdditively);
            ShowMainMenu();
                
            //print("isLoadedAdditively was set to true, is now = " + Game.Menu.isLoadedAdditively);
        }
    }
    
    public void ShowMainMenu()
    {
        
        LocalScene.eventSystem.enabled = false;
        LocalScene.audioListener.enabled = false;
        LocalScene.sun.enabled = false;
        LocalScene.canvas.enabled = false;
        
        // Hide the Gameplay GUI
        FindObjectOfType<UIWindowManager>().HideGameplayGUI();

        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        
        Game.Menu.isLoadedAdditively = true;
        Game.PauseGame();
    }

    public void HideMainMenu()
    {
        SceneManager.UnloadSceneAsync("Menu");
        
        LocalScene.eventSystem.enabled = true;
        LocalScene.audioListener.enabled = true;
        LocalScene.sun.enabled = true;
        LocalScene.canvas.enabled = true;
        
        // Show Gameplay GUI
        FindObjectOfType<UIWindowManager>().ShowGameplayGUI();
        
        Game.Menu.isLoadedAdditively = false;
        Game.ContinueGame();
    }
}
