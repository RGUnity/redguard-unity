using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ModelViewer_GUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ModelViewer modelViewer;
    
    [Header("Left Panel")]
    [SerializeField] private RectTransform button_ModeArea;
    [SerializeField] private RectTransform button_ModeObjects;
    [SerializeField] private RectTransform button_ModeTexture;
    [SerializeField] private RectTransform root_ButtonList;
    [SerializeField] private GameObject button3DC_Prefab;
    [SerializeField] private GameObject buttonROB_Prefab;

    [Header("Center Area")]
    [SerializeField] public TMP_Dropdown objectDropDown;
    [SerializeField] public TMP_Text fileNameText;
    
    [Header("Right Panel")]
    [SerializeField] public Toggle filterToggle;
    [SerializeField] public Toggle animationToggle;
    [SerializeField] public Toggle flyModeToggle;
    [SerializeField] public TMP_InputField exportPathInput;

    private readonly Color buttonColorDefault = Color.gray;
    private readonly Color buttonColorAccent = new(0.38f, 0.81f, 1, 1);
    
    private List<GameObject> buttonList =  new();
    
    private Dictionary<string, string> fileToPalette = new()
    {
        // Necromancer Isle, City Jail
        ["DEAD.3DC"] = "NECRO",
        ["JAILINT.ROB"] = "NECRO",
        ["NECRISLE.ROB"] = "NECRO",
        ["NECRTOWR.ROB"] = "NECRO",
        ["NCROCK.3D"] = "NECRO",
        ["SKELA001.3DC"] = "NECRO",
        ["SKELA002.3DC"] = "NECRO",
        ["SKELA003.3DC"] = "NECRO",
        ["SKELA004.3DC"] = "NECRO",
        ["VERMA001.3DC"] = "NECRO",
        ["VERMA002.3DC"] = "NECRO",
        ["VULTA001.3DC"] = "NECRO",
        ["ZOMBA001.3DC"] = "NECRO",
        ["ZOMBA002.3DC"] = "NECRO",
        
        // Goblin Caves, Mages Guild
        ["CAVERNS.ROB"] = "REDCAVE",
        ["CV_BOOM.3D"] = "REDCAVE",
        ["CV_BOOM.3DC"] = "REDCAVE",
        ["CV_EXPL1.3DC"] = "REDCAVE",
        ["CV_MUSH2.3DC"] = "REDCAVE",
        ["MGUILD.ROB"] = "REDCAVE",
        
        // Observatory, Dwemer Caves
        ["DGOLA001.3DC"] = "OBSERVAT",
        ["DRINT.ROB"] = "OBSERVAT",
        ["ERASA001.3DC"] = "OBSERVAT",
        ["GOLMA001.3DC"] = "OBSERVAT",
        ["GOLMA002.3DC"] = "OBSERVAT",
        ["OBSERVE.ROB"] = "OBSERVAT",
        
        // Restless League Hideout
        ["FLAG_RL.3DC"] = "HIDEOUT",
        ["HIDEINT.ROB"] = "HIDEOUT",
        ["HIDEOUT.ROB"] = "HIDEOUT",
        
        // Imperial Palace
        ["PALACE.ROB"] = "PALACE00",
        ["PALATEST.ROB"] = "PALACE00",
        
        // Catacombs
        ["CATACOMB.ROB"] = "CATACOMB"
    };

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
        switch (mode)
        {
            case ViewerModes.Areas:
                HighlightModeTab(button_ModeArea);
                BuildButtonList_Areas();
                break;
            case ViewerModes.Models:
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

        UpdateOverlays();
    }

    public void UpdateOverlays()
    {
        UpdateIsolationDropdown();
        UpdateFileNameDisplay();
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
        
        buttonList.Clear();
    }

    private void BuildButtonList_Areas()
    {
        ClearButtonList();
        
        List<RGINIStore.worldData> worldList = RGINIStore.GetWorldList();
        for(int i=0;i<worldList.Count;i++)
            SpawnButton_Area(worldList[i].RGM,worldList[i].WLD, worldList[i].COL);
        
        // Delete Island duplicates
        Destroy(buttonList[24]);
        print("Deleted area button #24 because that should just be the island at night.");
        Destroy(buttonList[25]);
        print("Deleted area button #24 because that should just be the island at sunset.");
        
        // Add the missing HIDEOUT area that is missing from WORLD.INI
        SpawnButton_Area("HIDEOUT", "HIDEOUT", "HIDEOUT");
    }

    private void SpawnButton_Area(string RGM, string WLD, string COL)
    {
        var newButton = Instantiate(buttonROB_Prefab, root_ButtonList);
        newButton.name = "Button_" + RGM;
        buttonList.Add(newButton);
        
        if (newButton.TryGetComponent(out ModelViewer_AreaButton component))
        {
            component.mv_GUI = this;
            component.RGM = RGM;
            component.WLD = WLD;
            component.COL = COL;
            component.SetButtonText(RGM);
        }
        
        print("Created new button with RGM=" + RGM + ", WLD=" + WLD +  ", COL=" + COL);
    }

    private void BuildButtonList_Objects()
    {
        ClearButtonList();
        
        DirectoryInfo dirInfo = new DirectoryInfo(Game.pathManager.GetArtFolder());
        var fileList3D = dirInfo.GetFiles("*.3D");
        var fileList3DC = dirInfo.GetFiles("*.3DC");
        var fileListROB = dirInfo.GetFiles("*.ROB");
        var combinedFileList = fileList3D.Concat(fileList3DC).Concat(fileListROB)
            .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
        
        // Set color palette
        foreach (var file in combinedFileList)
        {
            string palette = "ISLAND"; // Default
    
            if (fileToPalette.TryGetValue(file.Name, out string customPalette))
            {
                palette = customPalette;
            }
            else if (file.Name.Contains("CRAK"))
            {
                palette = "REDCAVE";
            }
    
            SpawnButton_Object(file.Name, palette);
        }
    }

    private void SpawnButton_Object(string fileName, string col)
    {
        var newButton = Instantiate(button3DC_Prefab, root_ButtonList);
        newButton.name = "Button_" + fileName;
        buttonList.Add(newButton);
        
        if (newButton.TryGetComponent(out ModelViewer_ModelButton component))
        {
            component.mv_GUI = this;
            component.COL = col;
            component.SetButtonText(fileName);

            // Analyze Type
            if (fileName.EndsWith(".3D"))
            {
                component.fileType = ModelFileType.file3D;
                component.meshName = fileName.Replace(".3D", "");
            }
            else if (fileName.EndsWith(".3DC"))
            {
                component.fileType = ModelFileType.file3DC;
                component.meshName = fileName.Replace(".3DC", "");
            }
            else if (fileName.EndsWith(".ROB"))
            {
                component.fileType = ModelFileType.fileROB;
                component.meshName = fileName.Replace(".ROB", "");
            }
        }
    }

    private void HighlightModeTab(RectTransform rect)
    {
        button_ModeArea.GetComponent<Image>().color = buttonColorDefault;
        button_ModeObjects.GetComponent<Image>().color = buttonColorDefault;
        button_ModeTexture.GetComponent<Image>().color = buttonColorDefault;
        
        rect.GetComponent<Image>().color = buttonColorAccent;
    }

    private void UpdateFileNameDisplay()
    {
        if (modelViewer.loadedFileName == String.Empty)
        {
            fileNameText.text = "None";
        }
        else
        {
            fileNameText.text = modelViewer.loadedFileName;
        }
    }
    
    private void UpdateIsolationDropdown()
    {
        objectDropDown.ClearOptions();
        
        List<TMP_Dropdown.OptionData>  optionsList = new List<TMP_Dropdown.OptionData>();
        optionsList.Add(new TMP_Dropdown.OptionData("None"));

        if (modelViewer.loadedObjects.Count > 1)
        {
            foreach (var obj in modelViewer.loadedObjects)
            {
                optionsList.Add(new TMP_Dropdown.OptionData(obj.name));
            }
            objectDropDown.AddOptions(optionsList);
            objectDropDown.interactable = true;
        }
        else
        {
            objectDropDown.interactable = false;
        }
    }
    
    // Button Signals
    public void ModeButton_Areas()
    {
        modelViewer.SwitchViewerMode(ViewerModes.Areas);
    }
    
    public void ModeButton_Objects()
    {
        modelViewer.SwitchViewerMode(ViewerModes.Models);
    }
    
    public void ModeButton_Textures()
    {
        modelViewer.SwitchViewerMode(ViewerModes.Textures);
    }
    
    // Redirected Button Signals
    public void RequestModel(string fileName, ModelFileType fileType, string col)
    {
        print("Requesting 3D file: " + fileName + ", fileType=" + fileType  + ", color palette=" + col);
        modelViewer.SpawnModel(fileName, fileType, col);
    }
    
    public void RequestArea(string RGM, string WLD, string COL)
    {
        print("Requesting area: " + RGM);
        modelViewer.SpawnArea(RGM, WLD, COL);
    }
    
    public void RequestExportGLTF()
    {
        modelViewer.glTFExporter.ExportGLTF(modelViewer._objectRootGenerated, modelViewer.GetExportDirectory());
    }

    public void RequestObjectIsolation()
    {
        modelViewer.IsolateObject(objectDropDown.options[objectDropDown.value].text);
    }
}
