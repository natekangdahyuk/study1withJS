using System;
using UnityEngine;
using UnityEngine.UI;


public class RankRewardPopup : baseUI
{
    public Text Desc;
    public Text Gold;
    public Text Stone;

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

    public void Apply(int type, int gold , int stone , int rank )
    {
        string str = RankModeRewardTBL.GetRewardString( type );        
        Desc.text = String.Format( str, rank.ToString() );
        Gold.text = gold.ToString("n0");
        Stone.text = stone.ToString("n0");
        OnEnter();
    }
}
