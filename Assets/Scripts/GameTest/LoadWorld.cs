using UnityEngine;
using System;
using System.Collections.Generic;

public class LoadWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
// OBS entry
//        WorldLoader.LoadWorld(5,0,0);
// OBS exit
//        WorldLoader.LoadWorld(1,6,1024);
// HARB3 entry
// SILVER1 entry
        WorldLoader.LoadWorld(18,0,1024);

RG3DStore.LoadMeshIntermediatesROB("INVENTRY");
Dictionary<int, RGINIStore.itemData> itemdict = RGINIStore.GetItemList();
foreach(KeyValuePair<int, RGINIStore.itemData> entry in itemdict)
{
    Debug.LogWarning($"item {entry.Key}: {entry.Value.name} | {entry.Value.description}");
}

// START, unknown entry
//        WorldLoader.LoadWorld(0,0,0);

        RGRGMScriptStore.flags[203] = 1; // OB_Fixed
        RGRGMScriptStore.flags[362] = 2; // DEADPIRATES

     }
    // Update is called once per frame
    bool worldChanged;
    void Update()
    {
        worldChanged = WorldLoader.LoadWorldIfRequested();
        if(Input.GetKeyUp("space"))
        {
            Game.uiManager.ShowInteractionText("HEYOO");
            Game.uiManager.AddDialogueOption("OPT1");
            Game.uiManager.AddDialogueOption("OPT2");
            Game.uiManager.AddDialogueOption("OPT3");
            Game.uiManager.AddDialogueOption("OPT4");
        }
    }
}
