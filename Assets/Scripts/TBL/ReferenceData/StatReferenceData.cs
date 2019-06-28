using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public enum STAT
{
    Attack = 0,
    Defence,
    Hp,
    Heal,
    Bonus_Attack,
    Bonus_Defence,
    Bonus_Hp,
    Bonus_Heal,
    MAX, 
}

public class StatReferenceData : IReferenceDataByKey, IReferenceDataByGroup
{
    public object GetKey()
    {
        return ReferenceID;
    }

    public object GetGroupKey()
    {
        return CardIndex;
    }

    public int ReferenceID;

    public int CardIndex;

    public int Star;

    public int[] Stat = new int[(int)STAT.MAX];
}
