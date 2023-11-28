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
    
    private GameObject _selectedButton;
    
    
    // OnEnable() is used by the parent object, "GenericMenuPage"


    protected override void OnEnableChild()
    {
        GenerateSaveFileList();
    }

    private void OnDisable()
    {
        // On exit, clear all unneeded children
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


    private void GenerateSaveFileList()
    {
        var foundFiles = BayatGames.SaveGameFree.SaveGame.GetFiles("Save/");
        
        foundFiles = foundFiles.OrderByDescending(p => p.LastWriteTimeUtc).ToArray();
        //print(foundFiles[0]);

        foreach (var file in foundFiles)
        {
            GameObject button = GameObject.Instantiate(saveSlotPrefab, saveSlotParent.transform);
            print(file.Name);
            string displayName = file.Name.Replace(".json", "");
            button.GetComponentInChildren<TMP_Text>().SetText(displayName);
        }
    }

    private void Update()
    {
        UpdateSlotView();
    }

    // The list of save slots is technically a grid layout group.
    // When then simply move the array left/right in 500 increments...
    // ... depending on which page the currently selected element is on
    public void UpdateSlotView()
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
        }
    }


    // TODO: DELETE THIS ----------
    public void SaveSlot1()
    {
        GameSaver.SaveGameBySlot(1);
    }
    public void SaveSlot2()
    {
        GameSaver.SaveGameBySlot(2);
    }
    public void SaveSlot3()
    {
        GameSaver.SaveGameBySlot(3);
    }
}
