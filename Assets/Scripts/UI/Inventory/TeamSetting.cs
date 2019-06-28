using System;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetting
{
    public List<Card> SelectTeamSettingCardList = new List<Card>();
    public List<Card> StartTeamSettingCardList = new List<Card>();

    public void Clear()
    {
        for( int i = 0 ; i < SelectTeamSettingCardList.Count ; i++ )
        {
            SelectTeamSettingCardList[ i ].SetSelectCheck( false );
            SelectTeamSettingCardList[ i ].bCheck = false;
        }
        SelectTeamSettingCardList.Clear();
        StartTeamSettingCardList.Clear();

    }

    public void Complete()
    {
        for( int i = 0 ; i < StartTeamSettingCardList.Count ; i++ )
        {
            if( GetSelectCard( StartTeamSettingCardList[ i ] ) == false  )
            {
                StartTeamSettingCardList[ i ].cardData.Leader[ DeckManager.I.CurrentDeckIndex - 1 ] = false;
                StartTeamSettingCardList[ i ].cardData.SubLeader[ DeckManager.I.CurrentDeckIndex - 1 ] = false;
            }

            StartTeamSettingCardList[ i ].SetLeader( StartTeamSettingCardList[ i ].cardData );
        }
    }

    public void Apply(List<Card> InvenCardList)
    {
        SelectTeamSettingCardList.Clear();        

        for( int i =0; i < InvenCardList.Count; i++)
        {
            if( InvenCardList[ i ].cardData.IsUse( DeckManager.I.CurrentDeckIndex - 1 ) )
            {
                SelectTeamSettingCardList.Add( InvenCardList[ i ] );
                StartTeamSettingCardList.Add( InvenCardList[ i ] );
                InvenCardList[ i ].SetSelectCheck( true );
            }
            else
            {
                InvenCardList[ i ].SetSelectCheck( false );                
            }
            InvenCardList[ i ].bCheck = false;
        }
    }

    public bool GetSelectCard(Card card)
    {
        for( int i = 0 ; i < SelectTeamSettingCardList.Count ; i++ )
        {
            if( SelectTeamSettingCardList[ i ] == card )
                return true;
        }

        return false;
        
    }

    public bool IsStartCard(Card card)
    {
        for( int i = 0 ; i < StartTeamSettingCardList.Count ; i++ )
        {
            if( StartTeamSettingCardList[ i ] == card )
                return true;
        }

        return false;
    }
    
    public void SelectCard(Card card)
    {
        for( int i =0; i < SelectTeamSettingCardList.Count; i++)
        {
            if( SelectTeamSettingCardList[i].cardData.bit == card.cardData.bit )
            {
                if(SelectTeamSettingCardList[i] == card)
                {
                    card.SetSelectCheck( false );
                    SelectTeamSettingCardList.Remove(card);
                    
                    if( IsStartCard( SelectTeamSettingCardList[ i ] ) == false )
                    {
                        card.cardData.Leader[ DeckManager.I.CurrentDeckIndex - 1 ] = false;
                        card.cardData.SubLeader[ DeckManager.I.CurrentDeckIndex - 1 ] = false;
                    }
                    return;
                }
                else
                {
                    SelectTeamSettingCardList[i].SetSelectCheck(false);
                    card.SetSelectCheck(true);
                    
                    card.cardData.Leader[ DeckManager.I.CurrentDeckIndex - 1 ] = SelectTeamSettingCardList[ i ].cardData.Leader[ DeckManager.I.CurrentDeckIndex - 1 ];
                    card.cardData.SubLeader[ DeckManager.I.CurrentDeckIndex - 1 ] = SelectTeamSettingCardList[ i ].cardData.SubLeader[ DeckManager.I.CurrentDeckIndex - 1 ];

                    SelectTeamSettingCardList.Add(card);                    
                    SelectTeamSettingCardList.Remove(SelectTeamSettingCardList[i]); 
                    

                    int index = (int)Mathf.Log( card.cardData.bit , 2 ) - 2;
                    card.bCheck = true;

                    return;
                }
            }
        }

        card.SetSelectCheck( true);
        SelectTeamSettingCardList.Add(card);
        
    }
}
