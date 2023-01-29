using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Interactable
{
    [SerializeField] string sceneToLoad;

    public override void Interact()
    {
        print("This door should load a scene");
        SceneManager.LoadScene("Some New Scene");
    }
}
