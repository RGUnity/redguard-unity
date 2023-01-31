using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "ScriptableObjects/Inventory")]
public class Inventory : ScriptableObject
{
    // The real Player inventory
    public List<InventoryObject> objects;
    // Only objects in this list can be added to the player inventory
    public List<InventoryObject> allowedObjects;

}
