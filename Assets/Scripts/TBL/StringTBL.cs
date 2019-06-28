using System.Collections.Generic;

public class StringTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public List<StringReferenceData> datalist = new List<StringReferenceData>();
    
    public void LoadData()
    {
		if(datalist.Count > 0)
		{
			datalist.Clear();
		}

        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
		_Loader.Load("table_local_string", "");
        _Loader.Load("table_base_string", "");
        _Loader.Load("table_char_string", "");
    }

    private void InsertData(string[] node )
    {
        StringReferenceData new_data = new StringReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        dataCount++;        
        new_data.text = node[dataCount++];
        dataCount++;        
        new_data.text = new_data.text.Replace("/n", "\n");
        Add(new_data);
    }

	public static bool IsEmpty
	{
		get {
			var tbl = TBLManager.I.GetTable<StringTBL>(TABLELIST_TYPE.text);
			if (tbl == null) return true;
			return tbl.datalist.Count == 0;
		}
	}

    public static string GetData(int key)
    {
        StringTBL TBL = TBLManager.I.GetTable<StringTBL>(TABLELIST_TYPE.text);
        StringReferenceData data = ((StringReferenceData)TBL.Find(key));

        if (data == null)
            return "";
        return data.text;
    }

	public static void Load()
	{
		StringTBL tbl = TBLManager.I.GetTable<StringTBL>(TABLELIST_TYPE.text);
		if(tbl != null)
		{
			tbl.datalist.Clear();
		}

		tbl.LoadData();
	}
}