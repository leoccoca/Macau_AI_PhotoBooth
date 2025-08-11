using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SurveyQuestion : MonoBehaviour
{
    // Start is called before the first frame update
    public Surveypage surveyController;
    [SerializeField] int questionID;
    [SerializeField] int answerID;

    [SerializeField] Button nextBtn;

    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] List<Toggle> toggles;

    private void OnEnable()
    {
        StartCoroutine(ResetToggles(toggleGroup));
        ResetQuestion();
    }

    void Start()
    {
        toggles.ForEach(t => t.onValueChanged.AddListener(isOn => OnAnswerSelected(t.name)));
        StartCoroutine(ResetToggles(toggleGroup));
        ResetQuestion();

    }
    private void OnDestroy()
    {
        toggles.ForEach(t => t.onValueChanged.RemoveListener(isOn => OnAnswerSelected(t.name)));
        //nextBtn.onClick.RemoveListener(SubmitAnswer);
    }

    IEnumerator ResetToggles(ToggleGroup toggleGroup)
    {
        yield return null;
        toggleGroup.SetAllTogglesOff();
    }

    /*
    IEnumerator ResetToggles()
    {
        yield return null;
        toggleGroup.allowSwitchOff = true;
        toggleGroup.SetAllTogglesOff();
        toggleGroup.allowSwitchOff = false;

        ResetQuestion();
    }
    */

    void ResetQuestion()
    {
        answerID = -1;
        //nextBtn.interactable = true;
        //no need nextbtn update
        setToggles(true);
        nextBtn.gameObject.SetActive(false);
    }


    public void OnAnswerSelected(string ans)
    {
        var selectedToggle = toggles.Find(x => x.isOn);

        if (selectedToggle == null)
        {
            return;
        }
        setToggles(false);
        answerID = int.Parse(ans);
        //nextBtn.interactable = true;
        StartCoroutine(DelayNextPage());
    }


    void setToggles(bool active)
    {
        foreach (var toggle in toggles)
        {
            toggle.interactable = active;
        }
    }

    IEnumerator DelayNextPage()
    {
        yield return new WaitForSeconds(1);
        SubmitAnswer();
    }



    public void SubmitAnswer()
    {
        surveyController.QuestionAnswer(questionID, answerID);
    }
}
