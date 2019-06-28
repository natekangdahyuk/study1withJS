
public enum RankModeType
{
    TimeLimit = 1,
    Mode2048,
    Time2048,
    TimeDefence,
}

public class RankModeStageReferenceData : IReferenceDataByKey
{


    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public RankModeType ModeType;

    public int DayOfWeek;

    public int[] StageMonster;

    public int ApCost;

    public int TileSize;

    public string StageBackGround;

    public int CrazyTime;

    public string StageTile;

    public string StageLobbyBGM;

    public string StageLobbyBG;



}