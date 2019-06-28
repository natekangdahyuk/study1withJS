using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : baseUI
{

    public Image Progress;

    public Text text;


    public override void Init()
    {
        if (text != null)
        {
            text.text = LoadingStringTBL.GetRandomData();
        }

    }

    public void SetProgress(float value)
    {
        Progress.fillAmount = value;
    }
}

