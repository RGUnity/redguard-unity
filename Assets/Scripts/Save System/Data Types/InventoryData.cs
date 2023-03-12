using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    //public InventoryObject activeObject = new();
    //public Dictionary<string, InventoryObject> objects = new();

    // Or just a direct dictionary?
    public string activeObject;
    public Dictionary<string, int> objects = new();
}
