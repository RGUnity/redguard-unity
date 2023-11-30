using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugShowMenuState : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textComponent.text = Game.Menu.State.ToString();
    }
}
