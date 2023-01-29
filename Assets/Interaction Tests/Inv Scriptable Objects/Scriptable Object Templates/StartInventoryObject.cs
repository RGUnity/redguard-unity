using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStartInventoryObject", menuName = "ScriptableObjects/StartInventoryObject")]
public class StartInventoryObject : ScriptableObject
{
    public InventoryObject stackType;
    //public String displayName;
    public int amount = 1;
}
