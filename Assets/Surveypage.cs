using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Surveypage : UIPage
{


    static Surveypage instance;
    public static Surveypage Instance => instance;
    [SerializeField] List<SurveyQuestion> questions;


    [SerializeField] string SurveyRecord;
    [SerializeField] int currentQuestion;

    public bool SurveyCompleted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("Multiple singleton object created, gameobject: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public override void OpenPage()
    {
        base.OpenPage();
        LanguageController.Instance.UpdatePosition(LanguageController.Instance.SurveyPos);
        LanguageController.Instance.LanguageBarActive(true);
        SurveyCompleted = false;
        ResetSurvey();

        GameManager.Instance.IsShowHomeBtn = true;
        NextQuestion(currentQuestion);
    }

    void ResetSurvey()
    {
        currentQuestion = 0;
        SurveyRecord = string.Empty;
        foreach (var q in questions) {
            q.surveyController = this;
            q.gameObject.SetActive(false);
        }

    }
    void NextQuestion()
    {
        questions[currentQuestion].gameObject.SetActive(false);

        currentQuestion += 1;
        questions[currentQuestion].gameObject.SetActive(true);

    }

    void NextQuestion(int question)
    {
        questions[question].gameObject.SetActive(true);
    }

    public void QuestionAnswer(int questionID, string answerID)
    {
        string questionans = questionID + "-" + answerID;
        if (questionID < questions.Count )
        {
            SurveyRecord += questionans + ",";
            NextQuestion();
        }
        else
        {
            SurveyRecord+= questionans;
            SurveySubmit();
        }
    }


    public void SurveySubmit()
    {
        Debug.Log("SurveyComplete:"+SurveyRecord);
        SurveyCompleted = true;
        GameManager.Instance.SurveyRecord = SurveyRecord;
        GameManager.Instance.UpdateSurveyRecord();
        Type nextPage = GameManager.Instance.AfterSurveyPage;
        if (nextPage!=null)
        {
            Debug.Log("SurveyComplete:" + nextPage);

            MethodInfo method = typeof(UIManager).GetMethod("Open", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            MethodInfo genericMethod = method.MakeGenericMethod(nextPage);
            genericMethod.Invoke(UIManager.Instance, null);
        }
        else
        {
            UIManager.Instance.Open<LoadingPage>();
        }

    }
}
