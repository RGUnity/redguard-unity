using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryObject", menuName = "ScriptableObjects/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    [Header("Required Data")]
    public String displayName;

    [Header("Optional Data")]
    public int startAmount = 0;
    public Sprite icon;
    
    [Header("Dynamic Variables")]
    public int amount = 0;
    public int inventorySlotIndex;
}
