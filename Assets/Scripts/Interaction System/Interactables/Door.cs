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
            if (!targetSceneName.IsNullOrWhitespace())
            {
                PlayerPrefs.DeleteKey("EnterThroughLoad");
                PlayerPrefs.SetString("EnterThroughDoor", targetSceneName);
                SaveNextStartPoint(targetSpawnPointAsset);

                SceneManager.LoadScene(targetSceneName);
            }
            Debug.LogWarning(gameObject.name + " has no Target Scene. Please enter a valid scene name for string [targetSceneName]");
        }
        print("Player can not enter this door [playerCanEnter] is set to " + playerCanEnter);

    }

    private void SaveNextStartPoint(PlayerSpawnPoint point)
    {
        PlayerPrefs.SetFloat("StartPositionX", point.position.x);
        PlayerPrefs.SetFloat("StartPositionY", point.position.y);
        PlayerPrefs.SetFloat("StartPositionZ", point.position.z);
        PlayerPrefs.SetFloat("StartRotationX", point.eulerRotation.x);
        PlayerPrefs.SetFloat("StartRotationY", point.eulerRotation.y);
        PlayerPrefs.SetFloat("StartRotationZ", point.eulerRotation.z);
    }
}
