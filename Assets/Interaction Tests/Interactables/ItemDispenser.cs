using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class generates random loot from a list of Loot Items.
// In Redguard, this basically how all dead bodies work.
// If you want it to only drop one specific item, just add a single object to the list
public class ItemDispenser : Interactable
{
    [SerializeField] Inventory inventory;
    [SerializeField] List<InventoryObject> inventoryStackLink;
    [SerializeField] private bool isEmpty = false;
    [SerializeField] List<LootObject> possibleObjects;


    public override void Interact()
    {
        // Make sure the Dispenser hasn't been looted already
        if (!isEmpty)
        {
            // In the original, randomized loot is generated when the NPC dies.
            // For now, we just do it when the player interacts
            LootObject randomObject = GenerateLoot();
            
            // First, Check if there already is an object of the same (Stack-)type in the inventory.
            // If not, add the Stack Type to the inventory list.
            // The stack type is a direct reference to the inventory object, and must be set in the scriptable object.
            if (!inventory.objects.Contains(randomObject.stackType))
            {
                inventory.objects.Add(randomObject.stackType);
            }
            
            // Check the Objects in the inventory, and once you find one with the same stack type...
            // ...add more of the specified amount
            foreach (var InventoryObject in inventory.objects)
            {
                if (InventoryObject == randomObject.stackType)
                {
                    InventoryObject.amount += randomObject.amount;
                    print("Added " +randomObject.amount + " " + InventoryObject.displayName);
                }
            }
            
            // Mark the Dispenser as empty so that the player can no longer interact with it
            isEmpty = true;
        }

    }

    // Pick a random element from the "possibleItems" list by generating an integer, ...
    // ... using the length of the list as the range for Random.Range().
    private LootObject GenerateLoot()
    {
        int randomIndex = Random.Range(0, possibleObjects.Count);
        LootObject randomObject = possibleObjects[randomIndex];
        return randomObject;
    }
}
