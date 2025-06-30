using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ModelViewer_3DCButton : MonoBehaviour
{
    [SerializeField] Button button3DC;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer_GUI mv_GUI;
    public string filename;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        mv_GUI.Request3DCFile(filename);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
