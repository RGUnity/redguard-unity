using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    public GameObject selectedObject;
    public Interactable inter;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(selectedObject!=null)
            {
                selectedObject.GetComponent<Interactable>().Interact();
                inter = selectedObject.GetComponent<Interactable>();
            }
            // if (selectedObject != null && selectedObject.CompareTag("NPC"))
            // {
            //     selectedObject.GetComponent<NPC>().Interact();
            // }
            // if (selectedObject != null && selectedObject.CompareTag("Item"))
            // {
            //     selectedObject.GetComponent<Item>().Interact();
            // }
            // if (selectedObject != null && selectedObject.CompareTag("Container"))
            // {
            //     selectedObject.GetComponent<Container>().Interact();
            //     //Inventory.addGold(5);
            // }
            // if (selectedObject != null && selectedObject.CompareTag("Door"))
            // {
            //     selectedObject.GetComponent<Door>().Interact();
            // }
            // if (selectedObject != null && selectedObject.CompareTag("Switch"))
            // {
            //     selectedObject.GetComponent<Switch>().Interact();
            // }
            //if (selectedObject != null && selectedObject.CompareTag("NPC"))
            //    print("Its an NPC");
        }
    }

    private void OnTriggerEnter(Collider detectedCollider)
    {
        //throw new NotImplementedException();
        //print("Entered Trigger: " + detectedCollider.gameObject.name);
        selectedObject = detectedCollider.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        selectedObject = null;
    }
}
