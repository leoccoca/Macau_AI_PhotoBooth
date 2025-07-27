using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectNumberOfPlayer : UIPage
{
    [SerializeField] List<Toggle> toggles;
    [SerializeField] ToggleGroup toggleGroup;
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
        int lastPlayers = GameManager.Instance.SelectedPlayer;
        var defaultToggle = toggles.Find(x => x.name == lastPlayers.ToString());

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
        GameManager.Instance.SelectedPlayer = int.Parse(selectedToggle.name);
        if(GameManager.Instance.SelectedPlayer == 1)
        {
            UIManager.Instance.Open<SelectGenderPage>();
        }
        else
        {
            UIManager.Instance.Open<SelectPosterPage>();
        }
    }
}
