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

    public void HideGameplayGUI()
    {
        inventoryPanel.SetActive(false);
        hudPanel.SetActive(false);
    }
    
    public void ShowGameplayGUI()
    {
        hudPanel.SetActive(true);
    }

    public void ToggleInventory()
    {
        ToggleWindow(inventoryPanel);
    }
}
