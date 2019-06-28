using System.Collections.Generic;

public class MissionTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    
    List<MissionReferenceData> missionList = new List<MissionReferenceData>();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_mission_daily" , "" );
    }

    private void InsertData( string[] node )
    {
        MissionReferenceData new_data = new MissionReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        new_data.name = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.type = (MissionReferenceData.MissionType)int.Parse( node[ dataCount++ ] );        
        new_data.count = int.Parse( node[ dataCount++ ] );
        new_data.Rewardtype = (MissionReferenceData.RewardType)int.Parse( node[ dataCount++ ] );

        new_data.RewardValue = int.Parse( node[ dataCount++ ] );
        new_data.list_Img = node[dataCount++];
        missionList.Add( new_data );
        Add( new_data );
    }


    public static List<MissionReferenceData> GetData()
    {
        MissionTBL TBL = TBLManager.I.GetTable<MissionTBL>( TABLELIST_TYPE.MissionDaily );
        return TBL.missionList;
    }
}