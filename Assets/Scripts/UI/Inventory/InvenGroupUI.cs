using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class InvenGroupUI : baseUI
{
    public enum InvenType
    {
        None,
        Promotion,
        Limit,
        sell,
        LevelUp,
    }

    [SerializeField]
    GameObject InvenContent = null;

    [SerializeField]
    GameObject[] Bit = null; //! up down

    [SerializeField]
    GameObject[] Grade = null;

    [SerializeField]
    public GameObject LeaderSettingMode = null;
    public int MinHeight = 400;
    public Action<Card> SelectCard;

    bool bFirstEnter = false;

    InvenType currenttype = InvenType.None;

    Card exceptCard = null;
    private void Awake()
    {
        Bit[0].SetActive(false);
        //Grade[0].SetActive(false);
        
    }
    // Use this for initialization
    public override void Init()
    {
              
    }

    public override bool OnExit()
    {
        GameOption.SaveOption();
        //return base.OnExit();
        return true;
    }
    //public override bool OnExit()
    //{
    //    InvenCardObjectPool.PushAll();

    //    return base.OnExit();
    //}

    public void ApplyInven( InvenType type = InvenType.None)
    {
        exceptCard = null;
        currenttype = type;
        if( bFirstEnter == false )
        {
            bFirstEnter = true;
            InvenCardObjectPool.SortByBit();
        }

        for(int i=0 ; i < Bit.Length ; i++ )
        {
            Bit[ i ].SetActive( GameOption.BitSort == i ? true : false );
            //Grade[ i ].SetActive( GameOption.GradeSort == i ? true : false );

        }

        InvenContent.transform.DetachChildren();
        for (int i = 0; i < InvenCardObjectPool.I.InvenCard.Count; i++)
        {
            Card card = InvenCardObjectPool.I.InvenCard[i];

            if( type == InvenType.sell)
            {
                if( card.cardData.IsUse() == true )
                    continue;

                if( card.cardData.Lock == true )
                    continue;
            }
            card.transform.SetParent(InvenContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 0f);
            card.OnClick = OnSelectInvenCard;
            if( type != InvenType.sell )
                card.SetSelect(false);
            card.gameObject.SetActive(true);
            card.SetNewCard();
        }
        int count = Mathf.CeilToInt((float)InvenCardObjectPool.I.InvenCard.Count / 6);

        int height = MinHeight;

        if (count * 168 > height)
            height = count * 168;

        InvenContent.GetComponent<RectTransform>().sizeDelta = new Vector2(InvenContent.GetComponent<RectTransform>().sizeDelta.x, height);        
    }

    public void ApplyInven( Card _exceptCard , InvenType type , bool bSelect = false)
    {
        exceptCard = _exceptCard;
        InvenContent.transform.DetachChildren();
        currenttype = type;
        int count = 0;
        for (int i = 0; i < InvenCardObjectPool.I.InvenCard.Count; i++)
        {
            Card card = InvenCardObjectPool.I.InvenCard[i];

            if (card.cardData.IsUse())
                continue;

            if( card.cardData.CardKey == InventoryManager.I.representCharacter.CardKey )
                continue;

            if (exceptCard == card)
                continue;

            if(type == InvenType.Promotion )
            {
                if( card.cardData.Star != exceptCard.cardData.Star )
                    continue;

                if( card.cardData.bit == 2 )
                    continue;

                if( card.cardData.Lock == true )
                    continue;
                //if (card.cardData.IsMaxLevel() == false)
                //    continue;
            }
            else if (type == InvenType.Limit)
            {
                if (card.cardData.CharacterID != exceptCard.cardData.CharacterID)
                    continue;

                if( card.cardData.Lock == true )
                    continue;
            }
            else if( type == InvenType.LevelUp)
            {
                if( card.cardData.Lock == true )
                    continue;
            }

            card.transform.SetParent(InvenContent.transform);
            card.transform.localScale = new Vector3(1f, 1f, 0f);
            card.OnClick = OnSelectInvenCard;

            if( bSelect == false )
            {
                card.SetSelect( false );
            }
                
            card.gameObject.SetActive(true);
            count++;
        }
        count = Mathf.CeilToInt((float)count / 6);

        int height = MinHeight;

        if (count * 168 > height)
            height = count * 168;

        InvenContent.GetComponent<RectTransform>().sizeDelta = new Vector2(InvenContent.GetComponent<RectTransform>().sizeDelta.x, height);
    }


    void OnSelectInvenCard(Card card)
    {
        SelectCard(card);
    }

    public void OnBitSort()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( GameOption.BitSort == 1 )
            GameOption.BitSort = 0;
        else
            GameOption.BitSort = 1;

        for( int i = 0 ; i < Bit.Length ; i++ )
        {
            Bit[ i ].SetActive( GameOption.BitSort == i ? true : false );
        }

        InvenCardObjectPool.SortByBit();

        if( exceptCard )
            ApplyInven( exceptCard ,currenttype , true );
        else
            ApplyInven( currenttype );
    }

    public void OnGradeSort()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( GameOption.GradeSort == 1 )
            GameOption.GradeSort = 0;
        else
            GameOption.GradeSort = 1;

        InvenCardObjectPool.SortByGrade();
        for( int i = 0 ; i < Grade.Length ; i++ )
        {
            Grade[ i ].SetActive( GameOption.GradeSort == i ? true : false );
        }
        ApplyInven();
    }
}
