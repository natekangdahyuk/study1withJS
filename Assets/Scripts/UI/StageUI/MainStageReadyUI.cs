using UnityEngine;
using UnityEngine.UI;
using System;
public partial class MainStageReadyUI : baseUI
{
    [SerializeField]
    GameObject DeckParent;
    DeckUI deckUI;
    PlayerInfoUI infoui;

    [SerializeField]
    DetailStarPopup detailStar;

    [SerializeField]
    CardRewardInfoPopup CardInfoPopup;

    //[SerializeField]
    RawImage CardTexture;

    public RawImage BackGround;

    [SerializeField]
    GameObject CardPosition;

    [SerializeField]
    Text ApCost;
    StageReferenceData stageData;

    Card TargetCard = null;

    Toggle[] toggleDifficulty;

    CardData cardData = new CardData();

    MonsterInfoPopup monsterInfo;
    void Awake()
    {
        deckUI = DeckUI.Load<DeckUI>(DeckParent, "MainSceneDeckUI");
        infoui = GlobalUI.GetUI<PlayerInfoUI>(UI_TYPE.PlayerInfoUI);
        infoui.OnExit();

        RewardTitle.text = StringTBL.GetData(901006);
        GoldTitle.text = StringTBL.GetData(901007);
        ExpTitle.text = StringTBL.GetData(901009);

      
        cardData = new CardData();
    }

    public override void Init()
    {
        deckUI.OnClick = OnSelectDeckCard;
        deckUI.OnteamSetting = OnTeamSetting;
        deckUI.onLeaderSetting = OnTeamSetting;
        deckUI.Apply( DeckUI.ModeType.MainStageReady );
    }
    public void Apply(StageReferenceData data, ThemaReferenceData themaData )
    {
        detailStar.OnExit(); 
        CardInfoPopup.OnExit();

        stageData = data;
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);
        OnEnter();
        
        ApplyBossInfo(data);
        cardData.ApplyData( CardTBL.GetDataByCharacterID( data.CharRewardList[ 0 ] ).ReferenceID , -1 );
        ApCost.text = data.ApCost.ToString();

        if( CardTexture != null )
        {
            GameObject.Destroy( CardTexture );
            CardTexture = null;
        }
        CardTexture = ResourceManager.Load<RawImage>( CardPosition , cardData.texture );
        BackGround.texture = ResourceManager.LoadTexture( themaData.LobbyTexture );

        if (monsterInfo != null)
        {
            monsterInfo.OnExit();
        }

    }

    public void OnTeamSetting()
    {
        InventoryUI Inven = GlobalUI.GetUI<InventoryUI>(UI_TYPE.InventoryUI);
        GlobalUI.I.AddSubSceneBack(this);
        Inven.Apply(InventoryUI.ModeType.Default);
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }
    public void OnStartGame()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_battle_start" , GameOption.EffectVoluem );

        if (StageManager.I.SelectStageIndex == -1)
            return;

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

        if( InventoryManager.I.IsMaxCount() )
        {
            GlobalUI.ShowOKPupUp( "카드 인벤토리가 가득 차서 더 이상 게임을 진행할 수 없습니다." );
            return;
        }


        NetManager.SetShoes(1, stageData.ApCost );
        GlobalUI.ShowUI(UI_TYPE.InGameLoadingUI);

        GameScene.modeType = ModeType.ModeDefault;
        SceneManager.Instance.ChangeScene("GameScene");

        MainScene.Starttype = MainScene.StartType.Field;
        
    }

    void OnSelectDeckCard(Card card)
    {
        infoui.Apply(card.cardData);
        GlobalUI.I.AddSubSceneBack(this);
        OnExit();

    }

    public void OnStarDetail()
    {
        detailStar.Apply(stageData);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnCardRewardButton()
    {
        CardInfoPopup.Apply( stageData );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnMonsterInfo()
    {
        if(monsterInfo == null)
        {
            monsterInfo = ResourceManager.Load<MonsterInfoPopup>(this.gameObject, "MonsterInfoPopup");
        }

        MonsterDetailReferenceData monsterDetailData = MonsterDetailTBL.GetData(stageData.MonsterIndex);
        MonsterReferenceData monsterData = MonsterTBL.GetData(monsterDetailData.MonsterIndex);
        monsterInfo.Apply(monsterData, monsterDetailData);
    }
}

public partial class MainStageReadyUI : baseUI
{
    [SerializeField]
    RawImage Monster;

    [SerializeField]
    RawImage[] MonsterSkill;


    [SerializeField]
    Text StageTitle;

    [SerializeField]
    Text Gold;

    [SerializeField]
    Text Exp;

    [SerializeField]
    Text Lv;

    [SerializeField]
    Text Hp;

    [SerializeField]
    RawImage Rank;

    //[SerializeField]
    //RawImage Attribute;

    [SerializeField]
    Image[] taskStar;

    [SerializeField]
    Text RewardTitle;

    [SerializeField]
    Text GoldTitle;

    [SerializeField]
    Text ExpTitle;

    [SerializeField]
    Text Attribute;

    void ApplyBossInfo( StageReferenceData Stagedata )
    {
        MonsterDetailReferenceData monsterDetailData = MonsterDetailTBL.GetData( Stagedata.MonsterIndex );
        MonsterReferenceData monsterData = MonsterTBL.GetData( monsterDetailData.MonsterIndex );
        Monster.texture = ResourceManager.LoadTexture( monsterData.LobbyTexture );

        StageTitle.text = StringTBL.GetData( Stagedata.StageName );
        Lv.text = monsterDetailData.Level.ToString();
        Hp.text = monsterDetailData.Hp.ToString("n0");
        Gold.text = Stagedata.GoldReward.ToString( "n0" );
        Exp.text = Stagedata.UserExpReward.ToString( "n0" );

        //UIUtil.LoadProperty(Attribute, (PROPERTY)monsterDetailData.property);
        UIUtil.LoadStarEx( Rank , monsterDetailData.Rank );

        Attribute.text = UIUtil.PropertyString( (PROPERTY)monsterDetailData.property );

        int star = StageManager.I.GetStar( stageData.ReferenceID );

        for( int i = 0 ; i < taskStar.Length ; i++ )
        {
            taskStar[ i ].gameObject.SetActive( star > i ? true : false );

        }

        for( int i = 0 ; i < MonsterSkill.Length ; i++ )
            MonsterSkill[ i ].gameObject.SetActive( false );

        for( int i = 0 ; i < monsterDetailData.MobAction.Length ; i++ )
        {
            MonsterActionReferenceData action = MonsterActionTBL.GetData( monsterDetailData.MobAction[ i ] );

            MonsterSkill[ i ].texture = ResourceManager.LoadTexture( action.ActionIcon );
            MonsterSkill[ i ].gameObject.SetActive( true );
        }
            
    }

    TutorialItem guide = null;
    public void OnGuide()
    {
        if (guide == null)
        {
            guide = ResourceManager.Load<TutorialItem>(this.gameObject, "TutorialUI_battle_ready");
            guide.action = HideGuide;
        }

        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.gameObject.SetActive(false);
        guide.gameObject.SetActive(true);
    }

    void HideGuide()
    {
        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.gameObject.SetActive(true);
    }

    public override bool OnExit()
    {
        if(guide)
        { 
            if (guide.gameObject.activeSelf)
            {
                guide.OnExit();
                return false;
            }
        }
        return base.OnExit();
    }
}