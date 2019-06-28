using System.Collections.Generic;

public class ItemTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
        
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_item" , "" );
    }

    private void InsertData( string[] node )
    {
        ItemReferenceData new_data = new ItemReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.ItemName = int.Parse( node[ dataCount++ ] );
        dataCount++;

        new_data.Image = node[ dataCount++ ] ;

        Add( new_data );
    }


    public static ItemReferenceData GetData( int key )
    {
        ItemTBL TBL = TBLManager.I.GetTable<ItemTBL>( TABLELIST_TYPE.Item );
        return (ItemReferenceData)TBL.Find( key );
    }
}