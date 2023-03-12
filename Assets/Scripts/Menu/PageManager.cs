using System;
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
        //print(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
        var timeA = DateTime.Now;

        var timeB = DateTime.Now.AddMinutes(1);

        //print(timeA - timeB);
        int result = timeA.CompareTo(timeB);
        
        if (result < 0)
        {
            print("time2 is newer than time1");
        }
        else if (result > 0)
        {
            print("time1 is newer than time2");
        }
        else
        {
            print("time1 and time2 are equal");
        }


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
