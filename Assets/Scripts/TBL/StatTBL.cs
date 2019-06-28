using System.Collections.Generic;
using UnityEngine;
public class StatTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_char_stat", "");
    }

    private void InsertData(string[] node)
    {
        StatReferenceData new_data = new StatReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.CardIndex = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.Star = int.Parse(node[dataCount++]);

        for (int i = 0; i < new_data.Stat.Length; i++)
            new_data.Stat[i] = int.Parse(node[dataCount++]);

        Add(new_data);
        AddByGroup(new_data);
    }


    public static StatReferenceData GetData(int key, int star )
    {
        StatTBL TBL = TBLManager.I.GetTable<StatTBL>(TABLELIST_TYPE.Stat);

        List<IReferenceDataByGroup> list = TBL.FindByGroup(key);

        for( int i =0; i < list.Count; i++)
        {
            StatReferenceData data = (StatReferenceData)list[i];
            if (data.Star == star)
            {
                return data;
            }
        }

        Debug.LogError(string.Format("TBL Error key: {0}  star : {1}", key, star));
        return null;
    }
}