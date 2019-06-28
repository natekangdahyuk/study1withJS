
public class ThemaReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int ThemaNo;

    public int Difficulty;

    public int ThemaName;

    public string BackGroundPrefab;

    public string LobbyTexture;

    public int[] ThemaReward = new int[3];

    public string rewardstring;
}