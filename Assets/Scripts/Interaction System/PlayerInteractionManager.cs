using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public GameObject selectedObject;

    void Update()
    {
        // When "interact" is pressed, trigger "Interact()" on the selectedObject
        if (Input.GetButtonDown("Fire1"))
        {
            if(selectedObject!=null)
            {
                selectedObject.GetComponent<Interactable>().Interact();
            }
        }
    }

    // Set selectedObject variable
    private void OnTriggerEnter(Collider detectedCollider)
    {
        //print("Entered Trigger: " + detectedCollider.gameObject.name);
        selectedObject = detectedCollider.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        selectedObject = null;
    }
}
