using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageController : MonoBehaviour
{
    // Start is called before the first frame update

    static LanguageController instance;

    public enum langOptions
    {
        en,
        tc,
        sc
    }


    public static LanguageController Instance => instance;

    void Awake()
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

    public void ChangeLanguage(LanguageController.langOptions lang)
    {
        GameManager.Instance.selectedLanguage = lang;
        switch (lang)
        {
            case langOptions.en:
                Debug.Log("Language EN");
                break;
            case langOptions.tc:
                Debug.Log("Language TC");
                break;
            case langOptions.sc:
                Debug.Log("Language SC");
                break;
        }
    }
}
