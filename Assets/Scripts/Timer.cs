using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private int startTime = 15;

    private void OnEnable()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        int timeLeft = startTime;

        while (timeLeft >= 0)
        {
            countdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        Debug.Log("disclaimer timesOut");
        GameManager.Instance.RestartGame();
    }
}
