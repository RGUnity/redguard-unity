using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    [SerializeField] Inventory inventory;
    // All inventory objects must be linked in one of the to lists
    [SerializeField] StartInventory startInventory;
    [SerializeField] Inventory AllInventoryObjects;

    // Start is called before the first frame update
    void Start()
    {
        SetStartValues();
    }
    
    // Kind of hacky but ScriptableObjects keep their values when you exit playmode, ...
    // ... So we reset them here to whatever we want them to be
    void SetStartValues()
    {
        // First, reset all "amounts" to zero and clear the inventory list
        foreach (var regObj in AllInventoryObjects.objects)
        {
            regObj.amount = 0;
            inventory.objects.Clear();
        }
        
        // Loop through the list of startInventory objects,and add them to the real inventory one by one
        // and set their "amounts" at the same time
        foreach (StartInventoryObject startObj in startInventory.objects)
        {
            if (startObj.amount > 0)
            {
                inventory.objects.Add(startObj.stackType);
                startObj.stackType.amount = startObj.amount;
            }
            else
            {
                Debug.LogWarning("Amount of inventory start item ---" + startObj.name + "--- is less than 1. Item is not added to inventory.");
            }
        }
    }
}
