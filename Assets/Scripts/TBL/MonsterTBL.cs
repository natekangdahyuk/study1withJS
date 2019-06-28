using System.Collections.Generic;

public class MonsterTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_mob_base", "");
    }

    private void InsertData(string[] node)
    {
        MonsterReferenceData new_data = new MonsterReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.Name = int.Parse(node[dataCount++]);
        dataCount++;

        new_data.Prefab = node[dataCount++];
        new_data.LobbyTexture = node[dataCount++];
        new_data.FieldPrefab = node[dataCount++];
        new_data.EngName = node[ dataCount++ ];


        //new_data.Hp = int.Parse(node[dataCount++]);
        //new_data.Level = int.Parse(node[dataCount++]);


        Add(new_data);
    }


    public static MonsterReferenceData GetData(int key)
    {
        MonsterTBL TBL = TBLManager.I.GetTable<MonsterTBL>(TABLELIST_TYPE.Monster);
        return (MonsterReferenceData)TBL.Find(key);
    }
}