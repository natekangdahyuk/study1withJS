using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Live2D.Cubism.Rendering;

public partial class PlayerInfoUI : baseUI
{
    [SerializeField]
    Text CharacterName;

    [SerializeField]
    Text CharacterLevel;

    [SerializeField]
    Text CharacterMaxLevel;

    [SerializeField]
    Text CharacterLimitLevel;

    [SerializeField]
    RawImage CharacterImage;

    [SerializeField]
    RawImage Star;

    [SerializeField]
    RawImage Property;

    [SerializeField]
    RawImage Class;

    [SerializeField]
    Slider ExpSlider;

    [SerializeField]
    Text Exp;

    [SerializeField]
    Text bit;

    [SerializeField]
    Text Hp;

    [SerializeField]
    Text Defence;

    [SerializeField]
    Text Attack;

    [SerializeField]
    Text Critical;

    [SerializeField]
    Text Heal;

    [SerializeField]
    Text LeaderBuff;

    [SerializeField]
    Text LockText;

    [SerializeField]
    Text ClassText;

    [SerializeField]
    Text[] SkinAbility;

    [SerializeField]
    Text OneWord;

    [SerializeField]
    Text CharCode;

    [SerializeField]
    Button Limit;

    [SerializeField]
    Button Promition;

    [SerializeField]
    GameObject InvenContent;

    [SerializeField]
    GameObject BGParent;

    [SerializeField]
    GameObject BGEffectParent;

    [SerializeField]
    SkinPopup skinPopup;

    [SerializeField]
    Toggle toggleLock;

    [SerializeField]
    Animation TouchAnim;

    [SerializeField]
    GameObject CharacterPosition;

    [SerializeField]
    Button RepresentBtn;

    [SerializeField]
    Text TeamInfo;

    [SerializeField]
    GameObject[] hideGroup;

    [SerializeField]
    GameObject uiShow;

    [SerializeField]
    Button PropertyBtn;

    public PlayerInfoDetail Detail;

    public ScrollController scroll;

    GameObject Live2DBG;
    GameObject Live2DModel;
    GameObject Live2DFx;
    PlayerLevelupUI levelupUI = null;
    PlayerPromotionUI promotionUI = null;
    PlayerLimitUI limitUI = null;

    Card CurrentCard = null;


    void Awake()
    {
        skinPopup.UnLockSkin = OnUnLockSkin;
        skinPopup.SelectSkin = OnSelectSkin;
        scroll.actoin = OnNextCard;
        scroll.actoin2 = OnCharacterTouch;
    }

