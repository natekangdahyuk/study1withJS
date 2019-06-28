using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CollectionManager : Singleton<CollectionManager>
{
    public List<int> Invenlist = new List<int>();

    public void Init()
    {
        List<DefaultCardReferenceData> data = DefaultCardTBL.GetData();

        for( int i= 0 ; i < data.Count ; i++ )
        {
            AddCard( data[ i ].CardID );
        }
    }

    public bool IsCard( int cardid )
    {
        CardReferenceData data = CardTBL.GetData( cardid );

        if( data == null )
            return false;

        for( int i = 0; i < Invenlist.Count; i++)
        {
            if ( data.characterIndex == Invenlist[i])
                return true;
        }

        return false;
    }

    public void AddCard(int cardid )
    {
        CharacterReferenceData data = CharacterTBL.GetData( cardid );

        if( data == null )
            return;

        for (int i = 0; i < Invenlist.Count; i++)
        {
            if ( data.ReferenceID == Invenlist[i])
                return ;
        }

        Invenlist.Add( data.ReferenceID );
    }
}