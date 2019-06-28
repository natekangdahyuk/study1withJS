using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum MailType
{
    StageReward = 1,
    LevelUpReward = 2,
    DailyMission = 3,
    RankReward = 4,
    AchieveReward = 5,
    ThemaReward = 6,
    MileageReward = 7,
    ShopBuy = 8,
    Event = 9,

}

public class MailData
{
    public enum MailRewardType
    {
        gold = 1,
        ruby,
        ap,
        stone,
        ruby2,        
        mileage,
    }

    public Int64       UID;
    public MailRewardType    ItemType;
    public float       value;
    public bool        bExpire;
    public DateTime    ExpireTime;
    public bool        bNew = true;
    public string Title;

    public MailData( long uid , int itemType , float count , bool Expire , DateTime expireTime , string title )
    {
        UID = uid;
        ItemType = (MailRewardType)itemType;
        value = count;
        bExpire = Expire;
        ExpireTime = expireTime;
        Title = title;
    }
}

public class MailManager : Singleton<MailManager>
{
    public Dictionary<long,MailData> maliList = new Dictionary<long,MailData>();

    int oldCount = 0;
    bool bNew = false;
    public void Delete( long uid )
    {
        maliList.Remove( uid );        
    }

    public void StartMailList()
    {
        oldCount = maliList.Count;
    }

    public void Add( long uid, int itemType, float value, bool Expire, DateTime expireTime , string Title )
    {
        MailData data = Get( uid );

        if( data == null )
        {
            data = new MailData( uid , itemType , value , Expire , expireTime, Title );
            maliList.Add( uid , data );
        }        
    }
    
    public void EndMailList()
    {
        if( oldCount < maliList.Count )
            bNew = true;
    }

    public void SetNew( bool New )
    {
        bNew = New;
    }

    public bool IsNew()
    {
        return bNew;
    }

    public MailData Get( long uid )
    {
        MailData data = null;
        maliList.TryGetValue( uid , out data );
        return data;
    }
}