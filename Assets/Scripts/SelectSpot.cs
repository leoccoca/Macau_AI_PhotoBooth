using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSpot : UIPage
{
    [SerializeField] List<Toggle> toggles;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] Button nextBtn;
    [SerializeField] Button backBtn;



    // Start is called before the first frame update
    void Start()
    {
        toggles.ForEach(t => t.onValueChanged.AddListener(OnToggleSelected));

        //nextBtn.onClick.AddListener(OnNextBtnClick);
        //backBtn.onClick.AddListener(OnBackBtnClick);
    }

    private void OnDestroy()
    {
        toggles.ForEach(t => t.onValueChanged.RemoveListener(OnToggleSelected));

        //nextBtn.onClick.RemoveListener(OnNextBtnClick);
        //backBtn.onClick.AddListener(OnBackBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        LanguageController.Instance.LanguageBarActive(false);
        GameManager.Instance.IsShowHomeBtn = true;

        //int lastSpot = GameManager.Instance.SelectedSpot;
        //var defaultToggle = toggles.Find(x => x.name == lastSpot.ToString());
        base.ResetToggles(toggleGroup);
        setToggles(true);
        /*
        if (defaultToggle != null)
        {
            defaultToggle.isOn = true;
        }
        else
        {
            base.ResetToggles(toggleGroup);
        }
        */
    }


    void setToggles(bool active)
    {
        foreach (var toggle in toggles)
        {
            toggle.interactable = active;
        }
    }

    public void OnNextBtnClick()
    {
        
        var selectedToggle = toggles.Find(x => x.isOn);

        if (selectedToggle == null)
        {
            setToggles(true);
            return;
        }

        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        GameManager.Instance.SelectedSpot = int.Parse(selectedToggle.name);
        UIManager.Instance.Open<SelectPosterPage>();
    }


    public void OnToggleSelected(bool valueChange)
    {
        if (valueChange == false) { return; }
        var selectedToggle = toggles.Find(x => x.isOn);

        if (selectedToggle == null)
        {
            return;
        }
        Debug.Log("SportClicked");
        setToggles(false);
        StartCoroutine(DelayNextPage());
    }
    IEnumerator DelayNextPage()
    {
        yield return new WaitForSeconds(1);

        OnNextBtnClick();
    }


    public void OnBackBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<SelectGenderPage>();
    }
}
