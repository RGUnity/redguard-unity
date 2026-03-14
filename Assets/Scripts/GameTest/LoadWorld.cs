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
//        WorldLoader.RequestLoadWorld(1,6,1024);
// SILVER1 entry
        WorldLoader.RequestLoadWorld(18,0,1024);
        WorldLoader.LoadWorldIfRequested();

RG3DStore.LoadMeshIntermediatesROB("INVENTRY");
/*
Dictionary<int, RGINIStore.itemData> itemdict;
 itemdict = RGINIStore.GetItemList();
foreach(KeyValuePair<int, RGINIStore.itemData> entry in itemdict)
{
    Debug.LogWarning($"item {entry.Key}: {entry.Value.name} | {entry.Value.description}");
}
*/

     }
    // Update is called once per frame
    void Update()
    {
        WorldLoader.LoadWorldIfRequested();

        if(Input.GetKeyUp("space"))
            Game.uiManager.HideLoadingScreen();
        if(Input.GetKeyDown("space"))
        {
            Material loadScreenMat = RGTexStore.GetMaterial_GXA("ISLAND", 0);
            Game.uiManager.ShowLoadingScreen(loadScreenMat.mainTexture);
        }
    }
}
