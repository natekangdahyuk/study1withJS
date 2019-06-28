using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
public class RankModeMainUI : baseUI
{
  
    //[SerializeField]
    //Text[] defaultText;

    //[SerializeField]
    //Text TimeModeAp;

    //[SerializeField]
    //Text Mode2048Ap;

    //[SerializeField]
    //Text TimeModeRank1;

    [SerializeField]
    Text[] ModeRankPoint;

    [SerializeField]
    Text[] ModeRankMy;

    //[SerializeField]
    //Text Mode2048Rank1;

    //[SerializeField]
    //Text Mode2048Rank2;

    public Text PlayerName;

    [SerializeField]
    GameObject[] CurrentDay;

    [SerializeField]
    GameObject[] CurrentDay2;

    RankingUI[] ranking = new RankingUI[4];

    RankModeReadyUI StageReadyUI;

    RankModeStageReferenceData[] RankStage = new RankModeStageReferenceData[4];
    RankModeStageReferenceData Rank2048Stage = null;

    MonsterInfoPopup monsterInfo;
    
    public void Awake()
    {
        StageReadyUI = RankModeReadyUI.Load<RankModeReadyUI>( "SubCanvas" , "RankModeReadyUI" );
        StageReadyUI.OnExit();

        //defaultText[ 0 ].text = StringTBL.GetData( 902086 );
        //defaultText[ 1 ].text = StringTBL.GetData( 902087 );
        //defaultText[ 2 ].text = StringTBL.GetData( 902088 );
        //defaultText[ 3 ].text = StringTBL.GetData( 902089 );
    }

    public override void Init()
    {

    }

    

    public void Apply()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );

        //DateTime current = DateTime.Now;
        //int day = (int)current.DayOfWeek == 0 ? 7 : (int)current.DayOfWeek;

        OnEnter();

        for( int i=0 ; i < 4 ; i++ )
            RankStage[i] = RankModeTBL.GetData( RankingStageManager.I.StageIndex[i] );
        
        //TimeModeAp.text = RankStage.ApCost.ToString();

        PlayerName.text = PlayerData.I.UserID;
        //Mode2048Ap.text = Rank2048Stage.ApCost.ToString();

        //for( int i = 0 ; i < RankingManager.I.MaxRankPoint.Length ; i++ )
        //{
        //    if( RankingManager.I.MaxRankPoint[ i ] == 0 )
        //        ModeRankPoint[ i ].text = "-";
        //    else
        //    {
        //        if(i==2)
        //        {   
        //            if( RankingManager.I.MaxRankPoint[ i ] < 0)
        //            {
        //                ModeRankPoint[ i ].text = "-";
        //            }
        //            else
        //            {                        
        //                ModeRankPoint[ i ].text = UIUtil.GetTimeEx2( RankingManager.I.MaxRankPoint[ i ] );
        //            }
                    
        //        }
        //        else
        //        {
        //            ModeRankPoint[ i ].text = RankingManager.I.MaxRankPoint[ i ].ToString( "n0" ) + " 점";
        //        }
        //    }

        //    if( RankingManager.I.MyRanking[ i ] == 0 )
        //        ModeRankMy[ i ].text = "-";
        //    else
        //        ModeRankMy[ i ].text = RankingManager.I.MyRanking[ i ].ToString( "n0" ) + " 위";
        //}


        for( int i = 0 ; i < CurrentDay.Length ; i++ )
        {
            CurrentDay[ i ].gameObject.SetActive( false );
            CurrentDay2[ i ].gameObject.SetActive( false );
        }

        DateTime reward = DateTime.Today;

        if( reward.DayOfWeek == DayOfWeek.Sunday )
        {
            CurrentDay[ 1 ].gameObject.SetActive( true );
            CurrentDay2[ 1 ].gameObject.SetActive( true );
        }

        if( reward.DayOfWeek == DayOfWeek.Saturday )
        {
            CurrentDay[ 0 ].gameObject.SetActive( true );
            CurrentDay2[ 0 ].gameObject.SetActive( true );
        }

        if( reward.DayOfWeek == DayOfWeek.Thursday )
        {
            CurrentDay[ 2 ].gameObject.SetActive( true );
            CurrentDay2[ 2 ].gameObject.SetActive( true );
        }

        if( reward.DayOfWeek == DayOfWeek.Tuesday )
        {
            CurrentDay[ 3 ].gameObject.SetActive( true );
            CurrentDay2[ 3 ].gameObject.SetActive( true );
        }

        if ( MainScene.Starttype  == MainScene.StartType.Rank2048)
        {
            OnModeRank( (int)RankModeType.Mode2048 );
        }
        else if( MainScene.Starttype == MainScene.StartType.RankTime )
        {
            OnModeRank( (int)RankModeType.TimeLimit );
           
        }
        else if( MainScene.Starttype == MainScene.StartType.Time2048 )
        {
            OnModeRank( (int)RankModeType.Time2048 );
        }
        else if( MainScene.Starttype == MainScene.StartType.TimeDefence )
        {
            OnModeRank( (int)RankModeType.TimeDefence );
        }

        MainScene.Starttype = MainScene.StartType.None;
    }

    public void OnModeStart(int type )
    {
        GlobalUI.I.AddSubSceneBack( this );
        StageReadyUI.Apply( RankStage[type-1] , type );
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnModeRank(int type)
    {
        RefreshRank( (RankModeType)type );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnHelp( int index )
    {
        if( index == (int)RankModeType.TimeLimit )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902087 ) );
        }
        else if( index == (int)RankModeType.Mode2048 )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902089 ) );
        }
        else if( index == (int)RankModeType.Time2048 )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902131 ) );
        }
        else
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902133 ) );
        }
    }
   

    public void RecvRank( int type )
    {
        for( int i=0 ; i < RankingManager.I.MaxRankPoint.Length ; i++ )
        {
            if( type-1 != i )
                continue;

            if( RankingManager.I.MaxRankPoint[ i ] == 0 )
                ModeRankPoint[ i ].text = "-";
            else
            {
                if(i == 2)
                {                    
                    ModeRankPoint[ i ].text = UIUtil.GetTimeEx2( RankingManager.I.MaxRankPoint[ i ] );
                }
                else
                    ModeRankPoint[ i ].text = RankingManager.I.MaxRankPoint[ i ].ToString( "n0" ) + " 점";
            }

            if( RankingManager.I.MyRanking[ i ] == 0 )
                ModeRankMy[ i ].text = "-";
            else
                ModeRankMy[ i ].text = RankingManager.I.MyRanking[ i ].ToString( "n0" ) + " 위";
        }
    }

    void RefreshRank( RankModeType type )
    {
        if( ranking[(int)type -1 ]== null )
        {
            GlobalUI.ShowUI( UI_TYPE.LoadingUI );
            ranking[ (int)type - 1 ] = RankingUI.Load<RankingUI>( "SubCanvas" , "RankModeRankingUI" );
            GlobalUI.CloseUI( UI_TYPE.LoadingUI );                
        }
        ranking[ (int)type -1 ].Apply( type );
       
        GlobalUI.I.AddSubSceneBack( this );
        OnExit();
    }
}