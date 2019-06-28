using UnityEngine;
using UnityEngine.UI;
using System;

public partial class GameEndUI : baseUI
{
    public GameObject SpecialGroup;
    public GameObject SpecialClearGroup;
    public GameObject SpecialFailedGroup;
    public Text OldScore;

    public Text CurrentScore;

    public Text CurrentGrade;

    public Text GradeChange;

    public GameObject GradeUp;
    public GameObject GradeDown;

    [SerializeField]
    GameObject tipSpecial1;

    [SerializeField]
    GameObject tipSpecial2;

    public void OnRankPage()
    {
        if( GameScene.modeType == ModeType.ModeTimeLimit )
        {
            MainScene.Starttype = MainScene.StartType.RankTime;
        }
        else if( GameScene.modeType == ModeType.Time2048 )
            MainScene.Starttype = MainScene.StartType.Time2048;
        else if( GameScene.modeType == ModeType.TimeDefence )
            MainScene.Starttype = MainScene.StartType.TimeDefence;
        else
            MainScene.Starttype = MainScene.StartType.Rank2048;

        GameExit();
    }

    public void GameEndSpecial( bool bClear, bool isFull )
    {
        SpecialGroup.SetActive( true );
        SpecialClearGroup.SetActive( bClear );
        SpecialFailedGroup.SetActive( !bClear );


        if( GameScene.modeType == ModeType.Time2048)
            OldScore.text = UIUtil.GetTimeEx2( RankingManager.I.OldRankPoint[ (int)GameScene.modeType - 1 ] );
        else
            OldScore.text = RankingManager.I.OldRankPoint[ (int)GameScene.modeType - 1 ].ToString("n0");

        if( GameScene.modeType == ModeType.Time2048 )
            CurrentScore.text = UIUtil.GetTimeEx2( RankingManager.I.CurrentRankingPoint[ (int)GameScene.modeType - 1 ] );
        else
            CurrentScore.text = RankingManager.I.CurrentRankingPoint[ (int)GameScene.modeType - 1 ].ToString( "n0" );
        CurrentGrade.text = "-";
        GradeChange.text = "-";
        GradeUp.SetActive( false );
        GradeDown.SetActive( false );
        ShowUI();


        if( isFull )
        {
            tipSpecial2.SetActive( true );
            tipSpecial1.SetActive( false );
        }
        else
        {
            tipSpecial1.SetActive( true );
            tipSpecial2.SetActive( false );
        }
    }
}