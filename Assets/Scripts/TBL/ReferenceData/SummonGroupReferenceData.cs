
public class SummonGroupReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int summonType; //1 default :  2 bit

    public int stone_cost;

    public int bit_cost;

    public int stone_num;

    public string stone_formula;

    public string summon_group;

    public string grou_ratio;

}