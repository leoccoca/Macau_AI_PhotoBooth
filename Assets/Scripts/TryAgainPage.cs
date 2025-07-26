using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TryAgainPage : UIPage
{
    [SerializeField] Button retakeBtn;
    [SerializeField] Button backToHomeBtn;

    // Start is called before the first frame update
    void Start()
    {
        //retakeBtn.onClick.AddListener(OnRetakeBtnClick);
        backToHomeBtn.onClick.AddListener(OnBackToHomeClick);
    }

    void OnDestroy()
    {
        //retakeBtn.onClick.RemoveListener(OnRetakeBtnClick);
        backToHomeBtn.onClick.RemoveListener(OnBackToHomeClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();


        /* TTD AI Photobooth remove retake function
        ErrorType errorType = GameManager.Instance.GameError;

        if (errorType == ErrorType.ApiError)
        {
            retakeBtn.gameObject.SetActive(true);
        } else if (errorType == ErrorType.NetworkError)
        {
            retakeBtn.gameObject.SetActive(false);
        }
        */
    }

    void OnRetakeBtnClick()
    {
        Debug.Log("TryAgainPage.OnRetakeBtnClick()");
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<PhotoTakingPage>();

        GameManager.Instance.GameError = ErrorType.None;
    }

    void OnBackToHomeClick()
    {
        Debug.Log("TryAgainPage.OnBackToHomeClick()");
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        GameManager.Instance.ResetData();
        UIManager.Instance.Open<HomePage>();

    }
}
