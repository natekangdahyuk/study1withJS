using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PopupOk : baseUI
{
    public enum SubType
    {
        ap,
        stone,
        gold,
        ruby,
        mileage,
    }

    public Text m_Desc;

    public Text m_DescEx;
    public Text m_Value;

    public GameObject DefaultGroup;
    public GameObject ExGroup;

    public GameObject ap;
    public GameObject stone;
    public GameObject gold;
    public GameObject ruby;
    public GameObject mileage;

    Action closedelegate = null;

    public override void Init()
    {

    }


    public void Set(string sDesc, Action gate, bool error)
    {
        DefaultGroup.SetActive(true);
        ExGroup.SetActive(false);
        m_Desc.text = sDesc;
        closedelegate = gate;
    }

    public void SetEx(string sDesc, string count, Action gate, bool error, SubType subType = SubType.ap)
    {
        DefaultGroup.SetActive(false);
        ExGroup.SetActive(true);
        m_DescEx.text = sDesc;
        closedelegate = gate;
        m_Value.text = count;

        if (subType == SubType.ap)
        {
            ap.SetActive(true);
            stone.SetActive(false);
            gold.SetActive(false);
            ruby.SetActive(false);
            mileage.SetActive(false);
        }
        else if (subType == SubType.gold)
        {
            ap.SetActive(false);
            stone.SetActive(false);
            gold.SetActive(true);
            ruby.SetActive(false);
            mileage.SetActive(false);
        }
        else if (subType == SubType.ruby)
        {
            ap.SetActive(false);
            stone.SetActive(false);
            gold.SetActive(false);
            ruby.SetActive(true);
            mileage.SetActive(false);
        }
        else if (subType == SubType.mileage)
        {
            ap.SetActive(false);
            stone.SetActive(false);
            gold.SetActive(false);
            ruby.SetActive(false);
            mileage.SetActive(true);
        }
        else
        {
            ap.SetActive(false);
            stone.SetActive(true);
            gold.SetActive(false);
            ruby.SetActive(false);
            mileage.SetActive(false);
        }

    }

    public void OnClose()
    {
        OnExit();

        if (closedelegate != null)
            closedelegate();

        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

}