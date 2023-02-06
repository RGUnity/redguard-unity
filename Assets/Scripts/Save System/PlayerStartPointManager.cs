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
    [SerializeField] private GameObject editorSpawnPoint;

    void Start()
    {
        // An optional setting to start from a specific point while playing in the Editor
        if (editorSpawnPoint != null && Application.isEditor)
        {
            UseSpecificSpawnPoint(editorSpawnPoint);
            editorSpawnPoint = null;
        }
        
        else
        {
            var thisSceneName = SceneManager.GetActiveScene().name;
            var loadedSceneName = PlayerPrefs.GetString("EnterThroughDoor");
            
            // True -> Player has entered the scene through a door
            // False -> Player is reloading from a savefile

            // Must make sure we dont accidentally load coordinates from another scene
            if (PlayerPrefs.HasKey("EnterThroughDoor"))
            //if (thisSceneName == loadedSceneName)
            {
                UseSceneSpawnPoint();
                print("Entered scene through Door, probably");
                PlayerPrefs.DeleteKey("EnterThroughDoor");
            }
            
            if (PlayerPrefs.HasKey("EnterThroughLoad"))
            {
                var transformDataIsOk = DataSerializer.HasKey("Player_Position") && DataSerializer.HasKey("Player_Rotation");
                if (transformDataIsOk)
                {
                    UseSavedPlayerSpawnPoint();
                    print("Entered scene through a Reload, probably");
                    PlayerPrefs.DeleteKey("EnterThroughLoad");
                }
                else
                {
                    // Do nothing
                    Debug.LogWarning("Failed to load [Player_Position] and [Player_Rotation] from savefile. Player will not be moved");
                }
            }

            if (!PlayerPrefs.HasKey("EnterThroughDoor") && PlayerPrefs.HasKey("EnterThroughLoad"))
            {
                print("No Spawn Point specified. Player will not be moved");
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


    private void UseSpecificSpawnPoint(GameObject obj)
    {
        var loadedPosition = obj.transform.position;
        var loadedRotation = obj.transform.rotation;
        loadedPosition += new Vector3(0, 1, 0);
        SetPlayerPositionAndRotation(loadedPosition, loadedRotation);
        print("Start at editorSpawnPoint: " + obj);
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
