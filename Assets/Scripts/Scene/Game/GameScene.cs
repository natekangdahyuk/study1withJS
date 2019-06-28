using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Game;


public enum ModeType
{
    ModeDefault = 0,
    ModeTimeLimit,
    Mode2048,
    Time2048,
    TimeDefence,
    
}

public partial class GameScene : MonoSinglton<GameScene>
{
    public static ModeType modeType = ModeType.ModeDefault;
    public static bool bDebugMode = false;
    [SerializeField]
    public GameManager GameMgr;

    [SerializeField]
    public Camera MainCamera;

    [SerializeField]
    private InGameUI GameUI;

    [SerializeField]
    TweenVolume TweenBGM;

    public GameObject DeathUI;

    private bool bGameEnd = false;

    public override void Constructor()
    {
       
        SceneManager.Create();
        if( modeType == ModeType.ModeDefault )
        {
            SoundManager.New( SoundManager.SoundType.InGameBGM , SoundManager.Instance , StageManager.I.GetData().BGM );
            SoundObject ob = SoundManager.Instance.Play( SoundManager.SoundType.InGameBGM , StageManager.I.GetData().BGM , GameOption.BGMVolume , true );

            TweenBGM.Set( ob );
        }
        else
        {
            
            SoundManager.New( SoundManager.SoundType.InGameBGM , SoundManager.Instance , RankingStageManager.I.CurrentData.StageLobbyBGM );
            SoundObject ob = SoundManager.Instance.Play( SoundManager.SoundType.InGameBGM , RankingStageManager.I.CurrentData.StageLobbyBGM , GameOption.BGMVolume , true );

            TweenBGM.Set( ob );
        }
    }

    public override void ClearAll()
    {

    }

    private void Start()
	{
   
        GameUI.LoadingComplete = InGameLoadingComplete;                        
        

        if( MainCamera.aspect > 0.565 )
            MainCamera.orthographicSize = 7;
        else
        {

            float value = (float)Screen.height / Screen.width * 7f;
            value /= ( 1280f / 720f );
            MainCamera.orthographicSize = value;
        }
    }

    private void Update()
    {
        if( GameUI.IsOptionShow() )
            return;
      
        if (GameMgr.Gamestate != GameManager.GameState.Play)
            return;

        bool bWin = true;
        if( GameMgr.IsGameEnd( out bWin ) )
        {
            if( bWin )
            {
                GameMgr.boss.SetDeath();

                if( GameMgr.boss.Hp <= 0 )
                    DeathUI.SetActive( true );
            }
            else
            {
                ClearGame( bWin );
            }

            TweenBGM.Play();
        }

        GameMgr.PostUpdate();

        GameUI.UpdateTime( GameMgr.stageBase);
    }

    private void InGameLoadingComplete()
    {
        GameMgr.InGameLoadingEnd();

      
        StartGame();
    }

    public void StartGame()
    {
        GameUI.SetBossInfo( GameMgr.boss );
        GameUI.SetBossMaxHP( GameMgr.BossHp );
        GameUI.SetPlayerInfo( GameMgr.player );
        GameUI.PlayStart( StartAnimComplete );
        GameUI.SetStageInfo( GameMgr.stageBase );
    }
    public void StartAnimComplete()
    {   
        GameMgr.PlayDeck();
        GameMgr.boss.OnEnter();
    }

    public void TurnEnd()
    {
        GameUI.TurnEnd();
    }

    public void PlayerAttack( int damage , int count , int comboCount, Vector3 pos , int turn, bool bCritical, int maxAttackerCount , string Name , int bit )
    {
        GameUI.SetPlayerAttack(damage , GameMgr.BossHp, GameMgr.BossMaxHp , count , comboCount, pos ,turn, bCritical , maxAttackerCount, Name, bit );    
    }

    public void PlayerHeal( int value  )
    {
        if( GameMgr.Gamestate != GameManager.GameState.Play )
            return;

        GameMgr.Heal( value );
        GameUI.SetHeal( value , GameMgr.player.Hp , GameMgr.player.MaxHp );
    }

    public void BossAttack( int damage, int hp  , int maxhp , Vector3 pos )
    {
        GameUI.SetBossAttack(damage , hp , maxhp , pos );
    }

