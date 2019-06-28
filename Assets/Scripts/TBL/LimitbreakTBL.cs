using System.Collections.Generic;

public class LimitbreakTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public static int MaxLimit = 0;
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_limitbreak_data" , "");
    }

    private void InsertData(string[] node)
    {
        limitbreakReferenceData new_data = new limitbreakReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.gold_cost = int.Parse(node[dataCount++]);
        new_data.stat_up = int.Parse(node[dataCount++]);

        MaxLimit = new_data.ReferenceID;

        Add(new_data);
    }


    public static limitbreakReferenceData GetData(int key)
    {
        LimitbreakTBL TBL = TBLManager.I.GetTable<LimitbreakTBL>(TABLELIST_TYPE.limitbreak);
        return (limitbreakReferenceData)TBL.Find(key);
    }
}