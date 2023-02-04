using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryObjectType", menuName = "ScriptableObjects/InventoryObjectType")]
public class InventoryObjectType : ScriptableObject
{
    [Header("Required Data")]
    public String displayName;

    [Header("Optional Data")]
    public int startAmount = 0;
    public Sprite icon;
    
    [Header("Dynamic Variables")]
    public int amount = 0;
}
