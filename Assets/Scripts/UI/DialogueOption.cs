using TMPro;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText;
    
    [SerializeField] public UIManager uiManager;
    [SerializeField] public int index;
    

    public void OnClick()
    {
        Debug.Log($"CLICKED {index}");
        uiManager.PickDialogueOption(index);
    }
    
    public void SetData(string text, int index)
    {
        buttonText.text = text;
        this.index = index;
    }
}
