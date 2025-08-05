using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedTextHandler : MonoBehaviour
{
    public List<TMP_Text> textList = new List<TMP_Text>();

    void OnEnable()
    {
        LanguageController.OnLanguageChanged += UpdateTextVisibility;
        UpdateTextVisibility();
    }

    void OnDisable()
    {
        LanguageController.OnLanguageChanged -= UpdateTextVisibility;
    }

    void Start()
    {
        // Auto-fill if empty
        if (textList.Count == 0)
        {
            TMP_Text[] children = GetComponentsInChildren<TMP_Text>(true);
            textList.AddRange(children);
        }

        UpdateTextVisibility();
    }

    void UpdateTextVisibility()
    {
        int langIndex = (int)LanguageController.Instance.currentLanguage;

        for (int i = 0; i < textList.Count; i++)
        {
            if (textList[i] != null)
            {
                textList[i].gameObject.SetActive(i == langIndex);
            }
        }
    }
}
