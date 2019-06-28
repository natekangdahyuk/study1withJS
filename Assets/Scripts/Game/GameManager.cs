using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using Game;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Wait,
        Play,
        End,
    }


    [SerializeField]
    private Deck deck;

    [SerializeField]
    private GameObject monster;

    [SerializeField]
    public TileManager tileMgr;

    [SerializeField]
    private Animation Warning;

    public StageBase stageBase { private set; get; }


    public Boss boss;
    public Player player = new Player();

    public GameState Gamestate = GameState.Wait;
    public GameObject BossPosition;
    public GameObject BgPosition;

    public int BossHp { get { return boss.Hp; } }
    public int BossMaxHp { get { return boss.MaxHp; } }

    public int MaxComboCount = 0;
    public int MaxHitCount = 0;    
    public int TotalDamage = 0;
    //stageBase.timeInfo

    //! 인게임 로딩 끝남
    public void InGameLoadingEnd()
    {
        stageBase = new StageBase();
        GameObject Bg;

        if( GameScene.modeType == ModeType.ModeDefault )
        {
            boss = Boss.Create( StageManager.I.GetBossDetailData() , BossPosition );
            Bg = ResourceManager.Load( BgPosition , StageManager.I.GetData().StageBackGround );
        }
        else
        {
            boss = Boss.Create( RankingStageManager.I.GetBossDetailData() , BossPosition );
            Bg = ResourceManager.Load( BgPosition , RankingStageManager.I.CurrentData.StageBackGround );
        }


        RectTransform form = Bg.GetComponent<RectTransform>();
        form.anchoredPosition = Vector2.zero;
        form.sizeDelta = Vector2.zero;

        if(GameOption.LowMode == true)
        {
            ParticleSystem[] pa =  Bg.transform.GetComponentsInChildren<ParticleSystem>();

            for( int i = 0 ; i < pa.Length ; i++ )
                pa[ i ].gameObject.SetActive( false );
        }
        


        deck.CreateAllCard( DeckManager.I.GetCurrentDeck() , DeckManager.I.CurrentDeckIndex - 1 );

        player.ApplyData( deck.GetHP() , ItemManager.I.ItemList );

        tileMgr.Initialize();
        tileMgr.CreateNewTileCallback = NewCard;
        tileMgr.UpgradeNewTileCallback = UpgradeNewCard;
        tileMgr.TurnEndCallback = UpdateTurnEnd;

        MaxComboCount = 0;
        MaxHitCount = 0;
        Warning.gameObject.SetActive( false );

        if( GameScene.Instance.MainCamera.aspect <= 0.5f )
        {
            if( GameScene.Instance.MainCamera.aspect < 0.4616 )
                BossPosition.transform.localScale = new Vector3( 1.15f , 1.15f , 1.15f );
            else
                BossPosition.transform.localScale = new Vector3( 1.1f , 1.1f , 1.1f );
        }
        
    }

    //! 덱애님
    public void PlayDeck()
    {
        deck.gameObject.SetActive( true );
        deck.ReadyCallback = GameStart;
        
        deck.PlayStartAnimation();
    }

    public void GameStart()
    {
        if( GameOption.FirstEnter == true )
        {
            GameOption.FirstEnter = false;
            GameOption.SaveOption();
            TutorialItem item = ResourceManager.Load<TutorialItem>( GameObject.Find( "UICanvasTop" ) , "TutorialUI_1-1" );
            item.action = play;
        }
        else
            play();
    }

    void play()
    {
        AttackEvent.I.GameStart();
        Gamestate = GameState.Play;
        stageBase.timeInfo.Reset();
        NewCard( 4 );
        NewCard( 4 );


        boss.GameStart();
        player.GameStart();
        Tile.TileIndex = 0;
    }

    public void ReStart()
    {
        boss.Reset();
        deck.gameObject.SetActive( false );
        Warning.gameObject.SetActive( false );
        Reset();
    }

    public void Reset()
    {
        monster.transform.position = new Vector3( 0 , -500 , 0 );
        monster.SetActive( false );
        tileMgr.Reset();
        Deck.Instance.Reset();
        ResetStageInfo();
    }

    private void UpgradeNewCard(int value, Position TilePosition )
    {
        if(  Tile.MaxValue < value )//! 2048 끼리 합쳐졌을때..
        {
            return;
        }
        Deck.Instance.SetCurrentCard(value, stageBase.currTurn);
        Tile tile = tileMgr.UpgradeTile(deck.GetCardPosition(value), TilePosition, value, tileMgr.TurnCount );
        AttackEvent.I.AddAttackList(tileMgr.TurnCount, tile);        
    }

    private void NewCard(int value)
    {
        Tile tile = tileMgr.NewTile(deck.GetCardPosition(value));

        if(tile)
            AttackEvent.I.AddAttackList( tileMgr.TurnCount , tile );

        Deck.Instance.SetCurrentCard( value , stageBase.currTurn );

    }
    
    public bool IsGameEnd( out bool bwin )
    {
        bwin = true;
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            if( tileMgr.isFull && !tileMgr.isCombinable && tileMgr.IsUpgrade() == false )
            {
                Gamestate = GameState.End;
                bwin = false;
                return true;
            }

            if( player.Hp <= 0 )
            {
                Gamestate = GameState.End;
                bwin = false;
                return true;
            }

            if( boss.Hp <= 0 )
            {
                Gamestate = GameState.End;
                return true;
            }
        }
        else if( GameScene.modeType == ModeType.Mode2048 )
        {
            if( player.Hp <= 0 ||( tileMgr.isFull && !tileMgr.isCombinable && tileMgr.IsUpgrade() == false ))
            {
                Gamestate = GameState.End;
                bwin = false;
                return true;
            }
            else if( deck.CurrentCardIndex >= 2048)
            {
                Gamestate = GameState.End;
                return true;
            }
        }
        else if( GameScene.modeType == ModeType.Time2048 )
        {
            if( player.Hp <= 0 || ( tileMgr.isFull && !tileMgr.isCombinable && tileMgr.IsUpgrade() == false ) )
            {
                Gamestate = GameState.End;
                bwin = false;
                return true;
            }
            else if( deck.CurrentCardIndex >= 2048 )
            {
                Gamestate = GameState.End;
                return true;
            }
        }
        else if( GameScene.modeType == ModeType.TimeDefence )
        {
            if( stageBase.timeInfo.currTime <= 0)
            {
                Gamestate = GameState.End;                
                return true;
            }            
            else if( player.Hp <= 0 || ( tileMgr.isFull && !tileMgr.isCombinable && tileMgr.IsUpgrade() == false ) )
            {
                Gamestate = GameState.End;
                bwin = false;
                return true;
            }
        }
        else
        {
            if( stageBase.timeInfo.currTime <= 0 || (tileMgr.isFull && !tileMgr.isCombinable && tileMgr.IsUpgrade() == false))
            {
                Gamestate = GameState.End;
                return true;
            }
        }

        return false;
    }

 
    void UpdateTurnEnd()
    {      
        boss.Attack();

        GameScene.I.TurnEnd();
        //Deck.Instance.TurnEnd( tileMgr.TurnCount );
     
        //player.SetDamage( data.ActionData.AttValue );
        //GameScene.I.BossAttack( data.ActionData.AttValue , player.Hp , player.MaxHp );
    }

    public void BossAttack( int value  , Vector3 pos)
    {
        player.SetDamage( value );

        GameScene.I.BossAttack( value , player.Hp , player.MaxHp , pos );

        if( player.Hp * 100 / player.MaxHp  >= 30 )
        {
            Warning.gameObject.SetActive( false );
        }
        else
            Warning.gameObject.SetActive( true );
    }

    public void Heal( int value )
    {
        player.SetHeal( value );

        if( player.Hp * 100 / player.MaxHp >= 30 )
        {
            Warning.gameObject.SetActive( false );
        }
        else
            Warning.gameObject.SetActive( true );
    }

    public void Attack( int damage , int attackCount , int ComboCount , Vector3 pos , int turn, bool bCritical , int maxAttackerCount, PROPERTY property , int value , string Name )
    {
        if (MaxComboCount < ComboCount)
            MaxComboCount = ComboCount;

        if( MaxHitCount < attackCount )
            MaxHitCount = attackCount;

        if( value == 4 )
        {
            ComboCount = 0;
            attackCount = 0;
        }

        float combo = ( float)ComboTBL.GetDataCombo( ComboCount );
        float hit = ( float)ComboTBL.GetDataHit( attackCount );

     

        damage = damage
                + (int)( damage * ( combo / 100f ) * ( Deck.I.GetLeaderComboBuffValue() / 100f ) )
                + (int)( damage * ( hit / 100f ) );

        damage = (int)( (float)damage * GameScene.I.GameMgr.boss.CheckPropertyValue( property ) );


        TotalDamage += damage;
        boss.SetDamage(damage);
        GameScene.I.PlayerAttack(damage, attackCount, ComboCount, pos, turn, bCritical, maxAttackerCount, Name ,value);
    }

    public void PostUpdate()
    {
        //turnMgr.PostUpdate();
        tileMgr.PostUpdate();

        UpdateStageInfo();
    }

    public void UseItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Combine:
                if (tileMgr.ForceCombine()) return;
                break;

            case ItemType.Upgrade:
                if (tileMgr.RandomUpgrade()) return;
                break;

            case ItemType.Return:
                if (tileMgr.Return()) return;
                break;
        }

        //! 여기까지 오면 에러메시지 띠워주기
        
    }

    private void ResetStageInfo()
    {
        if (null != stageBase)
        {
            stageBase.turnInfo.Reset();
            stageBase.timeInfo.Reset();
        }
        TotalDamage = 0;
    }

    private void UpdateStageInfo()
    {
        if (null != stageBase)
        {
            stageBase.turnInfo.Set(tileMgr.TurnCount);
            stageBase.timeInfo.RunTime();
        }
    }
}