using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebRequestManager : MonoBehaviour
{
    static WebRequestManager instance;
    public static WebRequestManager Instance => instance;

    int totalPoster;
    public int TotalPoster { set => totalPoster = value; }

    int noOfPosterDownloaded;

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

    public void UploadPhotoForAIProcess(string photoPath, Action<string> successCallback, Action<string> failCallback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "processPhoto.php";

        WWWForm form = new WWWForm();
        form.AddField("secret", GameDefine.API_SECRET);
        form.AddField("poster_id", GameManager.Instance.SelectedPoster.Id);
        form.AddField("poster_uuid", GameManager.Instance.SelectedPoster.RefImageId);
        form.AddField("job_id", GameManager.Instance.JobId);
        form.AddField("prompt", GameManager.Instance.SelectedPoster.Prompt);
        form.AddField("strength_1", GameManager.Instance.SelectedPoster.Strength1);
        form.AddField("strength_2", GameManager.Instance.SelectedPoster.Strength2);
        form.AddField("model_name", GameManager.Instance.SelectedPoster.ModelName);
        form.AddField("preset_style", GameManager.Instance.SelectedPoster.PresetStyle);

        byte[] bytes = File.ReadAllBytes(photoPath);
        form.AddBinaryData("file", bytes, Path.GetFileName(photoPath), "image/jpeg");

        StartCoroutine(RequestUrlPost(url, form, successCallback, failCallback));
    }

    public void ProcessPhotoFaceSwapOnly(string photoPath, Action<string> successCallback, Action<string> failCallback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "processPhotoFaceSwapOnly.php";

        WWWForm form = new WWWForm();
        form.AddField("secret", GameDefine.API_SECRET);
        form.AddField("poster_id", GameManager.Instance.SelectedPoster.Id);
        //form.AddField("poster_uuid", GameManager.Instance.SelectedPoster.RefImageId);
        form.AddField("job_id", GameManager.Instance.JobId);
        //form.AddField("prompt", GameManager.Instance.SelectedPoster.Prompt);
        //form.AddField("strength_1", GameManager.Instance.SelectedPoster.Strength1);
        //form.AddField("strength_2", GameManager.Instance.SelectedPoster.Strength2);
        //form.AddField("model_name", GameManager.Instance.SelectedPoster.ModelName);
        //form.AddField("preset_style", GameManager.Instance.SelectedPoster.PresetStyle);
        form.AddField("target_image_url", GameManager.Instance.SelectedPoster.TargetImageUrl);

        byte[] bytes = File.ReadAllBytes(photoPath);
        form.AddBinaryData("file", bytes, Path.GetFileName(photoPath), "image/jpeg");

        StartCoroutine(RequestUrlPost(url, form, successCallback, failCallback));
    }

    public void UploadPoster(string photoPath, Action<string> successCallback, Action<string> failCallback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "uploadPoster.php";

        WWWForm form = new WWWForm();
        form.AddField("secret", GameDefine.API_SECRET);

        byte[] bytes = File.ReadAllBytes(photoPath);
        form.AddBinaryData("file_upload", bytes, Path.GetFileName(photoPath), "image/jpeg");
        StartCoroutine(RequestUrlPost(url, form, successCallback, failCallback));
    }

    public void UpdatePrintJobRecord(Action<string> successCallback, Action<string> failCallback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "updatePrintJobCount.php";

        WWWForm form = new WWWForm();
        form.AddField("secret", GameDefine.API_SECRET);

        StartCoroutine(RequestUrlPost(url, form, successCallback, failCallback));
    }

    public IEnumerator RequestUrlPost(string url, WWWForm formData, Action<string> successCallback, Action<string> failCallback)
    {
        Debug.Log("Time: " + DateTime.Now.ToString() + " RequestUrl: " + url);

        DateTime startTime = DateTime.Now;
        UnityWebRequest www = UnityWebRequest.Post(url, formData);

        www.timeout = 180;
        yield return www.SendWebRequest();
        
        TimeSpan timeUsed = DateTime.Now - startTime;
        Debug.Log("Url: " + url + " : timeUsed: " + timeUsed.TotalSeconds);
        Debug.Log("Response Code:" + www.responseCode);

        if (www.result == UnityWebRequest.Result.Success)
        {
            successCallback?.Invoke(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Network Error: RequestUrl: " + url);
            Debug.LogError(www.error);

            failCallback?.Invoke(www.downloadHandler.text);
        }
    }

    public void GetTodayPrintJobCount(Action<string> successCallback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "getTodayPrintJobCount.php";
        StartCoroutine(RequestUrlGet(url, successCallback));
    }

    public void GetTotalPrintJobCount(Action<string> successCallback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "getTotalPrintJobCount.php";
        StartCoroutine(RequestUrlGet(url, successCallback));
    }

    public IEnumerator RequestUrlGet(string url, Action<string> successCallback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            successCallback?.Invoke(request.downloadHandler.text);
        }
    }

    public void GetAllPoster(Action<string> callback)
    {
        string url = ConfigManager.Instance.clientConfig.webServerURL + "getAllGamePoster.php";
        WWWForm form = new WWWForm();

        StartCoroutine(RequestUrlPost(url, form, callback, null));
    }

    public IEnumerator DownloadImage(PosterInfo posterInfo)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(posterInfo.PreviewImageUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                posterInfo.Texture = DownloadHandlerTexture.GetContent(uwr);
                noOfPosterDownloaded++;

                if (noOfPosterDownloaded >= totalPoster)
                {
                    GameManager.Instance.OnAllTextureDownloaded();
                }
            } else
            {
                Debug.LogError($"Failed to download image from {posterInfo.PreviewImageUrl}: {uwr.error}");
            }


        }
    }

    public IEnumerator DownloadImage(string url, Action<Texture2D> successCallback, Action failCallback)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                successCallback?.Invoke(texture);
            }
            else
            {
                Debug.LogError($"Failed to download image from {url}: {uwr.error}");
                failCallback?.Invoke();
            }
        }
    }
}

[Serializable]
public class PosterInfo
{
    public int Id { get; set; }
    public int Category { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Prompt { get; set; }
    public string PreviewImageUrl { get; set; }
    public string RefImageId { get; set; }
    public string RefImageUrl { get; set; }
    public string Strength1 { get; set; }
    public string Strength2 { get; set; }
    public string ModelName { get; set; }
    public string PresetStyle { get; set;}
    public int Frame { get; set; }
    public string TargetImageUrl { get; set; }

    public Texture2D Texture { get; set; }
}