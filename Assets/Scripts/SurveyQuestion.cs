using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveyQuestion : MonoBehaviour
{
    // Start is called before the first frame update
    public Surveypage surveyController;
    [SerializeField] int questionID;
    [SerializeField] int answerID;

    [SerializeField] Button nextBtn;

    [SerializeField] ToggleGroup toggleGroup;


    void Start()
    {
        nextBtn.onClick.AddListener(SubmitAnswer);
        StartCoroutine(ResetToggles());
    }
    private void OnDestroy()
    {

        nextBtn.onClick.RemoveListener(SubmitAnswer);
    }

    IEnumerator ResetToggles()
    {
        yield return null;
        toggleGroup.allowSwitchOff = true;
        toggleGroup.SetAllTogglesOff();
        toggleGroup.allowSwitchOff = false;

        ResetQuestion();
    }

    void ResetQuestion()
    {
        answerID = -1;
        nextBtn.interactable = false;
    }


    public void OnAnswerSelected(int ans)
    {
        answerID = ans;
        nextBtn.interactable = true;
    }

    public void SubmitAnswer()
    {
        surveyController.QuestionAnswer(questionID, answerID);
        Debug.Log("submit"+this.name);
    }
}
