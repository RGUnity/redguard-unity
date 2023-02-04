using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectManager : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private InventoryUIObject _activeObjectIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        // This resets the active object to the first inventory object
        if (_inventoryData.objects.Count != 0)
        {
            _inventoryData.activeObject = _inventoryData.objects[0];
        }
        else
        {
            print("Inventory is empty. No activeObject found.");
        }

        //UpdateActiveObjectIndicator();
    }

    // Update is called once per frame
    void Update()
    {
        // Calling this in Update for now because otherwise the counter on the activeObject ...
        //... would not update until you open the inventory
        UpdateActiveObjectIndicator();
    }
    

    public void UpdateActiveObjectIndicator()
    {
        // If no activeObject object is set, pick the first from the inventory
        if (_inventoryData.activeObject == null)
        {
            _inventoryData.activeObject = _inventoryData.objects[0];
        }

        // Set Text and sprite
        _activeObjectIndicator.label.text = _inventoryData.activeObject.displayName;
        _activeObjectIndicator.image.sprite = _inventoryData.activeObject.icon;

        // The amount indicator can be hidden if it is zero, so just input an empty string.
        if (_inventoryData.activeObject.amount > 1)
        {
            _activeObjectIndicator.amount.text = _inventoryData.activeObject.amount.ToString();
        }
        else
        {
            {
                _activeObjectIndicator.amount.text = "";
            }
        }
    }
}
