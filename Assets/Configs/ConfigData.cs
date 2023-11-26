using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigData
{
    public string NetBackendUrl;
    public ConfigData(string netBackendUrl)
    {
        NetBackendUrl = netBackendUrl;
    }
}
