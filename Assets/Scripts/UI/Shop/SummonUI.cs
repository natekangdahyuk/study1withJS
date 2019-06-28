using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SummonUI : baseUI
{
    public enum SummonType
    {
        Default,
        bit,        
    }

    [SerializeField]
    Text mileageText;

    [SerializeField]
    GameObjectPool ObjectPool;

    [SerializeField]
    GameObjectPool ObjectPool2;
    [SerializeField]
    Text title;

    [SerializeField]
    ToggleGroup toggleGroup;

    [SerializeField]
    Toggle[] toggle;

    [SerializeField]
    GameObject[] group;

    [SerializeField]
    GameObject[] groupContent;

    [SerializeField]
    SummonItem[] bitItem;

    [SerializeField]
    SummonItem[] defautItem;

    [SerializeField]
    Text[] toggle_OnText;
    bool bInit = false;
    
    void start()
    {
        if (bInit == true)
            return;

        bInit = true;
        for (int i = 0; i < defautItem.Length; i++)
        {
            defautItem[i] = ObjectPool.New().GetComponent<SummonItem>();
            defautItem[i].Slot = i + 1;
            defautItem[i].gameObject.SetActive(true);
            defautItem[i].type = SummonItem.BitType.Default;

            for (int z = 0; z < defautItem[i].bittoggle.Length; z++)
                defautItem[i].bittoggle[z].group = null;

            defautItem[i].transform.SetParent(groupContent[0].transform);
        }

        for (int i = 0; i < bitItem.Length; i++)
        {
            bitItem[i] = ObjectPool2.New().GetComponent<SummonItem>();
            bitItem[i].Slot = i + 1;
            bitItem[i].gameObject.SetActive(true);
            bitItem[i].type = SummonItem.BitType.Bit;
            bitItem[i].transform.SetParent(groupContent[1].transform);
        }
    }

    public void OnHelp()
    {
        GlobalUI.ShowOKPupUp( StringTBL.GetData( 902128 ) );
    }

    public override void Init()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );

        start();
        group[0].SetActive(toggle[0].isOn);
        group[1].SetActive(!toggle[0].isOn);

        InitItem();
    }

    public void InitItem()
    {
        for( int i = 0 ; i < defautItem.Length ; i++ )
        {
            ApplyItem( i );
        }

        for( int i = 0 ; i < bitItem.Length ; i++ )
        {
            ApplyBitItem( i );            
        }
    }

    public void ApplyItem( int i )
    {
        if( i < PlayerData.I.SummonOpenCount )
        {
            if( PlayerData.I.SummonReady[ i ] == true )
                defautItem[ i ].Apply( SummonItem.GroupType.ready , i );
            else
                defautItem[ i ].Apply( SummonItem.GroupType.summon , i );
        }
        else
        {
            defautItem[ i ].SetLockCost( DefaultDataTBL.GetData( DefaultData.summonOpenCost_1 + i - 1 ) );
            defautItem[ i ].Apply( SummonItem.GroupType.Lock , i );
        }
    }

    public void ApplyBitItem( int i )
    {
        if( i < PlayerData.I.SummonBitOpenCount )
        {
            if( PlayerData.I.BitSummonReady[ i ] == true )
                bitItem[ i ].Apply( SummonItem.GroupType.ready , i );
            else
                bitItem[ i ].Apply( SummonItem.GroupType.summon , i );
        }
        else
        {
            bitItem[ i ].SetLockCost( DefaultDataTBL.GetData( DefaultData.summonBitOpenCost_1 + i - 1 ) );
            bitItem[ i ].Apply( SummonItem.GroupType.Lock , i );
        }
    }

    public void RefreshItem()
    {
        for( int i = 0 ; i < defautItem.Length ; i++ )
        {
            if( i < PlayerData.I.SummonOpenCount )
            {
                if( PlayerData.I.SummonReady[ i ] == true )
                    defautItem[ i ].Refresh( SummonItem.GroupType.ready , i );
                else
                    defautItem[ i ].Refresh( SummonItem.GroupType.summon , i );
            }
            else
            {
                defautItem[ i ].SetLockCost( DefaultDataTBL.GetData( DefaultData.summonOpenCost_1 + i - 1 ) );
                defautItem[ i ].Refresh( SummonItem.GroupType.Lock , i );
            }
        }

        for( int i = 0 ; i < bitItem.Length ; i++ )
        {
            if( i < PlayerData.I.SummonBitOpenCount )
            {
                if( PlayerData.I.BitSummonReady[ i ] == true )
                    bitItem[ i ].Refresh( SummonItem.GroupType.ready, i );
                else
                    bitItem[ i ].Refresh( SummonItem.GroupType.summon , i );
            }
            else
            {
                bitItem[ i ].SetLockCost( DefaultDataTBL.GetData( DefaultData.summonBitOpenCost_1 + i - 1 ) );
                bitItem[ i ].Refresh( SummonItem.GroupType.Lock , i );
            }
        }
    }

    public void Apply()
    {
        mileageText.text = PlayerData.I.mileage.ToString( "n0" );
        OnEnter();      
        
    }

    public override bool OnExit()
    {
        if (guide)
        {
            if (guide.gameObject.activeSelf)
            {
                guide.OnExit();
                return false;
            }
        }

        return base.OnExit();
    }


    TutorialItem guide = null;
    public void OnGuide()
    {
        if (guide == null)
        {
            guide = ResourceManager.Load<TutorialItem>(this.gameObject, "TutorialUI_main_summon");
            guide.action = HideGuide;
        }

        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.gameObject.SetActive(false);
        guide.gameObject.SetActive(true);
    }

    void HideGuide()
    {
        TopbarUI topbar = GlobalUI.GetUI<TopbarUI>(UI_TYPE.TopBarUI);
        topbar.gameObject.SetActive(true);
    }

    public void Refresh()
    {
        Init();

        
    }

    public void OnRecvSummon(int bit, int summon)
    {
        if( summon == 1)
            ApplyItem( bit -1);
        else
            ApplyBitItem( bit-1 );
        mileageText.text = PlayerData.I.mileage.ToString( "n0" );
    }
    
    public void OnSummon( int summontype )
    {
       
        for( int i =0; i < group.Length; i++)
        {
            group[i].SetActive(i == summontype ? true : false);
            toggle_OnText[ i ].gameObject.SetActive( i == summontype ? true : false );
        }
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }
}