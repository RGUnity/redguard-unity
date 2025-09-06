using TMPro;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    
    [SerializeField] public GUIManager guiManager;
    

    public void OnClick()
    {
        guiManager.PickDialogueOption();
    }
    
    public void SetDisplayText(string text)
    {
        buttonText.text = text;
    }
}
