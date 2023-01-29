using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField] Inventory inventory;
    [SerializeField] InventoryObject _object;
    [SerializeField] int amount;

    public override void Interact()
    {
        // If the Object type is not yet part of the Inventory List, add it
        if (!inventory.objects.Contains(_object))
        {
            inventory.objects.Add(_object);
        }
        // Increase the "amount" value in the Scriptable Object by the specified amount
        _object.amount += amount;
        // Delete the object
        Destroy(this.gameObject);
    }
}