    private void Start()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
    }

    public override void Init()
    {

    }


    public void Apply(CardData card)
    {
        
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);

        CurrentCard = InvenCardObjectPool.Get(card);

        OnEnter();
        ApplyInfo(card);
        transform.SetSiblingIndex(10);

        Detail.gameObject.SetActive(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (CurrentCard != null)
            ApplyInven(CurrentCard);

        skinPopup.OnExit();
        uiShow.SetActive( false );
    }

    public override bool OnExit()
    {        
        if( uiShow.gameObject.activeSelf)
        {
            OnShowUI();
            return false;
        }
        
        return base.OnExit();
    }

   


    void ApplyInfo(CardData card)
    {
        CharacterName.text = card.Name;
        CharacterLevel.text = "Lv. " + card.Level.ToString() + " / ";
        CharacterMaxLevel.text = card.MaxLevl.ToString();

        CharacterLimitLevel.gameObject.SetActive(card.Limit == 0 ? false : true);

        CharacterLimitLevel.text = "+"+card.Limit.ToString();

        float per = (float)(card.Exp - card.OldMaxExp) / (card.MaxExp - card.OldMaxExp);
        ExpSlider.value = per;
        Exp.text = (per * 100).ToString("F0") + "%";
        bit.text = card.bit.ToString();

        if( card.bBest )
            bit.color = new Color( 1f , 1f , 75f / 255f , 1f );
        else
            bit.color = Color.white;




        LockText.text = card.Lock ? StringTBL.GetData( 902021 ) : StringTBL.GetData( 900025 );
        ClassText.text = UIUtil.ClassString(card.Class) + "/" + UIUtil.PropertyString(card.property);
        LeaderBuff.text = UIUtil.LeaderBuffString(card.leaderBuff, card.leaderBuffValue , card.property , card.Class );
        OneWord.text = card.referenceData.OneWord;

        //Limit.enabled = card.IsLimit() ? true : false;
        Promition.interactable = card.IsPromotion() ? true : false;
        
        UIUtil.LoadProperty(Property, card.property);
        UIUtil.LoadClass(Class, card.Class);
        UIUtil.LoadStarEx( Star, card.Star);

        if (toggleLock != null)
            toggleLock.isOn = card.Lock;


        int count = 0;
        TeamInfo.text = "";
        for( int i =0 ; i < card.IsUseDeck.Length ; i++)
        {
            if( card.IsUseDeck[ i ] == true )
            {
                if( count == 0)
                    TeamInfo.text += ( i + 1 ).ToString();
                else
                    TeamInfo.text += " , "+( i + 1 ).ToString();

                count++;
            }
        }

        if( count == 0 )
            TeamInfo.text = "-";

        ApplySkin( card );

        
        PropertyBtn.gameObject.SetActive( !card.bBest );
        
        //SoundManager.I.Play(card.Voice, GameOption.VoiceVoluem);

    }

    void ApplySkin(CardData card)
    {
        if( card.TotalHp == 0 )
            Hp.text = "-";
        else
            Hp.text = card.TotalHp.ToString( "n0" );

        if( card.TotalDefence == 0 )
            Defence.text = "-";
        else
            Defence.text = card.TotalDefence.ToString( "n0" );

        Attack.text = card.Totaldamage.ToString( "n0" );

        if( card.Critical == 0 )
            Critical.text = "-";
        else
            Critical.text = card.Critical.ToString( "n0" );

        if( card.TotalHeal == 0 )
            Heal.text = "-";
        else
            Heal.text = card.TotalHeal.ToString( "n0" );

        CharCode.text = card.referenceData.Code_Info;
        if (Live2DBG != null)
            GameObject.Destroy(Live2DBG.gameObject);

        if (Live2DModel != null)
            GameObject.Destroy(Live2DModel.gameObject);

        if (Live2DFx != null)
            GameObject.Destroy(Live2DFx.gameObject);

        Live2DBG = ResourceManager.Load(BGParent, card.Live2DBG);
        Live2DBG.transform.localPosition = new Vector3(0, 0, 1000);
        Live2DModel = ResourceManager.Load( CharacterPosition , card.Live2DModel);

        if (Live2DModel)
        {
            CubismRenderController controller = Live2DModel.GetComponent<CubismRenderController>();

            if (controller)
                controller.DepthOffset = 5;
        }

        for( int i = 0 ; i < SkinAbility.Length ; i++ )
        {            
            int value = card.GetSkinValue( (ABILITYTYPE)i+1 );

            if( value > 0 )
                SkinAbility[ i ].text = "+" + value + "%";
            else
                SkinAbility[ i ].text = "";

        }

        RepresentBtn.interactable = !(PlayerData.I.representIndex == CurrentCard.cardData.CardKey);
        //if( card.CurrentSkin.ablityType != ABILITYTYPE.None && card.CurrentSkin.ablityValue > 0 )
        //{            
        //    SkinAbility[ (int)card.CurrentSkin.ablityType-1 ].text = "+" + card.CurrentSkin.ablityValue + "%";
        //}



        //if( GameOption.LowMode == false )
        {
            string str = card.Live2DBG;

            str = str.Replace( "bg" , "fx" );
            Live2DFx = ResourceManager.Load( BGEffectParent , str );
        }
        
    }

    void ApplyInven(Card CurrentCard)
    {
        int index = 0;
        int count = 0;
        for (int i = 0; i < InvenCardObjectPool.I.InvenCard.Count; i++)
        {
            Card card = InvenCardObjectPool.I.InvenCard[ i ];
            card.SetSelect( false );
            card.SetSelectCheck( false );

            if( InvenCardObjectPool.I.InvenCard[ i ].cardData.bit == 2 )
                continue;

            
            card.transform.SetParent(InvenContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 0f);
            card.OnClick = OnSelectInvenCard;
           
            card.gameObject.SetActive(true);

            if (CurrentCard.cardData == card.cardData)
            {
                card.SetSelect(true);
                index = i;
            }
            count++;
        }


        InvenContent.GetComponent<RectTransform>().sizeDelta = new Vector2( count * 115, InvenContent.GetComponent<RectTransform>().sizeDelta.y);

        index = ( count / 2) - index;

        InvenContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * 115, InvenContent.GetComponent<RectTransform>().anchoredPosition.y);
    }


    void OnSelectInvenCard(Card card)
    {
        TouchAnim.Stop();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );

        if (CurrentCard == card)
            return;

        if (CurrentCard != null)
            CurrentCard.SetSelect(false);

        CurrentCard = card;
        card.SetSelect(true);
        ApplyInfo(card.cardData);
    }


    public void OnLevelUp()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( CurrentCard.cardData.IsMaxLevel() )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 900045 ) );
            return;
        }

        if (levelupUI == null)
        {
            levelupUI = PlayerLevelupUI.Load<PlayerLevelupUI>("SubCanvas", "PlayerLevelupUI");
        }
        GlobalUI.I.AddSubSceneBack(this);
        levelupUI.Apply(CurrentCard);
        OnExit();
    }

    public void OnPromotion() //! 승급
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if (CurrentCard.cardData.IsPromotion() == false)
        {
            GlobalUI.ShowOKPupUp(StringTBL.GetData(900050));
            return;
        }

        if (CurrentCard.cardData.Star >= 6 )
        {
            GlobalUI.ShowOKPupUp(StringTBL.GetData(900046));
            return;
        }

        if (promotionUI == null)
        {
            promotionUI = PlayerPromotionUI.Load<PlayerPromotionUI>( "SubCanvas" , "PlayerPromotionUI");
        }
        GlobalUI.I.AddSubSceneBack(this);
        promotionUI.Apply(CurrentCard);
        OnExit();
    }

    public void OnLimit() //! 한계돌파
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if (CurrentCard.cardData.IsLimit())
        {
            GlobalUI.ShowOKPupUp(StringTBL.GetData(900047));
            return;
        }

        if (limitUI == null)
        {
            limitUI = PlayerLimitUI.Load<PlayerLimitUI>( "SubCanvas" , "PlayerLimitUI");
        }
        GlobalUI.I.AddSubSceneBack(this);
        limitUI.Apply(CurrentCard);
        OnExit();
    }

    public void OnRepresent()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        PlayerData.I.representIndex = CurrentCard.cardData.CardKey;
        NetManager.represent(CurrentCard.cardData.CardKey, CurrentCard.cardData.ReferenceIndex);
        InventoryManager.I.SetRepresentCharacter();
        RepresentBtn.interactable = !(PlayerData.I.representIndex == CurrentCard.cardData.CardKey);


    }

    public void OnDetail()
    {
        Detail.Apply(CurrentCard.cardData);
    }

    public void OnLock()
    {
        
        if (toggleLock != null)
        {
            if (toggleLock.isOn == CurrentCard.cardData.Lock)
                return;
        }

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        CurrentCard.cardData.Lock = !CurrentCard.cardData.Lock;
        CurrentCard.SetLock(CurrentCard.cardData.Lock);
        NetManager.SetCardLock(CurrentCard.cardData.CardKey, CurrentCard.cardData.Lock ? 1 : 0);
        LockText.text = CurrentCard.cardData.Lock ? StringTBL.GetData( 902021 ) : StringTBL.GetData( 900025 ); //풀림, 잠금
    }

    public void OnSelectSkin(CardData carddata, int skinIndex)
    {
        CurSkinCardData = carddata;
        CurSkinIndex = skinIndex;
        
        NetManager.SetCardSkin( carddata.CardKey , skinIndex );

        //carddata.SelectSkin(skinIndex);
        //ApplySkin(carddata);
        //Card card = InvenCardObjectPool.ChangeSkin(carddata, skinIndex);
        //card.SetSelect(true);
        //SoundManager.I.Play(card.cardData.Voice, GameOption.VoiceVoluem);
    }

    CardData CurSkinCardData;
    int CurSkinIndex;
    public void OnUnLockSkin(CardData carddata, int skinIndex)
    {
        CurSkinCardData = carddata;
        CurSkinIndex = skinIndex;
        //carddata.AddSkin(skinIndex);
        //ApplySkin(carddata);
        //Card card = InvenCardObjectPool.ChangeSkin(carddata, skinIndex);
        //card.SetSelect(true);
        //SoundManager.I.Play(card.cardData.Voice, GameOption.VoiceVoluem);

        NetManager.BuyCardSkin( skinIndex );
    }

    public void RecvSelectSkin()
    {
        CurSkinCardData.AddSkin( CurSkinIndex );
        ApplySkin( CurSkinCardData );
        Card card = InvenCardObjectPool.ChangeSkin( CurSkinCardData , CurSkinIndex );
        card.SetSelect( true );
        SoundManager.I.Play( SoundManager.SoundType.voice , card.cardData.Voice , GameOption.VoiceVoluem );
        OnCharacterAnim();
    }
    public void RecvUnLockSkin()
    {
        NetManager.SetCardSkin( CurSkinCardData.CardKey , CurSkinIndex );
        InventoryManager.I.AddSkin( CurSkinIndex );
    }

    public void OnCostume()
    {
        skinPopup.Apply(CurrentCard.cardData);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        
    }

    SoundObject touchSound = null;

    public void OnNextCard(bool bNext)
    {
        int index = 0;
        for( int i = 0 ; i < InvenCardObjectPool.I.InvenCard.Count ; i++ )
        {
            Card card = InvenCardObjectPool.I.InvenCard[ i ];

            if( CurrentCard == card )
            {
                if( bNext )
                    index= i+1;
                else
                    index = i-1;


                if( index < 0 )
                    index = InvenCardObjectPool.I.InvenCard.Count-1;
                if( index >= InvenCardObjectPool.I.InvenCard.Count )
                    index = 0;

                while( true )
                {
                    if( InvenCardObjectPool.I.InvenCard[ index ] == CurrentCard )
                        break;

                    if(InvenCardObjectPool.I.InvenCard[ index ].cardData.bit != 2 )
                    {
                        break;
                    }

                    if( bNext )
                        index++;
                    else
                        index--;

                    if( index >= InvenCardObjectPool.I.InvenCard.Count )
                        index = 0;

                    if( index < 0 )
                        index = InvenCardObjectPool.I.InvenCard.Count-1;
                }

                OnSelectInvenCard( InvenCardObjectPool.I.InvenCard[ index ] );
                break;
            }
          
        }
    }

    public void OnCharacterAnim()
    {      
        if( CurrentCard.cardData != null )
        {
            if( TouchAnim.isPlaying )
            {
                TouchAnim.Stop();
                TouchAnim.Play();

            }
            else
            {
                TouchAnim.Play();
            }
        }
    }

    public void OnShowUI()
    {
        for( int i = 0 ; i < hideGroup.Length ; i++ )
            hideGroup[ i ].SetActive( true );

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.gameObject.SetActive( true );
        uiShow.SetActive( false );
    }

    public void OnCharacterTouch()
    {
        //if( hideGroup[0].activeSelf == false )
        //{
           

        //    return;
        //}
        OnCharacterAnim();

        if( CurrentCard.cardData != null )
        {
            if( touchSound )
            {
                if( touchSound.IsPlay() )
                    return;
            }

            touchSound = SoundManager.I.Play( SoundManager.SoundType.voice , CurrentCard.cardData.TouchSound , GameOption.VoiceVoluem );

        }
        else
            Debug.LogError( "carddata null" );
    }

    public void OnUIHide()
    {
        for( int i = 0 ; i < hideGroup.Length ; i++ )
            hideGroup[ i ].SetActive( false );

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.gameObject.SetActive( false );
        uiShow.SetActive( true );

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnPropertyChange()
    {
        if( CurrentCard.cardData.Limit < 4)
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902152 ) );
        }
        else
        {
            GlobalUI.ShowOKCancelPupUp( StringTBL.GetData( 902153 ) , OnPropertyChangeOk );
        }
    }

    public void OnPropertyChangeOk()
    {
        NetManager.ChangeProperty( CurrentCard.cardData.CardKey );
        //OnRecvProperty(  11013 , CurrentCard.cardData.CardKey );
    }

    public void OnRecvProperty( int cardid , long cardkey )
    {
        CardData card = InventoryManager.I.Get( cardkey );
        card.Limit = 0;
    
        card.ApplyData( cardid , cardkey , false );
        CurrentCard.ApplyData( card );
        CurrentCard.SetProperyt( card.property );
        CurrentCard.SetSelect( true );
        SummonCompletePopup summon = GlobalUI.GetUI<SummonCompletePopup>( UI_TYPE.SummonCompletePopup );
        summon.Apply( card );

        ApplyInfo( card );
    }

    
    public void RecvLock()
    {
        
    }

    public void RecvPromition()
    {
        
    }

    public void RecvLimit()
    {
        
    }

    public void RecvLevelUpComplete(long cardkey, int exp, int level, List<long> materialkey, int cost)
    {
        
        Card card = InvenCardObjectPool.Get(cardkey);
        
        levelupUI.RecvComplete(cardkey, exp, level,  materialkey, cost);
        ApplyInfo(card.cardData);

        InvenCardObjectPool.SortByBit();
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.Apply();
    }

    public void RecvPromotionComplete(long cardkey, int star, List<long> materialkey, int cost)
    {
        Card card = InvenCardObjectPool.Get(cardkey);

        promotionUI.RecvComplete(cardkey,star, materialkey, cost);
        
        ApplyInfo(card.cardData);

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.Apply();
    }

    public void RecvLimitComplete(long cardkey, int limit, long materialkey, int cost)
    {
        Card card = InvenCardObjectPool.Get(cardkey);

        limitUI.RecvComplete(cardkey, limit, materialkey, cost);

        ApplyInfo(card.cardData);

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.Apply();
    }
}