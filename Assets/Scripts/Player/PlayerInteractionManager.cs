using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public Interactable selectedObject;

    void Update()
    {
        // When "Use" is pressed, trigger "Interact()" on the selectedObject
        if (LocalScene.inputManager.use
            && selectedObject != null)
        {
            selectedObject.Interact();
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
