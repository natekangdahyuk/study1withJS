using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LeaderSetting
{
    public Card Leader = null;
    public Card SubLeader = null;

    List<Card> deckCardList = null;
    List<LeaderSettingItem> leaderSettingItemList = null;
    public void Clear()
    {
        if (Leader)
        {         
            Leader = null;
        }

        if(SubLeader)
        {        
            SubLeader = null;
        }

        if(deckCardList != null)
        {
            for( int i = 0 ; i < deckCardList.Count ; i++ )
            {
                deckCardList[ i ].SetSelect( false );
                leaderSettingItemList[i].Hide();
            }

            deckCardList = null;
            leaderSettingItemList = null;
        }
    }

    public void ChangeLeader(long leaderID, long subleaderID )
    {
        for (int i = 0; i < deckCardList.Count; i++)
        {
            if(deckCardList[i].cardData.Leader[DeckManager.I.CurrentDeckIndex - 1])
            {
                deckCardList[i].cardData.Leader[DeckManager.I.CurrentDeckIndex - 1] = false;
                Card card = InvenCardObjectPool.Get(deckCardList[i].cardData.CardKey);
                card.Apply();
            }

            if (deckCardList[i].cardData.SubLeader[DeckManager.I.CurrentDeckIndex - 1])
            {
                deckCardList[i].cardData.SubLeader[DeckManager.I.CurrentDeckIndex - 1] = false;
                Card card = InvenCardObjectPool.Get(deckCardList[i].cardData.CardKey);
                card.Apply();
            }

            if (deckCardList[i].cardData.CardKey == leaderID)
            {
                deckCardList[i].cardData.Leader[DeckManager.I.CurrentDeckIndex - 1] = true;
                Card card = InvenCardObjectPool.Get(deckCardList[i].cardData.CardKey);
                card.Apply();
            }

            if (deckCardList[i].cardData.CardKey == subleaderID)
            {
                deckCardList[i].cardData.SubLeader[DeckManager.I.CurrentDeckIndex - 1] = true;
                Card card = InvenCardObjectPool.Get(deckCardList[i].cardData.CardKey);
                card.Apply();
            }
        }
    }

    public void Apply(List<Card> cardlist , List<LeaderSettingItem> leaderSettingItem )
    {
        deckCardList = cardlist;
        leaderSettingItemList = leaderSettingItem;
        
        for( int i = 0 ; i < leaderSettingItemList.Count ; i++ )
            leaderSettingItemList[ i ].SetLeaderMode();

    }
    public bool SelectCard(Card card )
    {
        if(Leader == null)
        {
            Leader = card;
            Leader.SetSelect(false);

            for( int i = 0 ; i < deckCardList.Count ; i++ )
            {
                if(deckCardList[i].cardData.CardKey == card.cardData.CardKey)
                {
                    leaderSettingItemList[ i ].SetSelectLeader();
                }

                leaderSettingItemList[ i ].SetSubLeaderMode();
            }
            return false;
        }

        if (Leader == card)
            return false;

        if(SubLeader == null)
        {
            SubLeader = card;
            return true;
        }

        return true;
    }
}
