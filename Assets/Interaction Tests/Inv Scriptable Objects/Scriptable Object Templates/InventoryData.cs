using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryData", menuName = "ScriptableObjects/InventoryData")]
public class InventoryData : ScriptableObject
{
    public InventoryObjectType activeObject;

    public string testString;
    // The real Player inventory
    public List<InventoryObjectType> objects;
    
    // Only objects in this list can be added to the player inventory
    public List<InventoryObjectType> allowedObjects;

}
