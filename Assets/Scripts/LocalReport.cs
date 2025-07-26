using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalReport : MonoBehaviour
{
    static LocalReport instance;
    public static LocalReport Instance => instance;

    string localReportPath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple singleton object created, gameobject: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        localReportPath = Path.Combine(ConfigManager.Instance.clientConfig.imageDirectory, "local_report.csv");
    }

    public void WriteRecord(params string[] data)
    {
        StreamWriter sr = new StreamWriter(localReportPath, true);
        sr.Write(data[0]);

        for (int i = 1; i < data.Length; i++)
        {
            sr.Write("," + data[i]);
        }
        sr.WriteLine();
        sr.Close();
    }
}
