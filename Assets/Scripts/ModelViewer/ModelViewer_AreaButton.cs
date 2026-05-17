using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModelViewer_AreaButton : MonoBehaviour
{
    [SerializeField] Button buttonROB;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer_GUI mv_GUI;
    public int worldId;
    public string RGM;
    public string WLD;
    public string COL;
    public string prettyAreaName;
    
    
    public void OnClick()
    {
        mv_GUI.RequestArea(worldId, RGM, WLD,COL, prettyAreaName);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
