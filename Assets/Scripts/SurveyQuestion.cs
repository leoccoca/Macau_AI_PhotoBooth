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
    [SerializeField] string answerID;

    [SerializeField] Button nextBtn;

    [SerializeField] ToggleGroup toggleGroup;
    [SerializeField] List<Toggle> toggles;


    [SerializeField] bool isMultipleChoice = false;

    private void OnEnable()
    {
        StartCoroutine(ResetToggles(toggleGroup));
        ResetQuestion();
    }

    void Start()
    {
        StartCoroutine(ResetToggles(toggleGroup));
        ResetQuestion();
        if (isMultipleChoice)
        {
            toggleGroup.enabled = false;
            nextBtn.onClick.AddListener(SubmitAnswer);
        }
        else
        {
            toggleGroup.enabled = true;
            toggles.ForEach(t => t.onValueChanged.AddListener(isOn => OnAnswerSelected(t.name)));
        }

    }
    private void OnDestroy()
    {
        if (isMultipleChoice)
        {
            nextBtn.onClick.RemoveListener(SubmitAnswer);
        }
        else
        {
            toggles.ForEach(t => t.onValueChanged.RemoveListener(isOn => OnAnswerSelected(t.name)));
        }
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
        answerID = "-1";
        if (isMultipleChoice)
        {
            nextBtn.gameObject.SetActive(true);
            nextBtn.interactable = true;
        }
        else
        {
            nextBtn.gameObject.SetActive(false);
        }
        //no need nextbtn update
        setToggles(true);
    }


    public void OnAnswerSelected(string ans)
    {
        var selectedToggle = toggles.Find(x => x.isOn);
        
        if (selectedToggle == null)
        {
            return;
        }


        setToggles(false);
        answerID = ans;
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
        if (isMultipleChoice)
        {
            answerID = "";
            var selectedToggles = toggles.Where(x => x.isOn).ToList();

            if (selectedToggles.Count == 0)
            {
                Debug.Log("No Options selected");
                return;
            }
            string ans = "";
            foreach (var toggle in selectedToggles)
            {
                answerID += toggle.gameObject.name+".";
            }
        }

        surveyController.QuestionAnswer(questionID, answerID);
    }
}
