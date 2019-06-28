using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PopupOkCancel : baseUI
{
    public enum SubType
    {
        ap,
        stone,
        gold,
        Ruby,
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
    Action okelegate = null;
    Action Cancelelegate = null;

    public override void Init()
    {

    }

    public void Set(string sDesc, Action ok, Action cancel, bool error)
    {
        DefaultGroup.SetActive(true);
        ExGroup.SetActive(false);
        m_Desc.text = sDesc;
        okelegate = ok;
        Cancelelegate = cancel;
    }

    public void SetEx(string sDesc, string count, Action ok, Action cancel, bool error, SubType subType = SubType.ap)
    {
        DefaultGroup.SetActive(false);
        ExGroup.SetActive(true);
        m_DescEx.text = sDesc;
        okelegate = ok;
        m_Value.text = count;
        Cancelelegate = cancel;

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
        else if (subType == SubType.Ruby)
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

    public void OnOk()
    {
        OnExit();
        if (okelegate != null)
            okelegate();


        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void OnCancel()
    {
        if (Cancelelegate != null)
            Cancelelegate();
        OnExit();

        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }
}