using System.Collections.Generic;

public class SkinTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_char_skin", "");
    }

    private void InsertData(string[] node)
    {
        SkinReferenceData new_data = new SkinReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.texture = node[dataCount++];
        new_data.Live2DModel = node[dataCount++];
        new_data.Live2DBG = node[dataCount++];
        FileReferenceLoader_Cvs.GetParseStringArrayByString( node[ dataCount++ ] , out new_data.Sound );
        new_data.NameString = int.Parse( node[ dataCount++ ] );
        dataCount++;

        new_data.ablityString = int.Parse( node[ dataCount++ ] );
        dataCount++;

        new_data.ablityType = ( ABILITYTYPE)int.Parse( node[ dataCount++ ] );
        new_data.ablityValue = int.Parse( node[ dataCount++ ] );
        new_data.UnLockString = int.Parse( node[ dataCount++ ] );
        dataCount++;

        new_data.UnLockType = ( UnLockType)int.Parse( node[ dataCount++ ] );
        new_data.UnLockValue = int.Parse( node[ dataCount++ ] );
        new_data.Cost = int.Parse( node[ dataCount++ ] );
        new_data.oneWord = node[dataCount++];
        new_data.oneWord = new_data.oneWord.Replace( "/n" , "\n" );

        Add(new_data);
    }


    public static SkinReferenceData GetData(int key)
    {
        SkinTBL TBL = TBLManager.I.GetTable<SkinTBL>(TABLELIST_TYPE.Skin);
        return (SkinReferenceData)TBL.Find(key);
    }
}