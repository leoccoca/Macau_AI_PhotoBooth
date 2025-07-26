using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGenderPage : UIPage
{
    [SerializeField] List<Toggle> toggles;
    [SerializeField] ToggleGroup toggleGroup;

    [SerializeField] Button backBtn;
    [SerializeField] Button nextBtn;

    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(OnBackBtnClick);
        nextBtn.onClick.AddListener(OnNextBtnClick);
    }

    private void OnDestroy()
    {
        backBtn.onClick.AddListener(OnBackBtnClick);
        nextBtn.onClick.RemoveListener(OnNextBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        string gender = GameManager.Instance.SelectedGender;
        var defaultToggle = toggles.Find(x => x.name == gender);

        if (defaultToggle != null)
        {
            defaultToggle.isOn = true;
        }
        else
        {
            base.ResetToggles(toggleGroup);
        }
    }

    public void OnNextBtnClick()
    {
        var selectedToggle = toggles.Find(x => x.isOn);

        if (selectedToggle == null)
        {
            return;
        }
        GameManager.Instance.SelectedGender = selectedToggle.name;
        UIManager.Instance.Open<SelectPosterPage>();
    }

    public void OnBackBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<SelectCTVPage>();
    }
}
