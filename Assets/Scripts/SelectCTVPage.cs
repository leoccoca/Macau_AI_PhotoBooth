using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/* 20250714 TTDAI  CTV selection*/
public class SelectCTVPage : UIPage
{
    [SerializeField] List<Toggle> toggles;
    [SerializeField] Button nextBtn;

    // Start is called before the first frame update
    void Start()
    {
        nextBtn.onClick.AddListener(OnNextBtnClick);
    }

    private void OnDestroy()
    {
        nextBtn.onClick.RemoveListener(OnNextBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        //TTD AI Photobooth, default no selection(set -1, selection in 1,2,3)
        int ctv = GameManager.Instance.SelectedPlayer;
        var defaultToggle = toggles.Find(x => x.name == ctv.ToString());

        if (defaultToggle != null )
        {
            defaultToggle.isOn = true;
        }
        else
        {
            DebugManager.Instance.Ctv = -1;
        }
    }

    public void OnNextBtnClick()
    {
        var selectedToggle = toggles.Find(x => x.isOn);

        if (selectedToggle == null)
        {
            return;
        }
        //GameManager.Instance.SelectedCTV = int.Parse( selectedToggle.name);
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<SelectGenderPage>();
        toggleReset();
    }

    void toggleReset()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
        }
    }
}
