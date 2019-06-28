
public class GradeDataReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int materialCount;

    public int goldCost;

    public int maxlv;

    public int material_Exp;

    public int stone;

}