using UnityEngine;
using System;



public abstract class baseUI : MonoBehaviour
{
    public abstract void Init();
    //public abstract void SetString();

    public UI_TYPE eType = UI_TYPE.NONE;
    public UI_SUBTYPE eSubType = UI_SUBTYPE.POPUP;

    bool _hold;
    protected bool Hold { get { return _hold; } }

    public static T Load<T>(string parent, string name) where T : MonoBehaviour
    {
        T ui = ResourceManager.Load<T>(GameObject.Find(parent), name);

        ui.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        ui.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ui.transform.localScale = Vector3.one;
        return ui;
    }

    public static T Load<T>(GameObject parent, string name) where T : MonoBehaviour
    {
        T ui = ResourceManager.Load<T>(parent, name);

        ui.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        ui.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ui.transform.localScale = Vector3.one;
        return ui;
    }

    public void SetHold(bool hold)
    {
        _hold = hold;
    }

    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
        Init();
    }

    public virtual bool OnExit()
    {
        if (_hold == true) return true;

        gameObject.SetActive(false);

        return true;
    }

    void OnDisable()
    {
        if (eSubType == UI_SUBTYPE.POPUP)
        {
            if (GlobalUI.I)
                GlobalUI.I.DeleteBack(this);
        }
    }

    public void OnEnable()
    {
        if (eSubType == UI_SUBTYPE.POPUP)
            GlobalUI.I.AddBack(this);
    }

    public bool CheckLanguageChange()
    {
        //if (bCurrentLang == LanguageManger.I.lang)
        //	return false;

        //bCurrentLang = LanguageManger.I.lang;
        return true;
    }

}

