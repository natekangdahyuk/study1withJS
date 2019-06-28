using System.Collections.Generic;

public class MonsterDetailTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_mob_detail", "");
    }

    private void InsertData(string[] node)
    {
        MonsterDetailReferenceData new_data = new MonsterDetailReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        dataCount++;

        new_data.Name = int.Parse(node[dataCount++]);
        dataCount++;

        new_data.property = (PROPERTY)int.Parse(node[dataCount++]);

        new_data.Rank = int.Parse(node[dataCount++]);
        new_data.Level = int.Parse(node[dataCount++]);
        new_data.Hp = (int)float.Parse(node[dataCount++]);
        new_data.MonsterIndex = int.Parse(node[dataCount++]);         
        FileReferenceLoader_Cvs.GetParseIntArrayByString( node[ dataCount++ ] , out new_data.MobAction );
        new_data.mob_Info = node[dataCount++];
        new_data.mob_Info = new_data.mob_Info.Replace("/n", "\n");
        Add(new_data);
    }


    public static MonsterDetailReferenceData GetData(int key)
    {
        MonsterDetailTBL TBL = TBLManager.I.GetTable<MonsterDetailTBL>(TABLELIST_TYPE.MonsterDetail);
        return (MonsterDetailReferenceData)TBL.Find(key);
    }
}