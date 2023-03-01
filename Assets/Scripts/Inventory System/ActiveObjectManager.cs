using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // If the active objects is not part of the inventory, select [0] and continue
        print(_inventoryData.objects.Any());
        if (!_inventoryData.objects.Contains(_inventoryData.activeObject) && _inventoryData.objects.Any())
        {
            print(_inventoryData);
            _inventoryData.activeObject = _inventoryData.objects[0];
            print("activeObject is no longer in Inventory. New activeObject: " + _inventoryData.objects[0]);
        }
        
        // If the active objects is null, select [0] and continue
        if (_inventoryData.activeObject == null && _inventoryData.objects.Any())
        {
            _inventoryData.activeObject = _inventoryData.objects[0];
            print("activeObject was NULL. New activeObject: " + _inventoryData.activeObject);
        }
        
        //Set Type
        _activeObjectIndicator.stackType = _inventoryData.activeObject;
        
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
            _activeObjectIndicator.amount.text = "";
        }
    }
}