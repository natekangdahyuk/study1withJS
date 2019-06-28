using System.Collections.Generic;

public class ExpUserTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    int MaxLevel = 0;
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_exp_user", "");
    }

    private void InsertData(string[] node)
    {
        ExpUserReferenceData new_data = new ExpUserReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.exp = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.ap_Max = int.Parse(node[dataCount++]);
        new_data.apReward = int.Parse(node[dataCount++]);
        //new_data.goldReward = int.Parse(node[dataCount++]);
        new_data.CashReward = int.Parse(node[dataCount++]);
        MaxLevel = new_data.ReferenceID;
        Add(new_data);
    }


    public static ExpUserReferenceData GetData(int key)
    {
        ExpUserTBL TBL = TBLManager.I.GetTable<ExpUserTBL>(TABLELIST_TYPE.ExpUser);
        return (ExpUserReferenceData)TBL.Find(key);
    }

    public static ExpUserReferenceData GetDataByExp(int exp)
    {
        ExpUserTBL TBL = TBLManager.I.GetTable<ExpUserTBL>(TABLELIST_TYPE.ExpUser);

        
        foreach (KeyValuePair<string, IReferenceDataByKey> value in TBL._ReferenceContainer_By_Key)
        {            
            if (((ExpUserReferenceData)value.Value).exp > exp)
            {
                return (ExpUserReferenceData)value.Value;
            }

        }


        return GetData(TBL.MaxLevel);
    }
}