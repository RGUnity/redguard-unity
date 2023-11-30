using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetNamePopup : GenericUIPopup
{
    [SerializeField] private PageManager pageManager;
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] private GameObject fileAlreadyExistsError;
    [SerializeField] private GameObject invalidNameError;

    // This is triggered by OnEnable() in the the parent class, "GenericMenuPage"
    protected override void OnEnableChild()
    {
        inputField.text = "";
        fileAlreadyExistsError.SetActive(false);
        invalidNameError.SetActive(false);
    }
    
    public void CreateNewSaveFile()
    {
        
        // Grab the entered name from the inputField
        string saveFileName = inputField.text;

        if (saveFileName == "")
        {
            invalidNameError.SetActive(true);
        }
        else
        {
            invalidNameError.SetActive(false);
            
            // First, check if a savefile with that the entered name already exists
            if (BayatGames.SaveGameFree.SaveGame.Exists("Save/" + saveFileName + ".json"))
            {
                // If yes, show an error message
                print("Stop, file already exists: " + saveFileName);
                fileAlreadyExistsError.SetActive(true);
                // Set the input field as selected game object. 
                ButtonManager.menuEventSystem.SetSelectedGameObject(inputField.gameObject);
            }
            else
            {
                // Else, proceed to create the savefile
                print("Attempting to create savefile with name: " + saveFileName);
            
                // Hide the "name already exists" error
                fileAlreadyExistsError.SetActive(false);
            
                // Create new savefile with this name
                GameSaver.SaveGame(saveFileName);
            
                // Close popup
                pageManager.ClosePopup(this.gameObject);
            }
        }
        
    }
    private void OnDisable()
    {
        // When closing the popup, regenerate the underlying savefile list
        pageManager.savePage.GetComponent<SavePage>().GenerateSaveFileList();
    }
}
