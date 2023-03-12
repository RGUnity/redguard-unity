using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


// This class generates random loot from a list of "Loot Items".
// These LootItems are just presets with a type and amount.
// In Redguard, this basically how all dead bodies work.
// If you want it to only drop one specific item type, just add a single object to the list
public class ItemDispenser : MonoBehaviour
{
    [SerializeField] private bool isEmpty = false;
    [SerializeField] List<LootObject> possibleObjects;

    public void AddItems()
    {
        // Make sure the Dispenser hasn't been looted already
        if (!isEmpty)
        {
            // Pick a random LootObject
            LootObject randomObject = RandomLootObject();
            
            InventoryManager.AddItems(randomObject.objectInfo.id, randomObject.amount);

            // Mark the Dispenser as empty so that the player can no longer interact with it
            isEmpty = true;
        }
    }

    // Pick a random element from the "possibleItems" list by generating an integer, ...
    // ... using the length of the list as the range for Random.Range().
    private LootObject RandomLootObject()
    {
        int randomIndex = Random.Range(0, possibleObjects.Count);
        LootObject randomObject = possibleObjects[randomIndex];
        
        return randomObject;
    }
}
