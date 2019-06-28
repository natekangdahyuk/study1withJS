using System.Collections.Generic;

public enum DefaultData
{
    DefaultGold = 1,
    Default_Cash,
    lvup_Cost_base,
    defaultAp_chargeTime,
    LevelUpCardMaxCount,
    ExpBonue,
    summonOpenCost_1,
    summonOpenCost_2,
    summonOpenCost_3,
    summonBitOpenCost_1,
    summonBitOpenCost_2,
    summonBitOpenCost_3,
    summon_stoneCost ,
    field_stage_hard_open,
    field_stage_hell_open,
    char_summons_base_time,
    char_summons_add_time,
    char_summons_base_time_cost,
    char_summons_add_time_cost,
    inven_default_value,
    inven_max_value,
    inven_extend_value,
    inven_extend_cost,
    Final_Thema,
}

public class DefaultDataTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
        
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_default_data", "");
    }

    private void InsertData(string[] node)
    {
        DefaultDataReferenceData new_data = new DefaultDataReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        dataCount++;
        dataCount++;
        new_data.data = int.Parse(node[dataCount++]);    
        Add(new_data);
    }


    public static int GetData( DefaultData type )
    {
        DefaultDataTBL TBL = TBLManager.I.GetTable<DefaultDataTBL>(TABLELIST_TYPE.DefaultData);
        DefaultDataReferenceData data = (DefaultDataReferenceData)TBL.Find((int)type);
        return data.data;
    }
}