using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    private string ConfigPath;
    private string DefaultConfigPath;

    public void InitializeConfig()
    {
        ConfigPath = Application.persistentDataPath + "/Config.json";
        DefaultConfigPath = Application.streamingAssetsPath + "/Config_default.json";
    
        // If a config already exists, load it
        if (File.Exists(ConfigPath))
        {   
            LoadConfig(ConfigPath);
            print("Loaded config file from here: " + ConfigPath);
        }
        // if none is found, try to create a new config
        else
        {
            print("No Config found. Restoring default config.");
            if (File.Exists(DefaultConfigPath))
            {
                string jsonString = File.ReadAllText(DefaultConfigPath);
                File.WriteAllText(ConfigPath, jsonString);
                print("Saved new config as " + ConfigPath);
                LoadConfig(ConfigPath);
            }
            else
            {
                Debug.LogWarning("Failed to restore default config. Missing file at " + DefaultConfigPath);
            }
        }
    }

    private void LoadConfig(string path)
    {
        string jsonString = File.ReadAllText(path);
        //JsonUtility.FromJsonOverwrite(jsonString, Game.Config);
        Game.Config = JsonUtility.FromJson<ConfigData>(jsonString);
    }
    
    public void SaveConfig()
    {
        string jsonString = JsonUtility.ToJson(Game.Config);
        File.WriteAllText(ConfigPath, jsonString);
        print("Config Saved as " + ConfigPath);
    }
}

