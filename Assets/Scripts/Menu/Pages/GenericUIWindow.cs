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
                // I guess this is not an actual problem, but the error here still calls on start.
                // print("Failed to SetSelectedGameObject ");
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no firstSelectedButton! All pages (except for LoadPage) need one, for keyboard and gamepads to work");
        }

        OnEnableChild();
    }

    protected virtual void OnEnableChild()
    {
        
    }
}
