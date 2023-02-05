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
            //print("This door should load a scene");
            _targetSceneData.nextPlayerSpawnPoint = _targetSpawnPointAsset;
            //_targetSceneData.
            //print("Next SpawnPoint: " + _targetSceneData.nextEntryPoint);
            SceneManager.LoadScene(_targetSceneData.sceneName);
        }

    }
}
