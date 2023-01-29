using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : Interactable
{
    // Need to link these scriptable objects
    [SerializeField] Inventory inventory;
    [SerializeField] InventoryObject _object;

    // The ranges for the randomized amounts that this item can be dropped in
    [SerializeField] int minAmount = 3;
    [SerializeField] int maxAmount = 15;

    public override void Interact()
    {
        // If the Object type is not yet part of the Inventory List, add it
        if (!inventory.objects.Contains(_object))
        {
            inventory.objects.Add(_object);
        }
        // Increase the "amount" value in the Scriptable Object by the specified amount
        int addAmount = Random.Range(minAmount, maxAmount);
        _object.amount += addAmount;
        print("Added " + addAmount + " " + _object.displayName);
        // Delete the object
        Destroy(this.gameObject);
    }
}
