using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : MonoBehaviour
{
    [SerializeField] private PageManager pageManager;
    [SerializeField] private GameObject deletePopup;
    
    void Update()
    {
        if (Input.GetButtonDown("Delete") && Game.Menu.State == MenuStateEnum.SavePage)
        {
            GameObject selectedButton = ButtonManager.menuEventSystem.currentSelectedGameObject;
            
            if (selectedButton.TryGetComponent(out SaveFileListItem listItem))
            {
                pageManager.savePage.GetComponent<SavePage>().OpenDeletePopup(listItem.saveFileName);
            }
        }
    }
}
