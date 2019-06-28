using System.Collections.Generic;

public class AttributeModeTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_mode_attribute_stage" , "" );
    }

    private void InsertData( string[] node )
    {
        AttributeModeReferenceData new_data = new AttributeModeReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.StageGroup = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.StageName = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.StageMonster = int.Parse( node[ dataCount++ ] );
        new_data.ApCost = int.Parse( node[ dataCount++ ] );
        new_data.GoldReward = int.Parse( node[ dataCount++ ] );
        new_data.UserExpReward = int.Parse( node[ dataCount++ ] );

        FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.MainRewardList );
        FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.MainRewardCount );
        FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.MainRewardPer );

        new_data.TileSize = int.Parse( node[ dataCount++ ] );
        new_data.StageBackGround = node[ dataCount++ ] ;
        new_data.CrazyTime = int.Parse( node[ dataCount++ ] );
        Add( new_data );
    }


    public static AttributeModeReferenceData GetData( int key )
    {
        AttributeModeTBL TBL = TBLManager.I.GetTable<AttributeModeTBL>( TABLELIST_TYPE.AttributeMode );
        return (AttributeModeReferenceData)TBL.Find( key );
    }
}