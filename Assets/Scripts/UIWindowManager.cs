using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWindowManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject hudPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            ToggleWindow(inventoryPanel);
            //hudPanel.GetComponent<ActiveObjectManager>().UpdateActiveObjectIndicator();
        }
        
        if (Input.GetButtonDown("Pause"))
        {
            // Todo: Game.Pause() or something
            
            print("Game.isPaused = " + Game.isPaused);
            
            // Check if main menu is already shown
            if (Game.isPaused)
            {
                // Hide menu here
                HideMainMenu();
                Game.ContinueGame();

            }
            else
            {
                // Load menu scene here
                ShowMainMenu();
                Game.PauseGame();
            }
        }
    }

    void ToggleWindow(GameObject window)
    {
        // Because the HUD should always be hidden when any other window is open
        hudPanel.SetActive(!hudPanel.activeSelf);
        // Then do whatever
        window.SetActive(!window.activeSelf);


        if (Game.isPaused)
        {
            Game.ContinueGame();
        }
        else
        {
            Game.PauseGame();
        }
    }

    void ShowMainMenu()
    {
        var localScene = FindObjectOfType<LocalScene>();
        localScene.eventSystem.enabled = false;
        localScene.audioListener.enabled = false;
        localScene.sun.enabled = false;

        SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
    }

    void HideMainMenu()
    {
        SceneManager.UnloadSceneAsync("Menu");
        
        var localScene = FindObjectOfType<LocalScene>();
        localScene.eventSystem.enabled = true;
        localScene.audioListener.enabled = true;
        localScene.sun.enabled = true;
    }
}
