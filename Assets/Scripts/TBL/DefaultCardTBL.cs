using System.Collections.Generic;

public class DefaultCardTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public List<DefaultCardReferenceData> CardList = new List<DefaultCardReferenceData>();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_default_character", "");
    }

    private void InsertData(string[] node)
    {
        DefaultCardReferenceData new_data = new DefaultCardReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        dataCount++;
        dataCount++;
        new_data.CardID = int.Parse(node[dataCount++]);
        CardList.Add(new_data);
        Add(new_data);
    }


    public static List<DefaultCardReferenceData> GetData()
    {
        DefaultCardTBL TBL = TBLManager.I.GetTable<DefaultCardTBL>(TABLELIST_TYPE.DefaultCard);
        return TBL.CardList;
    }
}