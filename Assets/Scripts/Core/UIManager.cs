using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private DialogueOption optionPrefab;
    [SerializeField] private Transform optionsParent;
    [SerializeField] private TMP_Text interactionTextDisplay;
    private float interactionTextTimer;
    [SerializeField] public UIGraphics uiGraphics;
    
    private List<DialogueOption> options = new();

    public void Start()
    {
        ClearDialogueOption();
        HideInteractionText();
    }
    public void Update()
    {
        if(interactionTextTimer > 0.0f)
        {
            interactionTextTimer -= Time.deltaTime;
        }
        else
            HideInteractionText();
    }

    public void LinkData(LocalUIData data)
    {
        optionPrefab = data.optionPrefab;
        optionsParent =  data.optionsParent;
        interactionTextDisplay = data.interactionTextDisplay;
        uiGraphics = data.uiGraphics;
    }
    
    // GAMEPLAY UI

    public void ShowInteractionText(string text, float time = 0.5f)
    {
        interactionTextDisplay.text = text;
        interactionTextTimer = time;
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

    // UI GRAPHICS
    public void ShowLoadingScreen(Texture t)
    {
        uiGraphics.ShowLoadingScreen(t);
    }
    public void HideLoadingScreen()
    {
        uiGraphics.HideLoadingScreen();
    }

}
