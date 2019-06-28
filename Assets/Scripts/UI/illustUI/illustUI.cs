using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public class illustUI : baseUI
{
    [SerializeField]
    GameObjectPool bitPool;

    [SerializeField]
    Transform[] BitGroup;

    [SerializeField]
    GameObjectPool CardGroupPool;

    [SerializeField]
    GameObjectPool CardPool;

    [SerializeField]
    GameObjectPool EmptyCardPool;

    [SerializeField]
    Live2DModel model;

    [SerializeField]
    illustCardInfoUI modelView;

    [SerializeField]
    GameObject  group;

    [SerializeField]
    GameObject Content = null;

    bool bInit = false;

    List<illustCard> CardList = new List<illustCard>();
   
    public void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();

        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    public override void Init()
    {
    }
    public void InitData()
    {
        bInit = true;

        modelView.gameObject.SetActive( false );
        group.SetActive( true );

        float height = 0;

        for( int i = 1 ; i < 11 ; i++ )
        {
            double value = Math.Pow( 2 , i + 1 );
            illustBit bit = bitPool.New().GetComponent<illustBit>();
            bit.bitText.text = value.ToString() + " 비트";

            bit.transform.SetParent( BitGroup[ i - 1 ] );
            bit.gameObject.SetActive( true );
            bit.GetComponent<RectTransform>().anchoredPosition = new Vector2( 20 , -40 );

            //! 카드 그룹
            GameObject go = CardGroupPool.New();
            go.transform.SetParent( BitGroup[ i - 1 ] );
            go.gameObject.SetActive( true );

            
            List<CollectionReferenceData> datalist = CollectionTBL.GetList( (int)value );

            int cardCount = datalist.Count / 5;

            if (datalist.Count % 5 == 0)
                cardCount -= 1;

            go.GetComponent<RectTransform>().anchoredPosition = new Vector2( 20 , -170 - (cardCount * 80 ) );

            for( int z = datalist.Count - 1 ; z >= 0 ; z-- )
            {
                illustCard card = CardPool.New().GetComponent<illustCard>();
                card.transform.SetParent( go.transform );
                card.gameObject.SetActive( true );
                card.OnClick = OnCardClick;
                card.ApplyData( datalist[ z ].characterIndex );
                CardList.Add( card );
            }

            //카드
            int empty = 5 - ( datalist.Count % 5 );

            if( empty == 5 )
                empty = 0;

            for( int z = 0 ; z < empty ; z++ )
            {
                GameObject EmptyCard = EmptyCardPool.New();
                EmptyCard.transform.SetParent( go.transform );
                EmptyCard.gameObject.SetActive( true );
            }

            go.GetComponent<RectTransform>().sizeDelta = new Vector2( 620 , (cardCount + 1 ) * 168 );
            BitGroup[ i - 1 ].GetComponent<RectTransform>().sizeDelta = new Vector2( 620 , go.GetComponent<RectTransform>().sizeDelta.y + 100 );

            height += BitGroup[ i - 1 ].GetComponent<RectTransform>().sizeDelta.y;
        }


        Content.GetComponent<RectTransform>().sizeDelta = new Vector2( Content.GetComponent<RectTransform>().sizeDelta.x , height );
    }

    public void Apply()
    {
		TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        
        topbar.ChangeScene( this );
        OnEnter();

        if( bInit == false )
        {
            InitData();
        }
        else
        {
            for( int i = 0 ; i < CardList.Count ; i++ )
                CardList[ i ].Refresh();
        }

    }


    public void OnCardClick( illustCard card )
    {
        modelView.gameObject.SetActive( true );
        modelView.ApplyInfo(card.cardreferenceData);
        group.SetActive( false );
        model.Apply( SkinTBL.GetData( card.referenceData.DefaultSkin ), card.bGray , card.material.material);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_selectcard" , GameOption.EffectVoluem );
    }

    public void OnExitModel()
    {
        modelView.gameObject.SetActive( false );
        group.SetActive( true );        
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public override bool OnExit()
    {
        if(modelView.gameObject.activeSelf)
        {
            OnExitModel();
            return false;
        }

        return base.OnExit();
    }

  
}