using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SavePage : GenericUIWindow
{

    [SerializeField] private GameObject saveSlotParent;
    [SerializeField] private GameObject setNamePopup;
    [SerializeField] private GameObject fileAlreadyExistsError;
    [SerializeField] private GameObject saveSlotPrefab;
    [SerializeField] private GameObject newSaveFileButton;
    [SerializeField] private GameObject deletePopup;
    
    private GameObject _selectedButton;
    
    
    // OnEnable() is used by the parent object, "GenericMenuPage"


    protected override void OnEnableChild()
    {
        GenerateSaveFileList();
    }


    public void GenerateSaveFileList()
    {
        DeleteAllButtons();
        
        var foundFiles = BayatGames.SaveGameFree.SaveGame.GetFiles("Save/");
        
        foundFiles = foundFiles.OrderByDescending(p => p.LastWriteTimeUtc).ToArray();
        //print(foundFiles[0]);

        foreach (var file in foundFiles)
        {
            // Spawn the button prefab
            GameObject button = GameObject.Instantiate(saveSlotPrefab, saveSlotParent.transform);
            button.name = file.Name;
            
            // Set the name variable
            button.GetComponent<SaveFileListItem>().saveFileName = file.Name;
            
            // Set the displayed text
            string displayName = file.Name.Replace(".json", "");
            button.GetComponentInChildren<TMP_Text>().SetText(displayName);
        }
        
        // Lastly, set the first child object as the currently selected button
        ButtonManager.menuEventSystem.SetSelectedGameObject(saveSlotParent.transform.GetChild(0).gameObject);
    }

    private void DeleteAllButtons()
    {
        foreach (Transform child in saveSlotParent.transform) 
        {
            if (child.transform.GetSiblingIndex() == 0)
            {
                // skip the first one, because it is the "Create new savefile" button
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    private void Update()
    {
        FocusSelectedButton();
    }

    // The list of save slots is technically a grid layout group.
    // When then simply move the array left/right in 500 increments...
    // ... depending on which page the currently selected element is on
    public void FocusSelectedButton()
    {
        // Start by getting the currently selected button
        _selectedButton = ButtonManager.menuEventSystem.currentSelectedGameObject;
        
        // Get the sibling index number of the object
        int objectIndex = _selectedButton.transform.GetSiblingIndex();
        
        // Calculate on which page the object is by dividing the index by 9
        int objectPage = objectIndex / 9;
        
        // Move the slot view vertically one step per page
        Vector2 newPosition = new Vector2(500 * objectPage * -1, 0);
        
        saveSlotParent.GetComponent<RectTransform>().anchoredPosition =  newPosition;

    }

    public void OpenSetNamePopup()
    {
        // Hint: The text is cleared on the popup itself ("SetNamePopup.cs") 
        
        // Then reveal the popup
        setNamePopup.SetActive(true);
    }
    
    public void CreateNewSaveFile()
    {
        
        // Grab the entered name from the inputField
        string saveFileName = setNamePopup.GetComponent<SetNamePopup>().inputField.text;
        
        // First, check if a savefile with that the entered name already exists
        if (BayatGames.SaveGameFree.SaveGame.Exists("Save/" + saveFileName + ".json"))
        {
            // If yes, show an error message
            print("Stop, file already exists: " + saveFileName);
            fileAlreadyExistsError.SetActive(true);
            // Set the input field as selected game object. 
            ButtonManager.menuEventSystem.SetSelectedGameObject(setNamePopup.GetComponent<SetNamePopup>().inputField.gameObject);
        }
        else
        {
            // Else, proceed to create the savefile
            print("Attempting to create savefile with name: " + saveFileName);
            
            // Hide the "name already exists" error
            fileAlreadyExistsError.SetActive(false);
            
            // Close popup
            setNamePopup.SetActive(false);
            
            // Create new savefile with this name
            GameSaver.SaveGame(saveFileName);

            // Update the savefile UI list
            GenerateSaveFileList();
            
            // Select a button
            ButtonManager.menuEventSystem.SetSelectedGameObject(newSaveFileButton);
        }
    }

    public void OpenDeletePopup(string fileToDelete)
    {
        deletePopup.GetComponent<SavePageDeletePopup>().savefileToDelete = fileToDelete;
        deletePopup.SetActive(true);
    }
}
