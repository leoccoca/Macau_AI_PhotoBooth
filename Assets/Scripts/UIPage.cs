using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPage : MonoBehaviour
{
    public virtual void OpenPage()
    {
        gameObject.SetActive(true);
    }

    public virtual void ClosePage()
    {
        gameObject.SetActive(false);
    }

    public virtual void ResetToggles(ToggleGroup toggleGroup)
    {
        StartCoroutine(ForceAllTogglesOff(toggleGroup));
    }

    IEnumerator ForceAllTogglesOff(ToggleGroup toggleGroup)
    {
        yield return null;
        toggleGroup.SetAllTogglesOff();
    }
}
