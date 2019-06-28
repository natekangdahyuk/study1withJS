using UnityEngine;
using System.Collections.Generic;

public class CharacterTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();

    Dictionary<int, List<CharacterReferenceData>> datalist = new Dictionary<int, List<CharacterReferenceData>>();

    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_char_base", "");
    }

    private void InsertData(string[] node)
    {
        CharacterReferenceData new_data = new CharacterReferenceData();

        int dataCount = 0;

        try
        {
            new_data.ReferenceID = int.Parse(node[dataCount++]);
            new_data.NameIndex = int.Parse(node[dataCount++]);
            dataCount++;
            new_data.type = int.Parse(node[dataCount++]);
            new_data.exp_multiply = int.Parse(node[dataCount++]);
            new_data.bit = int.Parse(node[dataCount++]);
            new_data.Class = (CLASS)int.Parse(node[dataCount++]);
            //dataCount++;
            new_data.star = int.Parse(node[dataCount++]);
            new_data.Rare = int.Parse(node[dataCount++]);
            new_data.Texture = node[dataCount++];
            new_data.DefaultSkin = int.Parse(node[dataCount++]);
            new_data.SellPrice = int.Parse(node[dataCount++]);
            FileReferenceLoader_Cvs.GetParseIntArrayByString(node[dataCount++], out new_data.SkinList);
            FileReferenceLoader_Cvs.GetParseStringArrayByString(node[dataCount++], out new_data.TouchSound);
            new_data.OneWord = node[dataCount++];
            new_data.OneWord = new_data.OneWord.Replace("/n", "\n");

            new_data.charInfo = node[dataCount++];
            new_data.charInfo = new_data.charInfo.Replace("/n", "\n");

            new_data.charCode = node[dataCount++];
            new_data.Code_Info = node[dataCount++];



        }
        catch (System.Exception e)
        {
            Debug.LogError("## Table Load Failed, " + node[dataCount - 1] + ", current index " + (dataCount - 1));
            Debug.LogError("## Exception : " + e.Message + ", inner " + e.InnerException);
        }

        Add(new_data);


        List<CharacterReferenceData> listdata = null;

        if (datalist.TryGetValue(new_data.bit, out listdata) == false)
        {
            listdata = new List<CharacterReferenceData>();
            datalist.Add(new_data.bit, listdata);
        }

        listdata.Add(new_data);
    }


    public static CharacterReferenceData GetData(int key)
    {
        CharacterTBL TBL = TBLManager.I.GetTable<CharacterTBL>(TABLELIST_TYPE.Character);
        return (CharacterReferenceData)TBL.Find(key);
    }

    public static List<CharacterReferenceData> GetList(int bit)
    {
        CharacterTBL TBL = TBLManager.I.GetTable<CharacterTBL>(TABLELIST_TYPE.Character);

        List<CharacterReferenceData> listdata = null;

        TBL.datalist.TryGetValue(bit, out listdata);


        return listdata;


    }



}