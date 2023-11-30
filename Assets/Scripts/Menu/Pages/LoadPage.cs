using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LoadPage : GenericUIWindow
{
    [SerializeField] private GameObject saveSlotParent;
    [SerializeField] private GameObject loadSlotPrefab;
    
    private GameObject _selectedButton;
    
    
    // This is triggered by OnEnable() in the the parent class, "GenericMenuPage"
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
            GameObject button = GameObject.Instantiate(loadSlotPrefab, saveSlotParent.transform);
            button.name = file.Name;
            
            // Set the name variable
            button.GetComponent<SaveFileListItem>().saveFileName = file.Name;
            
            // Set the displayed text
            string displayName = file.Name.Replace(".json", "");
            button.GetComponentInChildren<TMP_Text>().SetText(displayName);
        }
        
        // Lastly, set the first child object as the currently selected button
        ButtonManager.menuEventSystem.SetSelectedGameObject(saveSlotParent.transform.GetChild(0).gameObject);
        print(ButtonManager.menuEventSystem.currentSelectedGameObject.name);
    }

    private void DeleteAllButtons()
    {
        foreach (Transform child in saveSlotParent.transform) 
        {
                Destroy(child.gameObject);
        }
    }
    
    private void Update()
    {
        FocusSelectedButton();
        //print(ButtonManager.menuEventSystem.currentSelectedGameObject.name);

        
    }

    // The list of save slots is technically a grid layout group.
    // When then simply move the array left/right in 500 increments...
    // ... depending on which page the currently selected element is on
    public void FocusSelectedButton()
    {
        // Sometimes the EventSystem loses its currentSelectedGameObject...
        // ... so if that happens, we simply select the first one again
        if (ButtonManager.menuEventSystem.currentSelectedGameObject == null)
        {
            ButtonManager.menuEventSystem.SetSelectedGameObject(saveSlotParent.transform.GetChild(0).gameObject);
        }
        
        else
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
    }
}
