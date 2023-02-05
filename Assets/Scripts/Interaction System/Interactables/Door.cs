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
            _targetSceneData.canLoadSavedPlayerTransforms = false;
            _targetSceneData.canLoadSavedInventory = false;
            
            SceneManager.LoadScene(_targetSceneData.sceneName);
        }

    }
}
