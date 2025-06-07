using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelViewer2_ROBButton : MonoBehaviour
{
    [SerializeField] Button buttonROB;
    [SerializeField] private TMP_Text buttonText;

    public ModelViewer2_GUI mv2_GUI;
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
        mv2_GUI.RequestROBFile(filename);
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }
}
