using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.RGFileImport;
using Assets.Scripts.RGFileImport.RGGFXImport;
using TMPro;
using UnityEngine.EventSystems;

public class ModelViewer_GUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ModelViewer modelViewer;
    [SerializeField] private RectTransform button_ModeLevel;
    [SerializeField] private RectTransform button_ModeObjects;
    [SerializeField] private RectTransform button_ModeTexture;
    [SerializeField] private RectTransform root_ButtonList;
    [SerializeField] private GameObject button3DC_Prefab;
    [SerializeField] private GameObject buttonROB_Prefab;
    [SerializeField] private GameObject errorPopup_Path;
    [SerializeField] public TMP_InputField pathInput;
    [SerializeField] public TMP_InputField exportPathInput;
    [SerializeField] public TMP_Dropdown objectDropDown;
    [SerializeField] public GameObject overlays_AreaMode;

    public bool IsMouseOverUI { get; private set; }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOverUI = false;
    }
    
    public void ClearButtonList()
    {
        // Clear all buttons
        if (root_ButtonList.transform.childCount > 0)
        {
            for (int i = 0; i < root_ButtonList.transform.childCount; i++)
            {
                Transform childTransform = root_ButtonList.transform.GetChild(i);
                Destroy(childTransform.gameObject);
            }
        }
    }
    
    // Build the UI
    public void UpdateUI_Models(FileInfo[]  fileList)
    {
        // Update Button appearance
        button_ModeLevel.GetComponent<Image>().color = Color.gray;
        button_ModeObjects.GetComponent<Image>().color = new Color(0.38f, 0.81f, 1, 1);
        button_ModeTexture.GetComponent<Image>().color = Color.gray;

        ClearButtonList();
        
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
        
        var newButton = Instantiate(button3DC_Prefab, root_ButtonList);
        newButton.name = "Button_" + prettyFileName;
        
        if (newButton.TryGetComponent(out ModelViewer_3DCButton component))
        {
            component.mv_GUI = this;
            component.filename = prettyFileName;
            component.SetButtonText(fileName);
        }
    }
    
    
    public void UpdateUI_Levels()
    {
        // Update Button appearance
        button_ModeLevel.GetComponent<Image>().color = new Color(0.38f, 0.81f, 1, 1);
        button_ModeObjects.GetComponent<Image>().color = Color.gray;
        button_ModeTexture.GetComponent<Image>().color = Color.gray;

        ClearButtonList();
        
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST
        List<RGINIStore.worldData> worldList = RGINIStore.GetWorldList();
        for(int i=0;i<worldList.Count;i++)
            SpawnButton_Level(worldList[i].RGM,worldList[i].WLD, worldList[i].COL);
    }
    
    public void SpawnButton_Level(string RGM, string WLD, string COL)
    {
        
        var newButton = Instantiate(buttonROB_Prefab, root_ButtonList);
        newButton.name = "Button_" + RGM;
        
        if (newButton.TryGetComponent(out ModelViewer_ROBButton component))
        {
            component.mv_GUI = this;
            component.RGM = RGM;
            component.WLD = WLD;
            component.COL = COL;
            component.SetButtonText(RGM);
        }
    }
    
    public void UpdateUI_Textures()
    {
        // Update Button appearance
        button_ModeLevel.GetComponent<Image>().color = Color.gray;
        button_ModeObjects.GetComponent<Image>().color = Color.gray;
        button_ModeTexture.GetComponent<Image>().color = new Color(0.38f, 0.81f, 1, 1);

        ClearButtonList();

        // todo: Show Textures
    }
    
    // Button Signals
    public void ModeButton_Levels()
    {
        modelViewer.ViewerMode_Areas();
    }
    
    public void ModeButton_Objects()
    {
        modelViewer.ViewerMode_Models();
    }
    
    public void ModeButton_Textures()
    {
        modelViewer.ViewerMode_Textures();
        
        // Update Button appearance
        button_ModeLevel.GetComponent<Image>().color = Color.gray;
        button_ModeObjects.GetComponent<Image>().color = Color.gray;
        button_ModeTexture.GetComponent<Image>().color = new Color(0.38f, 0.81f, 1, 1);
    }

    // Fill the Isolation Dropdown with all objects that are currently loaded
    public void PopulateIsolationDropdown(List<GameObject> objects)
    {
        List<TMP_Dropdown.OptionData>  options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData("None"));
        
        foreach (var obj in objects)
        {
            options.Add(new TMP_Dropdown.OptionData(obj.name));
        }
        
        objectDropDown.ClearOptions();
        objectDropDown.AddOptions(options);
    }

    // Clear the dropdown and display an idle text
    public void ClearIsolationDropdown()
    {
        List<TMP_Dropdown.OptionData>  options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData("None"));
        
        objectDropDown.ClearOptions();
        objectDropDown.AddOptions(options);
    }
    
    // Redirected Button Signals
    public void Request3DCFile(string fileName)
    {
        print("Requesting 3DC file: " + fileName);
        modelViewer.Spawn3DC(fileName, "ISLAND");
    }
    
    public void RequestArea(string RGM, string WLD, string COL)
    {
        print("Requesting area: " + RGM);
        modelViewer.SpawnArea(RGM, WLD, COL);
    }

    public void PathErrorMode(bool toggle)
    {
        errorPopup_Path.SetActive(toggle);
        if (toggle)
        {
            pathInput.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            pathInput.GetComponent<Image>().color = Color.white;
        }
    }
    
    public void RequestExportGLTF()
    {
        modelViewer.ExportGLTF();
    }

    public void RequestObjectIsolation()
    {
        modelViewer.IsolateObject(objectDropDown.options[objectDropDown.value].text);
    }
}
