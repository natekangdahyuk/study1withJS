using UnityEngine;
using System.Collections.Generic;

public class CollectionTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    Dictionary<int, List<CollectionReferenceData>> datalist = new Dictionary<int, List<CollectionReferenceData>>();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_char_collection", "");
    }

    private void InsertData(string[] node)
    {
        CollectionReferenceData new_data = new CollectionReferenceData();

        int dataCount = 0;

        try
        {
            new_data.ReferenceID = int.Parse(node[dataCount++]);
            new_data.bit = int.Parse(node[dataCount++]);
           
            new_data.characterIndex = int.Parse(node[dataCount++]);
        }
        catch (System.Exception e)
        {
            Debug.LogError("## Table Load Failed, " + node[dataCount - 1] + ", current index " + (dataCount - 1));
            Debug.LogError("## Exception : " + e.Message + ", inner " + e.InnerException);
        }

        Add(new_data);


        List<CollectionReferenceData> listdata = null;

        if (datalist.TryGetValue(new_data.bit, out listdata) == false)
        {
            listdata = new List<CollectionReferenceData>();
            datalist.Add(new_data.bit, listdata);
        }

        listdata.Add(new_data);
    }


    public static CollectionReferenceData GetData(int key)
    {
        CollectionTBL TBL = TBLManager.I.GetTable<CollectionTBL>(TABLELIST_TYPE.Collection);
        return (CollectionReferenceData)TBL.Find(key);
    }

    public static List<CollectionReferenceData> GetList(int bit)
    {
        CollectionTBL TBL = TBLManager.I.GetTable<CollectionTBL>(TABLELIST_TYPE.Collection);

        List<CollectionReferenceData> listdata = null;

        TBL.datalist.TryGetValue(bit, out listdata);


        return listdata;


    }



}