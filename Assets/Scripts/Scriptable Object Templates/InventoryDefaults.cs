using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDefaults", menuName = "ScriptableObjects/InventoryDefaults")]
public class InventoryDefaults : ScriptableObject
{
    public GenericDictionary<InventoryObjectInfo, int> objects = new();
}
