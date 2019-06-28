using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileDebuff
{
    public ActionType tileDebuffState = ActionType.None;
    public int DebuffValue = 0;
    public Action EndDebuff;

    public void SetDebuff( ActionType type , int value)
    {
        tileDebuffState = type;
        DebuffValue = value;
    }
    public void ReduceDebuffTime()
    {
        if (tileDebuffState == ActionType.None)
            return;

        if (DebuffValue > 0)
            DebuffValue--;

        if (DebuffValue <= 0)
        {
            EndDebuff();
            tileDebuffState = ActionType.None;            
        }
    }

    public void Init()
    {
        tileDebuffState = ActionType.None;
        DebuffValue = 0;
    }
}