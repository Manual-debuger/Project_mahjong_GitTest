using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Networking;

public class ConfigManager
{
    public static ConfigData Config { get; set; }    
    
    public void SaveConfig()
    {
        //Convert the ConfigData object to a JSON string.
        string json = JsonConvert.SerializeObject(Config);

        //Write the JSON string to a file on disk.
        try
        {
            File.WriteAllText(Application.dataPath + "/Configs/config.json", json);
            Debug.Log($"Config successfully Saved in {Application.dataPath + "/Configs/config.json"}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error Saving Config: {e.Message}");
            throw;
        }
    }

    public ConfigData LoadConfig()
    {

        //Get the JSON string from the file on disk.
        try
        {
            string text=File.ReadAllText(Application.dataPath + "/Configs/config.json");
            Config = JsonConvert.DeserializeObject<ConfigData>(text);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error Loading Config: {e.Message}");
            throw;
        }
        return Config;
        
    }
}