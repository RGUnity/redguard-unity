using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    public Inventory inventory;
    public int startGoldAmount = 0;
    public List<InventoryObject> startObjects;
    public List<InventoryObject> unusedObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        SetStartValues();
        //print(inventory.Objects[1].amount);
    }
    
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
