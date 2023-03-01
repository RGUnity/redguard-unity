using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void ToggleWindow(GameObject window)
    {
        // Because the HUD should always be hidden when any other window is open
        hudPanel.SetActive(!hudPanel.activeSelf);
        // Then do whatever
        window.SetActive(!window.activeSelf);

        if (Game.isGamePaused)
        {
            Game.ContinueGame();
        }
        else
        {
            Game.PauseGame();
        }
    }
}
