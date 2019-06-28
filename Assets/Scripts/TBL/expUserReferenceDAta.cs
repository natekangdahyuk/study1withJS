
public class ExpUserReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int exp;

    public int ap_Max;

    public int apReward;

    public int goldReward;

    public int CashReward;

}
