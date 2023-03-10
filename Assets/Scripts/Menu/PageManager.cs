using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject savePage;
    public GameObject loadPage;
    public GameObject optionsPage;
    public GameObject moviesPage;
    
    public List<GameObject> allPages;
    
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
            moviesPage
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") & Game.Menu.State != MenuStateEnum.MainPage)
        {
            print("Trying to switch page");
            SwitchToPage(mainPage);
            Game.Menu.State = MenuStateEnum.MainPage;
        }
    }
    
    public void SwitchToPage(GameObject targetPage)
    {
        foreach (var page in allPages)
        {
            if (page != targetPage)
            {
                page.SetActive(false);
            }
        }
        
        targetPage.SetActive(true);
    }
}
