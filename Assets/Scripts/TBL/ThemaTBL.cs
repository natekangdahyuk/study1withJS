using System.Collections.Generic;

public class ThemaTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public static int MinStageIndex = 10000;
    public static int MaxStageIndex = 0;

    public List<ThemaReferenceData> themaList = new List<ThemaReferenceData>();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_main_thema", "");
    }

    private void InsertData(string[] node)
    {
        ThemaReferenceData new_data = new ThemaReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.Difficulty = int.Parse( node[ dataCount++ ] );
        new_data.ThemaNo = int.Parse(node[dataCount++]);
        

        new_data.ThemaName = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.BackGroundPrefab = node[dataCount++];
        new_data.LobbyTexture = node[dataCount++];
        //dataCount++;
        new_data.ThemaReward[0] = int.Parse(node[dataCount++]);
        new_data.ThemaReward[1] = int.Parse(node[dataCount++]);
        new_data.ThemaReward[2] = int.Parse(node[dataCount++]);
        new_data.rewardstring = node[ dataCount++ ];

        if (MinStageIndex > new_data.ReferenceID)
            MinStageIndex = new_data.ReferenceID;

        if (MaxStageIndex < new_data.ReferenceID)
            MaxStageIndex = new_data.ReferenceID;


        if( new_data.ThemaNo > DefaultDataTBL.GetData( DefaultData.Final_Thema ) )
            return;

        Add(new_data);
        themaList.Add( new_data );
    }


    public static List<ThemaReferenceData> GetList()
    {
        ThemaTBL TBL = TBLManager.I.GetTable<ThemaTBL>( TABLELIST_TYPE.Thema );
        return TBL.themaList;
    }


    public static ThemaReferenceData GetData(int key)
    {
        ThemaTBL TBL = TBLManager.I.GetTable<ThemaTBL>(TABLELIST_TYPE.Thema);
        return (ThemaReferenceData)TBL.Find(key);
    }

    public static ThemaReferenceData GetDataByDifficulty(int index, int difficulty )
    {
        ThemaTBL TBL = TBLManager.I.GetTable<ThemaTBL>(TABLELIST_TYPE.Thema);

        foreach(var val in TBL._ReferenceContainer_By_Key)
        {
            if (((ThemaReferenceData)val.Value).ThemaNo == index && ((ThemaReferenceData)val.Value).Difficulty == difficulty)
                return (ThemaReferenceData)val.Value;
        }
        return null;
    }
}