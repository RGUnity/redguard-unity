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
        switch (actionState)
        {
            case NPCStateEnum.Idle:
                print("TODO: Start conversation with NPC");
                break;
            case NPCStateEnum.Patrolling:
                print("TODO: Start conversation with NPC");
                break;
            case NPCStateEnum.Fighting:
                // NPC can not talk
                break;
            case NPCStateEnum.Dead:
                // NPC will talk even less now
                if (TryGetComponent(out ItemDispenser dispenser))
                {
                    dispenser.AddItems();
                }
                else
                {
                    print("NPC " + gameObject.name + " is dead but has no ItemDispenser");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
