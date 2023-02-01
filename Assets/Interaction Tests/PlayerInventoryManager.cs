using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    [SerializeField] Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        SetStartValues();
    }
    
    // Kind of hacky but ScriptableObjects keep their values when you exit playmode, ...
    // ... So we reset them here to whatever we want them to be
    void SetStartValues()
    {
        int index = 0;
        // First, clear the inventory list.
        inventory.objects.Clear();
        
        // Then check all known inventory objects
        foreach (var allObj in inventory.allowedObjects)
        {
            // Then set each object's [amount] value to its [StartAmount]
            allObj.amount = allObj.startAmount;
            
            // To find out if an object should be in the inventory, we simply check the amount
            if (allObj.amount > 0)
            {
                inventory.objects.Add(allObj);
                allObj.inventorySlotIndex = inventory.objects.Count-1;

                // And, if it's the first object being added, make it the activeObject
                // because that probably shouldn't be empty
                
                // This is pointless because activeObject is already being read "OnEnable"...
                //.. by InventoryUI, and "OnEnable" happens before "Start"
                if (index == 0)
                {
                    inventory.activeObject = allObj;
                }
                
                // Increment the index
                index++;
            }

        }
    }
}
