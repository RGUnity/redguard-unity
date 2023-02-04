using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointUpdater : MonoBehaviour
{
    [SerializeField] private PlayerSpawnPoint _spawnPoint;


    public void SetTransformData()
    {
        _spawnPoint.position = gameObject.transform.position;
        _spawnPoint.eulerRotation = gameObject.transform.rotation.eulerAngles;
    }
}
