using UnityEngine;
using System;
using System.Collections.Generic;

public class LoadWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
Dictionary<int, RGINIStore.itemData> itemdict;
    void Start()
    {
// OBS entry
//        WorldLoader.LoadWorld(5,0,0);
// OBS exit
//        WorldLoader.LoadWorld(1,6,1024);
// HARB3 entry
// SILVER1 entry
        WorldLoader.RequestLoadWorld(18,0,1024);
        worldChanged = WorldLoader.LoadWorldIfRequested();

RG3DStore.LoadMeshIntermediatesROB("INVENTRY");
/*
 itemdict = RGINIStore.GetItemList();
foreach(KeyValuePair<int, RGINIStore.itemData> entry in itemdict)
{
    Debug.LogWarning($"item {entry.Key}: {entry.Value.name} | {entry.Value.description}");
}
*/

// START, unknown entry
//        WorldLoader.LoadWorld(0,0,0);

/*
        RGRGMScriptStore.flags[203] = 1; // OB_Fixed
        RGRGMScriptStore.flags[362] = 2; // DEADPIRATES
*/

     }
    // Update is called once per frame
    bool worldChanged;
    void Update()
    {
        worldChanged = WorldLoader.LoadWorldIfRequested();
        if(Input.GetKeyUp("space"))
            Game.uiManager.HideLoadingScreen();
        if(Input.GetKeyDown("space"))
        {
            Material loadScreenMat = RGTexStore.GetMaterial_GXA("ISLAND", 0);
            Game.uiManager.ShowLoadingScreen(loadScreenMat.mainTexture);
        }
    }
}
