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
        
        if (filePath != null)
        {
            IsInputValid(filePath);
        }
    }
    
    public void Button_Continue()
    {
        if (IsInputValid(inputField.text))
        {
            Game.HideSetup();
            Game.configManager.SaveConfig();
        }
    }

    private bool IsInputValid(string exePath)
    {
        string sanitizedExePath = exePath.Replace(@"\", "/");
        inputField.text = sanitizedExePath;
        string rootFolder = sanitizedExePath.Replace("/REDGUARD.EXE", "");

        if (Game.pathManager.SetPath(rootFolder))
        {
            errorText.text = string.Empty;
            print("Path looks valid. Setup will now exit.");
            return true;
        }
        else
        {
            errorText.text = "Invalid path";
            print("Bad path. Setup will not exit.");
            return false;
        }
    }
}
