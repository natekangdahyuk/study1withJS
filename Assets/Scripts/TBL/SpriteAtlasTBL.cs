using System.Collections.Generic;

public class SpriteAtlasTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_atlas", "");
    }

    private void InsertData(string[] node)
    {
        SpriteAtlasData data = new SpriteAtlasData();

        int dataCount = 1;
        data.AtlasName = node[dataCount++];
		data.Preload = int.Parse(node[dataCount++]) == 1 ? true : false;
        Add(data);
    }


    public static SpriteAtlasData GetData(int key)
    {
		SpriteAtlasTBL TBL = TBLManager.I.GetTable<SpriteAtlasTBL>(TABLELIST_TYPE.SpriteAtlas);
        return (SpriteAtlasData)TBL.Find(key);
    }

	public List<SpriteAtlasData> GetList()
	{
		if(_ReferenceContainer_By_Key.Count > 0)
		{
			var list = new List<SpriteAtlasData>();
			var enumer = _ReferenceContainer_By_Key.GetEnumerator();
			while (enumer.MoveNext())
			{
				list.Add(enumer.Current.Value as SpriteAtlasData);
			}

			return list;
		}

		return null;
	}
}