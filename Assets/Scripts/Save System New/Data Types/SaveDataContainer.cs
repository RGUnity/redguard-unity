using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataContainer
{
    // These have to be saved before the next scene get loaded
    // Or you manually save each SavableObjectData when it gets modified
    public Dictionary<string, SavableSceneData> SceneDataCache;
    // Inventory?
    
    // These only have to be set before serializing the savefile
    public string LastSceneName;
    public SavablePlayerData SavablePlayerData;
    public Dictionary<string, SavableNPCData> NPCDataDict = new();

}
