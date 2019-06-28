using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UserLevelUpPopup : baseUI
{
    [SerializeField]
    Text Ap;

    [SerializeField]
    Text Ruby;

    [SerializeField]
    Text Title;

    [SerializeField]
    Text Level;

    public Action action;

    public override void Init()
    {

    }


    public void Apply(int ruby , int ap , int level)
    {
        OnEnter();
        Ap.text = ap.ToString("n0");
        Ruby.text = ruby.ToString( "n0" );
        Level.text = "Lv." + level.ToString();
    }

    public void OnClose()
    {
        if( action != null )
            action();


        OnExit();
    }

    
}