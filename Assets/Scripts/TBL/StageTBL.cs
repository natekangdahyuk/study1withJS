using System.Collections.Generic;
using UnityEngine;

public class StageTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    Dictionary<int, List<StageReferenceData>> SubStageList = new Dictionary<int, List<StageReferenceData>>();
        
    List<StageReferenceData> SubStageListEx = new List<StageReferenceData>();


    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_main_stage", "");
    }

    private void InsertData( string[] node )
    {
        try
        {
            StageReferenceData new_data = new StageReferenceData();

            int dataCount = 0;
            new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
            new_data.Difficulty = int.Parse( node[ dataCount++ ] );
            new_data.ThemaIndex = int.Parse( node[ dataCount++ ] );
            new_data.SubIndex = int.Parse( node[ dataCount++ ] );
            new_data.StageName = int.Parse( node[ dataCount++ ] );
            dataCount++;
            new_data.MonsterIndex = int.Parse( node[ dataCount++ ] );
            new_data.ApCost = int.Parse( node[ dataCount++ ] );
            new_data.NeedNumber = int.Parse( node[ dataCount++ ] );
            new_data.GoldReward = int.Parse( node[ dataCount++ ] );
            new_data.UserExpReward = int.Parse( node[ dataCount++ ] );

            FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.CharRewardList );
            FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.CharRewardPer );

            FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.TaskType );
            FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.TaskValue );
            FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.TaskInfo );

            new_data.StarRewardType = int.Parse( node[ dataCount++ ] );
            new_data.StarRewardValue = int.Parse( node[ dataCount++ ] );

            new_data.TileSize = int.Parse( node[ dataCount++ ] );

            new_data.StageIcon = int.Parse( node[ dataCount++ ] );
            new_data.StageBackGround = node[ dataCount++ ];

            new_data.CrazyTime = int.Parse( node[ dataCount++ ] );
            new_data.BGM = node[ dataCount++ ];
            new_data.StageTile = node[ dataCount++ ];
            new_data.rewardstring = node[ dataCount++ ];

            Add( new_data );
            SubStageListEx.Add( new_data );

            List<StageReferenceData> stagelist;
            if( SubStageList.TryGetValue( new_data.ThemaIndex + ( 100000 * new_data.Difficulty ) , out stagelist ) )
            {
                stagelist.Add( new_data );
            }
            else
            {
                stagelist = new List<StageReferenceData>();
                stagelist.Add( new_data );
                SubStageList.Add( new_data.ThemaIndex + ( 100000 * new_data.Difficulty ) , stagelist );
            }
        }
        catch( System.Exception e )
        {
            Debug.LogError( e.Message );
        }
    }
    public static List<StageReferenceData> GetSubStageList(int mainstageIndex , int difficulty )
    {
        StageTBL TBL = TBLManager.I.GetTable<StageTBL>(TABLELIST_TYPE.Stage);
        List<StageReferenceData> stagelist;
        if (TBL.SubStageList.TryGetValue(mainstageIndex + (difficulty* 100000), out stagelist))
        {            
            return stagelist;
        }

        return null;
    }

    public static int GetNextStageIndex( int key , int diff )
    {
        StageTBL TBL = TBLManager.I.GetTable<StageTBL>(TABLELIST_TYPE.Stage);

        for ( int i =0; i < TBL.SubStageListEx.Count; i++)
        {
            if(key == TBL.SubStageListEx[i].ReferenceID)
            {
                if( ++i < TBL.SubStageListEx.Count)
                {
                    return TBL.SubStageListEx[i].ReferenceID;
                }
                return -1;
            }
        }

        for( int i = 0 ; i < TBL.SubStageListEx.Count ; i++ )
        {
            if( diff == TBL.SubStageListEx[ i ].Difficulty )
            {              
               return TBL.SubStageListEx[ i ].ReferenceID;              
            }
        }
        return TBL.SubStageListEx[ 0 ].ReferenceID;
    }

    public static StageReferenceData GetData(int key)
    {
        StageTBL TBL = TBLManager.I.GetTable<StageTBL>(TABLELIST_TYPE.Stage);
        return (StageReferenceData)TBL.Find(key);
    }


}