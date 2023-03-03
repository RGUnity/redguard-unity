using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryObjectType", menuName = "ScriptableObjects/InventoryObjectType")]
public class InventoryObjectType : ScriptableObject
{
    public String displayName;

    public GameObject worldObject;

    public Sprite icon;
}
