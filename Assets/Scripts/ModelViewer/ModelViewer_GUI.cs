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
    [SerializeField] private Image highlighter_ModeArea;
    [SerializeField] private Image highlighter_ModeObjects;
    [SerializeField] private Image highlighter_ModeTexture;
    [SerializeField] private RectTransform root_ButtonList;
    [SerializeField] private GameObject button3DC_Prefab;
    [SerializeField] private GameObject buttonROB_Prefab;

    [Header("Center Area")]
    [SerializeField] public TMP_Dropdown objectDropDown;
    [SerializeField] public string objectDropdownResetText = "All";
    [SerializeField] public TMP_Text fileNameText;
    
    [Header("Right Panel")]
    [SerializeField] public Toggle filterToggle;
    [SerializeField] public Toggle animationToggle;
    [SerializeField] public Toggle flyModeToggle;
    [SerializeField] public Button exportButton;

    private List<GameObject> areaButtonList = new();
    private List<GameObject> modelButtonList = new();
    private List<RGINIStore.worldData> areaList = new();
    private List<FileInfo> modelList = new();
    
    private Dictionary<string, string> modelPaletteDict = new()
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

    private Dictionary<string, string> areaNameDict = new()
    {
        ["BELLTOWR"] = "Bell Tower",
        ["BRENNANS"] = "Brennan's Ship",
        ["CARTOGR"] = "Cartographer",
        ["CATACOMB"] = "Palace Catacombs",
        ["CAVERNS"] = "Goblin Caverns",
        ["DRINT"] = "Dwarven Ruins",
        ["EXTPALAC"] = "Palace Courtyard",
        ["GERRICKS"] = "Gerrick's Store",
        ["HARBTOWR"] = "Harbor Tower",
        ["HIDEINT"] = "League Hideout Interior",
        ["HIDEOUT"] = "League Hideout Exterior",
        ["ISLAND"] = "Stros M'kai",
        ["JAILINT"] = "City Jail",
        ["JFFERS"] = "J'ffer's Book Store",
        ["MGUILD"] = "Mages Guild",
        ["NECRISLE"] = "N'Gasta's Island",
        ["NECRTOWR"] = "N'Gasta's Tower",
        ["OBSERVE"] = "Dwarven Observatory",
        ["PALACE"] = "Palace Interior",
        ["ROLLOS"] = "Rollo's House",
        ["SILVER1"] = "Silversmith's",
        ["SILVER2"] = "Silversmith's Dwelling",
        ["SMDEN"] = "Smuggler's Den",
        ["START"] = "Starting Area",
        ["TAVERN"] = "Draggin Tale Tavern",
        ["TEMPLE"] = "Temple of Arkay",
        ["VILE"] = "Realm of Clavicus Vile"
    };
        
    public void Initialize()
    {
        SwitchMode(ViewerModes.Areas);
        UpdateModelDependentUI();
    }
    
    public bool IsMouseOverUI { get; private set; }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOverUI = false;
    }

    private void SwitchMode(ViewerModes mode)
    {
        switch (mode)
        {
            case ViewerModes.Areas:
                HighlightModeTab(highlighter_ModeArea);
                BuildButtonList_Areas();
                break;
            case ViewerModes.Models:
                HighlightModeTab(highlighter_ModeObjects);
                BuildButtonList_Objects();
                break;
            case ViewerModes.Textures:
                HighlightModeTab(highlighter_ModeTexture);
                // todo: Add Texture GUI mode
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    private void HighlightModeTab(Image selectedImage)
    {
        highlighter_ModeArea.enabled = false;
        highlighter_ModeObjects.enabled = false;
        highlighter_ModeTexture.enabled = false;
        
        selectedImage.enabled = true;;
    }
    
    public void UpdateModelDependentUI()
    {
        UpdateFileNameDisplay();
        UpdateIsolationDropdown();
        UpdateExportButton();
    }

    private void UpdateFileNameDisplay()
    {
        if (modelViewer.loadedFileName == String.Empty)
        {
            fileNameText.text = "";
            fileNameText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            fileNameText.transform.parent.gameObject.SetActive(true);
            fileNameText.text = modelViewer.loadedFileName;
        }
    }
    
    private void UpdateIsolationDropdown()
    {
        objectDropDown.ClearOptions();
        
        List<TMP_Dropdown.OptionData>  optionsList = new List<TMP_Dropdown.OptionData>();
        optionsList.Add(new TMP_Dropdown.OptionData(objectDropdownResetText));

        if (modelViewer.loadedObjects.Count > 1)
        {
            objectDropDown.transform.parent.gameObject.SetActive(true);
            foreach (var obj in modelViewer.loadedObjects)
            {
                if (obj.TryGetComponent(out Renderer component))
                {
                    optionsList.Add(new TMP_Dropdown.OptionData(obj.name));
                }
            }
            objectDropDown.AddOptions(optionsList);
        }
        else
        {
            objectDropDown.transform.parent.gameObject.SetActive(false);
        }
    }

    private void UpdateExportButton()
    {
        exportButton.interactable = modelViewer.loadedObjects.Count > 0;
    }
    
    private void BuildButtonList_Areas()
    { 
        // generate the area list, if it is missing
        if (areaList.Count <= 0)
        {
            areaList = RGINIStore.GetWorldList().Values.ToList();

            for(int i=0;i<areaList.Count;i++)
            {
                Debug.Log($"area {i}: {areaList[i].RGM}, {areaList[i].WLD}, {areaList[i].COL}, {areaList[i].skyBoxGXA}, {areaList[i].skyBoxBSI}, {areaList[i].sunImage}, {areaList[i].loadScreen}");
                // to get materials for GXA/BSI:
                // GXA: RGTexStore.GetMaterial_GXA(GXA, 0);
                // BSI: RGTexStore.GetMaterial_BSI(COL, BSI);

            }
        
            // Delete Island duplicates from list
            areaList.RemoveAll(area =>
                (area.RGM == "ISLAND" && area.COL == "NIGHTSKY") 
                || (area.RGM == "ISLAND" && area.COL == "SUNSET")
            );
            
            // Add the missing HIDEOUT area that is missing from WORLD.INI
            if (File.Exists(Game.pathManager.GetMapsFolder() + "HIDEOUT.RGM"))
            {
                areaList.Add(new RGINIStore.worldData
                {
                    RGM = "HIDEOUT", 
                    COL = "HIDEOUT", 
                    WLD = "HIDEOUT" 
                });
                print("HIDEOUT.RGM found, adding button");
            }
            else
            {
                print("HIDEOUT.RGM not found, button will not be added");
            }
            
            
            // Remove the second "BRENNANS" entry because it has no noteworthy differences
            int brennansCount = 0;
            for (int i = 0; i < areaList.Count; i++)
            {
                if (areaList[i].RGM == "BRENNANS")
                {
                    if (brennansCount >= 1)
                    {
                        areaList.RemoveAt(i);
                    }
                    brennansCount++;
                }
            }
            
            // Sort areaList alphabetically
            areaList = areaList.OrderBy(item => 
                    areaNameDict.GetValueOrDefault(item.RGM, item.RGM), 
                StringComparer.OrdinalIgnoreCase).ToList();
        }

        // Hide all model buttons, if they exist
        if (modelButtonList.Count > 0)
        {
            foreach (var button in modelButtonList)
            {
                button.SetActive(false);
            }
        }
        
        // generate the area buttons, if they are missing
        if (areaButtonList.Count <= 0)
        {
            for(int i=0;i<areaList.Count;i++)
                SpawnButton_Area(areaList[i].RGM,areaList[i].WLD, areaList[i].COL);
        }
        // show them if they already exist
        else
        {
            foreach (var button in areaButtonList)
            {
                button.SetActive(true);
            }
        }
    }

    private void SpawnButton_Area(string RGM, string WLD, string COL)
    {
        var newButton = Instantiate(buttonROB_Prefab, root_ButtonList);
        newButton.name = "Button_" + RGM;
        areaButtonList.Add(newButton);
        
        if (newButton.TryGetComponent(out ModelViewer_AreaButton component))
        {
            component.mv_GUI = this;
            component.RGM = RGM;
            component.WLD = WLD;
            component.COL = COL;
            
            // Look for a pretty name in areaNameDict, or use the RGM string as text
            var prettyAreaName = areaNameDict.GetValueOrDefault(RGM, RGM);
            component.prettyAreaName = prettyAreaName;
            component.SetButtonText(prettyAreaName);
        }
    }

    private void BuildButtonList_Objects()
    {
        // generate the model list, if it is missing
        if (modelList.Count <= 0)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Game.pathManager.GetArtFolder());
            var fileList3D = dirInfo.GetFiles("*.3D");
            var fileList3DC = dirInfo.GetFiles("*.3DC");

            var List3Dand3DC = fileList3D.Concat(fileList3DC);
            List3Dand3DC = List3Dand3DC.OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase);
            
            var fileListROB = dirInfo.GetFiles("*.ROB");
            modelList = List3Dand3DC.Concat(fileListROB).ToList();
        }

        // hide the area buttons, if they exist
        if (areaButtonList.Count > 0)
        {
            foreach (var button in areaButtonList)
            {
                button.SetActive(false);
            }
        }
        
        // generate the model buttons, if they are missing
        if (modelButtonList.Count <= 0)
        {
            // Set color palette
            foreach (var file in modelList)
            {
                string palette = "ISLAND"; // Default
    
                if (modelPaletteDict.TryGetValue(file.Name, out string customPalette))
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
        // show the model buttons, if they do exist
        foreach (var button in modelButtonList)
        {
            button.SetActive(true);
        }
    }

    private void SpawnButton_Object(string fileName, string col)
    {
        var newButton = Instantiate(button3DC_Prefab, root_ButtonList);
        newButton.name = "Button_" + fileName;
        modelButtonList.Add(newButton);
        
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
    
    // Button Signals
    public void ModeButton_Areas()
    {
        SwitchMode(ViewerModes.Areas);
    }
    
    public void ModeButton_Objects()
    {
        SwitchMode(ViewerModes.Models);
    }
    
    public void ModeButton_Textures()
    {
        SwitchMode(ViewerModes.Textures);
    }
    
    // Redirected Button Signals
    public void RequestModel(string fileName, ModelFileType fileType, string col)
    {
        print("Requesting 3D file: " + fileName + ", fileType=" + fileType  + ", color palette=" + col);
        modelViewer.SpawnModel(fileName, fileType, col);
    }
    
    public void RequestArea(string RGM, string WLD, string COL, string prettyAreaName)
    {
        print("Requesting area: " + RGM);
        modelViewer.SpawnArea(RGM, WLD, COL, prettyAreaName);
    }
    
    public void RequestExportGLTF()
    {
        modelViewer.glTFExporter.ExportGLTF(modelViewer._objectRootGenerated, modelViewer.minimalLoadedFileName);
    }

    public void RequestObjectIsolation()
    {
        string selectedButtonText = objectDropDown.options[objectDropDown.value].text;
        
        if (selectedButtonText == objectDropdownResetText)
        {
            modelViewer.ResetIsolation();
        }
        else
        {
            modelViewer.IsolateObject(selectedButtonText);
        }
    }
}
