using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavePageDeletePopup : GenericUIPopup
{
    [SerializeField] private PageManager pageManager;
    [SerializeField] public TMP_Text nameDisplay;
    
    // This is triggered by OnEnable() in the the parent class, "GenericMenuPage"
    protected override void OnEnableChild()
    {
        nameDisplay.text = infoString.Replace(".json", "");
    }

    public void DeleteSavefile()
    {
        // Delete the savefile
        // Hint: infoString is a property of GenericUIPopup and is given by MenuInputManager to PageManager...
        // ... and then set by PageManager during OpenPopup
        BayatGames.SaveGameFree.SaveGame.Delete("Save/" + infoString);
        
        // Reset displayed parameters
        nameDisplay.SetText("");
        
        // Close popup
        pageManager.ClosePopup(this.gameObject);
    }
    
    public void CancelDelete()
    {
        // Reset displayed parameters
        nameDisplay.SetText("");
        
        // Close popup
        pageManager.ClosePopup(this.gameObject);
        
    }
    
    private void OnDisable()
    {
        // When closing the popup, regenerate the underlying savefile list
        pageManager.savePage.GetComponent<SavePage>().GenerateSaveFileList();
    }
}
