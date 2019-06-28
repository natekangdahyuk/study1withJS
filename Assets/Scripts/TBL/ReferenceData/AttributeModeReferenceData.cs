

public class AttributeModeReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int StageGroup;

    public int StageName;

    public int StageMonster;

    public int ApCost;

    public int GoldReward;

    public int UserExpReward;

    public int[] MainRewardList;

    public int[] MainRewardCount;

    public int[] MainRewardPer;

    public int TileSize;
    
    public string StageBackGround;

    public int CrazyTime;



}