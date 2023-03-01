using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableObjectData
{
    public bool isDeleted;
    public InventoryObjectType objectType;
    public Vector3 position;
    public Quaternion rotation;
    public int minAmount;
    public int maxAmount;
    public string id;
}
