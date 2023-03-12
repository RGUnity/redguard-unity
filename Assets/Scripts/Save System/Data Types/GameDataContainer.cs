using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataContainer
{
    // These have to be saved before the next scene get loaded
    // Or you manually save each SavableObjectData when it gets modified
    public Dictionary<string, SceneData> Scene  = new();

    // These only have to be set before serializing the savefile
    public string LastSceneName;
    public PlayerData Player = new();
    public Dictionary<string, NPCData> NPCDataDict = new();

    public System.DateTime Timestamp;
}
