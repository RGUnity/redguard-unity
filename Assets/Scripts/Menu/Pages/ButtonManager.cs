using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    public static EventSystem menuEventSystem;
    private GameObject lastSelectedButton;


    private void Awake()
    {
        menuEventSystem = _eventSystem;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (menuEventSystem.currentSelectedGameObject != null)
        {
            lastSelectedButton = menuEventSystem.currentSelectedGameObject;
        }
        else
        {
            menuEventSystem.SetSelectedGameObject(lastSelectedButton);
        }
    }
}
