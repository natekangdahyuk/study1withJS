using System.Collections.Generic;

public class ComboTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
        

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_combo_data" , "" );
    }

    private void InsertData( string[] node )
    {
        ComboReferenceData new_data = new ComboReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.combo = int.Parse( node[ dataCount++ ] );
        new_data.hit = int.Parse( node[ dataCount++ ] );


        Add( new_data );
    }


    public static int GetDataCombo( int key )
    {
        ComboTBL TBL = TBLManager.I.GetTable<ComboTBL>( TABLELIST_TYPE.Combo );

        ComboReferenceData data = (ComboReferenceData)TBL.Find( key );

        if( data == null )
            return 0;

        return data.combo;
    }

    public static int GetDataHit( int key )
    {
        ComboTBL TBL = TBLManager.I.GetTable<ComboTBL>( TABLELIST_TYPE.Combo );

        ComboReferenceData data = (ComboReferenceData)TBL.Find( key );

        if( data == null )
            return 0;

        return data.hit;
    }



}