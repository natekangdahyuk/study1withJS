using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetManager : MonoSinglton<NetManager>
{    
    NetReceive receive = new NetReceive();

//#if UNITY_EDITOR || USE_DEV
//	string httpAddress = "http://58.225.62.74:10001"; //!테스트
//#else
//	string httpAddress = "http://2048.lonnie.co.kr:10001"; //!클베 서버
//#endif
    //string httpAddress = "http://2048.lonnie.co.kr:10001";
    //string httpAddress = "http://58.225.62.74:10001"; //!테스트
    //string httpAddress = "http://221.140.152.73";


    public override void Awake()
    {
        base.Awake();
        receive.Create();
    }

    public override void ClearAll()
    {
    }

    public IEnumerator SendPacket(int index, string address, Dictionary<string, string> param , bool bShowLoadingUI = true , Action cb = null)
    {
        if( bShowLoadingUI )
            GlobalUI.ShowUI(UI_TYPE.LoadingUI);

        string data = JsonFx.Json.JsonWriter.Serialize(param);

		var url = NetURL.GetReqURL(address);
		Logger.N("send : "  + url + " param : " + data);
		WWW w = new WWW(url, System.Text.Encoding.UTF8.GetBytes(data));
                
        yield return w;

		if(w.isDone)
		{
			if (string.IsNullOrEmpty(w.error) == false)
			{
                GlobalUI.CloseUI( UI_TYPE.LoadingUI );
#if USE_LOG
				var popup = GlobalUI.GetUI<ErrorOk>(UI_TYPE.ErrorOK);

				if (popup)
				{
					popup.OnEnter();
					popup.Set(url + "/n param : /n" + data);
				}

#else
                var popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
				if (popup)
				{
					popup.OnEnter();
					popup.Set(StringTBL.GetData(107),
					() => { Application.Quit(); },
					true);
				}
#endif
				yield break;
			}
		}

        receive.Receive(index, w.text);

		if(cb != null)
		{
			cb();
		}
    }

    //static public void CreateAccount(string ID, string Password, string nickname)
    //{
    //    UIManager.ShowLoadingPupUp("loading..");
    //    Dictionary<string, string> param = new Dictionary<string, string>();
    //    param.Add("id", ID);
    //    param.Add("password", Password);
    //    param.Add("nickname", nickname);
    //    SendPacket((int)NETCALL.CREATEACCOUNT, "/mad_server/create_account_request.php", param, I.receive.Receive);
    //}

	static public void DevLogin(string private_id)
	{
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pLinkId" , string.Format( "{0}@mail.com" , private_id ) );
        param.Add( "pLinkKey" , private_id );
        param.Add( "pDeviceId" , "" );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.LOGIN , "/Auth/Auth.aspx" , param ) );
    }

    static public void Login(string ID = "", string Email = "")
    {  
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add( "pLinkId" , Email ); //!구글로그인할때만
        param.Add( "pLinkKey" , ID );
        param.Add( "pDeviceId" , SystemInfo.deviceUniqueIdentifier );
        //param.Add( "pLinkKey" , "117011363791747127460" ); //!구글로그인할때만
        //param.Add( "pLinkId" , "derod82@gmail.com" );
        //param.Add( "pDeviceId" , "cbf55b79dd3c9f566cd933d239df363a" );


        I.StartCoroutine(I.SendPacket((int)NETCALL.LOGIN, "/Auth/Auth.aspx", param));        
    }

    public static void CharacterCreate( string userName )
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pLinkToken", PlayerData.I.oLinktoken);
        param.Add("pUserName", userName);

        I.StartCoroutine(I.SendPacket((int)NETCALL.CREATEACCOUNT, "/Game/UserInit.aspx", param));
    }

    public static void represent( long uidx , int cardidx )
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pUidx", uidx.ToString());
        param.Add("pCardIdx", cardidx.ToString());

        I.StartCoroutine(I.SendPacket((int)NETCALL.represent, "/Game/SetTypical.aspx", param));
    }

    public static void SetLeader( long pUserIdx , bool bLeader , byte deckNo , long CardUidx )
    {

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pIsLeader", (bLeader == true ? 1: 0).ToString());
        param.Add("pDeckNo", deckNo.ToString());
        param.Add("pUidx", CardUidx.ToString());

        I.StartCoroutine(I.SendPacket((int)NETCALL.SetLeader, "/Game/SetLeader.aspx", param));
    }

    //public static void SetuserLevelUp(int pExp, int pLevel)
    //{
    //    Dictionary<string, string> param = new Dictionary<string, string>();
    //    param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
    //    param.Add("pExp", PlayerData.I.Exp.ToString());
    //    param.Add("pLevel", PlayerData.I.UserLevel.ToString());
                
    //    I.StartCoroutine(I.SendPacket((int)NETCALL.UserLevelUp, "/Game/SetUserExpLevel.aspx", param));
    //}

    public static void SetCardLock(long pUidx, int pIsLock)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pUidx", pUidx.ToString());
        param.Add("pIsLock", pIsLock.ToString());

        I.StartCoroutine(I.SendPacket((int)NETCALL.CardLock, "/Game/SetLock.aspx", param));
    }

    public static void SetUserLevelUpReward( int ap , int ruby )
    {
        I.receive.SetUserLevelUpReward( ap , ruby , null );
        //I.StartCoroutine( I.SendPacket( (int)NETCALL.UserLevelUpReward , "/Game/SetLock.aspx" , param ) );
    }
    //public static void SetCardLevelUp(long pUidx, int pExp , int pLevel)
    //{
    //    Dictionary<string, string> param = new Dictionary<string, string>();
    //    param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
    //    param.Add("pUidx", pUidx.ToString());
    //    param.Add("pExp", pExp.ToString());
    //    param.Add("pLevel", pLevel.ToString());

    //    I.StartCoroutine(I.SendPacket((int)NETCALL.CardLevelUp, "/Game/SetCardExpLevel.aspx", param));
    //}

    public static void GetSeverTime(Action cb)
    {
        I.StartCoroutine(I.SendPacket((int)NETCALL.GetServerTime, "/Game/GetServerTime.aspx", null,false, cb));
    }

    public static void SetCardUpgrade( int pExp , int pLevel , List<Card> materialList , long pUidx, int pUseGold,int pStar, int pOver)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pUidx", pUidx.ToString());
        param.Add("pExp", pExp.ToString());
        param.Add("pLevel", pLevel.ToString());
        param.Add("pStar", pStar.ToString());
        param.Add("pOver", pOver.ToString());
        param.Add("pUseGold", pUseGold.ToString());


        List<long> cardlist = new List<long>();
        string card = "";
        for (int i = 0; i < materialList.Count; i++)
        {
            cardlist.Add(materialList[i].cardData.CardKey);
            card += materialList[i].cardData.CardKey.ToString() + "|";
        }

        param.Add("pUseCard",card);

        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>(UI_TYPE.PlayerInfoUI);
        infoui.RecvLevelUpComplete(pUidx, pExp, pLevel, cardlist, pUseGold);

        I.StartCoroutine(I.SendPacket((int)NETCALL.CardUpgrade, "/Game/SetCardUpgrade.aspx", param));
    }

    public static void SetCardPromotion(int pExp, int pLevel, List<Card> materialList, long pUidx, int pUseGold, int pStar, int pOver)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pUidx", pUidx.ToString());
        param.Add("pExp", pExp.ToString());
        param.Add("pLevel", pLevel.ToString());
        param.Add("pStar", pStar.ToString());
        param.Add("pOver", pOver.ToString());
        param.Add("pUseGold", pUseGold.ToString());


        List<long> cardlist = new List<long>();
        string card = "";
        for (int i = 0; i < materialList.Count; i++)
        {
            cardlist.Add(materialList[i].cardData.CardKey);
            card += materialList[i].cardData.CardKey.ToString() + "|";
        }

        param.Add("pUseCard", card);

        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>(UI_TYPE.PlayerInfoUI);
        infoui.RecvPromotionComplete(pUidx, pStar, cardlist, pUseGold);

        I.StartCoroutine(I.SendPacket((int)NETCALL.CardPromotion, "/Game/SetCardUpgrade.aspx", param));
    }

    public static void SetCardLimit(int pExp, int pLevel, Card material, long pUidx, int pUseGold, int pStar, int pOver)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pUidx", pUidx.ToString());
        param.Add("pExp", pExp.ToString());
        param.Add("pLevel", pLevel.ToString());
        param.Add("pStar", pStar.ToString());
        param.Add("pOver", pOver.ToString());
        param.Add("pUseGold", pUseGold.ToString());        
        param.Add("pUseCard", material.cardData.CardKey.ToString());

        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>(UI_TYPE.PlayerInfoUI);
        infoui.RecvLimitComplete(pUidx, pOver, material.cardData.CardKey, pUseGold);

        I.StartCoroutine(I.SendPacket((int)NETCALL.CardLimit, "/Game/SetCardUpgrade.aspx", param));
    }

    public static StoneType GoldStone = StoneType.Default;
    public static int SummonType;
    public static int SummonSlot;
    public static void Summon(byte summonType,byte slot, int bit, byte stone, string formula, StoneType goldstone  )
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pSummonType", summonType.ToString());
        param.Add("pSlot", slot.ToString());
        param.Add("pCost", bit.ToString());
        param.Add("pStone", stone.ToString());
        param.Add("pFormula", formula.ToString());
        param.Add( "pIsBest" , ((int)goldstone).ToString() );
        GoldStone = goldstone;
        SummonSlot = slot;
        SummonType = summonType;
        I.StartCoroutine(I.SendPacket((int)NETCALL.Summon, "/Game/Summon.aspx", param));
    }

    public static void SummonComplete(byte summonType, byte slot, int ruby )
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        //param.Add("pSummonType", summonType.ToString());
        param.Add("pSlot", slot.ToString());
        param.Add("pUseRuby", ruby.ToString());

        I.StartCoroutine(I.SendPacket((int)NETCALL.SummonComplete, "/Game/GetSummon.aspx", param));
    }

    public static void GetStage()
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());

        I.StartCoroutine(I.SendPacket((int)NETCALL.GetStage, "/Game/GetStage.aspx", param));
    }

    public static void SetStage(int pStage , byte pStar )
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pStage", pStage.ToString());
        param.Add("pStar", pStar.ToString());

        Debug.LogError( pStar.ToString() );
        I.StartCoroutine(I.SendPacket((int)NETCALL.SetStage, "/Game/SetStage.aspx", param));
    }

    public static void SetStageReward(int pStage , int gold, int exp , int card, string taskno )
    {
        GlobalUI.CloseUI( UI_TYPE.LoadingUI );

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pStage", pStage.ToString());
        param.Add( "pGold" , gold.ToString() );
        param.Add( "pExp" , exp.ToString() );        
        param.Add( "pTaskNo" , taskno.ToString() );


        Debug.LogError("pUserIdx : "  + PlayerData.I.UserIndex.ToString()  + "   pStage : " + pStage.ToString() + "   pGold :" + gold.ToString() + "    pExp : "+ exp.ToString() + "       pTaskNo : "+ taskno.ToString());



        I.StartCoroutine(I.SendPacket((int)NETCALL.SetStageReward, "/Game/SetStageReward.aspx", param));
    }

    public static void GetRankRewardInfo()
    {
        GlobalUI.CloseUI( UI_TYPE.LoadingUI );
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetRankRewardInfo , "/Rank/GetRankRewardInfo.aspx" , param ) );
    }


    public static int CurrentRankRewardType;
    public static void SetRankReward( int type )
    {
        CurrentRankRewardType = type;
        GlobalUI.CloseUI( UI_TYPE.LoadingUI );
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pType" , type.ToString() );
        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetRankReward , "/Rank/SetRankReward.aspx" , param ) );
    }

    public static void SetShop()
    {
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("pShopType", "0");
        I.StartCoroutine(I.SendPacket((int)NETCALL.SetShop, "/Game/GetShop.aspx", param));
    }

    public static void BuyShop(byte pIsPaynow , int shopIndex ) //1 1 루비 2 골드 3 행동력 4 소환석  // 1 직과금 0 캐쉬
    {
        Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pIsPaynow", pIsPaynow.ToString());
        param.Add("pIdx", shopIndex.ToString());
        
        I.StartCoroutine(I.SendPacket((int)NETCALL.BuyShop, "/Game/BuyShop.aspx", param));
    }

    public static void TeamChange(int deckIndex, List<Card> listcard  )
    {
        InventoryUI Inven = GlobalUI.GetUI<InventoryUI>(UI_TYPE.InventoryUI);

        long[] key = new long[10];
        long leader=0, subleader = 0;

        for( int i = 0 ; i < listcard.Count ; i++ )
        {
            key[ i ] = listcard[ i ].cardData.CardKey;

            if( listcard[ i ].cardData.Leader[ deckIndex -1 ] )
                leader = listcard[ i ].cardData.CardKey;

            if( listcard[ i ].cardData.SubLeader[ deckIndex - 1 ] )
                subleader = listcard[ i ].cardData.CardKey;
        }

        Inven.OnRecvTeamChange(deckIndex, key);


        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pDeckNo" , deckIndex.ToString() );
        param.Add( "pLeader" , leader.ToString() );
        param.Add( "pLeader_2" , subleader.ToString() );

        String cardstring = "";

        for( int i =0 ; i < key.Length ; i++)
        {
            cardstring += key[ i ].ToString() + "|";
        }
        param.Add( "pSetCard" , cardstring ) ;


        I.StartCoroutine( I.SendPacket( (int)NETCALL.TeamChange , "/Game/SetDeck.aspx" , param ) );
    }

    public static void SellCard( long UniqueID )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pUseCard" , UniqueID.ToString() );
        

        I.StartCoroutine( I.SendPacket( (int)NETCALL.SellCard , "/Game/SellCard.aspx" , param ) );
    }

    public static void GetThemaReward( int ThemaIndex  )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pLinkToken" , PlayerData.I.oLinktoken );
        param.Add( "pThemeIdx" , ThemaIndex.ToString() );


        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetThemaReward , "/Game/GetThemeReward.aspx" , param ) );
    }

    public static void SetThemaReward( int ThemaIndex , int pRewardNo )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pLinkToken" , PlayerData.I.oLinktoken );
        param.Add( "pThemeIdx" , ThemaIndex.ToString() );
        param.Add( "pRewardNo" , pRewardNo.ToString() );


        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetThemaReward , "/Game/SetThemeReward.aspx" , param ) );
    }

    public static void GetCollection()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetCollection , "/Game/GetCollection.aspx" , param ) );
    }

    public static void GetCardSkin()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pLinkToken" , PlayerData.I.oLinktoken );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetCardSkin , "/Game/GetCardSkin.aspx" , param ) );
    }

    public static void SetCardSkin( long pUidx , int pSkinIdx )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pLinkToken" , PlayerData.I.oLinktoken );
        param.Add( "pUidx" , pUidx.ToString() );
        param.Add( "pSkinIdx" , pSkinIdx.ToString() );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetCardSkin , "/Game/SetCardSkin.aspx" , param ) );
    }

    public static void BuyCardSkin( int pSkinIdx )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pSkinIdx" , pSkinIdx.ToString() );


        I.StartCoroutine( I.SendPacket( (int)NETCALL.BuyCardSkin , "/Game/BuyCardSkin.aspx" , param ) );
    }
        
    public static void SetRankScore( int type, int score )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        CurrentRankType = type;
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pType" , type.ToString() );
        param.Add( "pScore" , score.ToString() );


        //RankingManager.I.MyRank( type , score , score );
        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetRankScore, "/Rank/SetRankScore.aspx" , param ) );
    }

    public static void SetShoes(int IsUse, int count)
	{
		var time_format = PlayerData.I.CurrentTime.ToString("yyyy-MM-dd tt hh:mm:ss");

		Dictionary<string, string> param = new Dictionary<string, string>();

        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pIsUse", IsUse.ToString());
		param.Add("pShoesCnt", count.ToString());
		param.Add("pClientTime", time_format);

		I.StartCoroutine(I.SendPacket((int)NETCALL.SetShoes , "/Game/SetShoes.aspx", param));
    }
    public static void OnInventoryExtend( int count , int ruby)
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.InventoryExtend , "/Game/BuyInven.aspx" , param ) );
    }

    public static void SetDailyMission( int type )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pType" , type.ToString() );

        
        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetDailyMission , "/Game/SetDailyMission.aspx" , param ) );
    }

    public static void GetDailyMission()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );        

        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetDailyMission , "/Game/GetDailyMission.aspx" , param ) );
    }

    public static void SetDailyMissionreward( int type )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pType" , type.ToString() );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetDailyMissionReward , "/Game/SetDailyMissionReward.aspx" , param ) );
    }

    public static int CurrentRankType = 1;
    public static void GetRank( int pType )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pType" , pType.ToString() );
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        CurrentRankType = pType;
        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetRank + pType - 1, "/Rank/GetRankTop100.aspx" , param ) );
    }

	public static void SetCheckReceipt(OneStore.ProductDetail detail, OneStore.PurchaseData purcase_data)
	{
		var param = new Dictionary<string, string>();
		param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
		Logger.N("pUserIdx : " + PlayerData.I.UserIndex.ToString());
		param.Add("pLinkToken", PlayerData.I.oLinktoken);
		Logger.N("pLinkToken : " + PlayerData.I.oLinktoken);
		param.Add("pProductId", purcase_data.productId);
		Logger.N("pProductId : " + purcase_data.productId);
		param.Add("pAmount", detail.price.ToString());
		Logger.N("pAmount : " + detail.price.ToString());
		param.Add("pOrderId", purcase_data.orderId);
		Logger.N("pOrderId : " + purcase_data.orderId);
		//param.Add("pApp_Id", VersionController.AppName);
		//Logger.N("pApp_Id : " + VersionController.AppName);
		I.StartCoroutine(I.SendPacket((int)NETCALL.CheckReceipt, "/Game/ChkReceipt.aspx", param));
	}

