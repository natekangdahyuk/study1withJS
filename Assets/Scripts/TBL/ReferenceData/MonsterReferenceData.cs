
public class MonsterReferenceData : IReferenceDataByKey
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public int ReferenceID;

    public int Name;

    public string Prefab;

    public string LobbyTexture;

    public string FieldPrefab;

    public string EngName;

    //public int Hp;

    //public int Level;

    //public int Name;

    

}