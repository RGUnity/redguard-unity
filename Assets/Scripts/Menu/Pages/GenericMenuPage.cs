using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMenuPage : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;
    
    private void OnEnable()
    {
        // Select first page button
        if (firstSelectedButton != null)
        {
            ButtonManager.menuEventSystem.SetSelectedGameObject(firstSelectedButton);
        }
        else
        {
            Debug.LogWarning(gameObject.name + " has no firstSelectedButton! Menu navigation might not work properly when using gamepad or keyboard");
        }
        
    }
}
