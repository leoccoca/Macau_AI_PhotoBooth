using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    [SerializeField] List<UIPage> pages;

    public static UIManager Instance => instance;

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
        HideAllPage();
    }

    private void HideAllPage()
    {
        foreach (UIPage page in pages)
        {
            page.ClosePage();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Open<HomePage>();
        // Open<LoadingPage>();
    }

    public T Open<T>() where T : UIPage
    {
        for (int i = 0; i < pages.Count; i++)
        {
            T page = pages[i] as T;
            if (page != null)
            {
                Open(page);
                return page;
            }
        }
        return null;
    }

    public void Open(UIPage page)
    {
        if (page == null)
        {
            return;
        }

        int idx = pages.IndexOf(page);

        if (idx == -1)
        {
            Debug.LogError("UIManager: Cannot find page: " + page.name);
            return;
        }

        HideAllPage();
        page.OpenPage();
        DebugManager.Instance.ViewName = page.name;
    }
}
