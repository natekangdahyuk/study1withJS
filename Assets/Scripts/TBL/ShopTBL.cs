using System.Collections.Generic;

public class ShopTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_shop", "");		
    }

    private void InsertData(string[] node)
    {
        ShopReferenceData new_data = new ShopReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.shopType   = (ShopType)int.Parse(node[dataCount++]);
        dataCount++;

        new_data.productImg = node[dataCount++];
        new_data.productType = (ProductType)int.Parse(node[dataCount++]);
        new_data.productNum = int.Parse(node[dataCount++]);
        new_data.costType = (CostType)int.Parse(node[dataCount++]);
        new_data.costValue = int.Parse(node[dataCount++]);

        if( node.Length > dataCount )
        {
            new_data.product_code = node[ dataCount++ ];
            if( string.IsNullOrEmpty( new_data.product_code ) == false )
            {
                new_data.product_code = new_data.product_code.Trim();
            }
        }
        else
            new_data.product_code = "";


        new_data.rewardString = node[ dataCount++ ];
        Add(new_data);
        AddByGroup(new_data);
    }


    public static ShopReferenceData GetData(int key)
    {
        ShopTBL TBL = TBLManager.I.GetTable<ShopTBL>(TABLELIST_TYPE.Shop);
        return (ShopReferenceData)TBL.Find(key);
    }

    public static List<IReferenceDataByGroup> GetGroup(ShopType groupKey)
    {
        ShopTBL TBL = TBLManager.I.GetTable<ShopTBL>(TABLELIST_TYPE.Shop);

        List<IReferenceDataByGroup> list = TBL.FindByGroup(groupKey);

        return list;
    }
}