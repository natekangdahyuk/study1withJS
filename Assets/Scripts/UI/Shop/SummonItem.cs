using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum StoneType
{
    Default = 0,
    gold,


}
public class SummonItem : MonoBehaviour
{
    public enum GroupType
    {
        ready = 0,
        summon,
        Lock,

    }

    public enum BitType
    {
        Default = 1,
        Bit,
    }

    [SerializeField]
    Text cost;

    [SerializeField]
    Text Lockcost;

    [SerializeField]
    Text Completecost;

    [SerializeField]
    public Toggle[] bittoggle;

    [SerializeField]
    public Toggle[] stonetoggle;

    [SerializeField]
    GameObject[] Group;

    [SerializeField]
    Text RemainTime;

    [SerializeField]
    Text CompleteText;

    [SerializeField]
    Button summon;

    [SerializeField]
    Image[] BtnStone;

    [SerializeField]
    RawImage[] SummonStoneCheckMark;

    [SerializeField]
    Button Complete;

    [SerializeField]
    Button Lock;


    [SerializeField]
    GameObject[] SubGroup;

    public CanvasGroup optionGroup;
    GroupType currentGroupType = GroupType.ready;
    public BitType type = BitType.Default;

    public int Slot = 0;
    public int SlotIndex = 0;
    string formula = "";
    int bit = 0;
    StoneType stonetype = StoneType.Default;
    public int[] bitValue = new int[10];
    int CurrentSummonCost = 0;
    int CurrentCompleteValue = -10000;

    public void OnSummon()
    {

        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);

        if (stonetype == StoneType.Default)
        {
            if (CurrentSummonCost > PlayerData.I.Stone)
            {
                GlobalUI.ShowOKCancelPupUp(StringTBL.GetData(800013), OnShop); // 
                return;
            }
        }
        else
        {
            if (CurrentSummonCost > PlayerData.I.mileage)
            {
                GlobalUI.ShowOKPupUp(StringTBL.GetData(902183));
                return;
            }
        }



