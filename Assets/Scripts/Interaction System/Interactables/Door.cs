using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    [SerializeField] private bool _playerCanEnter = true;
    
    [SerializeField] private SceneData _targetSceneData;
    [SerializeField] private PlayerSpawnPoint _targetSpawnPointAsset;


    
    public override void Interact()
    {
        if (_playerCanEnter)
        {
            _targetSceneData.playerSpawnPoint = _targetSpawnPointAsset;
            PlayerPrefs.SetInt("EnterThroughDoor", 0);
            SaveNextStartPoint(_targetSpawnPointAsset);

            SceneManager.LoadScene(_targetSceneData.sceneName);
        }

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
