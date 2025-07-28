using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGenderPage : UIPage
{
    [SerializeField] List<Toggle> toggles;
    [SerializeField] ToggleGroup toggleGroup;

    [SerializeField] Button nextBtn;
    [SerializeField] Button backBtn;

    // Start is called before the first frame update
    void Start()
    {
        nextBtn.onClick.AddListener(OnNextBtnClick);
        //backBtn.onClick.AddListener(OnBackBtnClick);
    }

    private void OnDestroy()
    {
        nextBtn.onClick.RemoveListener(OnNextBtnClick);
        //backBtn.onClick.AddListener(OnBackBtnClick);
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
        UIManager.Instance.Open<SelectSpot>();
    }

    public void OnBackBtnClick()
    {
        UIManager.Instance.Open<SelectGenderPage>();
    }
}
