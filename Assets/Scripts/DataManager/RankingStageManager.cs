using UnityEngine;


public class RankingStageManager : Singleton<RankingStageManager>
{
    public RankModeStageReferenceData CurrentData;
    public int []StageIndex = new int[4];

    public MonsterDetailReferenceData GetBossDetailData()
    {
        return MonsterDetailTBL.GetData( CurrentData.StageMonster[0] );
    }


    public void SetStageData( int[] stage )
    {
        for( int i = 0 ; i < stage.Length ; i++ )
            StageIndex[ i ] = stage[ i ];
    }

    public void ClearGame()
    {

    }
}