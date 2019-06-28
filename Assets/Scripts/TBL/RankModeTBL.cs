using System.Collections.Generic;

public class RankModeTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_mode_rank_stage" , "" );
    }

    private void InsertData( string[] node )
    {
        RankModeStageReferenceData new_data = new RankModeStageReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.ModeType = ( RankModeType )int.Parse( node[ dataCount++ ] );        
        new_data.DayOfWeek = int.Parse( node[ dataCount++ ] );
        dataCount++;

        FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.StageMonster );
        //new_data.StageMonster = int.Parse( node[ dataCount++ ] );
        new_data.ApCost = int.Parse( node[ dataCount++ ] );
        new_data.TileSize = int.Parse( node[ dataCount++ ] );
        new_data.StageBackGround = node[ dataCount++ ];
        new_data.CrazyTime = int.Parse( node[ dataCount++ ] );
        new_data.StageLobbyBGM = node[dataCount++];
        new_data.StageTile = node[ dataCount++ ];
        new_data.StageLobbyBG = node[ dataCount++ ];
        Add( new_data );
    }


    public static RankModeStageReferenceData GetData( int key )
    {
        RankModeTBL TBL = TBLManager.I.GetTable<RankModeTBL>( TABLELIST_TYPE.RankModeStage );
        return (RankModeStageReferenceData)TBL.Find( key );
    }
}