using System.Collections.Generic;

public class PackageTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public List<PackageReferenceData> list = new List<PackageReferenceData>();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_package" , "" );
    }

    private void InsertData( string[] node )
    {
        PackageReferenceData new_data = new PackageReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );

        dataCount++;

        new_data.productImg =  node[ dataCount++ ];

        new_data.ruby = int.Parse( node[ dataCount++ ] );
        new_data.stone = int.Parse( node[ dataCount++ ] );
        new_data.gold = int.Parse( node[ dataCount++ ] );
        new_data.cost = int.Parse( node[ dataCount++ ] );
        new_data.productcode = node[ dataCount++ ];
        new_data.rewardString = node[ dataCount++ ];        
        new_data.productBGImg = node[ dataCount++ ];

        list.Add( new_data );
        Add( new_data );
        
    }


    public static PackageReferenceData GetData( int key )
    {
        PackageTBL TBL = TBLManager.I.GetTable<PackageTBL>( TABLELIST_TYPE.Packeage );
        return (PackageReferenceData)TBL.Find( key );
    }

   
}