using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "ScriptableObjects/SceneData")]


public class SceneData : ScriptableObject
{
    public string sceneName;
    public PlayerSpawnPoint nextEntryPoint;
    public List<PlayerSpawnPoint> playerSpawnPoints;
}
