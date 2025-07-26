using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    static DebugManager instance;
    public static DebugManager Instance => instance;

    [SerializeField] Text pageNameText;
    /* 20250714 TTDAI Upate CTV selection*/
    [SerializeField] Text CTVText;
    [SerializeField] Text genderText;
    [SerializeField] Text selectedPosterText;
    [SerializeField] Text isRetakeText;
    [SerializeField] Text isCameraConnectedText;

    [SerializeField] Text todayPrintJobText;
    [SerializeField] Text totalPrintJobText;

    [SerializeField] Text gameErrorText;

    [SerializeField] GameObject debugPage;

    string gender;
    public string Gender
    {
        get => gender;
        set
        {
            gender = value;
            genderText.text = "Selected Gender: " + value;
        }
    }

    int ctv;
    public int Ctv
    {
        get => ctv;
        set
        {
            ctv = value;
            CTVText.text = "Selected CTV Style: " + value;
        }
    }

    string poster;
    public string Poster
    {
        get => poster;
        set
        {
            poster = value;
            selectedPosterText.text = "Selected Poster: " + poster;
        }
    }

    int todayPrintJobCount;
    public int TodayPrintJobCount
    {
        get => todayPrintJobCount;
        set
        {
            todayPrintJobCount = value;
            todayPrintJobText.text = "Today Print Job: " + todayPrintJobCount;
        }
    }

    int totalPrintJobCount;
    public int TotalPrintJobCount
    {
        get => totalPrintJobCount;
        set
        {
            totalPrintJobCount = value;
            totalPrintJobText.text = "Total Print Job: " + totalPrintJobCount;
        }
    }

    bool showDebug;
    public bool ShowDebug
    {
        get => showDebug;
        set
        {
            showDebug = value;
            debugPage.SetActive(showDebug);
        }
    }

    string viewName;
    public string ViewName
    {
        get => viewName;
        set
        {
            viewName = value;
            pageNameText.text = "Page: " + viewName;
        }
    }

    bool isRetake;
    public bool IsRetake
    {
        get => isRetake;
        set
        {
            isRetake = value;
            isRetakeText.text = "Is Retake: " + isRetake.ToString();
        }
    }

    bool isCameraConnected;
    public bool IsCameraConnected
    {
        get => isCameraConnected;
        set
        {
            isCameraConnected = value;

            if (isCameraConnected)
            {
                isCameraConnectedText.color = Color.green;
            }
            else
            {
                isCameraConnectedText.color = Color.red;
            }
            string message = isCameraConnected ? "Connected" : "Disconnected";
            isCameraConnectedText.text = "Is Camera Connected: " + message;
        }
    }

    string gameError;
    public string GameError
    {
        get => gameError;
        set
        {
            gameError = value;
            gameErrorText.text = "Game Error: " + gameError;
        }
    }

    void Awake()
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
        ShowDebug = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowDebug = !ShowDebug;
        }
    }
}
