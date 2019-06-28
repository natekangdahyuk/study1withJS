using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Live2D.Cubism.Rendering;

public class SkinPopup : baseUI
{
    [SerializeField]
    GameObject Content;
    
    [SerializeField]
    Text Name;

    [SerializeField]
    Text ability;

    [SerializeField]
    Text desc;

    [SerializeField]
    Text unlockValue;

    [SerializeField]
    Button OnSelectBtn;

    [SerializeField]
    Button UnLockBtn;

    [SerializeField]
    GameObject characterPos;

    List<Card> cardlist = new List<Card>();

    CardData SrcCard = null;
    Card CurrentSelectCard = null;
    public Action< CardData, int> UnLockSkin = null;
    public Action< CardData,int> SelectSkin = null;

    GameObject Live2DModel;
    int currentSkinIndex = 0;
    public void Awake()
    {
        desc.text = StringTBL.GetData( 900055 );
    }

    public override void Init()
    {
        
    }

    public void Exit()
    {
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnUnLock()
    {
        if( PlayerData.I.Cash < CurrentSelectCard.cardData.CurrentSkin.Cost )
        {
            GlobalUI.ShowOKCancelPupUp( StringTBL.GetData( 800014 ) , OnShop );
            return;
        }

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();
        popup.SetEx( StringTBL.GetData( 900057 ) , CurrentSelectCard.cardData.CurrentSkin.Cost.ToString() , OnBuy , null , false , PopupOkCancel.SubType.Ruby );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }


    public void OnBuy()
    {
        UnLockSkin( SrcCard , currentSkinIndex );
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnShop()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.Onshop();
    }

    public void OnSelect()
    {
        if( SrcCard.Skin != currentSkinIndex )
            SelectSkin( SrcCard, currentSkinIndex );
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void Apply( CardData card )
    {
       

        SrcCard = card;
        gameObject.SetActive(true);

        for( int i =0; i < cardlist.Count; i++)
        {
            InvenCardObjectPool.Delete(cardlist[i].gameObject);            
        }
        cardlist.Clear();
        for ( int i =0; i < card.referenceData.SkinList.Length; i++)
        {

            Card TargetCard = InvenCardObjectPool.Get();
            TargetCard.transform.SetParent(Content.transform);
            TargetCard.transform.localPosition = Vector3.zero;
            TargetCard.transform.localScale = Vector3.one;
            TargetCard.cardData = new CardData();
            TargetCard.cardData.ApplyData(card.ReferenceIndex, -1);
            TargetCard.cardData.SelectSkin(card.referenceData.SkinList[i]);
            TargetCard.Apply();
            TargetCard.SetSkinMode();
            TargetCard.OnClick = OnSelectCard;
            cardlist.Add(TargetCard);

            if( card.referenceData.SkinList[i] == card.Skin )
            {
                TargetCard.SetSelect( true );
                SelectCard( TargetCard );
            }
            else
                TargetCard.SetSelect( false );
                        
            if( SrcCard.IsSkin( card.referenceData.SkinList[ i ] ))
            {
                TargetCard.SetSkinLock( false );
            }
            else
                TargetCard.SetSkinLock( true );
        }

        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(cardlist.Count * 115, Content.GetComponent<RectTransform>().sizeDelta.y);
    }

    void OnSelectCard( Card card )
    {
        for( int i = 0 ; i < cardlist.Count ; i++ )
        {
            if( card == cardlist[i])
            {
                cardlist[i].SetSelect( true );
                SelectCard( card );
            }
            else
                cardlist[i].SetSelect( false );
        }
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );
    }

    void SetCharacterImage(CardData card)
    {
        if (Live2DModel != null)
            GameObject.Destroy(Live2DModel.gameObject);

        Live2DModel = ResourceManager.Load(characterPos, card.Live2DModel);

        if (Live2DModel)
        {
            CubismRenderController controller = Live2DModel.GetComponent<CubismRenderController>();

            if (controller)
            {
                controller.DepthOffset = 5;
                controller.SortingOrder = 5;
            }
        }

    }
    void SelectCard( Card card )
    {
        currentSkinIndex =  card.cardData.Skin;
        Name.text = StringTBL.GetData( card.cardData.CurrentSkin.NameString );
        ability.text = StringTBL.GetData( card.cardData.CurrentSkin.ablityString );
        unlockValue.text = card.cardData.CurrentSkin.Cost.ToString( "n0" );
        CurrentSelectCard = card;
        if( SrcCard.IsSkin( card.cardData.Skin) )
        {
            UnLockBtn.gameObject.SetActive( false );
            OnSelectBtn.gameObject.SetActive( true );
        }
        else
        {
            UnLockBtn.gameObject.SetActive( true );
            OnSelectBtn.gameObject.SetActive( false );
        }

        UnLockBtn.interactable = true;

        if( CurrentSelectCard.cardData.CurrentSkin.UnLockType == UnLockType.Level )
        {
            if( CurrentSelectCard.cardData.CurrentSkin.UnLockValue < card.cardData.Level )
                UnLockBtn.interactable = false;
        }
        else if( CurrentSelectCard.cardData.CurrentSkin.UnLockType == UnLockType.Star )
        {
            if( CurrentSelectCard.cardData.CurrentSkin.UnLockValue < card.cardData.Star )
                UnLockBtn.interactable = false;
        }

        SetCharacterImage(card.cardData);
    }



}