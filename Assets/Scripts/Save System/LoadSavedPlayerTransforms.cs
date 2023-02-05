using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolBox.Serialization;
using UnityEditor;

public class LoadSavedPlayerTransforms : MonoBehaviour
{
    [SerializeField] private SceneData _sceneData;
    [SerializeField] private bool useFirstSpawnPoint = false;
    [SerializeField] private Vector3 fallbackPosition = new (0,5,0);

    void Start()
    {
        // If a "nextEntryPoint" exists, use that
        if (_sceneData.nextPlayerSpawnPoint != null)
        {
            Vector3 loadedPosition = _sceneData.nextPlayerSpawnPoint.position;
            print("Use Spawn point " + _sceneData.nextPlayerSpawnPoint.name + " with position "+ loadedPosition);
            // Adding a small Y offset so that we dont fall through the floor
            loadedPosition += new Vector3(0, 1, 0);

            Quaternion loadedRotaion = Quaternion.Euler(_sceneData.nextPlayerSpawnPoint.eulerRotation);
            SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);
        }
        
        else
        {
            // "useFirstSpawnPoint" is primarily helpful for when we start a scene directly while in the Editor
            // Because if you are in a scene, but the saveData is from another scene...
            // ... you will spawn at the wrong position
            
            if (useFirstSpawnPoint)
            {
                Vector3 loadedPosition = _sceneData.allPlayerSpawnPoints[0].position;
                // Adding a small Y offset so that we dont fall through the floor
                loadedPosition += new Vector3(0, 1, 0);
                Quaternion loadedRotaion = Quaternion.Euler(_sceneData.allPlayerSpawnPoints[0].eulerRotation);
                SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);
            }
            else
            {
                // Then, try to load position & rotation from the savefile 
                if (DataSerializer.HasKey("Player_Position") && DataSerializer.HasKey("Player_Rotation"))
                {
                    Vector3 loadedPosition = DataSerializer.Load<Vector3>("Player_Position");
                    Quaternion loadedRotaion = DataSerializer.Load<Quaternion>("Player_Rotation");
                    SetPlayerPositionAndRotation(loadedPosition, loadedRotaion);


                    var sceneName = DataSerializer.Load<String>("CurrentScene");
                    print("Loading player transform data for scene [" + sceneName + "] at position " + loadedPosition);
                }
            
                // If nothing works, use the fallback vector as a position.
                else
                {
                    SetPlayerPositionAndRotation(fallbackPosition, new Quaternion());
                
                    print("No Entry point found. Using fallback position: " + fallbackPosition);
                }
            }

        }
    }

    private void SetPlayerPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
    {
        // The CharacterController must be turned off while we move the player
        var _playerController = gameObject.GetComponent<CharacterController>();
        _playerController.enabled = false;

        gameObject.transform.position = newPosition;
        gameObject.transform.rotation = newRotation;
        
        // aaand back on again
        _playerController.enabled = true;
    }
}
