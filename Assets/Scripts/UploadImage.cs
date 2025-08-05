using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class UploadImage : MonoBehaviour
{
    [Header("🔗 Image URLs (Add here or via code)")]
    public List<string> imageUrls = new List<string>();

    [Header("📌 Extracted landmarks_str (copyable)")]
    public List<string> landmarkResults = new List<string>();

    private string apiUrl = "https://bookfair.sharing.com.hk/AIPhotoBooth/testProcessPhoto.php";
    private string secret = "72b3a1320e612e2a";

    void Start()
    {
        StartCoroutine(ProcessAllImages());
    }

    IEnumerator ProcessAllImages()
    {
        landmarkResults.Clear(); // Clear previous results

        foreach (string imageUrl in imageUrls)
        {
            yield return StartCoroutine(SendToAPI(imageUrl));
        }

        Debug.Log("✅ All image URLs processed.");
    }

    IEnumerator SendToAPI(string imageUrl)
    {
        WWWForm form = new WWWForm();
        form.AddField("imageUrl", imageUrl);
        form.AddField("secret", secret);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonText = www.downloadHandler.text;

                // 🔍 Parse using SimpleJSON
                var parsed = JSON.Parse(jsonText);
                string landmarkStr = parsed["userPhotoFaceDetectRes"]["landmarks_str"][0];

                landmarkResults.Add(landmarkStr);
                Debug.Log($"✅ {imageUrl} → {landmarkStr}");
            }
            else
            {
                Debug.LogError($"❌ Error for {imageUrl}: {www.error}");
                landmarkResults.Add("ERROR");
            }
        }
    }
}
