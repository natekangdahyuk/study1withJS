using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class Summon_groupTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_summon_group", "");
    }

    private void InsertData(string[] node)
    {
        SummonGroupReferenceData new_data = new SummonGroupReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.summonType = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.stone_cost = int.Parse(node[dataCount++]);
        new_data.bit_cost = int.Parse(node[dataCount++]);
        new_data.stone_num = int.Parse(node[dataCount++]);

        new_data.stone_formula = node[dataCount++];
        new_data.summon_group = node[dataCount++];
        new_data.grou_ratio = node[dataCount++];

        Add(new_data);
        
    }


    public static SummonGroupReferenceData GetDataByfomula(string formula , int count )
    {
        Summon_groupTBL TBL = TBLManager.I.GetTable<Summon_groupTBL>(TABLELIST_TYPE.SummonGroup);

        SummonGroupReferenceData outData = null;
        foreach ( KeyValuePair< string, IReferenceDataByKey > value in TBL._ReferenceContainer_By_Key)
        {
            SummonGroupReferenceData data = value.Value as SummonGroupReferenceData;

            if(data.stone_num == count )
            {                

                if(data.stone_formula == formula)
                {
                    return data;
                }
                else if( data.stone_formula == "0")
                {
                    outData = data;
                }
            }
        }
        return outData;
    }

    public static SummonGroupReferenceData GetDataByBit(int bit)
    {
        Summon_groupTBL TBL = TBLManager.I.GetTable<Summon_groupTBL>(TABLELIST_TYPE.SummonGroup);
                
        foreach (KeyValuePair<string, IReferenceDataByKey> value in TBL._ReferenceContainer_By_Key)
        {
            SummonGroupReferenceData data = value.Value as SummonGroupReferenceData;

            if (data.bit_cost == bit)
            {
                return data;
            }
        }
        return null;
    }
}