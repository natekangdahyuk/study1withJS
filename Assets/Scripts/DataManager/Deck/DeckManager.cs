using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DeckManager : Singleton<DeckManager>
{
    public static int DeckMaxCount = 5;
    Dictionary<int,CardData>[] Decklist = new Dictionary<int,CardData>[5];

    
    public int CurrentDeckIndex = 1;
    public void Init()
    {
        for (int z = 0; z < 5; z++)
        {
            Decklist[z] = new Dictionary<int, CardData>();
        }
    }
   
    public void ClearDeck( int deckIndex )
    {
        foreach(KeyValuePair<int,CardData> value in Decklist[deckIndex])
        {
            value.Value.SetDeck(deckIndex,false);
        }

        Decklist[deckIndex].Clear();
    }
    public void SetDeck(int deckIndex, CardData card)
    {
        if (card == null)
            return;

        card.SetDeck(deckIndex, true);

        if( Decklist[ deckIndex ].ContainsKey( card.bit ) )
        {
            Debug.LogError( "겹침" + card.CardKey.ToString() );
            return;
        }
        Decklist[deckIndex].Add(card.bit, card);

    }

    public Dictionary<int,CardData> GetCurrentDeck()
    {
        return Decklist[CurrentDeckIndex - 1];
    }

}

