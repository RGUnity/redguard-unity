using TMPro;
using UnityEngine;


public class Setup : MonoBehaviour
{
    private ConfigData config;
    [SerializeField] TMP_InputField inputField;
    
    public void Button_Continue()
    {
        string path = inputField.text.Replace(@"\", "/");

        if (Game.pathManager.SetPath(path))
        {
            print("Path looks valid. Setup will now exit.");
            Game.HideSetup();
            Game.configManager.SaveConfig();
        }
        else
        {
            print("Bad path. Setup will not exit.");
        }
    }
}