	public void RestartGame()
	{
        SceneManager.Instance.ChangeScene( "GameScene" );

        //DeathUI.SetActive( false );
        //bGameEnd = false;
        //bClear = false;
        //GameMgr.ReStart();
        //GameUI.PlayStart( StartAnimComplete );        
        //GameUI.SetStageInfo(GameMgr.stageBase);
        //GameUI.InitBoss();
        //GameUI.SetBossMaxHP(GameMgr.BossHp);        
        //GameUI.SetPlayerInfo(GameMgr.player);

        //if( modeType == ModeType.ModeDefault )
        //{            
        //    SoundObject ob = SoundManager.Instance.Play( SoundManager.SoundType.InGameBGM , StageManager.I.GetData().BGM , GameOption.BGMVolume , true );
        //    TweenBGM.Set( ob );
        //}
        //else
        //{
                        
        //    SoundObject ob = SoundManager.Instance.Play( SoundManager.SoundType.InGameBGM , RankingStageManager.I.CurrentData.StageLobbyBGM , GameOption.BGMVolume , true );
        //    TweenBGM.Set( ob );
        //}

        
    }

    bool bClear = false;
    public void ClearGame( bool bWin = true )
    {
        if( bClear )
            return;

        bClear = true;

        if( modeType == ModeType.ModeDefault )
        {
            if( GameMgr.BossHp <= 0 )
            {               
                StageManager.I.ClearStage( GameMgr.MaxComboCount , GameMgr.MaxHitCount , GameMgr.stageBase.timeInfo.currTime , Deck.Instance.GetCurrentBit );
            }
            else
            {                
                GameUI.GameEnd( false , 0 , 0 , GameMgr.player.Hp > 0 );
            }
        }
        else if( modeType == ModeType.ModeTimeLimit )
        {
            RankingStageManager.I.ClearGame();
            NetManager.SetRankScore( (int)ModeType.ModeTimeLimit , GameMgr.TotalDamage);
        }
        else if( modeType == ModeType.Mode2048)
        {
            if( bWin )
            {
                NetManager.SetRankScore( (int)ModeType.Mode2048 , GameMgr.TotalDamage );
                RankingStageManager.I.ClearGame();                
            }
            else
            {
                RankingStageManager.I.ClearGame();
                GameUI.GameEndSpecial( false , GameMgr.player.Hp > 0 );
            }
        }
        else if( modeType == ModeType.Time2048 )
        {
            if( bWin )
            {
                int time = (int)(GameMgr.stageBase.timeInfo.deltaTime * 100);
                NetManager.SetRankScore( (int)ModeType.Time2048 , time );
                RankingStageManager.I.ClearGame();
            }
            else
            {
                RankingStageManager.I.ClearGame();
                GameUI.GameEndSpecial( false , GameMgr.player.Hp > 0 );
            }
        }
        else if( modeType == ModeType.TimeDefence )
        {
            if( bWin )
            {
                NetManager.SetRankScore( (int)ModeType.TimeDefence , GameMgr.TotalDamage );
                RankingStageManager.I.ClearGame();
            }
            else
            {
                RankingStageManager.I.ClearGame();
                GameUI.GameEndSpecial( false , GameMgr.player.Hp > 0 );
            }
        }
    }

    public void ShowGameEnd( int ocard , int oUidx )
    {
        GameMgr.Reset();
        GameUI.GameEnd( GameMgr.BossHp > 0 ? false : true , ocard , oUidx,false );
    }

    
    public void ShowStarRewardPopup(  int invokeTime )
    {
        Invoke( "ShowStarRewardPopup" , invokeTime );    
    }

    public void ShowStarRewardPopup()
    {
        if( NetReceive.CurrentRewardValue > 0 )
        {
            PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
            popup.OnEnter();

            if( NetReceive.CurrentRewardType == 1 )
                popup.SetEx( StringTBL.GetData( 850022 ) , NetReceive.CurrentRewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.gold );
            else if( NetReceive.CurrentRewardType == 2 )
                popup.SetEx( StringTBL.GetData( 850022 ) , NetReceive.CurrentRewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.ruby );
            else if( NetReceive.CurrentRewardType == 3 )
                popup.SetEx( StringTBL.GetData( 850022 ) , NetReceive.CurrentRewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.ap );
            else
                popup.SetEx( StringTBL.GetData( 850022 ) , NetReceive.CurrentRewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.stone );
        }
    }


    public void ShowGameEndSpecial()
    {
        GameUI.GameEndSpecial();
    }
    public void UseItem( ItemType type )
    {
        GameMgr.UseItem(type);
    }

    public void OnBack()
    {
        if (GameMgr.Gamestate == GameManager.GameState.End)
            return;

        GameUI.OnBack();
    }
    public void GameExit()
    {
        SoundManager.Instance.Clear( SoundManager.SoundType.Effect );
        SoundManager.Instance.Clear( SoundManager.SoundType.InGameBGM );
        SceneManager.Instance.ChangeScene("MainScene");        
    }
}
