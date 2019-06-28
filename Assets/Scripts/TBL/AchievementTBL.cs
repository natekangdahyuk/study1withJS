using System.Collections.Generic;

public class AchievementTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    List<AchievementReferenceData> missionList = new List<AchievementReferenceData>();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_mission_achieve" , "" );
    }

    private void InsertData( string[] node )
    {
        AchievementReferenceData new_data = new AchievementReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.name = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.type = (AchievementReferenceData.MissionType)int.Parse( node[ dataCount++ ] );
        new_data.missionNumber = int.Parse( node[ dataCount++ ] );
        new_data.missionMark = int.Parse( node[ dataCount++ ] );
        new_data.missionValue = long.Parse( node[ dataCount++ ] );
        new_data.rewardType = (MissionReferenceData.RewardType)int.Parse( node[ dataCount++ ] );
        new_data.RewardValue = int.Parse( node[ dataCount++ ] );
        new_data.list_Img = node[dataCount++];
        missionList.Add( new_data );
        Add( new_data );
    }


    public static List<AchievementReferenceData> GetData()
    {
        AchievementTBL TBL = TBLManager.I.GetTable<AchievementTBL>( TABLELIST_TYPE.Achievement );
        return TBL.missionList;
    }
}