using System.Collections.Generic;
using UnityEngine;

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
    public void ChangeLanguage(langOptions newLang)
    {
        if (currentLanguage == newLang) return;

        currentLanguage = newLang;
        OnLanguageChanged?.Invoke();
        //ApplyLanguage(newLang);
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
