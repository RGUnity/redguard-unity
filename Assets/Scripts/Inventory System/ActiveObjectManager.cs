using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectManager : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private InventoryUIObject _activeObjectIndicator;

    private void Start()
    {
        // This is necessary for the indicator to refresh on the first frame
        // I just hope it will never be executed AFTER LoadSavedInventory.cs refreshes the inventory...
        UpdateActiveObjectIndicator();
    }

    private void OnEnable()
    {
        UpdateActiveObjectIndicator();
    }
    

    public void UpdateActiveObjectIndicator()
    {
        //print("_inventoryData.activeObject is " + _inventoryData.activeObject);
        // If no activeObject object is set, pick the first from the inventory
        if (_inventoryData.activeObject == null)
        {
            _inventoryData.activeObject = _inventoryData.objects[0];
            print("activeObject set to " + _inventoryData.activeObject);
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