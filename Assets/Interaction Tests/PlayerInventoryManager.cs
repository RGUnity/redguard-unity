using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    [SerializeField] Inventory inventory;
    // All inventory objects must be linked in one of the to lists
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
        // First, clear the inventory list.
        inventory.objects.Clear();
        
        // Then set each object's [amount] value to its [StartAmount]
        // Lastly, add objects with an amount greater than 0 to the inventory list
        foreach (var regObj in AllInventoryObjects.objects)
        {
            regObj.amount = regObj.startAmount;
            
            if (regObj.amount > 0)
            {
                inventory.objects.Add(regObj);
            }
        }
    }
}
