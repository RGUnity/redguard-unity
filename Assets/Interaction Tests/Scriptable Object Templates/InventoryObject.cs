using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryObject", menuName = "ScriptableObjects/InventoryObject")]
public class InventoryObject : ScriptableObject
{
    public String displayName;
    public int amount = 1;
}