        if (type == BitType.Default)
        {
            int count = 0;

            for (int i = 0; i < bittoggle.Length; i++)
            {
                if (bitValue[i] == 1)
                    count++;
            }

            if (count == 0)
            {
                GlobalUI.ShowOKPupUp(StringTBL.GetData(902026)); //! 소환석 배치해야됨
                return;
            }
            GameOption.SaveBit(bitValue, SlotIndex, (int)stonetype);
            NetManager.Summon((byte)BitType.Default, (byte)Slot, 0, (byte)count, formula, stonetype);
        }
        else
        {
            if (bit == 0)
            {
                GlobalUI.ShowOKPupUp(StringTBL.GetData(902026));
                return;
            }
            NetManager.Summon((byte)BitType.Bit, (byte)(10 + Slot), bit, (byte)0, "", stonetype);
        }
    }

    public void Refresh(GroupType Grouptype, int index)
    {
        currentGroupType = Grouptype;

        for (int i = 0; i < Group.Length; i++)
        {
            Group[i].SetActive(i == (int)Grouptype);
        }

        if (Grouptype == GroupType.Lock)
        {
            if (type == BitType.Default)
                Lockcost.text = DefaultDataTBL.GetData(DefaultData.summonOpenCost_1 + Slot - 2).ToString("n0");
            else
                Lockcost.text = DefaultDataTBL.GetData(DefaultData.summonBitOpenCost_1 + Slot - 2).ToString("n0");

            optionGroup.gameObject.SetActive(false);
        }
        else if (Grouptype == GroupType.summon)
        {
            UpdateTime();

            optionGroup.gameObject.SetActive(true);

            optionGroup.interactable = false;
            optionGroup.alpha = 0.5f;
        }
        else
        {
            optionGroup.gameObject.SetActive(true);

            optionGroup.interactable = true;
            optionGroup.alpha = 1;
        }
    }


    public void Apply(GroupType Grouptype, int index)
    {
        Refresh(Grouptype, index);
        SlotIndex = index;

        if (type == BitType.Default)
        {
            if (Grouptype != GroupType.ready)
                stonetype = PlayerData.I.SummonType[Slot - 1];
            else
                stonetype = (StoneType)GameOption.bitType[SlotIndex];

            for (int i = 0; i < bittoggle.Length; i++)
            {
                if (GameOption.bitValue[SlotIndex, i] == 1)
                {
                    bitValue[i] = 1;
                    bittoggle[i].isOn = true;
                }
                else
                {
                    bittoggle[i].isOn = false;
                    bitValue[i] = 0;
                }
            }
            SetBitCost();
        }
        else
        {
            if (Grouptype != GroupType.ready)
                stonetype = PlayerData.I.BitSummonType[Slot - 1];
        }


        if (stonetype == StoneType.Default)
        {
            stonetoggle[0].isOn = true;
            stonetoggle[1].isOn = false;
        }
        else
        {
            stonetoggle[1].isOn = true;
            stonetoggle[0].isOn = false;
        }

        ChangeStoneType();
    }

    public void UpdateTime()
    {
        int remainTime = 0;
        if (type == BitType.Default)
        {
            remainTime = PlayerData.I.SummonSec[Slot - 1] - (int)Time.realtimeSinceStartup;
        }
        else
        {
            remainTime = PlayerData.I.BitSummonSec[Slot - 1] - (int)Time.realtimeSinceStartup;
        }

        if (remainTime <= 0)
        {
            //CompleteText.text = "완료";
            //RemainTime.gameObject.SetActive(false);
            //Completecost.gameObject.SetActive(false);
            //CurrentCompleteValue = 0;

            CurrentCompleteValue = -10000;
            SubGroup[0].gameObject.SetActive(false);
            SubGroup[1].gameObject.SetActive(true);
        }
        else
        {
            int time = DefaultDataTBL.GetData(DefaultData.char_summons_base_time);

            if (remainTime < time * 60)
            {
                CurrentCompleteValue = DefaultDataTBL.GetData(DefaultData.char_summons_base_time_cost);
            }
            else
            {
                CurrentCompleteValue = Mathf.FloorToInt(((float)remainTime / (DefaultDataTBL.GetData(DefaultData.char_summons_add_time) * 60))) * DefaultDataTBL.GetData(DefaultData.char_summons_add_time_cost);

            }



            SubGroup[0].gameObject.SetActive(true);
            SubGroup[1].gameObject.SetActive(false);
            //CompleteText.text = "즉시 완료";
            RemainTime.gameObject.SetActive(true);
            Completecost.gameObject.SetActive(true);
            Completecost.text = CurrentCompleteValue.ToString("n0");

            RemainTime.text = UIUtil.GetTimeEx(remainTime);
        }

    }

    void FixedUpdate()
    {
        UpdateTime();
    }

    public void SetLockCost(int cost)
    {
        Lockcost.text = cost.ToString();
    }
    public void Awake()
    {
        //OnToggleSummon(0);
    }

    public void OnLock()
    {
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
        int cash = 0;
        if (type == BitType.Default)
        {
            cash = DefaultDataTBL.GetData(DefaultData.summonOpenCost_1 + Slot - 2);
        }
        else
        {
            cash = DefaultDataTBL.GetData(DefaultData.summonBitOpenCost_1 + Slot - 2);
        }

        if (PlayerData.I.Cash < cash)
        {
            GlobalUI.ShowOKCancelPupUp(StringTBL.GetData(800014), OnShop); // 루비부족
            return;
        }

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>(UI_TYPE.PopupOkCancel);
        popup.OnEnter();
        popup.SetEx(StringTBL.GetData(902028), cash.ToString("n0"), OnUnLockOk, null, false, PopupOkCancel.SubType.Ruby);


    }

    public void OnUnLockOk()
    {
        if (type == BitType.Default)
            PlayerData.I.SummonOpenCount++;
        else
            PlayerData.I.SummonBitOpenCount++;
        NetManager.AddSummonSlot((int)type, DefaultDataTBL.GetData(DefaultData.summonOpenCost_1 + Slot - 2));
        //NetManager.SummonComplete((byte)type, (byte)Slot, CurrentCompleteValue);
    }

    public void OnComplete() //! 즉시완료
    {
        if (PlayerData.I.Cash < CurrentCompleteValue)
        {
            GlobalUI.ShowOKCancelPupUp(StringTBL.GetData(800014), OnShop); // 루비부족
            return;
        }


        if (CurrentCompleteValue > 0)
        {
            PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>(UI_TYPE.PopupOkCancel);
            popup.OnEnter();
            popup.SetEx(StringTBL.GetData(902024), CurrentCompleteValue.ToString("n0"), OnCompleteOk, null, false, PopupOkCancel.SubType.Ruby);

            //GlobalUI.ShowOKCancelPupUp(String.Format(StringTBL.GetData(902024), CurrentCompleteValue.ToString("n0")), OnCompleteOk); // 즉시완료
        }
        else
            OnCompleteOk();

        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void OnShop()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.Onshop();
    }

    public void OnCompleteOk()
    {
        if (InventoryManager.I.IsMaxCount())
        {
            GlobalUI.ShowOKPupUp("카드 인벤토리가 가득 차서 더 이상 소환할 수 없습니다.");
            return;
        }

        if (type == BitType.Default)
        {
            NetManager.SummonComplete((byte)type, (byte)Slot, CurrentCompleteValue);
        }
        else
            NetManager.SummonComplete((byte)type, (byte)(10 + Slot), CurrentCompleteValue);

    }

    void SetBitCost()
    {
        formula = "";
        int count = 0;
        for (int i = 0; i < bittoggle.Length; i++)
        {
            bitValue[i] = bittoggle[i].isOn ? 1 : 0;
            formula += bitValue[i].ToString();

            if (bitValue[i] == 1)
                count++;
        }

        if (count == 0)
        {
            cost.text = "-";
            return;
        }

        SummonGroupReferenceData data = Summon_groupTBL.GetDataByfomula(formula, count);
        cost.text = data.stone_cost.ToString();
        CurrentSummonCost = data.stone_cost;
    }

    public void OnToggleSummon(int index)
    {
        CurrentSummonCost = 0;
        if (type == BitType.Default)
        {
            SetBitCost();
        }
        else
        {
            bit = 0;
            for (int i = 0; i < bittoggle.Length; i++)
            {
                if (bittoggle[i].isOn)
                {
                    bit = (int)Math.Pow(2, i + 2);

                    SummonGroupReferenceData data = Summon_groupTBL.GetDataByBit(bit);
                    CurrentSummonCost = data.stone_cost;
                    cost.text = data.stone_cost.ToString();
                }
            }
        }
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_summon_insertstone", GameOption.EffectVoluem);
    }

    public void OnDefaultStone()
    {
        stonetype = StoneType.Default;

        if (type == BitType.Default)
            PlayerData.I.SummonType[Slot - 1] = stonetype;
        else
            PlayerData.I.BitSummonType[Slot - 1] = stonetype;

        ChangeStoneType();
    }

    public void OnGoldStone()
    {
        stonetype = StoneType.gold;
        if (type == BitType.Default)
            PlayerData.I.SummonType[Slot - 1] = stonetype;
        else
            PlayerData.I.BitSummonType[Slot - 1] = stonetype;

        ChangeStoneType();
    }

    void ChangeStoneType()
    {
        if (stonetype == StoneType.Default)
        {
            BtnStone[0].gameObject.SetActive(true);
            BtnStone[1].gameObject.SetActive(false);

            for (int i = 0; i < SummonStoneCheckMark.Length; i++)
                SummonStoneCheckMark[i].texture = Resources.Load("UIResource/btn_icon_summon_slot_insert") as Texture;
        }
        else
        {
            BtnStone[1].gameObject.SetActive(true);
            BtnStone[0].gameObject.SetActive(false);

            for (int i = 0; i < SummonStoneCheckMark.Length; i++)
                SummonStoneCheckMark[i].texture = Resources.Load("UIResource/btn_icon_summon_slot_insert2") as Texture;
        }
    }

}
