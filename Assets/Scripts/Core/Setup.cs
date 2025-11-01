using System;
using System.Linq;
using SFB;
using TMPro;
using UnityEngine;


public class Setup : MonoBehaviour
{
    private ConfigData config;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] private TMP_Text errorText;


    public void OnEnable()
    {
        if (Game.pathManager.ValidatePath(Game.pathManager.GetRootFolder()))
        {
            inputField.text = Game.pathManager.GetRootFolder() + "/REDGUARD.EXE";
        }
        else
        {
            inputField.text = string.Empty;
        }
        errorText.text = string.Empty;
    }

    public void Button_Browse()
    {
        var extensionList = new [] {
            new ExtensionFilter("Applications", "exe"),
        };
        
        var filePathArray = StandaloneFileBrowser.OpenFilePanel("Find Redguard.exe", "", extensionList, false);

        var filePath = filePathArray.SingleOrDefault();
        
        
        if (filePath == null)
        {
            print("No file selected");
        }
        else
        {
            string filePathSanitized = filePath.Replace(@"\", "/");
            inputField.text = filePathSanitized;
        }
    }
    
    public void Button_Continue()
    {
        string rootFolder = inputField.text.Replace(@"\", "/");
        rootFolder = rootFolder.Replace("/REDGUARD.EXE", "");

        if (Game.pathManager.SetPath(rootFolder))
        {
            errorText.text = string.Empty;
            
            print("Path looks valid. Setup will now exit.");
            Game.HideSetup();
            Game.configManager.SaveConfig();
        }
        else
        {
            errorText.text = "Invalid path";
            print("Bad path. Setup will not exit.");
        }
    }
}
