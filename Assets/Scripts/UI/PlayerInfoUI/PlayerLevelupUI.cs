
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class PlayerLevelupUI : baseUI
{
    InvenGroupUI invenGroup;
    LevelUpCompletePopup popup;

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
    Text costText;

    [SerializeField]
    RawImage Star;

    [SerializeField]
    Text MaterialCount;

    [SerializeField]
    Text MaterialTextEx;

    [SerializeField]
    GameObject CardEffect;

    [SerializeField]
    GameObjectPool levelupBonusPool;
    Card TargetCard = null;

    List<Card> materialList = new List<Card>();
    List<Card> SelectCardList = new List<Card>();

    int currentExp = 0;
    int currentLevel = 0;
    int currentCost = 0;

    int materialMaxCout = 10;

    void Awake()
    {
        invenGroup = InvenGroupUI.Load<InvenGroupUI>(this.gameObject, "UpgradeInvenGroupUI" );
        invenGroup.SelectCard = OnSelectInvenCard;

        popup = LevelUpCompletePopup.Load<LevelUpCompletePopup>("PopupCanvas", "LevelupCompletePopup");

        if (TargetCard == null)
        {
            TargetCard = InvenCardObjectPool.Get();
            TargetCard.transform.SetParent(cardPosition.transform);
            TargetCard.transform.localPosition = Vector3.zero;
            TargetCard.transform.localScale = Vector3.one;
        }
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

        if ( CardEffect.activeSelf)
            return false;

        Clear();
        return base.OnExit();
    }


    TutorialItem guide = null;
    public void OnGuide()
    {
        if (guide == null)
        {
            guide = ResourceManager.Load<TutorialItem>(this.gameObject, "TutorialUI_char_levelup");
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

    public void Apply( Card card )
    {
        Clear();

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);
        invenGroup.ApplyInven(card , InvenGroupUI.InvenType.LevelUp );

        //TargetCard.ApplyData(card.cardData);        

        TargetCard.cardData = null;
        TargetCard.cardData = new CardData();

        TargetCard.cardData.Star = card.cardData.Star;
        TargetCard.cardData.ApplyData(card.cardData.ReferenceIndex, card.cardData.CardKey);
        TargetCard.cardData.SelectSkin( card.cardData.Skin );
        TargetCard.cardData.Copy(card.cardData);
        TargetCard.Apply();
        TargetCard.HideTeamGroup();
        TargetCard.SetLock( false );

        ApplyInfo(card.cardData);
        OnEnter();
        MaterialTextEx.gameObject.SetActive( true );
    }

    void Clear()
    {
        for (int i = 0; i < materialList.Count; i++)
        {
            if( materialList[i].Bonus )
            {
                levelupBonusPool.Delete( materialList[ i ].Bonus );
                materialList[ i ].Bonus = null;
            }
            InvenCardObjectPool.Delete(materialList[i].gameObject);            
        }

        for( int i = 0 ; i < SelectCardList.Count ; i++ )
        {
            SelectCardList[ i ].SetSelectCheck( false );
        }
        SelectCardList.Clear();
        materialList.Clear();
    }

    void ApplyInfo(CardData card)
    {
        CharacterName.text = card.Name;
                
        SetExp(card.Level, card.MaxLevl , card.Exp);        

        UIUtil.LoadStarEx(Star, card.Star);        
    }

    void SetExp(int level, int maxlevel , int exp )
    {
        currentExp = exp;
        currentLevel = level > maxlevel ? maxlevel : level;

        CharacterLevel.text = "Lv. " + currentLevel.ToString() + " / " + maxlevel.ToString();

        //! 
        ExpCharReferencdData expLevelUpData = ExpCharTBL.GetLevelByExp(exp);

        if( expLevelUpData.ReferenceID > maxlevel-1)
        {
            expLevelUpData = ExpCharTBL.GetData(maxlevel-1);
            exp = expLevelUpData.exp;
        }
        
        if(level == maxlevel )
        {
            ExpSlider.value = 0;
            Exp.text = "0%";
        }
        else
        {
            int oldExp = expLevelUpData.ReferenceID == 1 ? 0 : ExpCharTBL.GetData(expLevelUpData.ReferenceID - 1).exp;

            float per = (float)(exp - oldExp) / (expLevelUpData.exp - oldExp);
            ExpSlider.value = per;
            Exp.text = (per * 100).ToString("F0") + "%";
        }
     
        CheckCost();
    }

    void CheckCost()
    {
        currentCost = 0;

        if(TargetCard != null)
        {
            currentCost = currentExp - TargetCard.cardData.Exp;
        }

        MaterialCount.text = materialList.Count.ToString() + "/" + materialMaxCout.ToString();
        costText.text = currentCost.ToString();

        MaterialTextEx.gameObject.SetActive( materialList.Count == 0 );
    }

    void OnSelectMaterialCard(Card materialCard)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_removecard" , GameOption.EffectVoluem );
        RemoveMaterialCard( materialCard );
        Card card = InvenCardObjectPool.Get(materialCard.cardData);
        card.SetSelectCheck( false);
        SelectCardList.Remove( card );

    }

    void RemoveMaterialCard( Card card )
    {
        if( card.Bonus )
        {            
            levelupBonusPool.Delete( card.Bonus );
            card.Bonus = null;
        }
        card.SetSelectCheck( false );
        SelectCardList.Remove( card );
        InvenCardObjectPool.Delete( card.gameObject );
        materialList.Remove( card );

        UpdateContent();
        CheckExp();
    }

    void AddMaterialCard( Card card , CardData cardData )
    {        
        card.ApplyData( cardData );

        if( card.cardData.property == TargetCard.cardData.property )
        {
            GameObject bonus = levelupBonusPool.New();
            card.Bonus = bonus;
            bonus.SetActive( true );
            bonus.transform.SetParent( card.transform );
            bonus.transform.localPosition = Vector3.zero;
        }        
        
        card.OnClick = OnSelectMaterialCard;
        materialList.Add( card );
        CheckExp();
        UpdateContent();
    }

    void OnSelectInvenCard(Card card)
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );
        if( card.cardData.Lock )
        {
            GlobalUI.ShowOKPupUp("캐릭터가 잠겨 있습니다.");
            return;
        }
        for ( int i =0; i < materialList.Count; i++)
        {
            if(materialList[i].cardData.CardKey == card.cardData.CardKey)
            {
                card.SetSelectCheck( false );
                SelectCardList.Remove( card );
                RemoveMaterialCard( materialList[ i ] );                
             
                return;
            }            
        }

        if (currentLevel == TargetCard.cardData.MaxLevl)
            return;

        if( materialList.Count >= materialMaxCout )
            return;

        card.SetSelectCheck( true );
        SelectCardList.Add( card );
        Card materialCard = InvenCardObjectPool.Get(materialContent.transform);
        AddMaterialCard( materialCard , card.cardData );        
        
        
    }


    void UpdateContent()
    {
        materialContent.transform.DetachChildren();
        for( int i = 0 ; i < materialList.Count ; i++ )
        {
            materialList[ i ].transform.SetParent( materialContent.transform );
        }

        if( materialList.Count * 115 > 620 )
            materialContent.GetComponent<RectTransform>().sizeDelta = new Vector2( materialList.Count * 115 , materialContent.GetComponent<RectTransform>().sizeDelta.y );
        else
            materialContent.GetComponent<RectTransform>().sizeDelta = new Vector2( 620 , materialContent.GetComponent<RectTransform>().sizeDelta.y );
    }
    void CheckExp()
    {
        int materialExp = 0;

        for (int i = 0; i < materialList.Count; i++)
        {
            GradeDataReferenceData data = GradeDataTBL.GetData(materialList[i].cardData.Star);
            ExpCharReferencdData expData = ExpCharTBL.GetData(materialList[i].cardData.Level);
            CharacterReferenceData characterData = CharacterTBL.GetData(materialList[i].cardData.CharacterID);

            int attribute = 1;
            if ( materialList[i].cardData.property == TargetCard.cardData.property)
            {
                attribute = (DefaultDataTBL.GetData(DefaultData.ExpBonue) / 100);
            }
            materialExp += ((data.material_Exp * characterData.exp_multiply * attribute ) + expData.material_exp);
        }

        materialExp += TargetCard.cardData.Exp;

        if( materialExp > TargetCard.cardData.MaxLevelExp )
            materialExp = TargetCard.cardData.MaxLevelExp;

        ExpCharReferencdData expLevelUpData = ExpCharTBL.GetLevelByExp(materialExp);

        SetExp(expLevelUpData.ReferenceID, TargetCard.cardData.MaxLevl, materialExp);        
    }

    public void OnLevelUp()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if (materialList.Count == 0 )
        {
            GlobalUI.ShowOKPupUp("강화에 필요한 재료가 부족합니다.");
            return;
        }

        if (PlayerData.I.Gold < currentCost)
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
        NetManager.SetCardUpgrade(currentExp, currentLevel, materialList, TargetCard.cardData.CardKey, currentCost, TargetCard.cardData.Star, TargetCard.cardData.Limit);
    }

    public void RecvComplete( long cardkey , int exp , int level , List<long> materialkey , int cost )
    {
        CardEffect.SetActive(false);
        Card card = InvenCardObjectPool.Get(cardkey);
        
        card.cardData.Exp = exp;
        card.cardData.Level = level;
        card.ApplyData(card.cardData);

        for ( int i =0; i < materialkey.Count; i++)
        {
            InventoryManager.I.Remove( materialkey[i]);
        }

        popup.Apply(TargetCard.cardData, card.cardData);

        Apply(card);
                
        PlayerData.I.UseGold(cost);

        
    }
}