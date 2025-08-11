using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPosterPage : UIPage
{
    [SerializeField] List<PosterTemplate> posterTemplates;
    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] List<Toggle> toggles;

    [SerializeField] Button backBtn;
    //[SerializeField] Button nextBtn;

    // Start is called before the first frame update
    void Start()
    {
        toggles.ForEach(t => t.onValueChanged.AddListener(OnToggleSelected));
        backBtn.onClick.AddListener(OnBackBtnClick);
        //nextBtn.onClick.AddListener(OnNextBtnClick);
    }

    void OnDestroy()
    {
        toggles.ForEach(t => t.onValueChanged.RemoveListener(OnToggleSelected));
        backBtn.onClick.RemoveListener(OnBackBtnClick);
        //nextBtn.onClick.RemoveListener(OnNextBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        LanguageController.Instance.LanguageBarActive(false);
        GameManager.Instance.IsShowHomeBtn = true;

        base.ResetToggles(toggleGroup);

        //string selectedGender = GameManager.Instance.SelectedGender;
        int selectedSpot = GameManager.Instance.SelectedSpot;
        List<PosterInfo> filteredPosters = GameManager.Instance.PosterInfos.FindAll(x => x.Category == selectedSpot);


        base.ResetToggles(toggleGroup);
        setToggles(true);
        //List<PosterInfo> filteredPosters = GameManager.Instance.PosterInfos.FindAll(x => (x.Gender == selectedGender)&& (x.Category == selectedSpot));
        /*
        if (GameManager.Instance.SelectedPlayer == 1)
        {
            selectedGender = GameManager.Instance.SelectedGender;
            filteredPosters = GameManager.Instance.PosterInfos.FindAll(x => (x.players == 1)&&(x.Gender == selectedGender));
        }
        else if (GameManager.Instance.SelectedPlayer == 2)
        {
            filteredPosters = GameManager.Instance.PosterInfos.FindAll(x => (x.players == 2));
        }
        else
        {
            Debug.LogError("Invalid Players number: "+GameManager.Instance.SelectedPlayer);
        }
        */


        for (int i=0; i<filteredPosters.Count; i++)
        {
            if (i == posterTemplates.Count)
            {
                break;
            }
            posterTemplates[i].Id = filteredPosters[i].Id;
            posterTemplates[i].PosterTexture = filteredPosters[i].Texture;
            
        }

        if (posterTemplates.Count > 0)
        {
            posterTemplates[0].PosterToggle.isOn = true;
        }
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
        var selectedToggle = posterTemplates.Find(x => x.PosterToggle.isOn);

        if (selectedToggle == null)
        {
            Debug.Log("SelectPosterPage error, cannot find selected toggle.");
            //not selected yet
            setToggles(true);

            return;
        }

        int selectedId = selectedToggle.Id;
        GameManager.Instance.AssignSelectedPoster(selectedId);
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<PhotoTakingPage>();
    }

    public void OnBackBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<SelectSpot>();
    }

    public void OnToggleSelected(bool valueChange)
    {
        if(valueChange==false) { return; }
        var selectedToggle = toggles.Find(x => x.isOn);

        if (selectedToggle == null) { return; }

        setToggles(false);
        StartCoroutine(DelayNextPage());
    }
    IEnumerator DelayNextPage()
    {
        yield return new WaitForSeconds(1);

        OnNextBtnClick();
    }
}
