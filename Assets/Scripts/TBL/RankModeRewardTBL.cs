using System.Collections.Generic;

public class RankModeRewardTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();


    public List<RankModeRewardReferenceData> rewardList = new List<RankModeRewardReferenceData>();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_mode_rank_reward" , "" );
    }

    private void InsertData( string[] node )
    {
        RankModeRewardReferenceData new_data = new RankModeRewardReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.rankType = int.Parse( node[ dataCount++ ] );
        new_data.ranktext = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.rewardRuby = int.Parse( node[ dataCount++ ] );    
        new_data.rewardGold = int.Parse( node[ dataCount++ ] );
        new_data.RewardName = node[ dataCount++ ];

        rewardList.Add( new_data );
        Add( new_data );
    }


    public static RankModeRewardReferenceData GetData( int key )
    {
        RankModeRewardTBL TBL = TBLManager.I.GetTable<RankModeRewardTBL>( TABLELIST_TYPE.RankModeReward );
        return (RankModeRewardReferenceData)TBL.Find( key );
    }

    public static List<RankModeRewardReferenceData> GetDataList()
    {
        RankModeRewardTBL TBL = TBLManager.I.GetTable<RankModeRewardTBL>( TABLELIST_TYPE.RankModeReward );
        return TBL.rewardList;
    }

    public static string GetRewardString( int type )
    {
        RankModeRewardTBL TBL = TBLManager.I.GetTable<RankModeRewardTBL>( TABLELIST_TYPE.RankModeReward );

        for( int i=0 ; i < TBL.rewardList.Count ; i++ )
        {
            if( TBL.rewardList[ i ].rankType == type )
            {
                return TBL.rewardList[ i ].RewardName;
            }
        }

        return "";
    }
}