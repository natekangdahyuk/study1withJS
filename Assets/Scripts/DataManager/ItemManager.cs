using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ItemType
{
    Combine = 0,
    Upgrade,
    Return,
}

public struct ItemData
{
   
    public ItemType Type;
    public int Count;

    public ItemData( ItemType type , int count )
    {
        Type = type;
        Count = count;
    }
}

public class ItemManager : Singleton<ItemManager>
{
    public List<ItemData> ItemList = new List<ItemData>();
    public void TestItem()
    {
        ItemList.Add(new ItemData(ItemType.Combine, 1));
        ItemList.Add(new ItemData(ItemType.Return, 1));
        ItemList.Add(new ItemData(ItemType.Upgrade, 1));
    }
}
