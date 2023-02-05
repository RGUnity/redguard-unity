using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{

    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private bool useDefaultInventory;

    // Start is called before the first frame update
    void Start()
    {
        // restore the default inventory if necessary
        if (useDefaultInventory || _inventoryData.objects.Count == 0)
        {
            RestoreInventoryDefaults();
        }
    }

    // This load the inventory based in the default values in the inventory objects
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
