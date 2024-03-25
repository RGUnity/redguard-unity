using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputManager : MonoBehaviour
{
    [SerializeField] private PageManager pageManager;
    
    public bool togglePauseMenu;
    public bool deleteSavefile;
    
    private InputAction _togglePauseMenuAction;
    private InputAction _deleteSavefileAction;


    private void Start()
    {
        _togglePauseMenuAction = InputSystem.actions.FindAction("Toggle Pause Menu");
        _deleteSavefileAction = InputSystem.actions.FindAction("Delete Savefile");
    }

    void Update()
    {
        togglePauseMenu = _togglePauseMenuAction.WasPressedThisFrame();
        deleteSavefile = _deleteSavefileAction.WasPressedThisFrame();
        
        if (deleteSavefile)
        {
            if (Game.Menu.State == MenuStateEnum.SavePage)
            {
                GameObject selectedButton = ButtonManager.menuEventSystem.currentSelectedGameObject;
            
                if (selectedButton.TryGetComponent(out GenericSaveFileListItem listItem))
                {
                    pageManager.OpenPopup(pageManager.saveDeletePopup, listItem.saveFileName);
                }
            }
            else if (Game.Menu.State == MenuStateEnum.LoadPage)
            {
                GameObject selectedButton = ButtonManager.menuEventSystem.currentSelectedGameObject;
            
                if (selectedButton.TryGetComponent(out GenericSaveFileListItem listItem))
                {
                    pageManager.OpenPopup(pageManager.loadDeletePopup, listItem.saveFileName);
                }
            }
        }
        
        if (togglePauseMenu)
        {
            pageManager.MoveUp();
        }
    }
}
