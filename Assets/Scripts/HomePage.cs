using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomePage : UIPage
{
    [SerializeField] Button startBtn;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HomePage.Start()");
        startBtn.onClick.AddListener(OnStartBtnClick);
    }

    void OnDestroy()
    {
        Debug.Log("HomePage.OnDestroy()");
        startBtn.onClick.RemoveListener(OnStartBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        OnLanguageChange((int)LanguageController.langOptions.en);
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


    public void OnLanguageChange(int lang)
    {
        LanguageController.Instance.ChangeLanguage((LanguageController.langOptions) lang);
    }
}
