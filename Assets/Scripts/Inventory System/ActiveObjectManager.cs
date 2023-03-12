using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveObjectManager : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private InventoryUIObject _activeObjectIndicator;
    [SerializeField] private InventoryObjectRegister objectRegister;

    private void Start()
    {
        // This is necessary for the indicator to refresh on the first frame
        UpdateActiveObjectIndicator();
    }

    private void OnEnable()
    {
        // Not recommended, because GameData is loaded on Awake, and Defaults are restored at Start
        if (Game.Data.Player.Inventory.activeObject != null)
        {
            UpdateActiveObjectIndicator();
        }
        
    }
    

    public void UpdateActiveObjectIndicator()
    {
        var inventoryObjects = Game.Data.Player.Inventory.objects;
        var activeObject = Game.Data.Player.Inventory.activeObject;
        // Some Safety measures in case something is wrong with the inventory

        if (Game.Data.Player.Inventory.activeObject == null)
        {
            print("activeObject is null");
        }

        if (Game.Data.Player.Inventory.objects == null)
        {
            print("inventoryObjects is null");
        }
        
        // If the active objects is not part of the inventory, select [0] and continue
        if (!inventoryObjects.ContainsKey(activeObject) && inventoryObjects.Any())
        {
            Game.Data.Player.Inventory.activeObject = inventoryObjects.First().Key;
            print("activeObject is no longer in Inventory. New activeObject: " + inventoryObjects.First().Value);
        }
        
        // If the active objects is null, select [0] and continue
        if (activeObject == null && inventoryObjects.Any())
        {
            Game.Data.Player.Inventory.activeObject = inventoryObjects.First().Key;
            print("activeObject was NULL. New activeObject: " + activeObject);
        }
        
        
        //Set Type
        _activeObjectIndicator.id = activeObject;
        
        // Set Text and sprite
        _activeObjectIndicator.labelComponent.text = activeObject;
        foreach (var objInfo in objectRegister.objects)
        {
            if (objInfo.id == activeObject)
            {
                _activeObjectIndicator.imageComponent.sprite = objInfo.icon;
            }
        }
        

        // The amount indicator can be hidden if it is zero, so just input an empty string.
        if (Game.Data.Player.Inventory.objects[activeObject] > 1)
        {
            _activeObjectIndicator.amountComponent.text = activeObject;
        }
        else
        {
            _activeObjectIndicator.amountComponent.text = "";
        }
    }
}