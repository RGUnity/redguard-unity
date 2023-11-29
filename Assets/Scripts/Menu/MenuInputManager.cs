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
            
                if (selectedButton.TryGetComponent(out SaveFileListItem listItem))
                {
                    pageManager.savePage.GetComponent<SavePage>().OpenDeletePopup(listItem.saveFileName);
                }
            }
            else if (Game.Menu.State == MenuStateEnum.LoadPage)
            {
                GameObject selectedButton = ButtonManager.menuEventSystem.currentSelectedGameObject;
            
                if (selectedButton.TryGetComponent(out SaveFileListItem listItem))
                {
                    pageManager.loadPage.GetComponent<LoadPage>().OpenDeletePopup(listItem.saveFileName);
                }
            }
        }
    }
}
