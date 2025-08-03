using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ModelViewer_GUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ModelViewer modelViewer;
    [SerializeField] private RectTransform button_ModeArea;
    [SerializeField] private RectTransform button_ModeObjects;
    [SerializeField] private RectTransform button_ModeTexture;
    [SerializeField] private RectTransform root_ButtonList;
    [SerializeField] private GameObject button3DC_Prefab;
    [SerializeField] private GameObject buttonROB_Prefab;
    [SerializeField] public TMP_InputField exportPathInput;
    [SerializeField] public TMP_Dropdown objectDropDown;
    [SerializeField] public GameObject overlays_AreaMode;

    private readonly Color buttonColorDefault = Color.gray;
    private readonly Color buttonColorAccent = new(0.38f, 0.81f, 1, 1);

    public bool IsMouseOverUI { get; private set; }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOverUI = false;
    }
    
    public void UpdateGUI(ViewerModes mode)
    {
        ClearButtonList();
        ClearIsolationDropdown();
        objectDropDown.interactable = false;
        overlays_AreaMode.SetActive(false);
        
        switch (mode)
        {
            case ViewerModes.Area:
                HighlightModeTab(button_ModeArea);
                BuildButtonList_Areas();
                overlays_AreaMode.SetActive(true);
                break;
            case ViewerModes.Objects:
                HighlightModeTab(button_ModeObjects);
                BuildButtonList_Objects();
                break;
            case ViewerModes.Textures:
                HighlightModeTab(button_ModeTexture);
                // todo: Add Texture GUI mode
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    private void ClearButtonList()
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

    private void BuildButtonList_Areas()
    {
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST
        List<RGINIStore.worldData> worldList = RGINIStore.GetWorldList();
        for(int i=0;i<worldList.Count;i++)
            SpawnButton_Area(worldList[i].RGM,worldList[i].WLD, worldList[i].COL);
    }

    private void SpawnButton_Area(string RGM, string WLD, string COL)
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

    private void BuildButtonList_Objects()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Game.pathManager.GetArtFolder());
        var fileList = dirInfo.GetFiles("*.3DC");
        
        foreach (var file in fileList)
        {
            SpawnButton_Object(file.Name);
        }
    }

    private void SpawnButton_Object(string fileName)
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

    private void HighlightModeTab(RectTransform rect)
    {
        button_ModeArea.GetComponent<Image>().color = buttonColorDefault;
        button_ModeObjects.GetComponent<Image>().color = buttonColorDefault;
        button_ModeTexture.GetComponent<Image>().color = buttonColorDefault;
        
        rect.GetComponent<Image>().color = buttonColorAccent;
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
    
    // Button Signals
    public void ModeButton_Areas()
    {
        modelViewer.SwitchViewerMode(ViewerModes.Area);
    }
    
    public void ModeButton_Objects()
    {
        modelViewer.SwitchViewerMode(ViewerModes.Objects);
    }
    
    public void ModeButton_Textures()
    {
        modelViewer.SwitchViewerMode(ViewerModes.Textures);
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
    
    public void RequestExportGLTF()
    {
        modelViewer.glTFExporter.ExportGLTF(modelViewer._objectRootGenerated, modelViewer.exportDirectory);
    }

    public void RequestObjectIsolation()
    {
        modelViewer.IsolateObject(objectDropDown.options[objectDropDown.value].text);
    }
}
