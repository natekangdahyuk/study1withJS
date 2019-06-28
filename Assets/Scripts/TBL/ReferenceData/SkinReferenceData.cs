

public enum ABILITYTYPE
{
    None,
    Attack,
    Defence,
    Health,
    Heal,
    Critical,
}

public enum UnLockType
{
    None,
    Star,
    Level,
}


public class SkinReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public string texture;

    public string Live2DModel;

    public string Live2DBG;


    public int NameString;

    public int ablityString;

    public ABILITYTYPE ablityType;

    public int ablityValue;

    public int UnLockString;

    public UnLockType UnLockType;

    public int UnLockValue;

    public int Cost;

    public string[] Sound;

    public string oneWord;

}
