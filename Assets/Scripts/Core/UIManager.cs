using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private DialogueOption optionPrefab;
    [SerializeField] private Transform optionsParent;
    [SerializeField] private TMP_Text interactionTextDisplay;
    
    private List<DialogueOption> options = new();

    public void Start()
    {
        ClearDialogueOption();
    }

    public void LinkData(LocalUIData data)
    {
        optionPrefab = data.optionPrefab;
        optionsParent =  data.optionsParent;
        interactionTextDisplay = data.interactionTextDisplay;
    }
    
    // GAMEPLAY UI

    public void ShowInteractionText(string text)
    {
        interactionTextDisplay.text = text;
    }

    public void HideInteractionText()
    {
        interactionTextDisplay.text = "";
    }
    
    // DIALOGUE UI
    
    public void ClearDialogueOption()
    {
        // Delete all dialogue options
        if (optionsParent.transform.childCount > 0)
        {
            for (int i = 0; i < optionsParent.transform.childCount; i++)
            {
                Transform childTransform = optionsParent.transform.GetChild(i);
                Destroy(childTransform.gameObject);
            }
        }
        options.Clear();
        
        // Hide the dialogue options panel
        optionsParent.gameObject.SetActive(false);
    }
    
    public void AddDialogueOption(string displayText)
    {
        // Spawn a new dialogue option
        var newOption = Instantiate(optionPrefab, optionsParent).GetComponent<DialogueOption>();
        newOption.uiManager = this;
        newOption.gameObject.name = displayText;
        newOption.SetDisplayText(displayText);
        options.Add(newOption);
        
        // Show the dialogue options panel
        optionsParent.gameObject.SetActive(true);
        
        print("Created DialogueOption " + displayText);
    }
    
    public void PickDialogueOption()
    {
        Debug.LogWarning("Dialogue actions are not yet implemented");
    }
}
