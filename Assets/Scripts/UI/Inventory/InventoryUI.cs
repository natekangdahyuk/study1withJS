using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;



public partial class InventoryUI : baseUI
{
    public enum ModeType
    {
        Default,
        TeamSetting,
        Sell,
        Combine,
        LeaderSetting,
    }

    DeckUI deckUI;
    PlayerInfoUI infoui;
    InvenGroupUI invenGroup;

    [SerializeField]
    GameObject DefaultModeGroup = null;

    [SerializeField]
    GameObject SellModeGroup = null;

    [SerializeField]
    GameObject InvenGroupParent;

    [SerializeField]
    Text CardCount;

    TopbarUI topbar;
    ModeType currentMode = ModeType.Default;
    
    void Awake()
    {
        deckUI = DeckUI.Load<DeckUI>(this.gameObject, "MainSceneDeckUI");

        infoui = GlobalUI.GetUI<PlayerInfoUI>(UI_TYPE.PlayerInfoUI);

        invenGroup = InvenGroupUI.Load<InvenGroupUI>(InvenGroupParent, "MainSceneInvenGroupUI");
        invenGroup.SelectCard = OnSelectInvenCard;
        infoui.OnExit();
        topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
    }
    public void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();

        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }


    public override void Init()
    {
        deckUI.OnClick = OnSelectDeckCard;
        deckUI.OnteamSetting = OnTeamSetting;
        deckUI.onCompleteDeck = OnTeamSettingComplete;
        deckUI.onLeaderSetting = OnLeader;
        deckUI.onDeckChange = OnDeckChange;
    }

    public void Apply( ModeType type )
    {
        
        topbar.ChangeScene(this);
        OnEnter();
        ChangeMode(type);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ApplyInven();
        deckUI.ApplyDeck(DeckManager.I.GetCurrentDeck());
    }

    public override bool OnExit()
    {
        if (guide)
        {
            if (guide.gameObject.activeSelf)
            {
                guide.OnExit();
                return false;
            }
        }


        invenGroup.OnExit();
        InventoryManager.I.ClearNewCard();
        if (currentMode == ModeType.Default)
        {
            base.OnExit();
            return true;
        }

        ChangeMode(ModeType.Default);
        
        //gameObject.SetActive(false);
        return false;
    }

    TutorialItem guide = null;
    public void OnGuide()
    {
        if (guide == null)
        {
            guide = ResourceManager.Load<TutorialItem>(this.gameObject, "TutorialUI_main_ella");
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

    void ChangePlayerInfoUI(Card card)
    {
        infoui.Apply(card.cardData);
        GlobalUI.I.AddSubSceneBack(this);
        OnExit();
    }
    public void ChangeMode(ModeType type)
    {
        teamSetting.Clear();
        leaderSetting.Clear();
        Sellsetting.Clear();
        currentMode = type;
        SellModeGroup.SetActive( false );
        invenGroup.LeaderSettingMode.SetActive( false );
        switch (type)
        {
            case ModeType.Default:
                DefaultModeGroup.SetActive(true);
                deckUI.Apply(DeckUI.ModeType.Default);
                break;

            case ModeType.TeamSetting:
                DefaultModeGroup.SetActive(false);
                deckUI.Apply(DeckUI.ModeType.TeamSetting);
                teamSetting.Apply(InvenCardObjectPool.I.InvenCard);
                break;

            case ModeType.Sell:
                DefaultModeGroup.SetActive(false);
                SellModeGroup.SetActive( true );
                deckUI.Apply(DeckUI.ModeType.Default);
                deckUI.SetLock( true );
                ApplyInven( InvenGroupUI.InvenType.sell );
                topbar.ShowStone( true );
                break;

            case ModeType.Combine:
                DefaultModeGroup.SetActive(false);
                deckUI.Apply(DeckUI.ModeType.Default);
            
                break;

            case ModeType.LeaderSetting:
                DefaultModeGroup.SetActive(false);
                deckUI.Apply(DeckUI.ModeType.LeaderSetting);
                leaderSetting.Apply(deckUI.deckCardList, deckUI.SelectLeaderList );
                invenGroup.LeaderSettingMode.SetActive( true );
                break;
        }
    }
    public void OnLeader()
    {
        ChangeMode(ModeType.LeaderSetting);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }
    
   

    public void OnTeamSetting()
    {
        ChangeMode(ModeType.TeamSetting);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnSort()
    {

    }

    public void OnExtend()
    {

        if(InventoryManager.I.MaxCardCount >= DefaultDataTBL.GetData(DefaultData.inven_max_value))
        {
            GlobalUI.ShowOKPupUp("더 이상 인벤토리를 확장할 수 없습니다.");
            return;
        }

        if( TopbarUI.CheckCost( DefaultDataTBL.GetData( DefaultData.inven_extend_cost ) ) == false )
            return;

        int count = InventoryManager.I.MaxCardCount + DefaultDataTBL.GetData(DefaultData.inven_extend_value);

        string str = string.Format(StringTBL.GetData(902091), count.ToString());

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>(UI_TYPE.PopupOkCancel);
        popup.OnEnter();
        popup.SetEx(str, DefaultDataTBL.GetData(DefaultData.inven_extend_cost).ToString(), OnExtendOk, null, false, PopupOkCancel.SubType.Ruby);

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }


    public void OnExtendOk()
    {
        NetManager.OnInventoryExtend(InventoryManager.I.MaxCardCount + DefaultDataTBL.GetData(DefaultData.inven_extend_value), DefaultDataTBL.GetData(DefaultData.inven_extend_cost));
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

    }

    public void OnRecvExtend(int count )
    {
        InventoryManager.I.MaxCardCount= count;
        CardCount.text = InventoryManager.I.Invenlist.Count.ToString() + " / " + InventoryManager.I.MaxCardCount.ToString();
    }

    public void OnSellReady()
    {        
        ChangeMode( ModeType.Sell );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnBind()
    {

    }

    public void OnSell()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

        if( Sellsetting.SelectCardList.Count == 0 )
            return;

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();

        int value = 0;
        for( int i = 0 ; i < Sellsetting.SelectCardList.Count ; i++ )
        {
            value += Sellsetting.SelectCardList[ i ].cardData.SellPrice;

            if( Sellsetting.SelectCardList[ i ].cardData.Star >= 4)
            {
                popup.Set( StringTBL.GetData( 900051 ) , SellPopupShow , null , false );
                return;
            }
        }

        popup.SetEx( string.Format(StringTBL.GetData( 900052 ), Sellsetting.SelectCardList.Count.ToString()) , value.ToString() , Sellok , null , false, PopupOkCancel.SubType.stone );
        
    }

    void SellPopupShow()
    {
        int value = 0;
        for( int i = 0 ; i < Sellsetting.SelectCardList.Count ; i++ )
        {
            value += Sellsetting.SelectCardList[ i ].cardData.SellPrice;
        }
        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();
        popup.SetEx( string.Format( StringTBL.GetData( 900052 ) , Sellsetting.SelectCardList.Count.ToString() ) , value.ToString() , Sellok , null , false , PopupOkCancel.SubType.stone );
    }

    void Sellok()
    {
        for( int i = 0 ; i < Sellsetting.SelectCardList.Count ; i++ )
        {
            NetManager.SellCard( Sellsetting.SelectCardList[ i ].cardData.CardKey );
            InventoryManager.I.Remove( Sellsetting.SelectCardList[ i ].cardData.CardKey );
        }

        ChangeMode( ModeType.Default );
        ApplyInven();
        topbar.ShowStone( false );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnSellCancel()
    {
        ChangeMode( ModeType.Default );
        ApplyInven();
        topbar.ShowStone( false );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }
  

}

public partial class InventoryUI : baseUI
{
    TeamSetting teamSetting = new TeamSetting();
    LeaderSetting leaderSetting = new LeaderSetting();
    SellSetting Sellsetting = new SellSetting();

    

    public void OnTeamSettingComplete()
    {
        if(teamSetting.SelectTeamSettingCardList.Count < 10)
        {
            //GlobalUI.ShowOKPupUp("덱 설정이 완료 되지않았습니다.");
            //return;
        }
        NetManager.TeamChange(DeckManager.I.CurrentDeckIndex, teamSetting.SelectTeamSettingCardList);

        
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );


    }

    public void OnRecvTeamChange(int deckIndex , long[] cardkey )
    {
        InventoryManager.I.ChangeDeck(deckIndex-1, cardkey);

        for( int i = 0 ; i < cardkey.Length ; i++ )
        {
            Card card = InvenCardObjectPool.Get( cardkey[i] );
            if( card != null )
            {
                card.SetTeam();
                card.SetLeader( card.cardData );                
            }
        }


        for( int i = 0 ; i < InvenCardObjectPool.I.InvenCard.Count ; i++ )
        {
            if( InvenCardObjectPool.I.InvenCard[ i ] == null )
                continue;

            InvenCardObjectPool.I.InvenCard[ i ].SetTeam();

            if( InvenCardObjectPool.I.InvenCard[i].cardData.IsUse() == false)
            {
                InvenCardObjectPool.I.InvenCard[ i ].cardData.ClearLeader();
            }
        }

        teamSetting.Complete();
        InvenCardObjectPool.SortByBit();
        ChangeMode(ModeType.Default);
        ApplyInven();
    }

    public void OnLeaderChange()
    {
        NetManager.SetLeader(PlayerData.I.UserIndex, true, (byte)DeckManager.I.CurrentDeckIndex , leaderSetting.Leader.cardData.CardKey);
        NetManager.SetLeader(PlayerData.I.UserIndex, false, (byte)DeckManager.I.CurrentDeckIndex, leaderSetting.SubLeader.cardData.CardKey);
                
        OnRecvLeaderChange(DeckManager.I.CurrentDeckIndex, leaderSetting.Leader.cardData.CardKey, leaderSetting.SubLeader.cardData.CardKey);        

    }

    public void OnRecvLeaderChange(int deckIndex, long leaderID, long subleaderID)
    {
        leaderSetting.ChangeLeader(leaderID, subleaderID);
        ChangeMode(ModeType.Default);
        
    }
    void ApplyInven( InvenGroupUI.InvenType type = InvenGroupUI.InvenType.None )
    {
        invenGroup.ApplyInven( type );

        CardCount.text = InventoryManager.I.Invenlist.Count.ToString() + " / " + InventoryManager.I.MaxCardCount.ToString();
    }

    void OnDeckChange()
    {
        
    }
    void OnSelectInvenCard( Card card )
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );

        if( ModeType.Sell != currentMode )
        {
            if( card.cardData.bit == 2 )
            {
                GlobalUI.ShowOKPupUp( StringTBL.GetData( 902127 ) );
                return;
            }
        }
        

        switch (currentMode)
        {
            case ModeType.Default:
                ChangePlayerInfoUI(card);
                break;

            case ModeType.TeamSetting:
                {
                    teamSetting.SelectCard(card);
                    deckUI.ApplyDeck(teamSetting.SelectTeamSettingCardList );
                  
                    
                    //deckUI.SelectCheck( card );
                }
                break;

            case ModeType.Sell:
                {
                    Sellsetting.SelectCard( card );
                }
                break;
            case ModeType.LeaderSetting:
                break;

        }
    }
        
    void OnSelectDeckCard(Card card)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );
        switch (currentMode)
        {
            case ModeType.Default:
                ChangePlayerInfoUI(card);
                break;

            case ModeType.TeamSetting:
                break;

            case ModeType.LeaderSetting:
                {
                    if (leaderSetting.SelectCard(card))
                    {
                        OnLeaderChange();
                    }
                    deckUI.SetLeaderSettingText(StringTBL.GetData(900033));
                }
                break;
        }


    }


    
}