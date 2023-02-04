using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLootObject", menuName = "ScriptableObjects/LootObject")]
public class LootObject : ScriptableObject
{
    public InventoryObjectType inventoryObjectType;
    public String displayName;
    public int amount = 1;
}
