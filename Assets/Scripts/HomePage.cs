using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomePage : UIPage
{
    [SerializeField] Button startBtn;
    [SerializeField] InActiveTimer timer;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HomePage.Start()");
        startBtn.onClick.AddListener(OnStartBtnClick);
        startBtn.onClick.AddListener(StartBackGroundTimer);
    }

    void OnDestroy()
    {
        Debug.Log("HomePage.OnDestroy()");
        startBtn.onClick.RemoveListener(OnStartBtnClick);
        startBtn.onClick.RemoveListener(StartBackGroundTimer);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        LanguageController.Instance.OnLanguageChange((int)LanguageController.langOptions.tc);
        LanguageController.Instance.UpdatePosition(LanguageController.Instance.HomePagePos);
        LanguageController.Instance.LanguageBarActive(true);
        GameManager.Instance.IsShowHomeBtn = false;
    }

    void OnStartBtnClick()
    {
        Debug.Log("HomePage.OnStartBtnClick()");

        if (!GameManager.Instance.IsGameReady)
        {
            Debug.LogWarning("HomePage cannot click start button, game is not ready!");
            return;
        }
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<SelectSpot>();
    }

    void StartBackGroundTimer()
    {
        timer.SetTimerActive(true);
    }


}
