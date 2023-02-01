using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUIObject _activeObjectIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public void UpdateActiveObjectIndicator()
    {
        // If no activeObject object is set, pick the first from the inventory
        if (inventory.activeObject == null)
        {
            inventory.activeObject = inventory.objects[0];
        }
        
        // Set Text and sprite
        _activeObjectIndicator.label.text = inventory.activeObject.displayName;
        _activeObjectIndicator.image.sprite = inventory.activeObject.icon;

        // The amount indicator can be hidden if it is zero, so just input an empty string.
        if (inventory.activeObject.amount > 1)
        {
            _activeObjectIndicator.amount.text = inventory.activeObject.amount.ToString();
        }
        else
        {
            {
                _activeObjectIndicator.amount.text = "";
            }
        }
    }
}
