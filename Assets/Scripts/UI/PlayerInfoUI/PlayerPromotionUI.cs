
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerPromotionUI : baseUI
{
    InvenGroupUI invenGroup;

    PromotionCompletePopup popup;

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
    RawImage Star;


    [SerializeField]
    Button PromotionBtn;

    [SerializeField]
    Text PromotionCount;

    [SerializeField]
    Text cost;

    [SerializeField]
    GameObject CardEffect;

    [SerializeField]
    Text MaterialTextEx;

    Card TargetCard = null;

    List<Card> materialList = new List<Card>();
    List<Card> SelectCardList = new List<Card>();

    GradeDataReferenceData data;
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
        popup = PromotionCompletePopup.Load<PromotionCompletePopup>("PopupCanvas", "PromotionCompletePopup");

        MaterialTextEx.text = StringTBL.GetData( 902116 );

    }

    public override void Init()
    {

    }

    public void Apply(Card card)
    {
        Clear();

        data = GradeDataTBL.GetData(card.cardData.Star);

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);
        invenGroup.ApplyInven(card , InvenGroupUI.InvenType.Promotion );

        TargetCard.cardData = null;
        TargetCard.cardData = new CardData();

        TargetCard.cardData.Star = card.cardData.Star;
        TargetCard.cardData.ApplyData(card.cardData.ReferenceIndex, card.cardData.CardKey);
        TargetCard.cardData.SelectSkin( card.cardData.Skin );
        TargetCard.cardData.Copy(card.cardData);
        TargetCard.Apply();

        ApplyInfo(card.cardData);
        cost.text = data.goldCost.ToString();
        //PromotionBtn.enabled = card.cardData.IsPromotion();
        CheckPromotion();
        TargetCard.HideTeamGroup();
        TargetCard.SetLock( false );
        OnEnter();
        MaterialTextEx.gameObject.SetActive( true );
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
            guide = ResourceManager.Load<TutorialItem>(this.gameObject, "TutorialUI_char_promotion");
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

    void Clear()
    {
        for (int i = 0; i < materialList.Count; i++)
        {
            InvenCardObjectPool.Delete(materialList[i].gameObject);
        }
        materialList.Clear();

        for( int i = 0 ; i < SelectCardList.Count ; i++ )
        {
            SelectCardList[ i ].SetSelectCheck( false );
        }
        CardEffect.SetActive( false );
    }


    void ApplyInfo(CardData card)
    {
        CharacterName.text = card.Name;
        CharacterLevel.text = "Lv. " + card.Level.ToString() + " / " + card.MaxLevl.ToString();

        float per = (float)(card.Exp - card.OldMaxExp) / (card.MaxExp - card.OldMaxExp);
        ExpSlider.value = per;
        Exp.text = (per * 100).ToString("F0") + "%";

        UIUtil.LoadStarEx(Star, card.Star);

    }

    void CheckPromotion()
    {        
        PromotionCount.text = materialList.Count.ToString() + "/" + data.materialCount.ToString();

        MaterialTextEx.gameObject.SetActive( materialList.Count == 0 );
    }

    void OnSelectMaterialCard(Card materialCard)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_removecard" , GameOption.EffectVoluem );
        materialCard.SetSelectCheck( false);
        InvenCardObjectPool.Delete(materialCard.gameObject);
        materialList.Remove(materialCard);
        

        Card card = InvenCardObjectPool.Get(materialCard.cardData);
        card.SetSelectCheck( false);
        SelectCardList.Remove( card );
        CheckPromotion();        
    }

    void OnSelectInvenCard(Card card)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );
        if (card.cardData.Lock)
        {
            GlobalUI.ShowOKPupUp("캐릭터가 잠겨 있습니다.");
            return;
        }

        for (int i = 0; i < materialList.Count; i++)
        {
            if (materialList[i].cardData.CardKey == card.cardData.CardKey)
            {
                card.SetSelectCheck( false);
                SelectCardList.Remove( card );
                InvenCardObjectPool.Delete(materialList[i].gameObject);
                materialList.Remove(materialList[i]);
                CheckPromotion();
                return;
            }
        }

        if (data.materialCount <= materialList.Count)
            return;

        card.SetSelectCheck( true);
        SelectCardList.Add( card );
        Card materialCard = InvenCardObjectPool.Get(materialContent.transform);
        materialCard.ApplyData(card.cardData);
        materialCard.OnClick = OnSelectMaterialCard;
        materialList.Add(materialCard);
        CheckPromotion();

        materialContent.transform.DetachChildren();
        for( int i = 0 ; i < materialList.Count ; i++ )
        {
            materialList[ i ].transform.SetParent( materialContent.transform );
        }

    }


    public void OnPromotion()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if (data.materialCount == 0)
            return;
            
        if (materialList.Count < data.materialCount)
        {
            GlobalUI.ShowOKPupUp("승급에 필요한 재료가 부족합니다.");
            return;
        }

        if (PlayerData.I.Gold < data.goldCost)
        {
            GlobalUI.ShowOKPupUp("골드가 부족합니다.");
            return;
        }

        CardEffect.SetActive(true);
        GlobalUI.ShowUI( UI_TYPE.LoadingUIEx );
        Invoke("SendUpgrade", 2f);                
    }


    public void SendUpgrade()
    {
        GlobalUI.CloseUI( UI_TYPE.LoadingUIEx );
        NetManager.SetCardPromotion(TargetCard.cardData.Exp, TargetCard.cardData.Level, materialList, TargetCard.cardData.CardKey, data.goldCost, TargetCard.cardData.Star + 1, TargetCard.cardData.Limit);
    }

    public void RecvComplete(long cardkey, int star, List<long> materialkey, int cost)
    {
        Card card = InvenCardObjectPool.Get(cardkey);

        card.cardData.Star = star;
        card.SetStar( card.cardData );
        card.cardData.ApplyData( card.cardData.ReferenceIndex , cardkey , false);
        
        card.ApplyData( card.cardData );


        for (int i = 0; i < materialkey.Count; i++)
        {
            InventoryManager.I.Remove( materialkey[i]);
        }

        popup.Apply(TargetCard.cardData, card.cardData);

        Apply(card);

        PlayerData.I.UseGold(cost);


    }


}