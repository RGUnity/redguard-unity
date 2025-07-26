using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObjectRegister", menuName = "ScriptableObjects/InventoryObjectRegister")]
public class InventoryObjectRegister : ScriptableObject
{
    public List<InventoryObjectInfo> objects = new();
}
