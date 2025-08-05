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
        LanguageController.Instance.LanguageBarActive(false);
        GameManager.Instance.IsShowHomeBtn = true; 
        int lastSpot = GameManager.Instance.SelectedSpot;
        var defaultToggle = toggles.Find(x => x.name == lastSpot.ToString());

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
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        GameManager.Instance.SelectedSpot = int.Parse(selectedToggle.name);
        UIManager.Instance.Open<SelectPosterPage>();
    }
    public void OnBackBtnClick()
    {
        SoundManager.Instance.PlaySfx(SoundFxID.buttonClick);
        UIManager.Instance.Open<SelectGenderPage>();
    }
}
