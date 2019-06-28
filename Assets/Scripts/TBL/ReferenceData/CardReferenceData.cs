
public enum LeaderBuff
{
    None,
    AllAttack,
    AttributeAttack,
    ClassAttack,
    AllHp,
    AttributeHp,
    ClassHp,
    ComboUp,
}


public class CardReferenceData : IReferenceDataByKey
{
  
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int characterIndex;

    public PROPERTY Property;

    public int best;

    public int[] PentagonValue = new int[5];

    public LeaderBuff BuffType;

    public int BuffValue;

    



}