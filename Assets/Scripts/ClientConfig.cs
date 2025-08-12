using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ClientConfig
{
    public bool isInit = false;
    public bool isFileExists = false;
    public int debugLog = 1;

    public string webCamName = string.Empty;
    public string imageDirectory = string.Empty;

    public string webServerURL = "";
    public string posterMobilePage = "";
    public int apiTimeout = 60;
    public int programTimeout = 60;

    // printer
    public int printQuotaPerDay = 0;
    public int HavePrintFunction = 0;

    // layout offset
    public float cameraIconOffset = 0;

    public static ClientConfig BuildFromFile()
    {
        ClientConfig clientConfig;

        //string curPath = Path.GetFullPath(".");

        string curPath = Application.dataPath + "/..";
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            curPath += "/../..";
        }

        string filePath = curPath + "/" + "ClientConfig.json";

        UnityEngine.Debug.Log("ClientConfig: filePath: " + filePath);

        if (File.Exists(filePath))
        {
            string configFileText = File.ReadAllText(filePath);
            clientConfig = JsonUtility.FromJson<ClientConfig>(configFileText);
            clientConfig.isFileExists = true;
        }
        else
        {
            clientConfig = new ClientConfig();
        }

        //clientConfig.UpdateRuntimeConfig();
        clientConfig.isInit = true;
        return clientConfig;
    }
}
