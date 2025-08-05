using UnityEngine;
using UnityEngine.UI;

public class LocalizedSpriteHandler : MonoBehaviour
{
    public Sprite sprite_en;
    public Sprite sprite_tc;
    public Sprite sprite_sc;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        LanguageController.OnLanguageChanged += UpdateSprite;
        UpdateSprite();
    }

    void OnDisable()
    {
        LanguageController.OnLanguageChanged -= UpdateSprite;
    }

    private void Start()
    {
        UpdateSprite();
    }
    void UpdateSprite()
    {
        if (image == null) return;

        switch (LanguageController.Instance.currentLanguage)
        {
            case LanguageController.langOptions.en:
                image.sprite = sprite_en;
                break;
            case LanguageController.langOptions.tc:
                image.sprite = sprite_tc;
                break;
            case LanguageController.langOptions.sc:
                image.sprite = sprite_sc;
                break;
        }
    }
}
