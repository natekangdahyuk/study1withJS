using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DeckCard : MonoBehaviour
{    
    Card card;

    [SerializeField]
    TweenPosition tweenPos;

    [SerializeField]
    TweenScale tweenScale;

    public int Value;
    public int SiblingIndex; //! 렌더링 순서
    int MaxSiblingIndex; //! 렌더링 순서

    public CardData cardData { get { return card == null ? null : card.cardData; } }
    bool bInit = false;
    public void Init()
    {
        card = ResourceManager.Load<Card>(this.gameObject, "Card_ingame" );
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = new Vector3(1f, 1f, 0f);
        SiblingIndex = transform.GetSiblingIndex();
        card.SetMask(true);
        card.SetSelect(false);
    }

    public void SetMask(int index)
    {
        if (Value == index)
        {
            card.SetMask(false);
            tweenScale.Play();
            transform.SetSiblingIndex(MaxSiblingIndex);
            card.SetSelect(true);
            //tweenPos.Play();
            
        }
        else
        {
            transform.SetSiblingIndex(SiblingIndex);            
            //tweenPos.Reset();

            if( tweenScale )
                tweenScale.Reset();
            card.SetSelect(false);
        }
    }

    public void Reset()
    {
        if( card == null )
            return;

        card.SetMask(true);
        transform.SetSiblingIndex(SiblingIndex);
        tweenScale.Reset();
        card.SetSelect(false);
    }

    public void ApplyData(CardData data)
    {
        if( bInit == false )
            Init();

        card.ApplyData(data, true);
    }
    public void SetValue(int index)
    {
        Value = index;
    }

    public void SetMaxSiblingIndex(int index)
    {
        MaxSiblingIndex = index;
    }

}