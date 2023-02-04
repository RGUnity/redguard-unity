using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewPlayerSpawnPoint", menuName = "ScriptableObjects/PlayerSpawnPoint")]


public class PlayerSpawnPoint : ScriptableObject
{
    [Header("Set these values with the button on [Set Entry Points Transforms]")]
    public Vector3 position;
    public Vector3 eulerRotation;
}
