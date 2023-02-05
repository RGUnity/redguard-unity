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

        // Load active object
        if (DataSerializer.TryLoad("InventoryActiveObject", out InventoryObjectType actObj))
        {
            _inventoryData.activeObject = actObj;
            print("Loaded active object: " + actObj);
        }
        else
        {
            print("No active object could be loaded");
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
        
        // If the temp list is not empty, match the lists
        if (loadedObjects.Count > 0)
        {
            _inventoryData.objects = loadedObjects;
        }
        else
        {
            RestoreInventoryDefaults();
            print("Loaded Inventory was empty. Restoring defaults!");
        }
    }
    
    private void RestoreInventoryDefaults()
    {
        print("Restoring inventory Defaults");
        _inventoryData.objects.Clear();
        foreach (var obj in _inventoryData.allowedObjects)
        {
            if (obj.startAmount > 0)
            {
                _inventoryData.objects.Add(obj);
                obj.amount = obj.startAmount;
            }
            else
            {
                obj.amount = 0;
            }
        }
    }
}
