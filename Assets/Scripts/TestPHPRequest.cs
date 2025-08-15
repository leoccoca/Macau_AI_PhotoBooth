using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TestPHPPostSimple : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SendDummyData());
    }

    private IEnumerator SendDummyData()
    {
        
        string url = "https://galaxywellnesshub.sharing.com.hk/api/add_player_record.php";

        WWWForm form = new WWWForm();
        form.AddField("sport", "2");
        form.AddField("surveyAns", "1-9,2-2,3-2,4-2,5-1,6-4.5.3.2.1,7-3.6.1,8-1");

        // Example job ID
        form.AddField("job_id", "12345");

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }
}
