

public class RankModeRewardReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int rankType;

    public int ranktext;

    public int rewardRuby;

    public int rewardGold;

    public string RewardName;
}