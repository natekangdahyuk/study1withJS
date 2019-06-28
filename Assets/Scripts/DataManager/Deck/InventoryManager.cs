using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class InventoryManager : Singleton<InventoryManager>
{
    public List<CardData> Invenlist = new List<CardData>();
    public List<int> skinList = new List<int>();
    public CardData representCharacter = null;
    public int MaxCardCount = 42;


    public bool IsMaxCount()
    {
        return MaxCardCount <= Invenlist.Count ? true : false;
    }
    //public void TestSetCard() //! 임시 카드 셋팅
    //{
    //    for( int z= 0; z < 5; z++)
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            CardReferenceData data = CardTBL.GetData(10011+ (i* 70)+z);
    //            Invenlist.Add(new CardData(data.ReferenceID, 10011 + (i * 70) + z));
    //        }
    //    }


    //    for (int z = 0; z < 5; z++)
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            //Invenlist[(z * 10) + i].SetDeck(z, true);
    //            DeckManager.I.SetDeck(z, Invenlist[(z*10)+ i]);

    //            if (representCharacter == null)
    //                representCharacter = Invenlist[i];
    //        }
    //    }

    //    InvenCardObjectPool.Apply();
    //}

    public void SetRepresentCharacter()
    {
        for( int i = 0 ; i < Invenlist.Count ; i++ )
        {
            if( Invenlist[ i ].CardKey == PlayerData.I.representIndex )
                representCharacter = Invenlist[ i ];
        }
    }
    public CardData NewCard( long uidx , int cardidx , int exp , int level , int star , int skin , int deck , bool Leader , bool SubLeader , bool bLock , int limit, bool bNew )
    {
        CardReferenceData data = CardTBL.GetData( cardidx );

        if( data == null )
            return null;

        CharacterReferenceData referenceData = CharacterTBL.GetData( data.characterIndex );

        if( referenceData.bit == 2 && deck > 0 )
            return null;

        CardData card = new CardData( cardidx , uidx, star );
        card.Exp = exp;
        card.Level = level;

        if( star == 0 )
        {
            star = 1;
            Debug.LogError( "태생성ㅣ 0이면안됨" );
        }
     
        card.SelectSkin( skin );
        
        card.Lock = bLock;
        card.SetLimit(limit);
        card.bNewCard = bNew;

        Invenlist.Add( card );
        InvenCardObjectPool.Add( card );

        if( deck > 0 )
        {
            DeckManager.I.SetDeck( deck - 1 , card );
            card.Leader[ deck-1 ] = Leader;
            card.SubLeader[ deck-1 ] = SubLeader;
        }

        SoundManager.New( SoundManager.SoundType.voice , SoundManager.Instance , card.Voice );

        for( int i = 0 ; i < card.referenceData.TouchSound.Length ; i++ )
            SoundManager.New( SoundManager.SoundType.voice , SoundManager.Instance , card.referenceData.TouchSound[ i ] );
                
        CollectionManager.I.AddCard( card.CharacterID );

        if(bNew)
            InvenCardObjectPool.SortByBit();
        return card;
    }

    public void Remove( long cardkey )
    {
        InvenCardObjectPool.Remove( cardkey );

        for( int i = 0 ; i < Invenlist.Count ; i++ )
        {
            if( Invenlist[ i ].CardKey == cardkey )
            {
                Invenlist.Remove( Invenlist[ i ] );
                return;
            }
        }


    }

    public void ChangeDeck( int deckIndex , long[] cardkey )
    {
        DeckManager.I.ClearDeck( deckIndex );

        for( int i = 0 ; i < cardkey.Length ; i++ )
        {
            DeckManager.I.SetDeck( deckIndex , Get( cardkey[ i ] ) );
        }
    }

    public CardData Get( long key )
    {
        for( int i = 0 ; i < Invenlist.Count ; i++ )
        {
            if( Invenlist[ i ].CardKey == key )
                return Invenlist[ i ];
        }
        return null;
    }

    public void ClearNewCard()
    {
        bool sort = false;
        for( int i = 0 ; i < Invenlist.Count ; i++ )
        {
            if( Invenlist[ i ].bNewCard == true )
            {
                Invenlist[ i ].bNewCard = false;
                sort = true;
            }
        }

        if(sort)
        {
            InvenCardObjectPool.SortByBit();
        }
        
    }

    public void ChangeDeck( List<Card> cardlist )
    {
        //for( int i =0; i< Invenlist.Count; i++)
        //{
        //    if( Invenlist[i].IsUse(DeckManager.I.CurrentDeckIndex - 1) )
        //        Invenlist[i].SetDeck(DeckManager.I.CurrentDeckIndex - 1, false);
        //}
        DeckManager.I.ClearDeck( DeckManager.I.CurrentDeckIndex - 1 );
        for( int i = 0 ; i < cardlist.Count ; i++ )
        {
            //cardlist[i].cardData.SetDeck(DeckManager.I.CurrentDeckIndex - 1, true);
            DeckManager.I.SetDeck( DeckManager.I.CurrentDeckIndex - 1 , cardlist[ i ].cardData );
        }
    }

    public void AddSkin( int skin )
    {
        for( int i =0 ; i < skinList.Count ; i++ )
        {
            if( skinList[ i ] == skin )
                return;
        }

        skinList.Add( skin );
    }

    public bool IsSkin(int index)
    {
        for( int i = 0 ; i < skinList.Count ; i++ )
        {

            if( skinList[ i ] == index )
            {
                return true;
            }
        }
        return false;
    }

}