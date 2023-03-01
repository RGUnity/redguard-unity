using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointOverride : MonoBehaviour
{
    [SerializeField] private GameObject editorSpawnPoint;

    void Start()
    {
        // An optional setting to start from a specific point while playing in the Editor
        // Bit flawed, because it also overrides the spawn point if you enter through a door
        if (editorSpawnPoint != null && Application.isEditor)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerController = player.GetComponent<CharacterController>();
            
            playerController.enabled = false;
            
            player.transform.position = editorSpawnPoint.transform.position + new Vector3(0, 1, 0);
            player.transform.rotation = editorSpawnPoint.transform.rotation;
            
            playerController.enabled = true;
            
            
            Debug.LogWarning("Using override SpawnPoint " + editorSpawnPoint.name);
            
            // Set this to null because we dont want to use this point again after we pressed play
            editorSpawnPoint = null;
        }
    }
}
