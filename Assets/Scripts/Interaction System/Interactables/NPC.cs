using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization.OdinSerializer.Utilities;
using UnityEngine;

//[RequireComponent(typeof(IDGenerator))]
public class NPC : Interactable
{
    public NPCConfig config;
    public string id;
    public NPCStateEnum actionState;
    public int health = 1;

    [SerializeField] private GameObject gameSaveManager;
    public override void Interact()
    {
        print("TODO: Start conversation with NPC");
    }



    private void Awake()
    {
        RegisterNPC();
    }

    public void RegisterNPC()
    {
        if (id.IsNullOrWhitespace())
        {
            Debug.LogWarning("NPC [" + gameObject.name + "] has no ID assigned! Object can not be saved.");
        }

        var dict = GameSaveManager.sceneNPCDict;
        // Then, if the id of the object is not already in the list, we can add it
        if (GameSaveManager.sceneNPCDict.ContainsKey(id))
        {
            if (dict[id].gameObject != gameObject &&
                dict[id].gameObject != null)
            {
                Debug.LogWarning(name +" at " +transform.position+  " has no unique ID! Please generate a new one.");
            }
            
            if (dict[id].gameObject == gameObject)
            {
                // This should only happen in the editor
                Debug.LogWarning("I guess " + name +" at " + transform.position+  " already was in the sceneNPCDict...? If you are playing in the Editor, that should be okay.");
            }

            if (dict[id].gameObject == null)
            {
                dict.Remove(id);
                print("Found one but its null");
                
                dict.Add(id, gameObject);
                //print("Added [" + gameObject.name + "] to sceneNPCDict");
            }
        }
        else
        {
            dict.Add(id, gameObject);
            //print("Added [" + gameObject.name + "] to sceneNPCDict");
        }
    }
    

    public void GenerateID()
    {
        string guid = Guid.NewGuid().ToString();

        int numberLength = 8;
        string shortGUID = guid.Remove(numberLength, guid.Length-numberLength);

        id = name.Replace(" ", "") +"--" + shortGUID;
        print("Generated new GUID for " + name);
    }
    
    public void RemoveHealth(int negativeHealth)
    {
        health -= negativeHealth;
        if (health <= 0)
        {
            health = 0;
            actionState = NPCStateEnum.Dead;
            
            // We simply move the NPC flat on the ground to visualise its death
            transform.localEulerAngles = new Vector3(-90, transform.localEulerAngles.y, 0);
        }
    }
}
