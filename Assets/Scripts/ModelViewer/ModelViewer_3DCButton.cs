using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModelViewer_3DCButton : MonoBehaviour
{
    [SerializeField] Button button3DC;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer_GUI mv_GUI;
    public string objectName;

    public void OnClick()
    {
        mv_GUI.Request3DCFile(objectName);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
