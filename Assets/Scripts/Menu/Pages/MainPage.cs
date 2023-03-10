using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        

        
    }


    public void ContinueGame()
    {
        // Get Cached Scene, reload it
        for (int i = 0; i < 19; i++)
        {
            var filePath = Application.persistentDataPath + "/Save_" + i + ".data";

            if (File.Exists(filePath))
            {
                print("found file Save_" + i);
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
