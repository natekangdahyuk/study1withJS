
public enum PROPERTY
{
    FIRE = 1,
    WATER,
    WIND,
    WHITE,
    BLACK,
    HEAL,
}

public enum CLASS
{
    ATTACK = 1,
    DEFENCE,
    HEAL,
}


public class CharacterReferenceData : IReferenceDataByKey
{

    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int NameIndex;

    public int type;

    public int exp_multiply;

    public int bit;

    public CLASS Class;

    public int star;

    public int Rare;

    public string Texture;

    public string bg;

    public string card_img;

    public string card_bg;

    public int DefaultSkin;

    public int SellPrice;

    public int[] SkinList;

    public string[] TouchSound;

    public string OneWord;

    public string charInfo;

    public string charCode;

    public string Code_Info;

}