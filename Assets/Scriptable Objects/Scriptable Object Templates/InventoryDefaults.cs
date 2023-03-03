using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDefaults", menuName = "ScriptableObjects/InventoryDefaults")]
public class InventoryDefaults : ScriptableObject
{
    // old because we just take the first one
    //public InventoryObject activeObject;
    
    public List<InventoryObjectType> objectTypes = new List<InventoryObjectType>();

    public List<int> objectAmounts = new List<int>();
}
