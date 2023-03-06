using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject saveMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        saveMenu.SetActive(false);
        
        // if no game is loaded, disable "Continue" game
    }

    // Update is called once per frame
    void Update()
    {
        
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
        saveMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void ShowLoadPage()
    {
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Interaction Tests");
    }
    
    public void ShowOptionsPage()
    {
        
    }
    
    public void ShowMoviesPage()
    {
        
    }
    
    public void QuitGame()
    {
        
    }
}
