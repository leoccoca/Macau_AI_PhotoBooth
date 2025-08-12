using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InActiveTimer : MonoBehaviour
{
    public float timeout = 90f; // seconds
    [SerializeField] private float timer;
    private bool isActive = true; // controls whether timer runs

    void OnEnable()
    {
    }
    private void Start()
    {
        timer = 0f;
        isActive = false;
        timeout = ConfigManager.Instance.clientConfig.programTimeout;
    }

    void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;

        // Detect activity
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            timer = 0f; 
        }

        // Timeout reached
        if (timer >= timeout)
        {
            GameManager.Instance.RestartGame();
            SetTimerActive(false);
        }
    }

    // Call this from your "Next Page" or scene change code
    public void SetTimerActive(bool active)
    {
        timer = 0f;
        isActive = active;
    }

}
