using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //public int testValue;
    public virtual void Interact()
    {
        print("This is the parent class for Interactables");
    }
}