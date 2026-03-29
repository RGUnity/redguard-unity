using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    [Header("Settings Panel")]
    [SerializeField] public Toggle filterToggle;
    [SerializeField] public Toggle animationToggle;
    [SerializeField] public Toggle flyModeToggle;
    [SerializeField] public Slider fovSlider;
    [SerializeField] public Slider flySpeedSlider;
    [SerializeField] public Slider bgColorR, bgColorG, bgColorB;
    [SerializeField] public Toggle cameraLightToggle;
    [SerializeField] public Toggle sceneLightToggle;
    [SerializeField] public Slider sceneLightIntensitySlider;
    [SerializeField] public Toggle fogToggle;
    [SerializeField] public Slider fogDensitySlider;
    [SerializeField] public Toggle scriptObjectsToggle;
    [SerializeField] public TMP_Dropdown paletteDropdown;

    [Header("Bottom Panel")]
    [SerializeField] public Button exportButton;

    private List<GameObject> areaButtonList = new();
    private List<GameObject> modelButtonList = new();
    private List<RGINIStore.worldData> areaList = new();
    private List<FileInfo> modelList = new();

    private int selectedButtonIndex = -1;
    private Color defaultButtonColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    private Color selectedButtonColor = new Color(0.3f, 0.4f, 0.6f, 1f);
    private ScrollRect fileListScrollRect;

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
        fileListScrollRect = root_ButtonList.GetComponentInParent<ScrollRect>();
        WireSettingsCallbacks();
        SwitchMode(ViewerModes.Areas);
        UpdateModelDependentUI();
    }

    private void WireSettingsCallbacks()
    {
        var settings = modelViewer.settings;

        // Sync all UI controls to code defaults before wiring callbacks
        if (flyModeToggle != null)
            flyModeToggle.SetIsOnWithoutNotify(settings.useFlyMode);
        if (filterToggle != null)
            filterToggle.SetIsOnWithoutNotify(settings.useTextureFiltering);
        if (animationToggle != null)
            animationToggle.SetIsOnWithoutNotify(settings.playAnimations);
        if (fovSlider != null)
            fovSlider.SetValueWithoutNotify(settings.fieldOfView);
        if (flySpeedSlider != null)
            flySpeedSlider.SetValueWithoutNotify(settings.flySpeedMultiplier);
        if (bgColorR != null)
            bgColorR.SetValueWithoutNotify(settings.backgroundColor.r);
        if (bgColorG != null)
            bgColorG.SetValueWithoutNotify(settings.backgroundColor.g);
        if (bgColorB != null)
            bgColorB.SetValueWithoutNotify(settings.backgroundColor.b);
        if (cameraLightToggle != null)
            cameraLightToggle.SetIsOnWithoutNotify(settings.useCameraLight);
        if (sceneLightToggle != null)
            sceneLightToggle.SetIsOnWithoutNotify(settings.useSceneLight);
        if (sceneLightIntensitySlider != null)
            sceneLightIntensitySlider.SetValueWithoutNotify(settings.sceneLightIntensity);
        if (fogToggle != null)
            fogToggle.SetIsOnWithoutNotify(settings.useFog);
        if (fogDensitySlider != null)
            fogDensitySlider.SetValueWithoutNotify(settings.fogDensity);
        if (scriptObjectsToggle != null)
            scriptObjectsToggle.SetIsOnWithoutNotify(settings.showScriptObjects);

        // Now wire callbacks
        if (flyModeToggle != null)
            flyModeToggle.onValueChanged.AddListener(val => settings.ToggleFlyMode(val));
        if (filterToggle != null)
            filterToggle.onValueChanged.AddListener(val => settings.ToggleTextureFiltering(val));
        if (animationToggle != null)
            animationToggle.onValueChanged.AddListener(val => settings.ToggleAnimations(val));
        if (fovSlider != null)
            fovSlider.onValueChanged.AddListener(val => settings.SetFieldOfView(val));
        if (flySpeedSlider != null)
            flySpeedSlider.onValueChanged.AddListener(val => settings.SetFlySpeedMultiplier(val));
        if (bgColorR != null)
            bgColorR.onValueChanged.AddListener(val => settings.SetBackgroundColor(new Color(val, settings.backgroundColor.g, settings.backgroundColor.b)));
        if (bgColorG != null)
            bgColorG.onValueChanged.AddListener(val => settings.SetBackgroundColor(new Color(settings.backgroundColor.r, val, settings.backgroundColor.b)));
        if (bgColorB != null)
            bgColorB.onValueChanged.AddListener(val => settings.SetBackgroundColor(new Color(settings.backgroundColor.r, settings.backgroundColor.g, val)));
        if (cameraLightToggle != null)
            cameraLightToggle.onValueChanged.AddListener(val => settings.ToggleCameraLight(val));
        if (sceneLightToggle != null)
            sceneLightToggle.onValueChanged.AddListener(val => settings.ToggleSceneLight(val));
        if (sceneLightIntensitySlider != null)
            sceneLightIntensitySlider.onValueChanged.AddListener(val => settings.SetSceneLightIntensity(val));
        if (fogToggle != null)
            fogToggle.onValueChanged.AddListener(val => settings.ToggleFog(val));
        if (fogDensitySlider != null)
            fogDensitySlider.onValueChanged.AddListener(val => settings.SetFogDensity(val));
        if (scriptObjectsToggle != null)
            scriptObjectsToggle.onValueChanged.AddListener(val => settings.ToggleScriptObjects(val));
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

    private void Update()
    {
        if (!modelViewer.settings.showUI) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        List<GameObject> activeButtons = GetActiveButtonList();
        if (activeButtons == null || activeButtons.Count == 0) return;

        if (kb.downArrowKey.wasPressedThisFrame)
        {
            SelectButton(Mathf.Min(selectedButtonIndex + 1, activeButtons.Count - 1), activeButtons);
        }
        else if (kb.upArrowKey.wasPressedThisFrame)
        {
            SelectButton(Mathf.Max(selectedButtonIndex - 1, 0), activeButtons);
        }
        else if (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame)
        {
            if (selectedButtonIndex >= 0 && selectedButtonIndex < activeButtons.Count)
            {
                LoadSelectedButton(activeButtons[selectedButtonIndex]);
            }
        }
    }

    private void SwitchMode(ViewerModes mode)
    {
        ResetButtonSelection();
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

        selectedImage.enabled = true;
    }

    public void UpdateModelDependentUI()
    {
        UpdateFileNameDisplay();
        UpdateIsolationDropdown();
        UpdateExportButton();

        // Determine current palette for loaded model
        string currentPal = "ISLAND";
        if (!string.IsNullOrEmpty(modelViewer.minimalLoadedFileName))
        {
            string[] extensions = { ".3DC", ".3D", ".ROB" };
            foreach (var ext in extensions)
            {
                string key = modelViewer.minimalLoadedFileName + ext;
                if (modelPaletteDict.TryGetValue(key, out string pal))
                {
                    currentPal = pal;
                    break;
                }
            }
            if (modelViewer.minimalLoadedFileName.Contains("CRAK"))
                currentPal = "REDCAVE";
        }
        UpdatePaletteDropdown(currentPal);
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

    private List<GameObject> GetActiveButtonList()
    {
        if (areaButtonList.Count > 0 && areaButtonList[0].activeSelf)
            return areaButtonList;
        if (modelButtonList.Count > 0 && modelButtonList[0].activeSelf)
            return modelButtonList;
        return null;
    }

    private void SelectButton(int index, List<GameObject> buttons)
    {
        // Deselect previous
        if (selectedButtonIndex >= 0 && selectedButtonIndex < buttons.Count)
        {
            var prevImg = buttons[selectedButtonIndex].GetComponent<Image>();
            if (prevImg != null) prevImg.color = defaultButtonColor;
        }

        selectedButtonIndex = index;

        // Highlight new
        if (selectedButtonIndex >= 0 && selectedButtonIndex < buttons.Count)
        {
            var img = buttons[selectedButtonIndex].GetComponent<Image>();
            if (img != null) img.color = selectedButtonColor;

            // Scroll to keep visible
            if (fileListScrollRect != null)
            {
                float normalizedPos = 1f - ((float)selectedButtonIndex / Mathf.Max(1, buttons.Count - 1));
                fileListScrollRect.verticalNormalizedPosition = normalizedPos;
            }
        }
    }

    private void LoadSelectedButton(GameObject button)
    {
        if (button.TryGetComponent(out ModelViewer_AreaButton areaBtn))
        {
            RequestArea(areaBtn.RGM, areaBtn.WLD, areaBtn.COL, areaBtn.prettyAreaName);
        }
        else if (button.TryGetComponent(out ModelViewer_ModelButton modelBtn))
        {
            RequestModel(modelBtn.meshName, modelBtn.fileType, modelBtn.COL);
        }
    }

    private void ResetButtonSelection()
    {
        var buttons = GetActiveButtonList();
        if (buttons != null && selectedButtonIndex >= 0 && selectedButtonIndex < buttons.Count)
        {
            var img = buttons[selectedButtonIndex].GetComponent<Image>();
            if (img != null) img.color = defaultButtonColor;
        }
        selectedButtonIndex = -1;
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

    public void SelectObjectInDropdown(string objectName)
    {
        for (int i = 0; i < objectDropDown.options.Count; i++)
        {
            if (objectDropDown.options[i].text == objectName)
            {
                objectDropDown.value = i;
                RequestObjectIsolation();
                return;
            }
        }
    }

    public void UpdatePaletteDropdown(string currentPalette)
    {
        if (paletteDropdown == null) return;

        paletteDropdown.ClearOptions();
        paletteDropdown.onValueChanged.RemoveAllListeners();

        string artFolder = Game.pathManager.GetArtFolder();
        if (!Directory.Exists(artFolder))
        {
            paletteDropdown.gameObject.SetActive(false);
            return;
        }

        var colFiles = new DirectoryInfo(artFolder).GetFiles("*.COL");
        if (colFiles.Length == 0)
        {
            paletteDropdown.gameObject.SetActive(false);
            return;
        }

        var options = new List<string>();
        int currentIndex = 0;
        foreach (var col in colFiles.OrderBy(f => f.Name))
        {
            string name = Path.GetFileNameWithoutExtension(col.Name);
            if (name.Equals(currentPalette, StringComparison.OrdinalIgnoreCase))
                currentIndex = options.Count;
            options.Add(name);
        }

        paletteDropdown.AddOptions(options);
        paletteDropdown.value = currentIndex;
        paletteDropdown.gameObject.SetActive(true);

        paletteDropdown.onValueChanged.AddListener(index =>
        {
            string selected = paletteDropdown.options[index].text;
            modelViewer.ReloadWithPalette(selected);
        });
    }
}
