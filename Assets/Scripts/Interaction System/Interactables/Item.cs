using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : Interactable
{
    // Need to link these scriptable objects
    [SerializeField] InventoryData _inventoryData;
    [SerializeField] InventoryObjectType _objectType;

    // The ranges for the randomized amounts that this item can be dropped in
    [SerializeField] int minAmount = 3;
    [SerializeField] int maxAmount = 15;

    public override void Interact()
    {
        // If the Object type is not yet part of the Inventory List, add it
        if (!_inventoryData.objects.Contains(_objectType))
        {
            _inventoryData.objects.Add(_objectType);
        }
        // Increase the "amount" value in the Scriptable Object by the specified amount
        int addAmount = Random.Range(minAmount, maxAmount);
        _objectType.amount += addAmount;
        print("Added " + addAmount + " " + _objectType.displayName);

        // If it has a SavableObject script, mark it as deleted
        if (gameObject.TryGetComponent(out SavableObject _saveableObject))
        {
            _saveableObject.RegisterObject();
        }
        
        // Delete the object
        Destroy(this.gameObject);
    }
}
