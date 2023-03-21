using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BayatGames.SaveGameFree;

public class MainPage : MonoBehaviour
{
    [SerializeField] private Button saveButton; 
    [SerializeField] private PageManager pageManager;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<Game>() == null)
        {
            // This means we started from the menu scene
            
            // if no game is loaded, disable "Save" button
            saveButton.interactable = false;
        }
        else
        {
            // this means we loaded the Menu from a running game
        }

        // Sometimes in the editor isLoadedAdditively can remain true if we exit playmode in a level scene
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Game.Menu.isLoadedAdditively = false;
        }

        
    }


    public void ContinueGame()
    {
        if (Game.Menu.isLoadedAdditively)
        {
            print("Menu Scene is loaded Additively");
            // This means a level is our active scene and the game is already running
            print(Game.Menu.isLoadedAdditively);
            
            FindObjectOfType<PauseMenuLoader>().HideMainMenu();
            Game.Menu.isLoadedAdditively = false;
            Game.PauseGame();
            
            // PauseMenuLoader handles this control when using the pause button

        }
        else
        {
            var foundFiles = SaveGame.GetFiles("Save");

            string newestFile = null;
            
            foreach (var file in foundFiles)
            {

                string filepath = "Save/" + file.Name;

                
                var gameData = SaveGame.Load<GameDataContainer>(filepath);
                if (gameData != null)
                {
                    print(file.Name + " has timestamp " + gameData.Timestamp);

                    if (newestFile == null)
                    {
                        //print("newestFile is null");
                        newestFile = filepath;
                    }
                    else
                    {
                        //print("newestFile is not null");
                        DateTime currentTimestamp = SaveGame.Load<GameDataContainer>(filepath).Timestamp;
                        DateTime prevTimestamp = SaveGame.Load<GameDataContainer>(newestFile).Timestamp;
                        
                        int result = currentTimestamp.CompareTo(prevTimestamp);
        
                        if (result < 0)
                        {
                            //print("prevTimestamp is newer than currentTimestamp");
                        }
                        else if (result > 0)
                        {
                            //print("currentTimestamp is newer than prevTimestamp");
                            newestFile = filepath;
                        }
                        else
                        {
                            //print("currentTimestamp and prevTimestamp are equal");
                        }
                    }
                }
                
            }

            var newestGameData = SaveGame.Load<GameDataContainer>(newestFile);
            if (newestGameData != null)
            {
                print("Newest file is " + newestFile + ". Using that for Continue.");
                Game.Data = newestGameData;
                Game.EnterSceneMode = EnterSceneModeEnum.Load;
                SceneManager.LoadScene(Game.Data.LastSceneName);
            }
            
            
        }
        
        // if (SceneManager.GetActiveScene == "Menu")
        // {Load latest savefile}
        // else {Hide Menu and unpause}
    }

    public void ShowSavePage()
    {
        Game.Menu.State = MenuStateEnum.SavePage;
        SwitchToPage(pageManager.savePage);
    }
    
    public void ShowLoadPage()
    {
        Game.Menu.State = MenuStateEnum.LoadPage;
        SwitchToPage(pageManager.loadPage);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Interaction Tests");
    }
    
    public void ShowOptionsPage()
    {
        Game.Menu.State = MenuStateEnum.OptionsPage;
        SwitchToPage(pageManager.optionsPage);
    }
    
    public void ShowMoviesPage()
    {
        Game.Menu.State = MenuStateEnum.MoviesPage;
        SwitchToPage(pageManager.moviesPage);
    }
    
    public void QuitGame()
    {
        
    }
    
    private void SwitchToPage(GameObject targetPage)
    {
        foreach (var page in pageManager.allPages)
        {
            if (page != targetPage)
            {
                page.SetActive(false);
            }
        }
        
        targetPage.SetActive(true);
    }
}
