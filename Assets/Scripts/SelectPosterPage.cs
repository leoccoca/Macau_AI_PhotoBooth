using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPosterPage : UIPage
{
    [SerializeField] List<PosterTemplate> posterTemplates;
    [SerializeField] ToggleGroup toggleGroup;

    [SerializeField] Button backBtn;
    [SerializeField] Button nextBtn;

    // Start is called before the first frame update
    void Start()
    {
        backBtn.onClick.AddListener(OnBackBtnClick);
        nextBtn.onClick.AddListener(OnNextBtnClick);
    }

    void OnDestroy()
    {
        backBtn.onClick.RemoveListener(OnBackBtnClick);
        nextBtn.onClick.RemoveListener(OnNextBtnClick);
    }

    public override void OpenPage()
    {
        base.OpenPage();
        base.ResetToggles(toggleGroup);

        string selectedGender = GameManager.Instance.SelectedGender;
        int selectedSpot = GameManager.Instance.SelectedSpot;
        List<PosterInfo> filteredPosters = GameManager.Instance.PosterInfos.FindAll(x => x.Category == selectedSpot);
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

    public void OnNextBtnClick()
    {
        var selectedToggle = posterTemplates.Find(x => x.PosterToggle.isOn);

        if (selectedToggle == null)
        {
            Debug.Log("SelectPosterPage error, cannot find selected toggle.");
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
}
