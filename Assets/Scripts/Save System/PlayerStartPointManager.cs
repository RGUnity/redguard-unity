using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolBox.Serialization;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerStartPointManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SceneData sceneData;
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
            var thisSceneName = SceneManager.GetActiveScene().name;
            var loadedSceneName = PlayerPrefs.GetString("EnterThroughDoor");
            
            // True -> Player has entered the scene through a door
            // False -> Player is reloading from a savefile

            //if (PlayerPrefs.HasKey("EnterThroughDoor"))
            if (thisSceneName == loadedSceneName)
            {
                UseSceneSpawnPoint();
                print("Entered scene through Door, probably");
            }

            else
            {
                var transformDataIsOk = DataSerializer.HasKey("Player_Position") && DataSerializer.HasKey("Player_Rotation");
                if (transformDataIsOk)
                {
                    UseSavedPlayerSpawnPoint();
                    print("Entered scene through a Reload, probably");
                }
                else
                {
                    UseFirstPlayerSpawnPoint();
                    Debug.LogWarning("Failed to load Player transform data. Using first known spawn point");
                }
            }
        }
    }
    

    private void SetPlayerPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        // The CharacterController must be turned off while we move the player
        var playerController = player.GetComponent<CharacterController>();
        playerController.enabled = false;

        player.transform.position = newPosition;
        player.transform.rotation = newRotation;
        
        // aaand back on again
        playerController.enabled = true;
    }

    private void UseFirstPlayerSpawnPoint()
    {
        Vector3 loadedPosition = sceneData.allPlayerSpawnPoints[0].position;
        // Adding a small Y offset so that we dont fall through the floor
        loadedPosition += new Vector3(0, 1, 0);
        Quaternion loadedRotaion = Quaternion.Euler(sceneData.allPlayerSpawnPoints[0].eulerRotation);
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
    
    private void UseSceneSpawnPoint()
    {
        Vector3 loadedPosition = new Vector3(
            PlayerPrefs.GetFloat("StartPositionX"),
            PlayerPrefs.GetFloat("StartPositionY"),
            PlayerPrefs.GetFloat("StartPositionZ")
            );
        // Adding a small Y offset so that we dont fall through the floor
        loadedPosition += new Vector3(0, 1, 0);
        print("Use Spawn point " + PlayerPrefs.GetString("EnterThroughDoor") + " with position "+ loadedPosition);


        Quaternion loadedRotaion = Quaternion.Euler(
            PlayerPrefs.GetFloat("StartRotationX"),
            PlayerPrefs.GetFloat("StartRotationY"),
            PlayerPrefs.GetFloat("StartRotationZ")
            );
        SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);
    }
}
