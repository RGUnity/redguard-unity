using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PauseMenuLoader _pauseMenuLoader; 
    
    private UIWindowManager UiWindowManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        UiWindowManager = FindObjectOfType<UIWindowManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Quick Save"))
        {
            print("Input Button Called: Quick Save");
            GameSaver.QuickSave();
        }
        if (Input.GetButtonDown("Quick Load"))
        {
            print("Input Button Called: Quick Load");
            GameLoader.QuickLoad();
        }
        

        if (Game.Menu.isLoadedAdditively)
        {
            // Inputs that can only happen when the additive pause menu is OPEN 
            if (Input.GetButtonDown("Pause") && Game.Menu.State == MenuStateEnum.MainPage)
            {
                //_pauseMenuLoader.HideMainMenu();
            }
            
        }
        else
        {
            // Inputs that can only happen when the additive pause menu is HIDDEN 
            
            if (Input.GetButtonDown("Fire2"))
            {
                UiWindowManager.ToggleInventory();
            }
            
            if (Input.GetButtonDown("Pause"))
            {
                _pauseMenuLoader.ShowMainMenu();
            }
        }
    }
}
