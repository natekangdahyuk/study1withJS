using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class LoadingStringTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    public List<LoadingStringReferenceData> datalist = new List<LoadingStringReferenceData>();

    public void LoadData()
    {
        if (datalist.Count > 0)
        {
            datalist.Clear();
        }

        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_loading_string", "");

    }

    private void InsertData(string[] node)
    {
        LoadingStringReferenceData new_data = new LoadingStringReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        dataCount++;
        new_data.text = node[dataCount++];
        dataCount++;
        new_data.text = new_data.text.Replace("/n", "\n");

        datalist.Add(new_data);
        Add(new_data);
    }

    public static bool IsEmpty
    {
        get
        {
            var tbl = TBLManager.I.GetTable<LoadingStringTBL>(TABLELIST_TYPE.LoadingString);
            if (tbl == null)
                return true;
            return tbl.datalist.Count == 0;
        }
    }

    public static string GetRandomData()
    {
        LoadingStringTBL TBL = TBLManager.I.GetTable<LoadingStringTBL>(TABLELIST_TYPE.LoadingString);
        int rand = Random.Range(0, TBL.datalist.Count - 1);
        return TBL.datalist[rand].text;

    }

    public static void Load()
    {
        LoadingStringTBL tbl = TBLManager.I.GetTable<LoadingStringTBL>(TABLELIST_TYPE.LoadingString);
        if (tbl != null)
        {
            tbl.datalist.Clear();
        }

        tbl.LoadData();
    }
}