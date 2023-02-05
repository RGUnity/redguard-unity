using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "ScriptableObjects/SceneData")]


public class SceneData : ScriptableObject
{
    [Header("Config Variables")]
    public string sceneName;
    public List<PlayerSpawnPoint> allPlayerSpawnPoints;
    
    [Header("Dynamic Start Parameters for this Scene")]
    public PlayerSpawnPoint playerSpawnPoint;

    public bool canLoadSavedPlayerTransforms;
    public bool canLoadSavedInventory;
}
