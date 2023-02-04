using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization;
using ToolBox.Serialization.OdinSerializer.Utilities;
using UnityEditor;
using UnityEngine;

public class DeletableObject : MonoBehaviour
{
    
    [SerializeField] private string GUID;
    

    void Start()
    {
        CheckDeletionStatus();
    }

    // GUID Generator. Triggered when Button is pressed.
    public void GenerateGUID()
    {
        GUID = Guid.NewGuid().ToString();
    }
    
    // This should be triggered in the "item" script, when the player interacts
    public void RememberAsDeleted()
    {
        GameSaveManager.deletedObjectCache.Add(GUID);
    }

    private void CheckDeletionStatus()
    {
        if (!GUID.IsNullOrWhitespace())
        {
            // If the savefile contains a matching key, delete this object on start
            if (DataSerializer.TryLoad(GUID, out bool isDeleted))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no GUID assigned! To fix, press the [Generate GUID] button on this script");
        }
    }
}
