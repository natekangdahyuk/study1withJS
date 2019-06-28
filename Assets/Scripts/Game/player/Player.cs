using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class Player
{
    public int Hp = 0;
    public int MaxHp = 0;

    public List<ItemData> ItemList = new List<ItemData>();
    public void GameStart()
    {
        Hp = MaxHp;
    } 

    public void SetDamage( int damage )
    {        
        Hp = Math.Max(0, Hp - damage);
    }

    public void SetHeal( int value )
    {
        Hp = Math.Min( MaxHp , Hp + value );
    }

    public void ApplyData( int hp , List<ItemData> itemList )
    {
        MaxHp = hp;
        Hp = hp;
        for (int i = 0; i < itemList.Count; i++)
        {
            ItemList.Add(itemList[i]);
        }

    }
}

