
public class PackageReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }


    public int ReferenceID;

    public string productImg;

    public int ruby;

    public int stone;

    public int gold;

    public int cost;

    public string productcode;

    public string rewardString;

    public string productBGImg;
}
