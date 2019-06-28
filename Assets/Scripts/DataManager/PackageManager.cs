using UnityEngine;
using System.Collections.Generic;

public class PackageData
{
    public int uid;
    public int ruby;
    public int stone;
    public int gold;
    public string prdCode;
    public int maxCnt;
    public int buyCount = 0;
    public int price;

    public string TitleImage;
    public string BGImage;

    public bool IsBuy()
    {
        if( buyCount >= maxCnt )
            return false;

        return true;
    }

    public bool Buy()
    {
        return false;
    }
}

public class PackageManager : Singleton<PackageManager>
{
    public List<PackageData> PackageList = new List<PackageData>();

    public void AddPackage( int uid , int ruby, int stone, int gold , string code , int maxcnt , int count )
    {
        PackageData data = new PackageData();

        data.uid = uid;
        data.ruby = ruby;
        data.stone = stone;
        data.gold = gold;
        
        data.maxCnt = maxcnt;
        data.buyCount = count;


        PackageReferenceData packdata = PackageTBL.GetData( uid );

        data.prdCode = packdata.productcode;
        data.price = packdata.cost;
        data.TitleImage = packdata.productImg;
        data.BGImage = packdata.productBGImg;
        PackageList.Add( data );
    }



    public void SetBuyPackage( int uid )
    {
        for( int i =0 ; i < PackageList.Count ; i++ )
        {
            if( PackageList[i].uid  == uid )
            {
                PackageList[ i ].buyCount++;
            }
        }
    }
}
