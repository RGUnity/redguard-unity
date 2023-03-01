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
    public NPCStateEnum actionState;
    public int health = 1;
    public string id;
    
    public override void Interact()
    {
        print("TODO: Start conversation with NPC");
    }
    
  
    public void GenerateID()
    {
        string guid = Guid.NewGuid().ToString();

        int numberLength = 8;
        string shortGUID = guid.Remove(numberLength, guid.Length-numberLength);

        id = name.Replace(" ", "") +"--" + shortGUID;
        print("Generated new ID for " + name);
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
