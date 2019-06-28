
public class MonsterDetailReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int Name;

    public PROPERTY property;

    public int Rank;

    public int Hp;

    public int Level;

    public int MonsterIndex;

    public int[] MobAction;

    public string mob_Info;



}