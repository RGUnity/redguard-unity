using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Item : Interactable
{
    // Need to link these scriptable objects
    [SerializeField] InventoryData _inventoryData;
    public InventoryObjectInfo objectInfo;

    // The ranges for the randomized amounts that this item can be dropped in
    public int minAmount = 3;
    public int maxAmount = 15;

    public bool isDeleted = false;

    public string id;

    public override void Interact()
    {
   
        // Generate a random amount
        int amount = Random.Range(minAmount, maxAmount);

        // Add the object with the amount
        InventoryManager.AddItems(objectInfo.id, amount);
        
        // Mark this object as deleted
        isDeleted = true;
        
        // And update its data in the data tree
        GameSaver.RememberObject(gameObject);
        
        // Delete the object
        Destroy(gameObject);
    }
    
    private void Start()
    {
        // This is primarily for objects that get spawned after the scene was loaded
        // We could probably also call this manually when objects are spawned
        if (!string.IsNullOrEmpty(id))
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
