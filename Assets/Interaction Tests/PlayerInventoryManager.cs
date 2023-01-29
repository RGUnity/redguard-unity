using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    public Inventory inventory;
    public int startGoldAmount = 0;
    // All inventory objects must be linked in one of the to lists
    public List<InventoryObject> startObjects;
    public List<InventoryObject> unusedObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        SetStartValues();
    }
    
    // Kind of hacky but ScriptableObjects keep their values when you exit playmode, ...
    // ... So we reset them here to whatever we wat them to be
    void SetStartValues()
    {
        inventory.objects = startObjects;
        inventory.objects[1].amount = startGoldAmount;
        foreach (var obj in unusedObjects)
        {
            obj.amount = 0;
        }
    }
}
