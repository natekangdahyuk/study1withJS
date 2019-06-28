using UnityEngine;
using System.Collections.Generic;

public class SellSetting
{
    public List<Card> SelectCardList = new List<Card>();

    public void Clear()
    {
        for( int i = 0 ; i < SelectCardList.Count ; i++ )
            SelectCardList[ i ].SetSelectCheck( false );
        SelectCardList.Clear();
    }

    

    public void SelectCard( Card card )
    {
        if( card.cardData.Lock)
        {
            GlobalUI.ShowOKPupUp( "캐릭터가 잠겨있습니다." );
            return;
        }

        if( card.cardData.IsUse())
        {
            GlobalUI.ShowOKPupUp( "팀에 포함 되어있는 카드입니다." );
            return;
        }

        for( int i =0 ; i < SelectCardList.Count ; i++ )
        {
            if( SelectCardList[i].cardData.CardKey == card.cardData.CardKey)
            {
                card.SetSelectCheck( false );
                SelectCardList.Remove( card );
                return;
            }
        }       

        card.SetSelectCheck( true );
        SelectCardList.Add( card );

    }
}