using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePageDeletePopup : GenericUIWindow
{
    [SerializeField] private PageManager _pageManager;
    [SerializeField] public string savefileToDelete;
    [SerializeField] public TMP_Text nameDisplay;
    [SerializeField] public GameObject newSaveGameButton;
    private void OnEnable()
    {
        nameDisplay.text = savefileToDelete.Replace(".json", "");
    }

    public void DeleteSavefile(string name)
    {
        // Delete the savefile
        BayatGames.SaveGameFree.SaveGame.Delete("Save/" + savefileToDelete);
        
        // Close the popup and reset parameters
        gameObject.SetActive(false);
        nameDisplay.SetText("");
        // ButtonManager.menuEventSystem.SetSelectedGameObject(newSaveGameButton);
        _pageManager.savePage.GetComponent<SavePage>().GenerateSaveFileList();
    }
    
    public void CancelDelete()
    {
        // Close the popup and reset parameters
        gameObject.SetActive(false);
        nameDisplay.SetText("");
        // ButtonManager.menuEventSystem.SetSelectedGameObject(newSaveGameButton);
        _pageManager.savePage.GetComponent<SavePage>().GenerateSaveFileList();
    }
    
}
