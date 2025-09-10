using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private InputData input;
    public Interactable selectedObject;

    void Update()
    {
        // When "Use" is pressed, trigger "Interact()" on the selectedObject
        if (input.use
            && selectedObject != null)
        {
            selectedObject.Interact();
        }

        if (selectedObject)
        {
            Game.uiManager.ShowInteractionText(selectedObject.interactionText);
        }
        else
        {
            Game.uiManager.HideInteractionText();
        }
    }

    // Set selectedObject variable
    private void OnTriggerEnter(Collider detectedCollider)
    {
        if (detectedCollider.TryGetComponent(out Interactable interactable))
        {
            selectedObject = interactable;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        selectedObject = null;
    }
}
