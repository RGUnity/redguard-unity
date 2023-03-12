using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObjectInfo", menuName = "ScriptableObjects/InventoryObjectInfo")]
public class InventoryObjectInfo : ScriptableObject
{
    public String id;

    public GameObject worldObject;

    public Sprite icon;

    public int startAmount;
}
