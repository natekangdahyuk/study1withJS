using UnityEngine;
using System.Collections.Generic;

public class InvenCardObjectPool : MonoSinglton<InvenCardObjectPool>
{
    [SerializeField]
    GameObjectPool Pool;

    public List<Card> InvenCard = new List<Card>();
    public override void Awake()
    {
        base.Awake();    
    }

    public override void ClearAll()
    {
        for( int i = 0 ; i < InvenCard.Count ; i++ )
            Delete( InvenCard[ i ].gameObject );

        InvenCard.Clear();
    }

    public static void Apply()
    {
        for( int i = 0 ; i < I.InvenCard.Count ; i++ )
            Delete( I.InvenCard[ i ].gameObject );

        I.InvenCard.Clear();

        List<CardData> cardlist = InventoryManager.I.Invenlist;

        for( int i =0; i < cardlist.Count; i++)
        {
            Card card = Get();
            card.ApplyData(cardlist[i]);
            I.InvenCard.Add(card);
        }        
    }

    public static Card ChangeSkin( CardData card , int skinIndex )
    {
        for( int i = 0 ; i < I.InvenCard.Count ; i++ )
        {
            if ( I.InvenCard[i].cardData.CardKey == card.CardKey )
            {
                I.InvenCard[ i ].Apply();
                return I.InvenCard[ i ];
            }
        }
        return null;
    }

    public static void Add(CardData carddata)
    {
        Card card = Get();
        card.ApplyData(carddata);
        I.InvenCard.Add(card);
    }

    public static void PushAll()
    {
        for (int i = 0; i < I.InvenCard.Count; i++)
        {
            Delete(I.InvenCard[i].gameObject);
        }

        I.InvenCard.Clear();
    }
    public static void Remove(long cardkey)
    {
        for (int i = 0; i < I.InvenCard.Count; i++)
        {
            if (I.InvenCard[i].cardData.CardKey == cardkey)
            {
                Delete(I.InvenCard[i].gameObject);
                I.InvenCard.Remove(I.InvenCard[i]);
                return;
            }
                
        }
    }

    public static Card Get(CardData card)
    {
        for (int i = 0; i < I.InvenCard.Count; i++)
        {
            if (I.InvenCard[i].cardData == card)
                return I.InvenCard[i];
        }
        return null;
    }

    public static Card Get(long cardKey )
    {
        for (int i = 0; i < I.InvenCard.Count; i++)
        {
            if (I.InvenCard[i].cardData.CardKey == cardKey)
                return I.InvenCard[i];
        }
        return null;
    }


    public static Card Get()
    {
        return I.Pool.New().GetComponent<Card>();
    }

    public static Card Get( Transform parent )
    {
        Card TargetCard = Get();
        TargetCard.transform.SetParent(parent);
        TargetCard.transform.localPosition = Vector3.zero;
        TargetCard.transform.localScale = Vector3.one;

        return TargetCard;
    }

    public static void Delete( GameObject go )
    {
        I.Pool.Delete(go);
    }
   
    public static void SortByBit()
    {
        I.InvenCard.Sort( I.SortByBit );
    }

    public static void SortByGrade()
    {
        I.InvenCard.Sort( I.SortByGrade );
    }


    int SortByBit( Card a , Card b)
    {
        if( a.cardData.bNewCard && b.cardData.bNewCard == false )
            return -1;

        if( b.cardData.bNewCard && a.cardData.bNewCard == false )
            return 1;

        if( a.cardData.IsUse() && b.cardData.IsUse() == false)
            return -1;

        if( b.cardData.IsUse() && a.cardData.IsUse() == false )
            return 1;

       

        if( GameOption.BitSort == 0 ) //오름차순 0
        {
            if( a.cardData.bit > b.cardData.bit )
                return -1;

            if( a.cardData.bit == b.cardData.bit )
            {
                if( a.cardData.IsUse() && b.cardData.IsUse() )
                {
                    if( a.cardData.GetTeamIndex() < b.cardData.GetTeamIndex() )
                        return -1;
                    else
                        return 1;
                }

                if( a.cardData.Star > b.cardData.Star )
                    return -1;

                if( a.cardData.Star == b.cardData.Star )
                {
                    if( a.cardData.Level > b.cardData.Level )
                    {
                        return -1;
                    }
                }
            }
        }
        else
        {        
            if( a.cardData.bit < b.cardData.bit )
                return -1;

            if( a.cardData.bit == b.cardData.bit )
            {
                if( a.cardData.IsUse() && b.cardData.IsUse() )
                {
                    if( a.cardData.GetTeamIndex() < b.cardData.GetTeamIndex() )
                        return -1;
                    else
                        return 1;
                }

                if( a.cardData.Star < b.cardData.Star )
                    return -1;

                if( a.cardData.Star == b.cardData.Star )
                {
                    if( a.cardData.Level < b.cardData.Level )
                    return -1;
                }

            }
        }

        return 1;
    }

    int SortByGrade( Card a, Card b)
    {
        if( GameOption.GradeSort == 0)
        {
            if( a.cardData.Star > b.cardData.Star )
                return -1;
        }
        else
        {
            if( a.cardData.Star < b.cardData.Star )
                return -1;
        }

        if( a.cardData.Star == b.cardData.Star )
        {
            if( GameOption.BitSort == 0 )
            {
                if( a.cardData.bit > b.cardData.bit )
                    return -1;
            }
            else
            {
                if( a.cardData.bit < b.cardData.bit )
                    return -1;
            }
        }

        return 1;
    }
}
