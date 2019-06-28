using UnityEngine;
using System.Collections;
using System;

public enum NewType
{
    mail = 0,
    mission,
}


public class MainScene : MonoBehaviour
{
    public enum StartType
    {
        None,
        Field,
        Dungeon,
        FieldReady,
        DungeonReady,
        Rank2048,
        RankTime,
        Time2048,
        TimeDefence,
    }

 
    InventoryUI Inven = null;
    MainStageUI mainStage = null;
    RankModeMainUI rankModeUI = null;

    [SerializeField]
    MainSceneUI mainSceneUI = null;

    [SerializeField]
    SummonUI summonUI = null;


    public static StartType Starttype = StartType.None;

    void Awake()
    {
        Apply();

        if (Starttype == StartType.Field)
            OnDungeon();        
        if (Starttype == StartType.Dungeon)
            OnField();
        if( Starttype == StartType.DungeonReady )
            OnField();
        if (Starttype == StartType.FieldReady)
            OnDungeon();
        else if( Starttype == StartType.Rank2048 )
            OnDungeon();
        else if( Starttype == StartType.RankTime )
            OnDungeon();
        else if( Starttype == StartType.Time2048 )
            OnDungeon();
        else if( Starttype == StartType.TimeDefence )
            OnDungeon();

    }

