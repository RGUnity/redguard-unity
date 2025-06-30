using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModelViewer_ROBButton : MonoBehaviour
{
    [SerializeField] Button buttonROB;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer_GUI mv_GUI;
    public string fileName;
    
    
    public void OnClick()
    {
        mv_GUI.RequestArea(fileName);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
