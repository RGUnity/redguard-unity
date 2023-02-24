using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization;
using ToolBox.Serialization.OdinSerializer.Utilities;
using UnityEditor;
using UnityEngine;

public class SavableObject : MonoBehaviour
{
    
    [SerializeField] private string id;
    

    void Start()
    {
        CheckDeletionStatus();
    }

    // GUID Generator. Triggered when Button is pressed.
    public void GenerateID()
    {
        string guid = Guid.NewGuid().ToString();

        int numberLength = 8;
        string shortGUID = guid.Remove(numberLength, guid.Length-numberLength);

        id = name.Replace(" ", "") +"--" + shortGUID;
        print("Generated new ID for " + name);
    }
    
    // This should be triggered in the "item" script, when the player interacts
    public void RegisterObject()
    {
        GameSaveManager.saveableObjects.Add(id, gameObject);
    }

    private void CheckDeletionStatus()
    {
        if (!id.IsNullOrWhitespace())
        {
            // If the savefile contains a matching key, delete this object on start
            if (DataSerializer.TryLoad(id, out bool isDeleted))
            {
                Destroy(gameObject);
                print(gameObject.name + " Found in Savefile");
            }

            if (GameSaveManager.saveableObjects.ContainsKey(id))
            {
                Destroy(gameObject);
                print(gameObject.name + " Found in deletedObjectCache");
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no GUID assigned! To fix, press the [Generate GUID] button on this script");
        }
    }
}
