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
        form.AddField("sport", "TestSport");
        form.AddField("q1", "1");
        form.AddField("q2", "2");
        form.AddField("q3", "3");
        form.AddField("q4", "4");
        form.AddField("q5", "5");
        form.AddField("q6", "6");
        form.AddField("q7", "7");
        form.AddField("job_id", "JIASDJHIAOSDN");
        

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
