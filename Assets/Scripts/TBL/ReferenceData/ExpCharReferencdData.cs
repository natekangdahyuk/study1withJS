
public class ExpCharReferencdData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int exp;

    public int lvup_cost;

    public int material_exp;

}