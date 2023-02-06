using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class LoadSavedInventory : MonoBehaviour
{
    // This primarily loads the inventory data.
    // Objects can independently add themselves to the inventory list
    // The list is then saved by GameSaveManager.cs
    
    [SerializeField] private InventoryData _inventoryData;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("EnterThroughDoor"))
        {
            LoadInventory();
        }
    }
    

    private void LoadInventory()
    {
        // This threw an error in a Build one (when entering a new scene) so i'm gonna null check this
        if (_inventoryData != null)
        {
            _inventoryData.objects.Clear();
        }
        

        // Load inventory objects from savefile
        List<InventoryObjectType> loadedObjects = new List<InventoryObjectType>();
        foreach (InventoryObjectType objType in _inventoryData.allowedObjects)
        {
            if (DataSerializer.TryLoad(objType.name, out int loadedAmount))
            {
                //print("Savefile contains " + objType.displayName);
                objType.amount = loadedAmount;
                //print("Loaded: " + objType.amount + " " + objType.name);
                
                loadedObjects.Add(objType);
            }

            else
            {
                objType.amount = 0;
            }
        }
        
        // If the temp list is not empty, match the lists and load the active object
        if (loadedObjects.Count > 0)
        {
            _inventoryData.objects = loadedObjects;
            print("Inventory loaded successfully");
            
            var loadedActiveObject = DataSerializer.Load<InventoryObjectType>("InventoryActiveObject");
            if (_inventoryData.objects.Contains(loadedActiveObject))
            {
                _inventoryData.activeObject = loadedActiveObject;
                print("Loaded active object: " + _inventoryData.activeObject);
            }
            else
            {
                // Make [0] the new active object
                _inventoryData.activeObject = _inventoryData.objects[0];
                print("New active object: " + _inventoryData.activeObject);
            }
        }
        else
        {
            print("No Inventory objects could be loaded. Restoring defaults!");
            RestoreInventoryDefaults();
            // Make [0] the new active object
            _inventoryData.activeObject = _inventoryData.objects[0];
            print("New active object: " + _inventoryData.activeObject);
        }
    }
    
    private void RestoreInventoryDefaults()
    {
        // Clear the inventory
        _inventoryData.objects.Clear();
        
        // For every known object, amount = startAmount
        // Objects that are not empty will be added to the inventory
        foreach (var obj in _inventoryData.allowedObjects)
        {
            if (obj.startAmount > 0)
            {
                _inventoryData.objects.Add(obj);
                obj.amount = obj.startAmount;
            }
            // Reset the amount of excluded objects to zero, just to be sure
            else
            {
                obj.amount = 0;
            }
        }
    }
}
