using UnityEngine;
using System;
using System.Collections.Generic;

public class LoadWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
// OBS entry
//        WorldLoader.RequestLoadWorld(5,0,0);
// OBS exit
        WorldLoader.RequestLoadWorld(1,6,1024);
// TEMPLE entry
//        WorldLoader.RequestLoadWorld(12,0,0);
// JFFER entry
//        WorldLoader.RequestLoadWorld(26,1,1724);
// GERRICKS entry
//        WorldLoader.RequestLoadWorld(22,0,1024);
// SILVER1 entry
//        WorldLoader.RequestLoadWorld(18,0,1024);
        WorldLoader.LoadWorldIfRequested();

      }
    // Update is called once per frame
    void Update()
    {
        WorldLoader.LoadWorldIfRequested();

        if(Input.GetKeyUp("space"))
            Game.uiManager.HideLoadingScreen();
        if(Input.GetKeyDown("space"))
        {
            Material loadScreenMat = FFIGxaLoader.GetMaterial_GXA("ISLAND", 0);
            if (loadScreenMat != null)
            {
                Game.uiManager.ShowLoadingScreen(loadScreenMat.mainTexture);
            }
        }
    }
}
