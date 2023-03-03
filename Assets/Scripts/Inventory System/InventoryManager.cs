using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryDefaults defaultInventory;


    // Start is called before the first frame update
    void Start()
    {
        if (defaultInventory == null)
        {
            Debug.Log("InventoryManager has no Default Inventory assigned! Please create a scriptable object and assign it!");
        }
        
        if (!Game.Data.Player.Inventory.objects.Any())
        {
            print("Inventory is empty. Restoring Default Inventory.");
            LoadDefaultInventory();
        }
        else
        {
            print("Inventory contains at least one item, like "+ Game.Data.Player.Inventory.objects.First());
        }
    }

    public void LoadDefaultInventory()
    {
        Game.Data.Player.Inventory.objects.Clear();
        
        // Loop through the list of default objectTypes, and create new Objects for all
        int index = 0;
        foreach (var type in defaultInventory.objectTypes)
        {
            var newObject = new InventoryObject
            {
                type = defaultInventory.objectTypes[index],
                amount = defaultInventory.objectAmounts[index]
                    
            };
            // Add it to the list
            Game.Data.Player.Inventory.objects.Add(type.displayName, newObject);
            index++;
        }
        // use the first object as the activeObjecte
        Game.Data.Player.Inventory.activeObject = Game.Data.Player.Inventory.objects.First().Value;
    }

    public static void AddItems(InventoryObjectType type, int amount)
    {
        if (Game.Data.Player.Inventory.objects.ContainsKey(type.displayName))
        {
            // increase object amount
            Game.Data.Player.Inventory.objects[type.displayName].amount += amount;
        }

        else
        {
            // Create new InventoryObject
            var newObject = new InventoryObject
            {
                type = type,
                amount = amount
                    
            };
            
            Game.Data.Player.Inventory.objects.Add(type.displayName, newObject);
        }
    }
}
