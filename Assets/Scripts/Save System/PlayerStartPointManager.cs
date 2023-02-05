using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolBox.Serialization;
using UnityEditor;

public class PlayerStartPointManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SceneData _sceneData;
    [SerializeField] private EditorSpawnPointOverride overrideInEditor;

    void Start()
    {


        if (Application.isEditor && overrideInEditor != EditorSpawnPointOverride.None)
        {

            if (overrideInEditor == EditorSpawnPointOverride.DoNotMove)
            {
                // Do nothing
                print("EditorSpawnPointOverride == DoNotMove. Player will not be moved to a spawn point");
            }

            if (overrideInEditor == EditorSpawnPointOverride.FirstAvailablePoint)
            {
                UseFirstPlayerSpawnPoint();
                print("EditorSpawnPointOverride == FirstAvailablePoint. Moving Player to first available spawn point ");
            }
        }
        
        else
        {
            // True -> Player is reloading from a savefile
            // False -> Player has entered the scene through a door
            if (_sceneData.canLoadSavedPlayerTransforms)
            {
                bool transformDataIsOK = DataSerializer.HasKey("Player_Position") && DataSerializer.HasKey("Player_Rotation");
                if (transformDataIsOK)
                {
                    UseSavedPlayerSpawnPoint();
                }
                else
                {
                    UseFirstPlayerSpawnPoint();
                    Debug.LogWarning("Failed to load Player transform data. Using first known spawn point");
                }
            }
            else
            {
                if (_sceneData.playerSpawnPoint != null)
                {
                    UseSceneSpawnPoint();
                }
                else
                {
                    UseFirstPlayerSpawnPoint();
                    Debug.LogWarning("Missing [playerSpawnPoint] selection. Using first known spawn point. If you are in Editor, this is probably okay.");
                }
            }
        }
    }
    

    private void SetPlayerPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        // The CharacterController must be turned off while we move the player
        var _playerController = player.GetComponent<CharacterController>();
        _playerController.enabled = false;

        player.transform.position = newPosition;
        player.transform.rotation = newRotation;
        
        // aaand back on again
        _playerController.enabled = true;
    }

    private void UseFirstPlayerSpawnPoint()
    {
        Vector3 loadedPosition = _sceneData.allPlayerSpawnPoints[0].position;
        // Adding a small Y offset so that we dont fall through the floor
        loadedPosition += new Vector3(0, 1, 0);
        Quaternion loadedRotaion = Quaternion.Euler(_sceneData.allPlayerSpawnPoints[0].eulerRotation);
        SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);
    }

    private void UseSavedPlayerSpawnPoint()
    {
        Vector3 loadedPosition = DataSerializer.Load<Vector3>("Player_Position");
        Quaternion loadedRotaion = DataSerializer.Load<Quaternion>("Player_Rotation");
        SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);

        var sceneName = DataSerializer.Load<String>("CurrentScene");
        print("Loading player transform data for scene [" + sceneName + "] at position " + loadedPosition);
    }

    // Only works if:
    // !_sceneData.canLoadSavedPlayerTransforms
    // _sceneData.playerSpawnPoint != null
    private void UseSceneSpawnPoint()
    {
        Vector3 loadedPosition = _sceneData.playerSpawnPoint.position;
        print("Use Spawn point " + _sceneData.playerSpawnPoint.name + " with position "+ loadedPosition);
        // Adding a small Y offset so that we dont fall through the floor
        loadedPosition += new Vector3(0, 1, 0);

        Quaternion loadedRotaion = Quaternion.Euler(_sceneData.playerSpawnPoint.eulerRotation);
        SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);
    }
}
