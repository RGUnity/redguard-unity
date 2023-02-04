using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryData;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private GameObject UIObjectPrefab;

    private GameObject _selectedButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
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
            _inventoryData.activeObject = _selectedButton.GetComponent<InventoryUIObject>().stackType;
        }
        

    }

    private void OnEnable()
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
        foreach (InventoryObjectType obj in _inventoryData.objects)
        {
            // Instantiate a Grid element from the prefab
            GameObject newUIObject = GameObject.Instantiate(UIObjectPrefab, gridLayout.transform);
            // And rename it just because it looks nicer
            newUIObject.name = obj.displayName;
            
            // Get the component which has links to the text and image components
            InventoryUIObject invObj = newUIObject.GetComponent<InventoryUIObject>();
            
            // Match the Grid element's displaying properties to the Scriptable Object
            invObj.stackType = obj;
            invObj.label.text = obj.displayName;
            invObj.amount.text = obj.amount.ToString();
            invObj.image.sprite = obj.icon;

            // If we are dealing with the activeObject, mark it as selected
            if (obj == _inventoryData.activeObject)
            {
                _selectedButton = newUIObject;
            }
        }
        
        // Tell the EventSystem about our selected button
        EventSystem.current.SetSelectedGameObject(_selectedButton);
    }
}
