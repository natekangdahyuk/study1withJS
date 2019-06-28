
public class StageReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int Difficulty; //난이도 1 2 3

    public int ThemaIndex;

    public int SubIndex;

    public int StageName;

    public int MonsterIndex;

    public int ApCost;

    public int NeedNumber;

    public int GoldReward;

    public int CharExpReward;

    public int UserExpReward;

    public int[] CharRewardList;

    public int[] CharRewardPer;

    public int[] TaskType = new int[3];

    public int[] TaskValue = new int[3];

    public int[] TaskInfo = new int[3];

    public int StarRewardType;

    public int StarRewardValue;

    public int TileSize;

    public int StageIcon;

    public string StageBackGround;

    public int CrazyTime;

    public string BGM;

    public string StageTile;

    public string rewardstring;


}