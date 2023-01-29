using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStartInventory", menuName = "ScriptableObjects/StartInventory")]
public class StartInventory : ScriptableObject
{
    public List<StartInventoryObject> objects;

}
