using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModelViewer_3DButton : MonoBehaviour
{
    [SerializeField] Button button3DC;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer_GUI mv_GUI;
    public string meshName;
    public bool is3dcFile;
    public string COL;

    public void OnClick()
    {
            mv_GUI.Request3DFile(meshName, is3dcFile, COL);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
