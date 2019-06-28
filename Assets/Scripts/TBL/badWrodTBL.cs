using System.Collections.Generic;

public class badWrodTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public List<StringReferenceData> datalist = new List<StringReferenceData>();

    public void LoadData()
    {
        if( datalist.Count > 0 )
        {
            datalist.Clear();
        }

        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_badword_string" , "" );
      
    }

    private void InsertData( string[] node )
    {
        StringReferenceData new_data = new StringReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.text = node[ dataCount++ ];
        dataCount++;
        new_data.text = new_data.text.Replace( "/n" , "\n" );
        Add( new_data );
        datalist.Add( new_data );
    }


    public static bool CheckWord( string name )
    {
        badWrodTBL TBL = TBLManager.I.GetTable<badWrodTBL>( TABLELIST_TYPE.BadWord );

        for( int i =0 ; i < TBL.datalist.Count ; i++)
        {
            if( name.Contains( TBL.datalist[ i ].text ) )
                return false;
        }

        return true;
    }
    public static void Load()
    {
        badWrodTBL tbl = TBLManager.I.GetTable<badWrodTBL>( TABLELIST_TYPE.BadWord );
        if( tbl != null )
        {
            tbl.datalist.Clear();
        }

        tbl.LoadData();
    }
}