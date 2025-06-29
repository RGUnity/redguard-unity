using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryObjectRegister objectRegister;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private GameObject UIObjectPrefab;

    private GameObject _selectedButton;
    
    // Update is called once per frame
    void Update()
    {
        // previously i just checked if (Input.anyKeyDown) here but i think this is better
        if (_selectedButton != EventSystem.current.currentSelectedGameObject)
        {
            // This ensures there is always a selected UI button, so that directional button navigation works
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                _selectedButton = EventSystem.current.currentSelectedGameObject;
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(_selectedButton);
            }

            // Update the ScriptableObject with the latest _selectedButton information
            
            Game.Data.Player.Inventory.activeObject = _selectedButton.GetComponent<InventoryUIObject>().id;
        }
        

    }

    private void Start()
    {
        //UpdateInventoryUIObjects();
    }

    private void OnEnable()
    {
        UpdateInventoryUIObjects();
    }

    private void UpdateInventoryUIObjects()
    {
        // If the Grid Layout is not empty, clear it by deleting all children
        if (gridLayout.transform.childCount > 0)
        {
            for (int i = 0; i < gridLayout.transform.childCount; i++)
            {
                Transform childTransform = gridLayout.transform.GetChild(i);
                Destroy(childTransform.gameObject);
            }
        }
        
        // Loop through the list of objects in the Inventory

        foreach (var entry in Game.Data.Player.Inventory.objects)
        {
            string id = entry.Key;
            int amount = entry.Value;
            
            
            // Instantiate a Grid element from the prefab
            GameObject newUIObject = GameObject.Instantiate(UIObjectPrefab, gridLayout.transform);
            // And rename it just because it looks nicer
            newUIObject.name = id;
            
            // Get the component which has links to the text and image components
            InventoryUIObject invObj = newUIObject.GetComponent<InventoryUIObject>();
            
            // Match the Grid element's displaying properties to the Scriptable Object
            invObj.labelComponent.text = id;
            invObj.amountComponent.text = amount.ToString();

 
            
            foreach (var objInfo in objectRegister.objects)
            {
                if (objInfo.id == id)
                {
                    invObj.imageComponent.sprite = objInfo.icon;
                }
            }
            

            // If we are dealing with the activeObject, mark it as selected
            if (id == Game.Data.Player.Inventory.activeObject)
            {
                _selectedButton = newUIObject;
            }
        }
        
        // Tell the EventSystem about our selected button
        EventSystem.current.SetSelectedGameObject(_selectedButton);
    }
}
