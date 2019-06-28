using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


public class FileReferenceLoader_Cvs : IReferenceLoader
{

    public bool Load(string patch, string table_name)
    {

        TextAsset text_asset = (TextAsset)ResourceManager.GetTable(patch);

        if (null == text_asset)
            return false;

        MemoryStream ms = new MemoryStream(text_asset.bytes);
        StreamReader reader = new StreamReader(ms, Encoding.UTF8);

        string strLine = null;
        int idx = 0;
        while (!reader.EndOfStream)
        {
            strLine = reader.ReadLine();

            string[] textArray = strLine.Split(","[0]);

            if (idx <= 0)
            {
                idx++;
                continue;
            }


            _InsertDataHandlerDelete(textArray);

        }

        reader.Close();
        ms.Close();
        return true;
    }

    public static void GetParseIntArrayByString(string node, out int[] value)
    {
        string[] textArray = node.Split(";"[0]);
        value = new int[textArray.Length];

        for (int i = 0; i < value.Length; i++)
        {
            value[i] = int.Parse(textArray[i]);
        }

    }

    public static void GetParseStringArrayByString(string node, out string[] value)
    {
        string[] textArray = node.Split(";"[0]);
        value = new string[textArray.Length];

        for (int i = 0; i < value.Length; i++)
        {
            value[i] = textArray[i];
        }

    }

    public InsertDataHandlerDelete_Cvs InsertData_Event_Cvs
    {
        set
        {
            _InsertDataHandlerDelete = value;
        }
    }


    public InsertDataHandlerDelete InsertData_Event
    {
        set
        {
        }
    }
    InsertDataHandlerDelete_Cvs _InsertDataHandlerDelete = null;


}


