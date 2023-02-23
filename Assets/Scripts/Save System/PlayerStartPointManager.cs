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
    // This point is used, if we enter neither through a savefile nor a door
    //[SerializeField] private GameObject initialSpawnPoint;
    // In the editor, this overrides the initialSpawnPoint. Has no effect in builds.
    [SerializeField] private GameObject editorSpawnPoint;

    void Awake()
    {
        // An optional setting to start from a specific point while playing in the Editor
        // Bit flawed, because it also overrides the spawn point if you enter through a door
        if (editorSpawnPoint != null && Application.isEditor)
        {
            UseSpecificSpawnPoint(editorSpawnPoint);
            editorSpawnPoint = null;
            print("Player Start point: [editorSpawnPoint]");
        }
        
        // If we are not in the editor, proceed with the regular checks.
        else
        {
            // First, check if we have any entrance key at all. If not, use the initialSpawnPoint.
            if (!PlayerPrefs.HasKey("EnterThroughDoor") && !PlayerPrefs.HasKey("EnterThroughLoad"))
            {
                print("No spawn point specified. Player will not be moved.");
                
                // Decided to keep this off for now, Since we can also just use the point that the ...
                // ... Player object is at by default. Makes it easier for testing in the editor too, i think.
                
                //UseSpecificSpawnPoint(initialSpawnPoint);
                // print("Player Start point: None specified. Using [initialSpawnPoint]");
            }
            
            // or else, check which key we have.
            else
            {
                // If this is true, the Player has entered the scene through a door
                if (PlayerPrefs.HasKey("EnterThroughDoor"))
                {
                    UseSceneSpawnPoint();
                    print("Entered scene through Door, probably");
                    
                    // Info: The entrance key is then deleted on Start in GameSaveManager.cs
                }
            
                // If this is true, the Player is reloading from a savefile
                if (PlayerPrefs.HasKey("EnterThroughLoad"))
                {
                    // Do a quick check if the savefile actually has the Vector3s
                    var transformDataIsOk = DataSerializer.HasKey("Player_Position") && DataSerializer.HasKey("Player_Rotation");
                    if (transformDataIsOk)
                    {
                        UseSavedPlayerSpawnPoint();
                        print("Entered scene through a Reload, probably");
                        
                        // Info: The entrance key is then deleted on Start in GameSaveManager.cs
                    }
                    else
                    {
                        // Do nothing
                        Debug.LogWarning("Failed to load [Player_Position] and [Player_Rotation] from savefile. Player will not be moved");
                    }
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
