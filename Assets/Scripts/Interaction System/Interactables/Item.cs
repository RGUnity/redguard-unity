using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization.OdinSerializer.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Item : Interactable
{
    // Need to link these scriptable objects
    [SerializeField] InventoryData _inventoryData;
    public InventoryObjectType objectType;

    // The ranges for the randomized amounts that this item can be dropped in
    public int minAmount = 3;
    public int maxAmount = 15;

    public bool isDeleted = false;

    public string id;

    public override void Interact()
    {
        // If the Object type is not yet part of the Inventory List, add it
        if (!_inventoryData.objects.Contains(objectType))
        {
            _inventoryData.objects.Add(objectType);
        }
        // Increase the "amount" value in the Scriptable Object by the specified amount
        int addAmount = Random.Range(minAmount, maxAmount);
        objectType.amount += addAmount;
        print("Added " + addAmount + " " + objectType.displayName);
        
        
        isDeleted = true;
        GameSaver.RememberObject(gameObject);
        
        
        
        // Delete the object
        Destroy(this.gameObject);
    }
    
    private void Start()
    {
        if (!id.IsNullOrWhitespace())
        {
            GameSaver.RememberObject(gameObject);
        }
        else
        {
            // This probably means the NPC is not meant to be remembered, or will get its ID after being spawned
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
}
