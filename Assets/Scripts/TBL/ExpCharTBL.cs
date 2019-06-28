using System.Collections.Generic;

public class ExpCharTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    int MaxLevel = 0;
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_exp_char", "");
    }

    private void InsertData(string[] node)
    {
        ExpCharReferencdData new_data = new ExpCharReferencdData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.exp = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.lvup_cost = int.Parse(node[dataCount++]);
        new_data.material_exp = int.Parse(node[dataCount++]);

        MaxLevel = new_data.ReferenceID;
        Add(new_data);
    }


    public static ExpCharReferencdData GetData(int key)
    {
        ExpCharTBL TBL = TBLManager.I.GetTable<ExpCharTBL>(TABLELIST_TYPE.ExpChar);
        return (ExpCharReferencdData)TBL.Find(key);
    }

    public static ExpCharReferencdData GetLevelByExp(int Exp)
    {
        ExpCharTBL TBL = TBLManager.I.GetTable<ExpCharTBL>(TABLELIST_TYPE.ExpChar);

        foreach( KeyValuePair< string, IReferenceDataByKey > value in TBL._ReferenceContainer_By_Key )
        {
            if( ((ExpCharReferencdData)value.Value).exp > Exp )
            {
                return ((ExpCharReferencdData)value.Value);
            }
        }
        return GetData(TBL.MaxLevel);
    }
}