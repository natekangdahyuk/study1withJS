using System.Collections.Generic;

public class CardTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public Dictionary<int, List<CardReferenceData>> CardList = new Dictionary<int , List<CardReferenceData>>();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_char_card", "");
    }

    private void InsertData(string[] node)
    {
        CardReferenceData new_data = new CardReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.characterIndex = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.Property = (PROPERTY)int.Parse(node[dataCount++]);
        new_data.best = int.Parse(node[dataCount++]);

        for( int i =0; i < new_data.PentagonValue.Length; i++)
            new_data.PentagonValue[i] = int.Parse(node[dataCount++]);

        new_data.BuffType = ( LeaderBuff)int.Parse( node[ dataCount++ ] );
        new_data.BuffValue = int.Parse( node[ dataCount++ ] );


        List<CardReferenceData> cardlist;
        if( CardList.TryGetValue( new_data.characterIndex , out cardlist ) )
        {
            cardlist.Add( new_data );
        }
        else
        {
            cardlist = new List<CardReferenceData>();
            CardList.Add( new_data.characterIndex , cardlist );
            cardlist.Add( new_data );
        }
        Add(new_data);
    }


    public static CardReferenceData GetData(int key)
    {
        CardTBL TBL = TBLManager.I.GetTable<CardTBL>(TABLELIST_TYPE.Card);
        return (CardReferenceData)TBL.Find(key);
    }

    public static CardReferenceData GetDataByCharacterID( int key )
    {
        CardTBL TBL = TBLManager.I.GetTable<CardTBL>( TABLELIST_TYPE.Card );

        List<CardReferenceData> cardlist;
        if( TBL.CardList.TryGetValue( key , out cardlist ) )
        {
            return cardlist[ 0 ];
        }


        return null;
    }
}