using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.RGFileImport;
using Assets.Scripts.RGFileImport.RGGFXImport;

public class ModelViewer2_GUI : MonoBehaviour
{
    [SerializeField] private ModelViewer2 modelViewer2;
    [SerializeField] private RectTransform Root_Button3DC;
    [SerializeField] private GameObject button3DC_Prefab;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void GenerateButton3DC(string fileName)
    {
        var prettyFileName = fileName.Replace(".3DC", "");
        
        var newButton = Instantiate(button3DC_Prefab, Root_Button3DC);
        newButton.name = "Button_" + prettyFileName;
        
        if (newButton.TryGetComponent(out ModelViewer2_3DCButton component))
        {
            component.mv2_GUI = this;
            component.filename = prettyFileName;
            component.SetButtonText(prettyFileName);
        }
    }

    public void Request3DCFile(string filename)
    {
        print(filename);
        modelViewer2.Load3DC(filename);
    }
}
