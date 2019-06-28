

public class MissionReferenceData : IReferenceDataByKey
{

    public enum MissionType
    {
        All = 1,
        FieldClear,
        RankClear,
        Summon,
        LevelUp,
        GoldBuy,
    }

    public enum RewardType
    {
        Gold = 1,
        Ruby,
        AP,
        Stone,
    }


    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int name;

    public MissionType type;

    public int count;

    public RewardType Rewardtype;

    public int RewardValue;

    public string list_Img;

}