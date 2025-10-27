using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModelViewer_ModelButton : MonoBehaviour
{
    [SerializeField] Button button3DC;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer_GUI mv_GUI;
    public string meshName;
    public ModelFileType fileType;
    public string COL;

    public void OnClick()
    {
            mv_GUI.RequestModel(meshName, fileType, COL);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
