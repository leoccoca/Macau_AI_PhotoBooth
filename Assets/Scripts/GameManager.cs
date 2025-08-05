using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<RawImage> posterRawImages;
    [SerializeField] RawImage resultImage;

    [SerializeField] Button homeBtn;

    static GameManager instance;
    public static GameManager Instance => instance;

    public bool AIProcessSucess;
    public bool DownloadPhotoIsReady;

    public Type AfterSurveyPage;

    public string JobId { get; private set; }

    string selectedGender;
    public string SelectedGender
    {
        get => selectedGender;
        set
        {
            if (value == GameDefine.MALE || value == GameDefine.FEMALE || value == GameDefine.MALEKID || value == GameDefine.FEMALEKID)
            {
                selectedGender = value;
                DebugManager.Instance.Gender = selectedGender;
            } else
            {
                Debug.LogError($"Set Gender error,");
            }
        }
    }

    int selectSpot;
    public int SelectedSpot
    {
        get => selectSpot;
        set
        {
            if (value >= 1 && value <= 4)
            {
                selectSpot = value;
            }
            else
            {
                Debug.LogError($"Set spot error, must either 1/2/3/4.");
            }
        }
    }

    public List<PosterInfo> PosterInfos { get; private set; }

    PosterInfo selectedPoster;
    public PosterInfo SelectedPoster {
        get => selectedPoster;
        set
        {
            selectedPoster = value;

            if (selectedPoster == null)
            {
                DebugManager.Instance.Poster = "";
            } else
            {
                DebugManager.Instance.Poster = selectedPoster.Name;
            }
        }
    }

    bool isRetakeAlready;
    public bool IsRetakeAlready { 
        get => isRetakeAlready;
        set { 
            isRetakeAlready = value;
            DebugManager.Instance.IsRetake = isRetakeAlready;
        }
    }
    public string SelfiePath { get; private set; }
    public string PosterPath { get; private set; }

    bool isShowHomeBtn;
    public bool IsShowHomeBtn
    {
        get => isShowHomeBtn;
        set
        {
            isShowHomeBtn = value;
            homeBtn.gameObject.SetActive(value);
        }
    }

    ErrorType gameError;
    public ErrorType GameError
    {
        get => gameError;
        set
        {
            gameError = value;
            DebugManager.Instance.GameError = gameError.ToString();
        }
    }

    public bool IsGameReady { get; private set; }

    int todayPrintJobCount;
    public int TodayPrintJobCount
    {
        get => todayPrintJobCount;
        set
        {
            todayPrintJobCount = value;
            DebugManager.Instance.TodayPrintJobCount = todayPrintJobCount;
        }
    }

    int totalPrintJobCount;
    public int TotalPrintJobCount
    {
        get => totalPrintJobCount;
        set
        {
            totalPrintJobCount = value;
            DebugManager.Instance.TotalPrintJobCount = totalPrintJobCount;
        }
    }

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
        Debug.Log("GameManager.Start()");
        // Prevent fps overflow, there will freeze web camera
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        Webcam.instance.SetupWebcam();

        ResetData();
        PosterInfos = new List<PosterInfo>();

        homeBtn.onClick.AddListener(RestartGame);
        GetAllPoster();
    }

    private void OnDestroy()
    {
        homeBtn.onClick.RemoveListener(RestartGame);
    }


    public void GetAllPoster()
    {
        WebRequestManager.Instance.GetAllPoster(PosterCallback);
    }

    public void PosterCallback(string text)
    {
        JSONNode json = SimpleJSON.JSON.Parse(text);
        if (json == null)
        {
            Debug.LogError("PosterCallback error, json is null");
            return;
        }

        bool isSuccess = json["success"].AsBool;

        if (!isSuccess)
        {
            Debug.LogError("PosterCallback Fail!");
            return;
        }

        JSONArray jsonArr = json["data"].AsArray;
        WebRequestManager.Instance.TotalPoster = jsonArr.Count;

        foreach (JSONNode posterJson in jsonArr)
        {
            //string jsonString = posterJson.ToString();
            PosterInfo posterInfo = new PosterInfo();

            posterInfo.Id = posterJson["id"].AsInt;
            posterInfo.Category = posterJson["category"].AsInt;
            posterInfo.Name = posterJson["name"].Value;
            posterInfo.Gender = posterJson["gender"].Value;
            posterInfo.Prompt = posterJson["prompt"].Value;
            posterInfo.PreviewImageUrl = posterJson["preview_image_url"].Value;
            posterInfo.RefImageId = posterJson["ref_image_id"].Value;
            posterInfo.RefImageUrl = posterJson["ref_image_url"].Value;

            posterInfo.Strength1 = posterJson["strength_1"].Value;
            posterInfo.Strength2 = posterJson["strength_2"].Value;
            posterInfo.ModelName = posterJson["model_name"].Value;
            posterInfo.PresetStyle = posterJson["preset_style"].Value;

            posterInfo.Frame = posterJson["frame"].AsInt;
            posterInfo.TargetImageUrl = posterJson["target_image_url"];

            StartCoroutine(WebRequestManager.Instance.DownloadImage(posterInfo));

            PosterInfos.Add(posterInfo);
        }
    }

    public void OnAllTextureDownloaded()
    {
        IsGameReady = true;
    }

    public void AssignRandomJobID()
    {
        JobId = Guid.NewGuid().ToString();
    }

    public void SaveSelfieAsJPEG(Texture2D tex)
    {
        string photoDirectory = Path.Combine(ConfigManager.Instance.clientConfig.imageDirectory, "photo");

        if (!Directory.Exists(photoDirectory))
        {
            Directory.CreateDirectory(photoDirectory);
        }

        string photoName = JobId + "_photo.jpg";
        SelfiePath = Path.Combine(photoDirectory, photoName);
        byte[] bytes = tex.EncodeToJPG();

        File.WriteAllBytes(SelfiePath, bytes);
        Debug.Log("Texture saved to " + SelfiePath);
    }

    public void SavePosterAsJPEG(Texture2D tex)
    {
        string photoDirectory = Path.Combine(ConfigManager.Instance.clientConfig.imageDirectory, "poster");

        if (!Directory.Exists(photoDirectory))
        {
            Directory.CreateDirectory(photoDirectory);
        }

        string photoName = JobId + "_poster.jpg";
        PosterPath = Path.Combine(photoDirectory, photoName);
        byte[] bytes = tex.EncodeToJPG();

        File.WriteAllBytes(PosterPath, bytes);
        Debug.Log("Texture saved to " + PosterPath);
    }

    public void RestartGame()
    {
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        ResetData();
        LanguageController.Instance.OnLanguageChange((int)LanguageController.langOptions.en);
        UIManager.Instance.Open<HomePage>();
    }

    public void AssignSelectedPoster(int posterId)
    {
        SelectedPoster = PosterInfos.Find(x=>x.Id == posterId);

        /* 20250714 TTDAI Upate Frame assign*/
        Webcam.instance.SetFrame( SelectedPoster.Frame);
    }

    public void UploadPhotoForAIProcess()
    {
        if (string.IsNullOrEmpty(SelfiePath))
        {
            Debug.LogError("GameManager.UploadPhotoForAIProcess() error, SelfiePath  is null or empty");
            return;
        }

        // Remark: Change to FaceSwapOnly
        //WebRequestManager.Instance.UploadPhotoForAIProcess(SelfiePath, UploadPhotoForAIProcessSuccessCallback, UploadPhotoForAIProcessFailCallback);
        WebRequestManager.Instance.ProcessPhotoFaceSwapOnly(SelfiePath, UploadPhotoForAIProcessSuccessCallback, UploadPhotoForAIProcessFailCallback);
    }

    void UploadPhotoForAIProcessSuccessCallback(string res)
    {
        Debug.Log("UploadPhotoSuccess res: " + res);

        JSONNode json = JSON.Parse(res = res.Trim('\uFEFF'));

        AIProcessSucess = json["success"].AsBool;

        if (AIProcessSucess)
        {
            string photoUrl = json["face_swap_photo_url"];
            StartCoroutine(WebRequestManager.Instance.DownloadImage(photoUrl, DownloadImageSuccessCallback, DownloadImageFailCallback));
        } else
        {
            GameError = ErrorType.ApiError;
            //UIManager.Instance.Open<TryAgainPage>();
            if (Surveypage.Instance.SurveyCompleted)
            {
                UIManager.Instance.Open<TryAgainPage>();
            }
            else
            {
                AfterSurveyPage = typeof(TryAgainPage);
            }

        }
    }

    void UploadPhotoForAIProcessFailCallback(string res)
    {
        Debug.Log("UploadPhotoFail res: " + res);
        GameError = ErrorType.NetworkError;
        //UIManager.Instance.Open<TryAgainPage>();
        if (Surveypage.Instance.SurveyCompleted)
        {
            UIManager.Instance.Open<TryAgainPage>();
        }
        else
        {
            AfterSurveyPage = typeof(TryAgainPage);
        }

    }

    void DownloadImageSuccessCallback(Texture2D tex)
    {
        DownloadPhotoIsReady = true;
        Webcam.instance.FinalPosterImage.texture = tex;
        //if survet complete before Get the result
        //UIManager.Instance.Open<ResultPage>();
        if (Surveypage.Instance.SurveyCompleted)
        {
            UIManager.Instance.Open<ResultPage>();
        }
        else
        {
            AfterSurveyPage = typeof(ResultPage);
        }
    }

    void DownloadImageFailCallback()
    {
        DownloadPhotoIsReady = false;
        //if survet complete before Get the result
        //UIManager.Instance.Open<TryAgainPage>();
        if (Surveypage.Instance.SurveyCompleted)
        {
            UIManager.Instance.Open<ResultPage>();
        }
        else
        {
            AfterSurveyPage = typeof(TryAgainPage);
        }
    }

    public void ResetData()
    {
        SelectedGender = GameDefine.MALE;
        SelectedSpot = 1;
        JobId = Guid.NewGuid().ToString();
        IsRetakeAlready = false;
        SelfiePath = "";
        PosterPath = "";
        GameError = ErrorType.None;
        SelectedPoster = null;
        DownloadPhotoIsReady = false;
        AIProcessSucess = false;
        AfterSurveyPage = null;


        WebRequestManager.Instance.GetTodayPrintJobCount(GetTodayPrintJobCallback);
        WebRequestManager.Instance.GetTotalPrintJobCount(GetTotalPrintJobCallback);
    }

    void GetTodayPrintJobCallback(string text)
    {
        JSONNode json = JSON.Parse(text);

        bool isSuccess = json["success"].AsBool;

        if (!isSuccess) {
            Debug.LogError("GetTodayPrintJobCallback Fail!");
            return;
        }

        TodayPrintJobCount = json["today_count"].AsInt;
    }

    void GetTotalPrintJobCallback(string text)
    {
        JSONNode json = JSON.Parse(text);

        bool isSuccess = json["success"].AsBool;

        if (!isSuccess)
        {
            Debug.LogError("GetTotalPrintJobCallback Fail!");
            return;
        }

        TotalPrintJobCount = json["total_count"].AsInt;
    }

    public void UploadPoster(Action<string> successCallback, Action<string> failCallback)
    {
        if (string.IsNullOrEmpty(PosterPath))
        {
            Debug.LogError("GameManager.UploadPoster() error, PostePath is null or empty");
            return;
        }
        WebRequestManager.Instance.UploadPoster(PosterPath, successCallback, failCallback);
    }

    public void WriteLocalReport()
    {
        DateTime startTime = DateTime.Now;;
        LocalReport.Instance.WriteRecord(startTime.ToString(), JobId + ".jpg", SelectedPoster.Id.ToString(), SelectedPoster.Gender);
    }
}

public enum ErrorType
{
    ApiError,
    NetworkError,
    None
}

public enum SaveType
{
    Selfie,
    Poster
}