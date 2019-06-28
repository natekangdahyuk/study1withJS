using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IAP
using UnityEngine.Purchasing;
#endif

public class ShopPackagePopup : baseUI
{
    public Button[] btn = new Button[3];

    public ShopPackageItem[] item;

    public override void Init()
    {

    }

    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }
       
    public void Apply()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );
        OnEnter();

        for( int i=0 ; i <  PackageManager.I.PackageList.Count ; i++)
        {
            item[ i ].Apply( PackageManager.I.PackageList[ i ] );
            item[ i ].onBuy = OnBuy;
        }
    }

    public void OnRecvBuyPackage( int uid )
    {
        PackageManager.I.SetBuyPackage( uid );
        for( int i = 0 ; i < PackageManager.I.PackageList.Count ; i++ )
        {
            item[ i ].Apply( PackageManager.I.PackageList[ i ] );         
        }
    }

    IEnumerator coPurchase( PackageData _data )
    {        
        bool success = false;
        yield return StartCoroutine( PurchaseManager.I.InGamePurchase( _data.prdCode , ( result ) =>
        {
            success = result;
        } ) );

        if( success == true )
        {
            NetManager.BuyPackage( _data.uid );
            CheckBtn( _data );
        }
    }

    public void CheckBtn( PackageData _data )
    {
        for( int i = 0 ; i < item.Length ; i++ )
        {
            if( _data == item[ i ].data )
            {
                if( PackageManager.I.PackageList[ i ].IsBuy() == false )
                    item[ i ].SetInteractable( false );
            }
        }
    }

    PackageData currentPackageData = null;
    #if UNITY_IAP
    public void GoogleProduct( Product product , bool bSuccess )
    {
        if( bSuccess )
        {
            NetManager.BuyShop( 0 , currentPackageData.uid );
        }
        else
        {
            GlobalUI.ShowOKPupUp( "구매에 실패하였습니다." );
        }

    }
#endif

    public void OnBuy( PackageData _data )
    {
        
            SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
#if USE_SNS_LOGIN

        currentPackageData = _data;

        if (string.IsNullOrEmpty( _data.prdCode ) == false)
		{
#if UNITY_IAP
            UnityIAP_StoreListener.I.RequestPurchase( _data.prdCode , GoogleProduct );
#else
			StartCoroutine(coPurchase( currentPackageData ) );
#endif
		}
		else
		{
            NetManager.BuyPackage( _data.uid );     
            CheckBtn( _data );
        }        
#else
        NetManager.BuyPackage( _data.uid );
        CheckBtn( _data );
#endif

    }

//    public void onBuy1()
//    {
//        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
//#if USE_SNS_LOGIN

//        currentPackageData = PackageManager.I.PackageList[ 0 ];

//        if (string.IsNullOrEmpty( PackageManager.I.PackageList[ 0 ].prdCode ) == false)
//		{
//#if UNITY_IAP
//            UnityIAP_StoreListener.I.RequestPurchase( PackageManager.I.PackageList[ 0 ].prdCode , GoogleProduct );
//#else
//			StartCoroutine(coPurchase(0));
//#endif
//		}
//		else
//		{
//            NetManager.BuyPackage( PackageManager.I.PackageList[ 0 ].uid );
//            btn[ 0 ].interactable = false;
//        }        
//#else
//        NetManager.BuyPackage( PackageManager.I.PackageList[ 0 ].uid );
//        btn[ 0 ].interactable = false;
//#endif
//        }

//    public void onBuy2()
//    {

//        currentPackageData = PackageManager.I.PackageList[ 1 ];
//        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
//#if USE_SNS_LOGIN
//		if (string.IsNullOrEmpty(PackageManager.I.PackageList[ 1 ].prdCode) == false)
//		{
//#if UNITY_IAP
//            UnityIAP_StoreListener.I.RequestPurchase( PackageManager.I.PackageList[ 1 ].prdCode , GoogleProduct );
//#else
//			StartCoroutine(coPurchase(1));
//#endif
//		}
//		else
//		{
//			NetManager.BuyPackage( PackageManager.I.PackageList[ 1 ].uid );
//            btn[ 1 ].interactable = false;
//		}        
//#else
//        NetManager.BuyPackage( PackageManager.I.PackageList[ 1 ].uid );
//        btn[ 1 ].interactable = false;
//#endif

//        }

//    public void onBuy3()
//    {

//        currentPackageData = PackageManager.I.PackageList[ 2 ];
//        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
//#if USE_SNS_LOGIN
//		if (string.IsNullOrEmpty(PackageManager.I.PackageList[ 2 ].prdCode) == false)
//        {
//#if UNITY_IAP
//            UnityIAP_StoreListener.I.RequestPurchase( PackageManager.I.PackageList[ 2 ].prdCode , GoogleProduct );
//#else
//			StartCoroutine(coPurchase(2));
//#endif
//		}
//		else
//		{
//			NetManager.BuyPackage( PackageManager.I.PackageList[ 2 ].uid );
//            btn[ 2 ].interactable = false;
//		}        
//#else
//        NetManager.BuyPackage( PackageManager.I.PackageList[ 2 ].uid );
//        btn[ 2 ].interactable = false;
//#endif


//        }

//    public void onBuy4()
//    {
//        currentPackageData = PackageManager.I.PackageList[ 3 ];
//        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
//#if USE_SNS_LOGIN
//		if (string.IsNullOrEmpty(PackageManager.I.PackageList[ 3 ].prdCode) == false)
//        {
//#if UNITY_IAP
//            UnityIAP_StoreListener.I.RequestPurchase( PackageManager.I.PackageList[ 3 ].prdCode , GoogleProduct );
//#else
//			StartCoroutine(coPurchase(3));
//#endif
//		}
//		else
//		{
//			NetManager.BuyPackage( PackageManager.I.PackageList[ 3 ].uid );
//            btn[ 3 ].interactable = false;
//		}        
//#else
//        NetManager.BuyPackage( PackageManager.I.PackageList[ 3 ].uid );
//        btn[ 3 ].interactable = false;
//#endif


//        }


    }