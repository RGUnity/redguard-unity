using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalSceneSetter : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private Light sun;
    [SerializeField] private Canvas canvas;
    
    // Set the variables in LocalScene according to their exposed variables
    void Awake()
    {
        LocalScene.eventSystem = eventSystem;
        LocalScene.inputManager = inputManager;
        LocalScene.audioListener = audioListener;
        LocalScene.sun = sun;
        LocalScene.canvas = canvas;
    }
    
    // On exit, clear all the variables from memory
    private void OnDestroy() 
    {
        LocalScene.eventSystem = null;
        LocalScene.inputManager = null;
        LocalScene.audioListener = null;
        LocalScene.sun = null;
        LocalScene.canvas = null;
    }
}
