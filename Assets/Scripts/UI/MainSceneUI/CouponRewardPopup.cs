using System;
using UnityEngine;
using UnityEngine.UI;


public class CouponRewardPopup : baseUI
{
    public Text Desc;
    public Text Gold;
    public Text Stone;
    public Text Ruby;

    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;

    }
    public override void Init()
    {

    }

    public void Exit()
    {
        OnExit();
    }

    public void Apply( string desc , int gold , int stone , int ruby )
    {
        Desc.text = desc;
        Gold.text = gold.ToString( "n0" );
        Stone.text = stone.ToString( "n0" );
        Ruby.text = ruby.ToString( "n0" );
        OnEnter();
    }
}
