using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : MonoBehaviour
{
    [SerializeField] private PageManager pageManager;
    
    void Update()
    {
        if (Input.GetButtonDown("Delete"))
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
        
        if (Input.GetButtonDown("Pause"))
        {
            pageManager.MoveUp();
        }
    }
}