#if UNITY_IAP
	public static void SetCheckReceipt_Google( UnityEngine.Purchasing.Product product )
	{
		var param = new Dictionary<string, string>();
		param.Add( "pUserIdx", PlayerData.I.UserIndex.ToString() );
		Logger.N( "pUserIdx : " + PlayerData.I.UserIndex.ToString() );
		param.Add( "pLinkToken", PlayerData.I.oLinktoken );
		Logger.N( "pLinkToken : " + PlayerData.I.oLinktoken );
		param.Add( "pProductId", product.definition.id );
		Logger.N( "pProductId : " + product.definition.id );
		param.Add( "pPurchaseToken", product.transactionID );
		Logger.N( "pPurchaseToken : " + product.transactionID );
		param.Add( "pPackageName", Application.identifier );
		Logger.N( "pPackageName : " + Application.identifier );
		param.Add( "pAmount", product.metadata.localizedPriceString );
		Logger.N( "pAmount : " + product.metadata.localizedPriceString );
		I.StartCoroutine( I.SendPacket( (int)NETCALL.CheckReceipt_Google, "/Game/ChkReceipt_Google.aspx", param ) );
	}
#endif

	public static void AddSummonSlot( int slotType , int ruby )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();

        param.Add( "pSlotType" , slotType.ToString() );
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pUseRuby" , ruby.ToString() );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.AddSummonSlot , "/Game/AddSummonSlot.aspx" , param ) );
    }

    public static void SetGameBuff( int gold )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pUseGold" , gold.ToString() );
        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetGameBuff , "/Game/SetGameBuff.aspx" , param ) );
    }

    public static void SendLog( string log )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pClientLog" , log );
        I.StartCoroutine( I.SendPacket( (int)NETCALL.SetGameLog , "/Log/clientLog.aspx" , param ) );
    }

    public static void GetPackage()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pShopType" , "0" );
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetPackage , "/Game/GetPackage.aspx" , param ) );
    }

    public static int packageUID = 0;
    public static void BuyPackage(int uid )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pUid" , uid.ToString() );

        packageUID = uid;
        I.StartCoroutine( I.SendPacket( (int)NETCALL.BuyPackage , "/Game/BuyPackage.aspx" , param ) );
    }


    public static void UseCoupon( string coupon )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pLinkToken" , PlayerData.I.oLinktoken );
        param.Add( "pCoupon" , coupon );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.BuyCoupon , "/Game/UseCoupon.aspx" , param ) );

    }

    public static void GetPackageHistory()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );        

        I.StartCoroutine( I.SendPacket( (int)NETCALL.PackageHistory , "/Game/GetPackageHistory.aspx" , param ) );
    }

    public static int achieveRewardType = 0;
    public static void GetAchieveReward( int type )
    {
        achieveRewardType = type;
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("pUserIdx", PlayerData.I.UserIndex.ToString());
        param.Add("pType", type.ToString());

        I.StartCoroutine(I.SendPacket((int)NETCALL.AchieveReward, "/Game/GetAchieveReward.aspx", param));
    }

    public static void GetAchieveUser()
    {        
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        

        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetAchieve , "/Game/GetAchieveUser.aspx" , param ) );
    }

    public static void GetMailList()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );

        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetMailList , "/Game/GetMailList.aspx" , param ) );
    }

    public static long currentMailIndex = 0;
    public static void ReceiveMail( long MailIndex )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pMailIdx" , MailIndex.ToString() );
        currentMailIndex = MailIndex;
        I.StartCoroutine( I.SendPacket( (int)NETCALL.ReceiveMail , "/Game/ReceiveMail.aspx" , param ) );
    }

    public static void ChangeProperty( long pUidx )
    {
        Dictionary<string , string> param = new Dictionary<string , string>();
        param.Add( "pUserIdx" , PlayerData.I.UserIndex.ToString() );
        param.Add( "pUidx" , pUidx.ToString() );        
        I.StartCoroutine( I.SendPacket( (int)NETCALL.ChangeProperty , "/Game/ChangeAttr.aspx" , param ) );
    }

    public static void GetRankList()
    {
        Dictionary<string , string> param = new Dictionary<string , string>();        
        I.StartCoroutine( I.SendPacket( (int)NETCALL.GetRankList , "/Game/GetRankList.aspx" , param ) );
    }

    static void OnResult(string result)
    {
        Debug.LogError(result);
    }
}



