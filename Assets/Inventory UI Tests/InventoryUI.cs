using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private GameObject UIObjectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogWarning("Dude [InventoryUIObject] HUD is running on >>Update<<");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Clear the Grid Layout
        if (gridLayout.transform.childCount > 0)
        {
            for (int i = 0; i < gridLayout.transform.childCount; i++)
            {
                Transform childTransform = gridLayout.transform.GetChild(i);
                Destroy(childTransform.gameObject);
            }
        }
        
        // Loop through the list of objects in the Inventory
        foreach (InventoryObject obj in _inventory.objects)
        {
            // Instantiate a Grid element from the prefab
            GameObject newUIObject = GameObject.Instantiate(UIObjectPrefab, gridLayout.transform);
            // And rename it just because it looks nicer
            newUIObject.name = obj.displayName;
            
            // Get the component which has links to the text and image components
            InventoryUIObject invObj = newUIObject.GetComponent<InventoryUIObject>();
            
            // Match the Grid element's displaying properties to the Scriptable Object
            invObj.label.text = obj.displayName;
            invObj.amount.text = obj.amount.ToString();
            invObj.image.sprite = obj.icon;
        }
    }
}
