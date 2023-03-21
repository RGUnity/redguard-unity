using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    string configPath;
    string defaultConfigPath;
    private ConfigData configData;
    
    // Start is called before the first frame update
    void Start()
    {
        configPath = Application.persistentDataPath + "/Config.json";
        defaultConfigPath = Application.streamingAssetsPath + "/Config_default.json";
        
        if (File.Exists(configPath))
        {
            string jsonString = File.ReadAllText(configPath);
            configData = JsonUtility.FromJson<ConfigData>(jsonString);
            print("Config Found with version " + configData.iniversion );
        }
        else
        {
            print("No Config found. Restoring default config.");
            if (File.Exists(defaultConfigPath))
            {
                string jsonString = File.ReadAllText(defaultConfigPath);
                File.WriteAllText(configPath, jsonString);
                print("Saved new config as " + configPath);
            }
            else
            {
                Debug.LogWarning("Failed to restore default config. Missing file at " + defaultConfigPath);
            }
        }
        
        
   

        // print(data.Values.Count);
        // var iniversion = data["iniversion"];
        // print(iniversion);

        // string redguardPath = configData.redguardPath;
        // print(redguardPath);
        // print(JsonUtility.ToJson(configData, true));

        // var ConfigData = new ConfigData();
        // var jsonString = JsonUtility.ToJson(ConfigData);
        // print(jsonString);


    }
    
    private class ConfigData
    {
        public int iniversion;
        public string redguardPath;
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

}