    private void Start()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        if( PlayerData.I.bFirstEnter == false )
        {         
            PlayerData.I.bFirstEnter = true;
            SetNoticePopup();
        }        
    }

    void SetNoticePopup()
    {
        string str = PlayerPrefs.GetString( "notice" , "" );

        DateTime current = DateTime.Now;

        if( str.Length > 0 )
        {
            DateTime current2 = DateTime.Parse( str );

            if( current.DayOfYear == current2.DayOfYear )
                return;
        }
        
        PlayerPrefs.SetString( "notice" , current.ToLongDateString() );
        PlayerPrefs.Save();
        NoticePopup Notice = GlobalUI.GetUI<NoticePopup>( UI_TYPE.NoticePopup );
        Notice.Apply();
    }
  

    public void OnField()
    {
        if( mainStage == null )
        {
            GlobalUI.ShowUI( UI_TYPE.LoadingUI );
            mainStage = MainStageUI.Load<MainStageUI>( "SubCanvas" , "MainStageUI" );
            GlobalUI.CloseUI( UI_TYPE.LoadingUI );
        }
        
        mainStage.Apply( StageManager.I.SelectStageIndex );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_main" , GameOption.EffectVoluem );
    }

    public void OnDungeon()
    {
        if( rankModeUI == null )
        {
            GlobalUI.ShowUI( UI_TYPE.LoadingUI );
            rankModeUI = RankModeMainUI.Load<RankModeMainUI>( "SubCanvas" , "RankModeUI" );
            GlobalUI.CloseUI( UI_TYPE.LoadingUI );
        }

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_main" , GameOption.EffectVoluem );
        NetManager.GetRank( 1 );
        NetManager.GetRank( 2 );
        NetManager.GetRank( 3 );
        NetManager.GetRank( 4 );

        rankModeUI.Apply();
    }

    public void OnDetailInfo()
    {
        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>(UI_TYPE.PlayerInfoUI);

        infoui.Apply(InventoryManager.I.representCharacter);
    }

    public void OnRecvRankingList( int type )
    {
        rankModeUI.RecvRank( type );
    }

    public void OnStroy()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        StoryMainUI storyUI = GlobalUI.GetUI<StoryMainUI>( UI_TYPE.StoryMainUI );
        storyUI.Apply();
    }

    public void OnGuide()
    {
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
        TutorialUI tutorialUI = GlobalUI.GetUI<TutorialUI>(UI_TYPE.TutorialUI);
        tutorialUI.Apply();
    }

    public void OnHome()
    {
      
    }

    public void Apply()
    {
        mainSceneUI.OnEnter();

#if USE_SNS_LOGIN
		if (PurchaseManager.I.IsBillingSupport == false)
		{
			StartCoroutine(coPreparePurchase());
		}
#endif
	}

	IEnumerator coPreparePurchase()
	{
		GlobalUI.ShowSpinner();

		var err_code = PurchaseManager.ErrorCode.None;
		var fin = false;
		PurchaseManager.I.PreprarePurchase((code, msg) =>
		{
			err_code = code;
			fin = true;
		});

		yield return new WaitUntil(() => { return fin; });
		fin = false;

		GlobalUI.HideSpinner();

		if (err_code != PurchaseManager.ErrorCode.None)
		{
			yield break;
		}

		GlobalUI.ShowSpinner();
		PurchaseManager.I.RequestProducts();
		yield return new WaitUntil(() => { return PurchaseManager.I.ProductRequestFin; });
		GlobalUI.HideSpinner();

		var unconsume_purchase_data = PurchaseManager.I.UnconsumePurchaseData;
		if (unconsume_purchase_data != null)
		{
            var popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
			popup.OnEnter();
			popup.Set(StringTBL.GetData(902123), () =>
			{
				fin = true;
			}, false);

			yield return new WaitUntil(() => { return fin; });
			fin = false;

			var product_data = PurchaseManager.I.FindProductData(unconsume_purchase_data.productId);
			if (product_data == null)
			{
				var err_popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
				err_popup.OnEnable();
				err_popup.Set(StringTBL.GetData(902124), () =>
				{
					fin = true;
				}, true);

				yield return new WaitUntil(() => { return fin; });
				fin = false;

				yield break;
			}
			else
			{
				GlobalUI.ShowSpinner();
				// 영수증 검증
				var check_reciept = false;
				PurchaseManager.I.CheckReceipt(product_data._onestore_product_detail, unconsume_purchase_data, (result) =>
				{
					check_reciept = result;
					fin = true;
				});

				yield return new WaitUntil(() => { return fin; });
				fin = false;
				GlobalUI.HideSpinner();

				if (check_reciept == false)
				{
					var err_popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
					err_popup.OnEnable();
					err_popup.Set(StringTBL.GetData(902125), () =>
					{
						fin = true;
					}, true);
				}
				else
				{
					GlobalUI.ShowSpinner();
					PurchaseManager.I.ReqConsume((data, code, msg) =>
					{
						err_code = code;
						fin = true;
					}, unconsume_purchase_data);

					yield return new WaitUntil(() => { return fin; });
					fin = false;
					GlobalUI.HideSpinner();

					switch (product_data.Type)
					{
						case PurchaseManager.InAppProductData.ProductType.Package:
							{
								NetManager.BuyPackage((product_data as PurchaseManager.InAppPackageData)._data.uid);
							}
							break;
						case PurchaseManager.InAppProductData.ProductType.Shop:
							{
								NetManager.BuyShop(0, (product_data as PurchaseManager.InAppShopData)._data.ReferenceID);
							}
							break;
					}
				}
			}
		}
	}


	public void OnElla()
    {
        if (Inven == null)
        {
            GlobalUI.ShowUI(UI_TYPE.LoadingUI);
            Inven = GlobalUI.GetUI<InventoryUI>(UI_TYPE.InventoryUI);
            GlobalUI.CloseUI(UI_TYPE.LoadingUI);
        }

        Inven.Apply(InventoryUI.ModeType.Default);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_menu" , GameOption.EffectVoluem );
    }

    public void OnSummon()
    {
        if (summonUI == null)
        {
            GlobalUI.ShowUI(UI_TYPE.LoadingUI);
            summonUI = MainStageUI.Load<SummonUI>("SubCanvas", "SummonUI");
            GlobalUI.CloseUI(UI_TYPE.LoadingUI);            
        }
        summonUI.Apply();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_menu" , GameOption.EffectVoluem );
    }

    public void RecvSummonSlotExtend()
    {
        summonUI.RefreshItem();
    }
    public void RecvSummon( int slot , int summon)
    {
        summonUI.OnRecvSummon( slot, summon);
    }

    public void OnShop()
    {
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.Onshop();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_menu" , GameOption.EffectVoluem );
    }

    public void OnBook()
    {
        illustUI infoui = GlobalUI.GetUI<illustUI>( UI_TYPE.illustUI );

        infoui.Apply();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_menu" , GameOption.EffectVoluem );
    }

    public void OnMission()
    {
        MissionUI infoui = GlobalUI.GetUI<MissionUI>(UI_TYPE.MissionUI);

        infoui.Apply();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_menu" , GameOption.EffectVoluem );
    }

    public void OnBanner()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        Application.OpenURL( "https://cafe.naver.com/2048girls" );
    }

    public void OnBanner2()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        GlobalUI.ShowUI( UI_TYPE.CouponPopup );
        //Application.OpenURL( "https://goo.gl/forms/tZYpe61cHAtQpSnI2" );
    }

    public void OnPackage()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        ShopPackagePopup package = GlobalUI.GetUI<ShopPackagePopup>( UI_TYPE.ShopPackagePopup );
        package.Apply();
    }

    public void OnRecvBuyPackage( int uid )
    {
        ShopPackagePopup package = GlobalUI.GetUI<ShopPackagePopup>( UI_TYPE.ShopPackagePopup );
        package.OnRecvBuyPackage( uid );
    }

    public void OnRefreshMission()
    {
        MissionUI infoui = GlobalUI.GetUI<MissionUI>( UI_TYPE.MissionUI );

        infoui.RefreshMission();
    }

    public void OnRefresh()
    {
        MissionUI infoui = GlobalUI.GetUI<MissionUI>( UI_TYPE.MissionUI );

        infoui.OnRefresh();
    }

    public void OnMail()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

        MailUI mail = GlobalUI.GetUI<MailUI>( UI_TYPE.MailUI );
        mail.Apply();
    }

    public void OnRefreshAchieve()
    {       
        if( MissionManager.I.IsNew() == false )
            mainSceneUI.SetNew( NewType.mission , AchievementManager.I.IsNew() );

        MissionUI infoui = GlobalUI.GetUI<MissionUI>( UI_TYPE.MissionUI );
        infoui.OnRefreshAchieve();


    }

    public void OnRecvMailList()
    {
        mainSceneUI.SetNew( NewType.mail , MailManager.I.IsNew() );
        //MailUI mail = GlobalUI.GetUI<MailUI>( UI_TYPE.MailUI );
        //mail.RecvMailList();
    }

    public void OnRecvMail( long index )
    {
        MailUI mail = GlobalUI.GetUI<MailUI>( UI_TYPE.MailUI );
        mail.OnRecvMail( index );
    }

    public void Update()
    {
        PlayerData.I.Update();
    }

  
}


