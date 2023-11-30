using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageManager : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject savePage;
    public GameObject loadPage;
    public GameObject optionsPage;
    public GameObject moviesPage;
    public GameObject saveDeletePopup;
    public GameObject saveSetNamePopup;
    public GameObject loadDeletePopup;
    
    [HideInInspector] public List<GameObject> allPages;
    
    // Start is called before the first frame update
    void Start()
    {
        // Remember to add new pages here if necessary
        allPages = new List<GameObject>
        {
            mainPage,
            savePage,
            loadPage,
            optionsPage,
            moviesPage,
            saveDeletePopup,
            saveSetNamePopup,
            loadDeletePopup
        };
        
        // Restore Start State
        foreach (var page in allPages)
        {
            // Hide all that are not the main page
            if (page != mainPage)
            {
                page.SetActive(false);
            }
            
            // if main page is hidde, enable it
            if (!mainPage.activeSelf)
            {
                mainPage.SetActive(true);
            }
        }
    }
    
    public void SwitchToPage(GameObject targetPage)
    {
        // Disable all other pages
        foreach (var page in allPages)
        {
            if (page != targetPage)
            {
                page.SetActive(false);
            }
        }
        
        // Enable the target page
        targetPage.SetActive(true);
        
        // Update the enum state
        Game.Menu.State = targetPage.GetComponent<GenericUIWindow>().associatedEnumState;
    }
    
    public void OpenPopup(GameObject popup, string infoString)
    {
        // First, set the info string
        popup.GetComponent<GenericUIPopup>().infoString = infoString;
        
        // Activate the popup
        popup.SetActive(true);

        // Update the menu state
        Game.Menu.State = popup.GetComponent<GenericUIWindow>().associatedEnumState;
        print("Set Menu state to " + popup.GetComponent<GenericUIWindow>().associatedEnumState);
    }

    public void ClosePopup(GameObject popup)
    {
        popup.SetActive(false);
        
        if (popup == loadDeletePopup)
        {
            Game.Menu.State = MenuStateEnum.LoadPage;
        }
        else if (popup == saveDeletePopup)
        {
            Game.Menu.State = MenuStateEnum.SavePage;
        }
        else if (popup == saveSetNamePopup)
        {
            Game.Menu.State = MenuStateEnum.SavePage;
        }
    }

    public void MoveUp()
    {
        print("MENU STATE: " + Game.Menu.State);
        switch (Game.Menu.State)
        {
            case MenuStateEnum.MainPage:
                if (Game.Menu.isLoadedAdditively)
                {
                    // If the game is already running, "back" means returning to the game
                    mainPage.GetComponent<MainPage>().ContinueGame();
                    //print("CONTINUE GAME");
                }
                else
                {
                    // Do nothing
                }
                break;
            case MenuStateEnum.SavePage:
                SwitchToPage(mainPage);
                break;
            case MenuStateEnum.LoadPage:
                SwitchToPage(mainPage);
                break;
            case MenuStateEnum.OptionsPage:
                SwitchToPage(mainPage);
                break;
            case MenuStateEnum.MoviesPage:
                SwitchToPage(mainPage);
                break;
            case MenuStateEnum.SaveDeletePopup:
                ClosePopup(saveDeletePopup);
                break;
            case MenuStateEnum.SaveSetNamePopup:
                ClosePopup(saveSetNamePopup);
                break;
            case MenuStateEnum.LoadDeletePopup:
                ClosePopup(loadDeletePopup);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
