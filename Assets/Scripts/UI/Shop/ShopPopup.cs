using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using IgaworksUnityAOS;

public class ShopPopup : baseUI
{
    public enum SHOPTYPE
    {
        ruby = 0,
        gold,
        ap,
        stone,
    }

    [SerializeField]
    Toggle[] toggle;

    [SerializeField]
    GameObject[] toggle_OnText;

    [SerializeField]
    GameObject[] content;

    [SerializeField]
    GameObjectPool ObjectPool;

    ShopGroup[] group = new ShopGroup[4];

    void Awake()
    {
        ObjectPool.Init();
        for (int i = 0; i < group.Length; i++)
        {
            group[i] = new ShopGroup();
            group[i].Init(content[i], ObjectPool, (ShopType)(i + 1));
        }
    }

    private void Start()
    {
        RectTransform form = GetComponent<RectTransform>();
        form.anchoredPosition = Vector2.zero;
        form.sizeDelta = Vector2.zero;
    }

    public override void Init()
    {

    }

    public void Apply( SHOPTYPE type = SHOPTYPE.ruby )
    {
#if USE_SNS_LOGIN
		IgaworksUnityPluginAOS.Adbrix.firstTimeExperience("ShopEnter");
#endif

		TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );
        OnEnter();

        toggle[ (int)type ].isOn = true;
        gameObject.SetActive(true);
        OnChangeToggle();
	}
	
    public void OnChangeToggle()
    {
        for( int i =0; i < toggle.Length; i++)
        {
            group[i].SetActive(toggle[i].isOn);
            toggle_OnText[i].SetActive(toggle[i].isOn);
        }

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );

        if( toggle[ 3 ].isOn )
            topbar.ShowStone( true );
        else
            topbar.ShowStone( false );

    }

    public void OnOut()
    {
        OnExit();
    }

}

public class ShopGroup
{
    GameObject Top;
    ShopItem[] item;
        
    public void SetActive(bool active)
    {
        Top.SetActive(active);
    }
    public void Init( GameObject top , GameObjectPool ObjectPool, ShopType type )
    {
        Top = top;

        List<IReferenceDataByGroup> list = ShopTBL.GetGroup(type);

        item = new ShopItem[list.Count];
        for ( int i =0; i < list.Count; i++)
        {
            GameObject go = ObjectPool.New();
            go.SetActive(true);
            go.transform.SetParent(Top.transform);
            item[i] = go.GetComponent<ShopItem>();
            item[i].Apply((ShopReferenceData)list[i], i+1);
        }
        
    }
}
