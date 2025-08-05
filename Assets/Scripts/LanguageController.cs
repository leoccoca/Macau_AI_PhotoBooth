using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    public static LanguageController Instance { get; private set; }

    public enum langOptions { en, tc, sc }
    public langOptions currentLanguage = langOptions.en;

    [System.Serializable]
    public class LocalizedObject
    {
        public string key;
        public GameObject enObj;
        public GameObject tcObj;
        public GameObject scObj;
    }

    [SerializeField] List<Toggle> langToggles = new List<Toggle>();
    public Transform HomePagePos, SurveyPos;
    [SerializeField] Transform LangBar;

    public List<LocalizedObject> localizedObjects = new List<LocalizedObject>();

    public static event System.Action OnLanguageChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Multiple LanguageController instances detected. Destroying: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void OnLanguageChange(int lang)
    {
        LanguageController.Instance.ChangeLanguage((LanguageController.langOptions)lang);
    }
    public void ChangeLanguage(langOptions newLang)
    {
        if (currentLanguage == newLang) return;
        Debug.Log("LANG change to:" + newLang);
        currentLanguage = newLang;
        UpdateLangToggle();
        OnLanguageChanged?.Invoke();
        SoundManager.Instance.PlaySfx(SoundFxID.langClick);
    }

    void UpdateLangToggle()
    {
        for (int i = 0; i < langToggles.Count; i++)
        {
            if ((int)currentLanguage == i) {
                langToggles[i].isOn = true;
            }
            else
            {
                langToggles[i].isOn = false;
            }
        }
    }

    public void UpdatePosition(Transform target)
    {
        LangBar.position = target.position;
    }

    public void LanguageBarActive(bool active)
    {
        LangBar.gameObject.SetActive(active);
    }

    private void ApplyLanguage(langOptions lang)
    {
        foreach (var obj in localizedObjects)
        {
            if (obj.enObj != null) obj.enObj.SetActive(lang == langOptions.en);
            if (obj.tcObj != null) obj.tcObj.SetActive(lang == langOptions.tc);
            if (obj.scObj != null) obj.scObj.SetActive(lang == langOptions.sc);
        }

        Debug.Log("Language changed to: " + lang);
    }
}
