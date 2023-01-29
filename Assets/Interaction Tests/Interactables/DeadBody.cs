using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : Interactable
{
    [SerializeField] bool isEmpty = false;
    [SerializeField] int goldAmount;
    [SerializeField] InventoryObject gold;
    
    public override void Interact()
    {
        if (!isEmpty)
        {
            gold.amount += goldAmount;
            isEmpty = true;
        }

    }
}
