using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class GlobalUI : MonoSinglton<GlobalUI>
{
    public static baseUI ShowUI(UI_TYPE eType)
    {
        baseUI UI;
        if (I.Get(eType, out UI) == true)
        {
            UI.OnEnter();
            return UI;
        }

        return null;
    }

    public static T GetUI<T>(UI_TYPE eType) where T : baseUI
    {
        baseUI UI;

        if (GlobalUI.I.Get(eType, out UI))
        {
            return (T)UI;
        }
        return null;
    }

    public static void ShowOKPupUp(string sDesc, Action gate = null, bool error = false)
    {
        baseUI UI;
        if (GlobalUI.I.Get(UI_TYPE.PopupOk, out UI) == false)
            Debug.LogError("showui error");

        if (UI == null)
            Debug.LogError("showui error2");


        UI.OnEnter();
        ((PopupOk)UI).Set(sDesc, gate, error);
    }

    public static void ShowOKCancelPupUp(string sDesc, Action okgate = null, Action Cancelgate = null, bool error = false)
    {
        baseUI UI;
        if (GlobalUI.I.Get(UI_TYPE.PopupOkCancel, out UI) == false)
            Debug.LogError("showui error");

        if (UI == null)
            Debug.LogError("showui error2");


        UI.OnEnter();
        ((PopupOkCancel)UI).Set(sDesc, okgate, Cancelgate, error);
    }


    public static void CloseUI(UI_TYPE eType)
    {
        I._CloseUI(eType);
    }

}

public partial class GlobalUI : MonoSinglton<GlobalUI>
{
    Dictionary<UI_TYPE, baseUI> UIList = new Dictionary<UI_TYPE, baseUI>();
    List<baseUI> BackList = new List<baseUI>();
    public Stack<baseUI> SubSceneBackList = new Stack<baseUI>();
    UILoader loader;

    public override void Constructor()
    {
        base.Constructor();
        Init(new UILoader());
        DontDestroyOnLoad(this.gameObject);
    }

    public void Init(UILoader _loader)
    {
        loader = _loader;
    }

    public override void ClearAll()
    {
        UIList.Clear();
        BackList.Clear();
        SubSceneBackList.Clear();


    }

    public void ClearBackList()
    {
        BackList.Clear();
        SubSceneBackList.Clear();
    }
    baseUI _ShowUI(UI_TYPE eType)
    {
        baseUI UI;
        if (Get(eType, out UI) == false)
            Debug.LogError("showui error");

        if (UI == null)
        {
            Debug.LogError("showui error2 : " + eType);
            return null;
        }

        UI.OnEnter();
        return UI;
    }

    void _CloseUI(UI_TYPE eType)
    {
        baseUI UI;
        if (I.UIList.TryGetValue(eType, out UI) == false)
        {
            return;
        }
        UI.OnExit();
    }


    protected bool Get(UI_TYPE eType, out baseUI UI) //
    {
        if (UIList.TryGetValue(eType, out UI) == false)
        {
            UI = loader.Load(eType);
            UIList.Add(eType, UI);
        }

        if (UI == null)
        {
            UI = loader.Load(eType);
            if (UIList.ContainsKey(eType) == true)
            {
                UIList[eType] = UI;
            }
            else
            {
                UIList.Add(eType, UI);
            }
        }

        if (UI == null)
        {
            Debug.LogError("UI load failed : " + eType);
        }

        return true;
    }

    public void AddSubSceneBack(baseUI _UI)
    {
        SubSceneBackList.Push(_UI);
    }

    public void DeleteSubSceneBack(baseUI _UI)
    {
        SubSceneBackList.Clear();
    }

    public baseUI GetBackSubScene()
    {
        if (SubSceneBackList.Count == 0)
            return null;

        return SubSceneBackList.Pop();
    }

    public void AddBack(baseUI _UI)
    {
        BackList.Add(_UI);
    }

    public void DeleteBack(baseUI _UI)
    {
        BackList.Remove(_UI);
    }

    public bool OnBackButton()
    {
        if (BackList.Count == 0)
        {
            TopbarUI topbar = (TopbarUI)ShowUI(UI_TYPE.TopBarUI);

            if (topbar.CurrentScene == null)
            {
                GlobalUI.ShowOKCancelPupUp(StringTBL.GetData(902182), GameEnd);
            }
            else
                topbar.OnBack();
        }
        else
        {
            BackList[BackList.Count - 1].OnExit();
        }

        return true;
    }

    public bool OnInGameBackButton()
    {
        if (BackList.Count == 0)
        {
            return false;
        }
        else
        {
            BackList[BackList.Count - 1].OnExit();
        }

        return true;
    }

    public void GameEnd()
    {
        Debug.LogError("게임종료");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
        //System.Diagnostics.Process.GetCurrentProcess().Kill();      
#endif
    }

    public static void ShowSpinner()
    {
        ShowUI(UI_TYPE.SpinnerUI);
    }

    public static void HideSpinner()
    {
        CloseUI(UI_TYPE.SpinnerUI);
    }
}