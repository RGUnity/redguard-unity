using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    private string ConfigPath;
    private string DefaultConfigPath;

    public class ConfigData
    {
        public int iniversion;
        public string redguardPath;
        public bool useGlidePaths;
        public int windowMode;
        public bool vsync;
        public bool limitFPS;
        public int maxFPS;
        public bool antiAliasing;
        public bool shadows;
        public int music;
        public int effects;
        public int  voices;
        public bool subtitles;
        public bool autoDefend;
        public bool newControls;
    }

    private void Awake()
    {
        InitializeConfig();
    }

    private void InitializeConfig()
    {
        ConfigPath = Application.persistentDataPath + "/Config.json";
        DefaultConfigPath = Application.streamingAssetsPath + "/Config_default.json";
    
        // If a config already exists, load it
        if (File.Exists(ConfigPath))
        {
            string jsonString = File.ReadAllText(ConfigPath);
            Game.configData = JsonUtility.FromJson<ConfigData>(jsonString);
            print("Config Found with version " + Game.configData.iniversion );
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
            }
            else
            {
                Debug.LogWarning("Failed to restore default config. Missing file at " + DefaultConfigPath);
            }
        }
    }
}

