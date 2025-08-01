using TMPro;
using UnityEngine;


public class Setup : MonoBehaviour
{
    private ConfigData config;
    [SerializeField] TMP_InputField inputField;
    
    public void Button_Continue()
    {
        string path = inputField.text.Replace(@"\", "/");
        Game.Config.redguardPath = path;
        Game.StartupChecks();
    }
}
