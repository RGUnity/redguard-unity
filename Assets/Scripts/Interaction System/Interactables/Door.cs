using System.Collections;
using System.Collections.Generic;
using System.IO;
using ToolBox.Serialization.OdinSerializer.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    [SerializeField] private bool playerCanEnter = true;
    [SerializeField] private string targetSceneName;
    [SerializeField] private PlayerSpawnPoint targetSpawnPointAsset;

    
    public override void Interact()
    {
        if (playerCanEnter)
        {
            if (targetSceneName.IsNullOrWhitespace())
            {
                Debug.LogWarning("string: [targetSceneName] is null. Please enter a valid scene name");
            }

            if (targetSpawnPointAsset == null)
            {
                Debug.LogWarning("PlayerSpawnPoint: [targetSpawnPointAsset] is null. Please link a valid asset");
            }
            if (!targetSceneName.IsNullOrWhitespace() && targetSpawnPointAsset != null)
            {
                
                // To tell the next scene that we entered it through a door
                Game.EnterSceneMode = EnterSceneModeEnum.Door;
                
                // Get the coordinates of the next spawn Point and store it in memory 
                SaveNextStartPoint(targetSpawnPointAsset);
                
                // Load the scene
                SceneManager.LoadScene(targetSceneName);
            }
        }
        else
        {
            print("Player can not enter this door [playerCanEnter] is set to " + playerCanEnter);
        }
        

    }

    private void SaveNextStartPoint(PlayerSpawnPoint point)
    {
        Game.Data.Player.PlayerPosition = point.position;
        Game.Data.Player.PlayerRotation = Quaternion.Euler(point.eulerRotation);
    }
}
