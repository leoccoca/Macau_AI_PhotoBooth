using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPage : UIPage
{
    public override void OpenPage()
    {
        base.OpenPage();

        GameManager.Instance.IsShowHomeBtn = false;
    }
}
