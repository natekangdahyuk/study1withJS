

public class limitbreakReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int gold_cost;

    public int stat_up;
}