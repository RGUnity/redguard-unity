using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavablePlayerData
{
    public Vector3 PlayerPosition;
    public Quaternion PlayerRotation;
    public InventoryObjectType InventoryActiveObject;
    public List<InventoryObjectType> InventoryObjects = new List<InventoryObjectType>();
}
