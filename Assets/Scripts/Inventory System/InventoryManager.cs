using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryDefaults defaultInventory;
    [SerializeField] private InventoryObjectRegister objectRegister;

    public void LoadDefaultInventory()
    {
        Game.Data.Player.Inventory.objects.Clear();

        // Loop through the list of all objects and create new Objects for all

        foreach (var objInfo in defaultInventory.objects)
        {
            var newObject = new InventoryObject
            {
                id = objInfo.Key.id,
                amount = objInfo.Value
                    
            };
            Game.Data.Player.Inventory.objects.Add(objInfo.Key.id, objInfo.Value);
        }
        
        Game.Data.Player.Inventory.activeObject = Game.Data.Player.Inventory.objects.First().Key;
        print("Loaded default inventory");
    }

    public static void AddItems(string id, int amount)
    {
        if (Game.Data.Player.Inventory.objects.ContainsKey(id))
        {
            // increase object amount
            Game.Data.Player.Inventory.objects[id] += amount;
        }

        else
        {
            // Create new InventoryObject
            Game.Data.Player.Inventory.objects.Add(id, amount);
        }
        print("Added " + amount + " " + id + " to Player Inventory" );
    }
}
