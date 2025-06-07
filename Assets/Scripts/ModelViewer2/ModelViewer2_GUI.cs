using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.RGFileImport;
using Assets.Scripts.RGFileImport.RGGFXImport;

public class ModelViewer2_GUI : MonoBehaviour
{
    [SerializeField] private ModelViewer2 modelViewer2;
    [SerializeField] private RectTransform Root_Button3DC;
    [SerializeField] private GameObject button3DC_Prefab;
    [SerializeField] private GameObject buttonROB_Prefab;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    // Build the UI
    public void BuildList_Models(FileInfo[]  fileList)
    {
        // Clear all buttons
        if (Root_Button3DC.transform.childCount > 0)
        {
            for (int i = 0; i < Root_Button3DC.transform.childCount; i++)
            {
                Transform childTransform = Root_Button3DC.transform.GetChild(i);
                Destroy(childTransform.gameObject);
            }
        }
        
        foreach (var file in fileList)
        {
            //print(file.Name);
            SpawnButton_Model(file.Name);
        }
        
        // todo: generate other button lists
    }
    
    public void SpawnButton_Model(string fileName)
    {
        var prettyFileName = fileName.Replace(".3DC", "");
        
        var newButton = Instantiate(button3DC_Prefab, Root_Button3DC);
        newButton.name = "Button_" + prettyFileName;
        
        if (newButton.TryGetComponent(out ModelViewer2_3DCButton component))
        {
            component.mv2_GUI = this;
            component.filename = prettyFileName;
            component.SetButtonText(fileName);
        }
    }
    
    
    public void BuildList_Levels()
    {
        // Clear all buttons
        if (Root_Button3DC.transform.childCount > 0)
        {
            for (int i = 0; i < Root_Button3DC.transform.childCount; i++)
            {
                Transform childTransform = Root_Button3DC.transform.GetChild(i);
                Destroy(childTransform.gameObject);
            }
        }
        
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        
        SpawnButton_Level("BELLTOWR");
        SpawnButton_Level("BRENNANS");
        SpawnButton_Level("CARTOGR");
        SpawnButton_Level("CATACOMB");
        SpawnButton_Level("CAVERNS");
        SpawnButton_Level("DRINT");
        SpawnButton_Level("EXTPALAC");
        SpawnButton_Level("GERRICKS");
        SpawnButton_Level("HARBOTWR");
        SpawnButton_Level("HIDEINT");
        SpawnButton_Level("HIDEOUT");
        SpawnButton_Level("INVENTRY");
        SpawnButton_Level("ISLAND");
        SpawnButton_Level("JAILINT");
        SpawnButton_Level("JFFERS");
        SpawnButton_Level("MENU");
        SpawnButton_Level("MGUILD");
        SpawnButton_Level("NECRISLE");
        SpawnButton_Level("NECTROWR");
        SpawnButton_Level("OBSERVE");
        SpawnButton_Level("PALACE");
        SpawnButton_Level("PALATEST");
        SpawnButton_Level("ROLLOS");
        SpawnButton_Level("SILVER1");
        SpawnButton_Level("SILVER2");
        SpawnButton_Level("SMDEN");
        SpawnButton_Level("START");
        SpawnButton_Level("TAVERN");
        SpawnButton_Level("TEMPLE");
        SpawnButton_Level("TEMPEST");
        SpawnButton_Level("VILE");

    }
    
    public void SpawnButton_Level(string fileName)
    {
        
        var newButton = Instantiate(buttonROB_Prefab, Root_Button3DC);
        newButton.name = "Button_" + fileName;
        
        if (newButton.TryGetComponent(out ModelViewer2_ROBButton component))
        {
            component.mv2_GUI = this;
            component.filename = fileName;
            component.SetButtonText(fileName + ".ROB");
        }
    }
    
    // Button Signals
    public void ModeButton_Levels()
    {
        modelViewer2.ViewerMode_Levels();
    }
    
    public void ModeButton_Objects()
    {
        modelViewer2.ViewerMode_Models();
    }
    
    public void ModeButton_Textures()
    {
        modelViewer2.ViewerMode_Textures();
    }
    

    
    // Redirected Button Signals
    public void Request3DCFile(string filename)
    {
        print("Requesting 3DC file: " + filename);
        modelViewer2.Load3DC(filename);
    }
    
    public void RequestROBFile(string filename)
    {
        print("Requesting ROB file: " + filename);
        modelViewer2.LoadROB(filename);
    }
}
