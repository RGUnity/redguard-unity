using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BayatGames.SaveGameFree;

public class MainPage : GenericUIWindow
{
    [SerializeField] private Button continueButton; 
    [SerializeField] private Button saveButton; 
    [SerializeField] private Button loadButton; 

    // OnEnable() is used by the parent object, "GenericMenuPage"
    
    void Start()
    {
        if (FindFirstObjectByType<Game>() == null)
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
        
        // If no savefiles exist, disable the "Continue" and "Load" Button
        var foundFiles = BayatGames.SaveGameFree.SaveGame.GetFiles("Save");
        
        if (foundFiles.Length <= 0)
        {
            continueButton.interactable = false;
            loadButton.interactable = false;
        }
    }


    public void ContinueGame()
    {
        if (Game.Menu.isLoadedAdditively)
        {
            print("Menu Scene is loaded Additively");
            // This means a level is our active scene and the game is already running
            print(Game.Menu.isLoadedAdditively);
            
            FindFirstObjectByType<PauseMenuLoader>().HideMainMenu();
            
            // PauseMenuLoader handles this control when using the pause button

        }
        else
        {
            GameLoader.LoadLatestGame();
        }
        
        // if (SceneManager.GetActiveScene == "Menu")
        // {Load latest savefile}
        // else {Hide Menu and unpause}
    }


    public void NewGame()
    {
        SceneManager.LoadScene("Interaction Tests");
    }
    
    
    public void QuitGame()
    {
        
    }
}
