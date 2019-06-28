using UnityEngine;
using UnityEngine.UI;
using System;
public partial class RankModeReadyUI : baseUI
{
    [SerializeField]
    GameObject DeckParent;
      
    [SerializeField]
    Text ApCost;

    [SerializeField]
    Text MyRank1;

    [SerializeField]
    Text MyRank2;

    DeckUI deckUI;
    PlayerInfoUI infoui;


    RankModeStageReferenceData stageData;
    int type = 0;
    MonsterInfoPopup monsterInfo;

    void Awake()
    {
        deckUI = DeckUI.Load<DeckUI>( DeckParent , "MainSceneDeckUI" );
        infoui = GlobalUI.GetUI<PlayerInfoUI>( UI_TYPE.PlayerInfoUI );
        infoui.OnExit();

    }

    public override void Init()
    {
        deckUI.OnClick = OnSelectDeckCard;
        deckUI.OnteamSetting = OnTeamSetting;
        deckUI.onLeaderSetting = OnTeamSetting;
        deckUI.Apply( DeckUI.ModeType.MainStageReady );
    }
    public void Apply( RankModeStageReferenceData data , int _type)
    {
        stageData = data;
        type = _type;
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );
        OnEnter();

        
        ApCost.text = data.ApCost.ToString();
        ApplyBossInfo( data );

        if( RankingManager.I.MaxRankPoint[ type - 1 ] == 0 )
            MyRank1.text = "-";
        else
        {
            if(type == 3)
            {
                MyRank1.text = UIUtil.GetTimeEx2( RankingManager.I.MaxRankPoint[ type - 1 ] );
            }
            else
            {
                MyRank1.text = RankingManager.I.MaxRankPoint[ type - 1 ].ToString( "n0" ) + " 점";
            }
            
        }

        if( RankingManager.I.MyRanking[ type - 1 ] == 0 )
            MyRank2.text = "-";
        else
            MyRank2.text = RankingManager.I.MyRanking[ type - 1 ].ToString( "n0" ) + " 위";

        if (monsterInfo != null)
        {
            monsterInfo.OnExit();
        }
    }

    public void OnTeamSetting()
    {
        InventoryUI Inven = GlobalUI.GetUI<InventoryUI>( UI_TYPE.InventoryUI );
        GlobalUI.I.AddSubSceneBack( this );
        Inven.Apply( InventoryUI.ModeType.Default );
        OnExit();
    }

    public void OnStartGame()
    {

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_battle_start" , GameOption.EffectVoluem );

        if( DeckManager.I.GetCurrentDeck().Count < 10 )
        {
            GlobalUI.ShowOKPupUp( "덱이 적합하지 않습니다." );
            return;
        }

        if( stageData.ApCost > PlayerData.I.shoes )
        {
            GlobalUI.ShowOKPupUp( "행동력이 모자랍니다." );
            return;
        }

        NetManager.SetShoes( 1 , stageData.ApCost );        

        if( type == 1 )
            GameScene.modeType = ModeType.ModeTimeLimit;
        else if(type == 2)
            GameScene.modeType = ModeType.Mode2048;
        else if( type == 3 )
            GameScene.modeType = ModeType.Time2048;
        else
            GameScene.modeType = ModeType.TimeDefence;


        RankingStageManager.I.CurrentData = stageData;


        SceneManager.Instance.ChangeScene( "GameScene" );
    }

    void OnSelectDeckCard( Card card )
    {
        infoui.Apply( card.cardData );
        GlobalUI.I.AddSubSceneBack( this );
        OnExit();
    }

    public void OnMonsterInfo()
    {
        if (monsterInfo == null)
        {
            monsterInfo = ResourceManager.Load<MonsterInfoPopup>(this.gameObject, "MonsterInfoPopup");
        }

        MonsterDetailReferenceData monsterDetailData = MonsterDetailTBL.GetData(stageData.StageMonster[0]);
        MonsterReferenceData monsterData = MonsterTBL.GetData(monsterDetailData.MonsterIndex);
        monsterInfo.Apply(monsterData, monsterDetailData);
    }

}

public partial class RankModeReadyUI : baseUI
{
    [SerializeField]
    RawImage Monster;

    [SerializeField]
    Text StageTitle;

    [SerializeField]
    Text Lv;

    [SerializeField]
    Text Hp;

    [SerializeField]
    RawImage Rank;

    [SerializeField]
    Text Attribute;

    [SerializeField]
    RawImage[] MonsterSkill;

    public RawImage BackGround;

    void ApplyBossInfo( RankModeStageReferenceData Stagedata )
    {
        MonsterDetailReferenceData monsterDetailData = MonsterDetailTBL.GetData( Stagedata.StageMonster[0] );
        MonsterReferenceData monsterData = MonsterTBL.GetData( monsterDetailData.MonsterIndex );
        Monster.texture = ResourceManager.LoadTexture( monsterData.LobbyTexture );
    
        if( Stagedata.ModeType == RankModeType.TimeLimit )
            StageTitle.text = StringTBL.GetData( 902086 );
        else if( Stagedata.ModeType == RankModeType.Time2048 )
            StageTitle.text = StringTBL.GetData( 902130 );
        else if( Stagedata.ModeType == RankModeType.TimeDefence )
            StageTitle.text = StringTBL.GetData( 902132 );
        else
            StageTitle.text = StringTBL.GetData( 902088 );

        Lv.text = monsterDetailData.Level.ToString();
        Hp.text = monsterDetailData.Hp.ToString( "n0" );
        UIUtil.LoadStarEx( Rank , monsterDetailData.Rank );

        Attribute.text = UIUtil.PropertyString( (PROPERTY)monsterDetailData.property );

        BackGround.texture = ResourceManager.LoadTexture( Stagedata.StageLobbyBG );

        for( int i = 0 ; i < MonsterSkill.Length ; i++ )
            MonsterSkill[ i ].gameObject.SetActive( false );

        for( int i = 0 ; i < monsterDetailData.MobAction.Length ; i++ )
        {
            MonsterActionReferenceData action = MonsterActionTBL.GetData( monsterDetailData.MobAction[ i ] );

            MonsterSkill[ i ].texture = ResourceManager.LoadTexture( action.ActionIcon );
            MonsterSkill[ i ].gameObject.SetActive( true );
        }
    }
}