using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private bool useDefaultInventory;

    // Start is called before the first frame update
    void Start()
    {
        if (useDefaultInventory)
        {
            RestoreInventoryDefaults();
        }
        else
        {
            RebuildInventory();
        
            // Maybe if the inventory is empty, we should restore the default as well?
            // I will keep this disabled for now.
            
            if (_inventoryData.objects.Count == 0)
            {
                RestoreInventoryDefaults();
            }
        }

    }
    
    private void RebuildInventory()
    {
        // if object has amount <= 0, remove it
        foreach (var objType in _inventoryData.allowedObjects)
        {
            if (objType.amount <= 0)
            {
                //_inventoryData.objects.Add(objType);
                _inventoryData.objects.Remove(objType);
            }
            
            // if (!_inventoryData.objects.Contains(objType))
            // {
            //     objType.amount = 0;
            // }
        }
    }

    private void RestoreInventoryDefaults()
    {
        print("Restoring inventory Defaults");
        _inventoryData.objects.Clear();
        foreach (var obj in _inventoryData.allowedObjects)
        {
            if (obj.startAmount > 0)
            {
                _inventoryData.objects.Add(obj);
                obj.amount = obj.startAmount;
            }
            else
            {
                obj.amount = 0;
            }
        }
    }
    
    // Kind of hacky but ScriptableObjects keep their values when you exit playmode, ...
    // ... So we reset them here to whatever we want them to be
    void SetStartValues()
    {
        int index = 0;
        // First, clear the inventory list.
        _inventoryData.objects.Clear();
        
        // Then check all known inventory objects
        foreach (var allObj in _inventoryData.allowedObjects)
        {
            // Then set each object's [amount] value to its [StartAmount]
            allObj.amount = allObj.startAmount;
            
            // To find out if an object should be in the inventory, we simply check the amount
            if (allObj.amount > 0)
            {
                _inventoryData.objects.Add(allObj);

                // 1. And, if it's the first object being added, make it the activeObject
                // because that probably shouldn't be empty
                
                // 2. Okay, actually this is pointless because activeObject is already being read "OnEnable"...
                //.. by InventoryUI, and "OnEnable" happens before "Start"
                
                // 3. I suggest we keep this turned off. This means the activeObject will always ...
                // ... be pulled from the Scriptable Object, will not revert to any default value.
                
                // if (index == 0)
                // {
                //     _inventoryData.activeObject = allObj;
                // }
                
                // Increment the index
                index++;
            }

        }
    }
}
