
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerLimitUI : baseUI
{
    InvenGroupUI invenGroup;
    LimitCompletePopup popup;

    [SerializeField]
    GameObject materialContent = null;

    [SerializeField]
    Text CharacterName;

    [SerializeField]
    Text CharacterLevel;

    [SerializeField]
    Slider ExpSlider;

    [SerializeField]
    Text Exp;

    [SerializeField]
    GameObject cardPosition;

    [SerializeField]
    Text cost;


    [SerializeField]
    RawImage Star;

    [SerializeField]
    GameObject CardEffect;

    [SerializeField]
    Text MaterialTextEx;

    Card TargetCard = null;

    Card materialCard;

    Card SelectCard;

    limitbreakReferenceData limit;
    void Awake()
    {
        invenGroup = InvenGroupUI.Load<InvenGroupUI>(this.gameObject, "UpgradeInvenGroupUI" );
        invenGroup.SelectCard = OnSelectInvenCard;

        if (TargetCard == null)
        {
            TargetCard = InvenCardObjectPool.Get();
            TargetCard.transform.SetParent(cardPosition.transform);
            TargetCard.transform.localPosition = Vector3.zero;
            TargetCard.transform.localScale = Vector3.one;
        }


        popup = LimitCompletePopup.Load<LimitCompletePopup>("PopupCanvas", "LimitCompletePopup");
        MaterialTextEx.text = StringTBL.GetData( 902116 );
    }

    public override void Init()
    {

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

        if ( CardEffect.activeSelf )
            return false;


        Clear();

        return base.OnExit();
    }


    TutorialItem guide = null;
    public void OnGuide()
    {
        if (guide == null)
        {
            guide = ResourceManager.Load<TutorialItem>(this.gameObject, "TutorialUI_char_limit");
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


    public void Apply(Card card)
    {
        Clear();

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);
        invenGroup.ApplyInven(card, InvenGroupUI.InvenType.Limit );

        TargetCard.cardData = null;
        TargetCard.cardData = new CardData();

        TargetCard.cardData.Star = card.cardData.Star;
        TargetCard.cardData.ApplyData(card.cardData.ReferenceIndex, card.cardData.CardKey);
        TargetCard.cardData.SelectSkin( card.cardData.Skin );
        TargetCard.cardData.Copy(card.cardData);
        
        TargetCard.Apply();

        
        ApplyInfo(card.cardData);
        TargetCard.HideTeamGroup();
        TargetCard.SetLock( false );
        SetLimit( TargetCard.cardData.Limit + 1 );


        OnEnter();
        MaterialTextEx.gameObject.SetActive( true );
    }


    void SetLimit( int value )
    {
        if( value > 10 )
            value = 10;
        limit = LimitbreakTBL.GetData( value );
        if( limit == null )
            cost.text = "-";
        else
            cost.text = limit.gold_cost.ToString();
    }

    void ApplyInfo(CardData card)
    {
        materialCard = null;

        CharacterName.text = card.Name;
        CharacterLevel.text = "Lv. " + card.Level.ToString() + " / " + card.MaxLevl.ToString();

        float per = (float)(card.Exp - card.OldMaxExp) / (card.MaxExp - card.OldMaxExp);
        ExpSlider.value = per;
        Exp.text = (per * 100).ToString("F0") + "%";

        UIUtil.LoadStarEx(Star, card.Star);

    }

    void Clear()
    {
        if( materialCard )
        {
            InvenCardObjectPool.Delete( materialCard.gameObject );
            materialCard.SetSelectCheck( false );

            if( SelectCard )
                SelectCard.SetSelectCheck( false );
        }

        materialCard = null;
        CardEffect.SetActive( false );
        MaterialTextEx.gameObject.SetActive( true );
    }


    void OnSelectMaterialCard(Card materialCard)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_removecard" , GameOption.EffectVoluem );
        materialCard.SetSelectCheck( false);
        InvenCardObjectPool.Delete(materialCard.gameObject);
        
        Card card = InvenCardObjectPool.Get(materialCard.cardData);
        card.SetSelectCheck( false);
        materialCard = null;

        MaterialTextEx.gameObject.SetActive( true );

        SetLimit( TargetCard.cardData.Limit + 1 );
    }

    void OnSelectInvenCard(Card card)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );
        if (card.cardData.Lock)
        {
            GlobalUI.ShowOKPupUp("캐릭터가 잠겨 있습니다.");
            return;
        }
               
        if( materialCard != null )
        {
            if (materialCard.cardData.CardKey == card.cardData.CardKey)
            {
                card.SetSelectCheck( false);
                SelectCard = null;
                Clear();
                SetLimit( TargetCard.cardData.Limit + 1 );
                return;
            }


            Card oldcard = InvenCardObjectPool.Get(materialCard.cardData.CardKey);
            oldcard.SetSelectCheck( false);
            Clear();
        }
       
        card.SetSelectCheck( true);
        SelectCard = card;
        materialCard = InvenCardObjectPool.Get(materialContent.transform);
        materialCard.ApplyData(card.cardData);
        materialCard.OnClick = OnSelectMaterialCard;
        MaterialTextEx.gameObject.SetActive( false );
        SetLimit( TargetCard.cardData.Limit + materialCard.cardData.Limit + 1 );


    }

    public void OnLimit()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if (materialCard == null )
        {
            GlobalUI.ShowOKPupUp("한계돌파에 필요한 재료가 부족합니다.");
            return;
        }

        if (PlayerData.I.Gold < limit.gold_cost)
        {
            GlobalUI.ShowOKPupUp("골드가 부족합니다.");
            return;
        }

        if( TargetCard.cardData.IsLimit() )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 900047 ) );
            return;
        }


        CardEffect.SetActive(true);
        GlobalUI.ShowUI( UI_TYPE.LoadingUIEx );
        Invoke("SendUpgrade", 2f);
    }

    public void SendUpgrade()
    {
        GlobalUI.CloseUI( UI_TYPE.LoadingUIEx );
        //TargetCard.cardData.Limit = 

        int value = TargetCard.cardData.Limit + materialCard.cardData.Limit + 1;

        if( value > 10 )
            value = 10;

        NetManager.SetCardLimit(TargetCard.cardData.Exp, TargetCard.cardData.Level, materialCard, TargetCard.cardData.CardKey, limit.gold_cost, TargetCard.cardData.Star, value );
    }

    public void RecvComplete(long cardkey, int limit, long materialkey, int cost)
    {
        Card card = InvenCardObjectPool.Get(cardkey);

        card.cardData.SetLimit(limit);
        card.SetLimit(card.cardData);
      
        InventoryManager.I.Remove( materialkey);
       

        popup.Apply(TargetCard.cardData, card.cardData);

        Apply(card);

        PlayerData.I.UseGold(cost);


    }

    public void RecvComplete(CardData card, CardData card2)
    {
        popup.Apply(card, card2);
    }
}