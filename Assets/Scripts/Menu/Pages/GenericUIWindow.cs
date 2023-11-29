using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUIWindow : MonoBehaviour
{
    // This is a class for all UI windows and popups.
    // We can use this for things like setting a default selected button, when the window opens
    
    [SerializeField] public MenuStateEnum associatedEnumState;
    [SerializeField] private GameObject firstSelectedButton;

    
    private void OnEnable()
    {
        // Select first page button
        if (firstSelectedButton != null)
        {
            if (ButtonManager.menuEventSystem != null)
            {
                ButtonManager.menuEventSystem.SetSelectedGameObject(firstSelectedButton);
            }
            else
            {
                print("Failed to SetSelectedGameObject ");
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no firstSelectedButton! Menu navigation might not work properly when using gamepad or keyboard");
        }

        OnEnableChild();
    }

    protected virtual void OnEnableChild()
    {
        
    }
}
