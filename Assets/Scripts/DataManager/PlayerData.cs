using UnityEngine;
using System;

public class PlayerData : Singleton<PlayerData>
{
    public string UserID = "";
    public string oLinktoken = "";
    public int UserIndex = 0;

    public int UserLevel = 99;
    public int Gold = 10000;
    public int Stone = 10000;
    public int Cash = 10000;
    public int shoes = 100;
    public int Maxshoes = 100;
    public float shoesSec = 0;
    public int Exp = 10;
    public float mileage = 0;

    public long representIndex = 10011;
    public int MaxDeckCount = 5;

    public int SummonOpenCount = 2;
    public int SummonBitOpenCount = 1;

    public int[] SummonSec = { 0, 0, 0, 0 };
    public StoneType[] SummonType = { 0, 0, 0, 0 };

    public int[] BitSummonSec = { 0, 0, 0, 0 };
    public StoneType[] BitSummonType = { 0, 0, 0, 0 };

    public bool[] SummonReady = { true, true, true, true };
    public bool[] BitSummonReady = { true, true, true, true };
    public DateTime ServerTime;

    public bool bFirstEnter = false;

    public DateTime CurrentTime { get { return ServerTime.AddSeconds((double)Time.realtimeSinceStartup); } }

    public void Summon(double remainTime, int slot, SummonItem.BitType type, int stone, StoneType summonType)
    {
        if (type == SummonItem.BitType.Default)
        {
            SummonSec[slot - 1] = (int)remainTime + (int)Time.realtimeSinceStartup;
            SummonReady[slot - 1] = false;
            SummonType[slot - 1] = summonType;
        }
        else
        {
            BitSummonSec[slot - 11] = (int)remainTime + (int)Time.realtimeSinceStartup;
            BitSummonReady[slot - 11] = false;
            BitSummonType[slot - 11] = summonType;
        }
        SetStone(stone);
    }

    public void SummonComplete(int slot, SummonItem.BitType type, int ruby)
    {
        if (type == SummonItem.BitType.Default)
        {
            SummonSec[slot - 1] = 0;
            SummonReady[slot - 1] = true;
        }
        else
        {
            BitSummonSec[slot - 11] = 0;
            BitSummonReady[slot - 11] = true;
        }

        SetRuby(ruby);
    }

    public void UseGold(int gold)
    {
        Gold -= gold;

        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.Apply();
    }

    public void SetStone(int stone)
    {
        Stone = stone;

        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        if (topbar)
            topbar.Apply();
    }

    public void Setmileage(float mil)
    {
        mileage = mil;

        //TopbarUI topbar = GlobalUI.GetUI<TopbarUI>( UI_TYPE.TopBarUI );
        //if( topbar )
        //    topbar.Apply();
    }


    public void SetRuby(int ruby)
    {
        Cash = ruby;

        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.Apply();
    }

    public void SetGold(int gold)
    {
        Gold = gold;

        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.Apply();
    }

    public void Setshoes(int value, bool bUpdate = true)
    {
        shoes = value;

        if (bUpdate)
        {
            shoesSec = Time.realtimeSinceStartup;
            TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
            topbar.Apply();
        }
    }

    public void Update()
    {
        if (Maxshoes <= shoes)
            return;

        float current = Time.realtimeSinceStartup - shoesSec;

        if (current >= 300)
        {
            int count = (int)current / 300;
            Setshoes(shoes + count);
            shoesSec = Time.realtimeSinceStartup + (int)current % 300;
        }

    }


}