using TMPro;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    
    [SerializeField] public UIManager uiManager;
    

    public void OnClick()
    {
        uiManager.PickDialogueOption();
    }
    
    public void SetDisplayText(string text)
    {
        buttonText.text = text;
    }
}
