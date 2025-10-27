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
    
    private List<GameObject> buttonList =  new();
    
    private HashSet<string> goblinFiles = new()
    { 
        "CV_BOOM.3D",
        "CV_BOOM.3DC",
        "CV_EXPL1.3DC",
        "CV_MUSH2.3DC",
    };
    
    private HashSet<string> necroisleFiles = new()
    {
        "DEAD.3DC",
        "NCROCK.3D",
        "SKELA001.3DC",
        "SKELA002.3DC",
        "SKELA003.3DC",
        "SKELA004.3DC",
        "VERMA001.3DC",
        "VERMA002.3DC",
        "VULTA001.3DC",
        "ZOMBA001.3DC",
        "ZOMBA002.3DC"
    };

    private HashSet<string> observatoryFiles = new()
    {
        "DGOLA001.3DC",
        "ERASA001.3DC",
        "GOLMA001.3DC",
        "GOLMA002.3DC"
    };
    
    private HashSet<string> hideoutFiles = new()
    {
        "FLAG_RL.3DC"
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
        // ROBs without RGM currently dont work:
        // INVENTRY
        // MENU
        // PALATEST
        // TEMPTEST
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
            if (goblinFiles.Contains(file.Name) || file.Name.Contains("CRAK"))
            {
                SpawnButton_Object(file.Name, "REDCAVE");
            }
            else if (necroisleFiles.Contains(file.Name))
            {
                SpawnButton_Object(file.Name, "NECRO");
            }
            else if (observatoryFiles.Contains(file.Name))
            {
                SpawnButton_Object(file.Name, "OBSERVAT");
            }
            else if (hideoutFiles.Contains(file.Name))
            {
                SpawnButton_Object(file.Name, "HIDEOUT");
            }
            else
            {
                // If none apply, use ISLAND as the default color palette
                SpawnButton_Object(file.Name, "ISLAND");
            }
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
