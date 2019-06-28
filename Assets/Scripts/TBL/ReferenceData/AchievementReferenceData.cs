

public class AchievementReferenceData : IReferenceDataByKey
{

    public enum MissionType
    {        
        NormalStageClear = 11,
        HardStageClear,
        HellStageClear,
        StagePlayCount,
        RankStagePlayCount,
        Gold,
        Level,
        Star,
        Limit,
        illustCount,

    }
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int name;

    public MissionType type;

    public int missionNumber;

    public int missionMark;

    public long missionValue;

    public MissionReferenceData.RewardType rewardType;

    public int RewardValue;

    public string list_Img;

}