
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NETCALL
{
    LOGIN = 1,
    represent,
    CREATEACCOUNT,
    SetLeader,
    UserLevelUp,
    CardLock,
    CardLevelUp,
    CardUpgrade,
    CardPromotion,
    CardLimit,
    Summon,
    SummonComplete,
    SetShop,
    BuyShop,
    GetStage,
    SetStage,
    SetStageReward,
    GetServerTime,
    SellCard,
    UserLevelUpReward,
    SetShoes,
    TeamChange,
    GetThemaReward,
    SetThemaReward,
    GetCollection,
    GetCardSkin,
    SetCardSkin,
    BuyCardSkin,
    SetRankScore,
    InventoryExtend,
    SetDailyMission,
    GetDailyMission,
    SetDailyMissionReward,
    GetRank,
    GetRank2,
    GetRank3,
    GetRank4,
    GetRankRewardInfo,
    AddSummonSlot,
    SetRankReward,
    SetGameBuff,
    SetGameLog,
    GetPackage,
    BuyPackage,
	CheckReceipt,
    BuyCoupon,
    PackageHistory,
    AchieveReward,
    Achieve,
    SetAchieve,
    GetAchieve,
    GetMailList,
    ReceiveMail,
    ChangeProperty,
    GetRankList,
}

public enum Result_Enum
{
    SUCCESS = 200,
    ExistID = 109,
    UserInit = 300,
   
}



public delegate void NetEventCall(SimpleJSON.JSONNode value);

public class NetReceive
{
    public static Dictionary<NETCALL, NetEventCall> ReceiveFunc = new Dictionary<NETCALL, NetEventCall>();

    public void Create()
    {        
        ReceiveFunc.Add(NETCALL.LOGIN, this.Login);
        ReceiveFunc.Add(NETCALL.represent, this.represent);
        ReceiveFunc.Add(NETCALL.CREATEACCOUNT, this.CreateAccount);
        ReceiveFunc.Add(NETCALL.SetLeader, this.SetLeader);
        ReceiveFunc.Add(NETCALL.UserLevelUp, this.UserLevelUp);
        ReceiveFunc.Add(NETCALL.CardLock, this.SetCardLock);
        ReceiveFunc.Add(NETCALL.CardLevelUp, this.SetCardLevelUp);
        ReceiveFunc.Add(NETCALL.CardUpgrade, this.SetCardUpgrade);
        ReceiveFunc.Add(NETCALL.CardPromotion, this.SetCardPromotion);
        ReceiveFunc.Add(NETCALL.CardLimit, this.SetCardLimit);
        ReceiveFunc.Add(NETCALL.Summon, this.SetSummon);
        ReceiveFunc.Add(NETCALL.SummonComplete, this.SetSummonComplete);
        ReceiveFunc.Add(NETCALL.BuyShop, this.BuyShop);
        ReceiveFunc.Add(NETCALL.SetShop, this.SetShop);
        ReceiveFunc.Add(NETCALL.GetStage, this.GetStage);
        ReceiveFunc.Add(NETCALL.SetStage, this.SetStage);
        ReceiveFunc.Add(NETCALL.SetStageReward, this.SetStageReward);
        ReceiveFunc.Add(NETCALL.GetServerTime, this.GetServerTime);
        ReceiveFunc.Add( NETCALL.SellCard , this.SellCard );
        ReceiveFunc.Add(NETCALL.SetShoes, this.SetShoes);
        ReceiveFunc.Add( NETCALL.TeamChange , this.TeamChange );
        ReceiveFunc.Add( NETCALL.GetThemaReward , this.GetThemaReward );
        ReceiveFunc.Add( NETCALL.SetThemaReward , this.SetThemaReward );
        ReceiveFunc.Add( NETCALL.GetCollection , this.GetCollection );
        ReceiveFunc.Add( NETCALL.GetCardSkin , this.GetCardSkin );
        ReceiveFunc.Add( NETCALL.SetCardSkin , this.SetCardSkin );
        ReceiveFunc.Add( NETCALL.BuyCardSkin , this.BuyCardSkin );
        ReceiveFunc.Add( NETCALL.SetRankScore , this.SetRankScore );
        ReceiveFunc.Add(NETCALL.InventoryExtend, this.InventoryExtend);
        ReceiveFunc.Add( NETCALL.GetRank , this.GetRank );
        ReceiveFunc.Add( NETCALL.GetRank2 , this.GetRank2 );
        ReceiveFunc.Add( NETCALL.GetRank3 , this.GetRank3 );
        ReceiveFunc.Add( NETCALL.GetRank4 , this.GetRank4 );
        ReceiveFunc.Add( NETCALL.SetDailyMission , this.SetDailyMission );
        ReceiveFunc.Add( NETCALL.GetDailyMission , this.GetDailyMission );
        ReceiveFunc.Add( NETCALL.SetDailyMissionReward , this.SetDailyMissionReward );
        ReceiveFunc.Add( NETCALL.AddSummonSlot , this.AddSummonSlot );
        ReceiveFunc.Add( NETCALL.GetRankRewardInfo , this.GetRankRewardInfo );
        ReceiveFunc.Add( NETCALL.SetRankReward , this.SetRankReward );
        ReceiveFunc.Add( NETCALL.SetGameBuff , this.SetGameBuff );
        ReceiveFunc.Add( NETCALL.SetGameLog , this.SetGameLog );

        ReceiveFunc.Add( NETCALL.GetPackage , this.GetPackage );
        ReceiveFunc.Add( NETCALL.BuyPackage , this.BuyPackage );
        ReceiveFunc.Add( NETCALL.BuyCoupon , this.BuyCoupon );
        ReceiveFunc.Add( NETCALL.PackageHistory , this.PackageHistory );
		ReceiveFunc.Add(NETCALL.CheckReceipt, this.CheckReceipt);
        ReceiveFunc.Add(NETCALL.AchieveReward, this.AchieveReward);
        //ReceiveFunc.Add(NETCALL.Achieve, this.Achieve);
        //ReceiveFunc.Add( NETCALL.SetAchieve , this.SetAchieve );
        ReceiveFunc.Add( NETCALL.GetAchieve , this.GetAchieveUser );

        ReceiveFunc.Add( NETCALL.GetMailList , this.GetMailList );
        ReceiveFunc.Add( NETCALL.ReceiveMail , this.ReceiveMail );
        ReceiveFunc.Add( NETCALL.ChangeProperty , this.ChangeProperty );
        ReceiveFunc.Add( NETCALL.GetRankList , this.GetRankList );




        //ReceiveFunc.Add( NETCALL.UserLevelUpReward , this.SetUserLevelUpReward );




    }

    NetEventCall GetCallFunc(NETCALL idx)
    {
        NetEventCall data;
        if (true == ReceiveFunc.TryGetValue(idx, out data))
            return data;

        return null;
    }

    public void Receive(int protocol_identity_index, string d)
    {
        GlobalUI.CloseUI(UI_TYPE.LoadingUI);

        if (d == null)
        {
            //UIManager.I.ShowOKPupUp("", "패킷 null");
            return;
        }		

        var value = SimpleJSON.JSON.Parse(d);

        int result = 200;

        if( protocol_identity_index != (int)NETCALL.SetGameLog )
            NetManager.SendLog( d );

        if ( value["oCode"] != null )
        {
            result = int.Parse(value["oCode"]);
        }        

        if (result != 200)
        {
            Debug.LogError("server packet error");
            Debug.LogError(value);
            CheckError((Result_Enum)result, protocol_identity_index, value );
            return;
        }
        
		GetCallFunc((NETCALL)protocol_identity_index)(value);
    }

    public void GetServerTime(SimpleJSON.JSONNode value)
    {
        PlayerData.I.ServerTime = DateTime.Parse(value["oMessage"].Value , new System.Globalization.CultureInfo("ko-KR",true));
        PlayerData.I.ServerTime = PlayerData.I.ServerTime.AddSeconds((double)-Time.realtimeSinceStartup);
	}

    public void Login(SimpleJSON.JSONNode value)
    {
		Debug.Log("## login");

        PlayerData.I.UserIndex = int.Parse(value["oUserIdx"]);
        PlayerData.I.oLinktoken = value["oLinkToken"];

        SimpleJSON.JSONArray oInfo = SimpleJSON.JSON.Parse(value["oInfo"]).AsArray ;
                
        PlayerData.I.UserID = oInfo[0]["cUserName"];
        PlayerData.I.UserLevel = int.Parse(oInfo[0]["cUserLevel"]);
        PlayerData.I.Exp = int.Parse(oInfo[0]["cUserExp"]);
        PlayerData.I.Maxshoes = int.Parse(oInfo[0]["cMaxShoes"]);        
        PlayerData.I.Cash = int.Parse(oInfo[0]["cRuby"]);
        PlayerData.I.Gold = int.Parse(oInfo[0]["cGold"]);
        PlayerData.I.Stone = int.Parse(oInfo[0]["cStone"]);
        PlayerData.I.representIndex = int.Parse(oInfo[0]["cUidx"]);
        InventoryManager.I.MaxCardCount = int.Parse(oInfo[0]["cMaxCard"]);
        PlayerData.I.SummonOpenCount = int.Parse( oInfo[ 0 ][ "cSummonLock" ] );
        PlayerData.I.SummonBitOpenCount = int.Parse( oInfo[ 0 ][ "cSummonLock_bit" ] );
        PlayerData.I.mileage = float.Parse( oInfo[ 0 ][ "cGoldStone" ] );

        if (oInfo[0]["cShoes"].Value == "null")
            PlayerData.I.Setshoes( 0 ,  false );
        else
            PlayerData.I.Setshoes(int.Parse(oInfo[0]["cShoes"]), false);        

        if (oInfo[0]["cLeftSec"].Value == "null")
            PlayerData.I.shoesSec = 0;
        else
            PlayerData.I.shoesSec = int.Parse(oInfo[0]["cLeftSec"]) + Time.realtimeSinceStartup;

        SimpleJSON.JSONArray oCard = SimpleJSON.JSON.Parse(value["oCard"]).AsArray;		

        for (int i = 0; i < oCard.Count; i++)
        {
            SimpleJSON.JSONNode card = oCard[i];

            CardData data = InventoryManager.I.Get( long.Parse( card[ "cUidx" ] ) );
            int deck = int.Parse( card[ "cDeckNo" ] );

            if( data != null )
            {
                if( deck > 0 )
                {
                    if( deck > 0 )
                    {
                        DeckManager.I.SetDeck( deck - 1 , data );
                        data.Leader[ deck - 1 ] = bool.Parse( card[ "cLeader" ]);
                        data.SubLeader[ deck - 1 ] = bool.Parse( card[ "cLeader_2" ]);
                    }
                }
            }
            else
            {
                InventoryManager.I.NewCard( long.Parse( card[ "cUidx" ] ) , int.Parse( card[ "cCardIdx" ] ) , int.Parse( card[ "cExp" ] ) , int.Parse( card[ "cLevel" ] ) ,
            int.Parse( card[ "cStar" ] ) , int.Parse( card[ "cSkin" ] ) , deck , bool.Parse( card[ "cLeader" ] ) , bool.Parse( card[ "cLeader_2" ] ) , bool.Parse( card[ "cIsLock" ] ) , int.Parse( card[ "cOver" ] ) , false);
            }
            

        }

        //! 소환 정보
        if (value["oSummon"] != null)
        {
            SimpleJSON.JSONArray oSummon = SimpleJSON.JSON.Parse(value["oSummon"]).AsArray;


            for (int i = 0; i < oSummon.Count; i++)
            {
                //int stone = int.Parse(oSummon[i]["cLeftStone"]);
                int cardIdx = int.Parse(oSummon[i]["cCardIdx"]);
                int slot = int.Parse(oSummon[i]["cSlot"]);
                SummonItem.BitType type = (SummonItem.BitType)int.Parse(oSummon[i]["cSummonType"]);
                int cSummonSec = int.Parse(oSummon[i]["cSummonSec"]);
                DateTime cSummonCompleteTime = DateTime.Parse(oSummon[i]["cSummonTime"]);

                int summontype = bool.Parse( oSummon[ i ][ "cIsBest" ] ) == true ? 1: 0;
                double remainTime = (cSummonCompleteTime - PlayerData.I.CurrentTime).TotalSeconds;

                PlayerData.I.Summon(remainTime, slot, type, PlayerData.I.Stone, ( StoneType)summontype );
            }

        }

        //PlayerData.I.Summon(100, 1, SummonItem.BitType.Default, 10);
        InventoryManager.I.SetRepresentCharacter();

      

        CollectionManager.I.Init();

        NetManager.GetStage();
        NetManager.SetShop();
        RankingManager.I.Init();
        NetManager.GetCollection();
        NetManager.GetCardSkin();
        NetManager.GetDailyMission();
        NetManager.GetRankRewardInfo();
        NetManager.GetPackage();
        NetManager.GetRankList();
    }


    
    public void TeamChange( SimpleJSON.JSONNode value )
    {
        
    }

    
    public void CreateAccount(SimpleJSON.JSONNode value)
    {
#if USE_SNS_LOGIN
		NetManager.Login(FirebaseManager.I.GoogleSigninID, FirebaseManager.I.Email);
#else
		var private_id = PlayerPrefs.GetString("id", SystemInfo.deviceUniqueIdentifier);
		NetManager.DevLogin(private_id);
#endif
		GlobalUI.GetUI<CreateAccountPopup>(UI_TYPE.CreateAccountUI).ReceiveAccount(true);
    }


    public void represent(SimpleJSON.JSONNode value)
    {
        
    }

    public void SetLeader(SimpleJSON.JSONNode value)
    {
        //InventoryUI Inven = GlobalUI.GetUI<InventoryUI>(UI_TYPE.InventoryUI);
        //Inven.OnRecvLeaderChange(DeckManager.I.CurrentDeckIndex, leaderSetting.Leader.cardData.CardKey, leaderSetting.SubLeader.cardData.CardKey);
    }

    public void UserLevelUp(SimpleJSON.JSONNode value)
    {
        Debug.LogError("UserLevelUp success");

        
    }

    public void SetCardLock(SimpleJSON.JSONNode value)
    {
        Debug.LogError("lock success");
    }

    public void SetCardLevelUp(SimpleJSON.JSONNode value)
    {
        Debug.LogError("SetCardLevelUp success");
        
    }

    public void SetCardUpgrade(SimpleJSON.JSONNode value)
    {
        Debug.LogError("SetCardUpgrade success");
        MissionManager.I.Clear( MissionReferenceData.MissionType.LevelUp );
    }

    public void SetCardPromotion(SimpleJSON.JSONNode value)
    {

    }

    public void SetCardLimit(SimpleJSON.JSONNode value)
    {

    }

    public void SetSummon(SimpleJSON.JSONNode value)
    {
        SimpleJSON.JSONArray oInfo = SimpleJSON.JSON.Parse(value["oSummonInfo"]).AsArray;
        int stone = int.Parse(oInfo[0]["cLeftStone"]);

        float cLeftGoldStone = 0;
        if( oInfo[ 0 ][ "cLeftGoldStone" ].Value != "null")
            cLeftGoldStone = float.Parse( oInfo[ 0 ][ "cLeftGoldStone" ] );

        
        int cardIdx = int.Parse(oInfo[0]["cCardIdx"]);
        int slot = int.Parse(oInfo[0]["cSlot"]);        
        SummonItem.BitType type = (SummonItem.BitType)int.Parse(oInfo[0]["cSummonType"]);
        int cSummonSec = int.Parse(oInfo[0]["cSummonSec"]);
        DateTime cSummonCompleteTime = DateTime.Parse(oInfo[0]["cSummonTime"]);
        //double remainTime = (cSummonCompleteTime - PlayerData.I.CurrentTime).TotalSeconds;
        PlayerData.I.mileage = cLeftGoldStone;
        PlayerData.I.Summon(cSummonSec, slot, type, stone, NetManager.GoldStone );

      
        if (SceneManager.I.scene == SCENE.MainScene)
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().RecvSummon( NetManager.SummonSlot , NetManager.SummonType );
        }

    }

    public void SetSummonComplete(SimpleJSON.JSONNode value)
    {
        SimpleJSON.JSONArray oInfo = SimpleJSON.JSON.Parse(value["oSummonInfo"]).AsArray;

        int cardIdx = int.Parse(oInfo[0]["cCardIdx"]);        
        int cRuby = int.Parse(oInfo[0]["cLeftRuby"]);
        int uIdx = int.Parse(oInfo[0]["cUidx"]);
        int slot = int.Parse(oInfo[0]["cSlot"]);
        int type = int.Parse(oInfo[0]["cSummonType"]);


        PlayerData.I.SummonComplete(slot, (SummonItem.BitType )type, cRuby);

        CardReferenceData data = CardTBL.GetData(cardIdx);
        CharacterReferenceData chadata = CharacterTBL.GetData(data.characterIndex);
        CardData card = InventoryManager.I.NewCard(uIdx, cardIdx, 0, 1, chadata.star, 0, -1, false, false, false ,0, true);
        SummonCompletePopup summon = GlobalUI.GetUI<SummonCompletePopup>(UI_TYPE.SummonCompletePopup);
        summon.Apply(card);

        if (SceneManager.I.scene == SCENE.MainScene)
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().RecvSummon( slot, type );
        }

        MissionManager.I.Clear( MissionReferenceData.MissionType.Summon );



    }

    public void BuyShop(SimpleJSON.JSONNode value) //1 1 루비 2 골드 3 행동력 4 소환석  // 1 현금 2 루비
    {
        SimpleJSON.JSONArray oInfo = SimpleJSON.JSON.Parse(value["oBuying"]).AsArray;

        int cPrdType = int.Parse(oInfo[0]["cPrdType"]);
        //int cPrdNum = int.Parse(oInfo[0]["cPrdCnt"]);

        //if (cPrdType == 1)
        //{
        //    PlayerData.I.SetRuby( cPrdNum );
        //}
        if (cPrdType == 2)
        {
        //    PlayerData.I.SetGold(cPrdNum);
            MissionManager.I.Clear( MissionReferenceData.MissionType.GoldBuy );
        }
        //else if (cPrdType == 3)
        //{
        //    PlayerData.I.Setshoes(cPrdNum);
        //}
        //else if (cPrdType == 4)
        //{
        //    PlayerData.I.SetStone(cPrdNum);
        //}

        PlayerData.I.SetRuby( int.Parse(oInfo[0]["cLeftRuby"]) );
        GlobalUI.ShowOKPupUp( StringTBL.GetData( 902158 ) );


    }

    public void SetShop(SimpleJSON.JSONNode value)
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for (int i = 0; i < oInfo.Count; i++)
        {
            ShopReferenceData data = new ShopReferenceData();

            data.productImg = oInfo[i]["cImg"];
            data.shopType = (ShopType)int.Parse(oInfo[i]["cShopType"]);
            data.productType = (ProductType)int.Parse(oInfo[i]["cPrdType"]);
            data.productNum = int.Parse(oInfo[i]["cPrdNum"]);
            data.costType = (CostType)byte.Parse(oInfo[i]["cCostType"]);
            data.costValue = int.Parse(oInfo[i]["cCostValue"]);
            //data.de = oInfo[i]["cDesc"];
        }
    }


    public void GetStage(SimpleJSON.JSONNode value)
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for(int i =0; i < oInfo.Count; i++)
        {
            int stage = int.Parse(oInfo[i]["cStage"]);
            int star = int.Parse(oInfo[i]["cStar"]);

            int[] Star = new int[ 3 ];

            Star[0] = byte.Parse(oInfo[i]["cReward_01"]);
            Star[ 1 ] = byte.Parse( oInfo[ i ][ "cReward_02" ] );
            Star[ 2 ] = byte.Parse( oInfo[ i ][ "cReward_03" ] );

            bool last = bool.Parse(oInfo[i]["cIsLast"]); //!마지막에했던대

            if (stage == 0)
                continue;
              

            StageManager.I.AddStageClearInfo(stage, star, last, Star);

            if (last)
                StageManager.I.ApplyStage( stage );
        }
        List<ThemaReferenceData> list = ThemaTBL.GetList();
        for( int i = 0 ; i < list.Count ; i++ )
            NetManager.GetThemaReward( list[ i ].ReferenceID );


    }

    public void SellCard( SimpleJSON.JSONNode value )
    {        
        PlayerData.I.SetStone( int.Parse( value[ "oStone" ] ) );
    }

    public void SetStage(SimpleJSON.JSONNode value)
    {

    }

    public static int CurrentRewardValue = 0;
    public static int CurrentRewardType = 0;
    public static bool bLevelUp = false;
    public static bool bStarUp = false;
    public void SetStageReward(SimpleJSON.JSONNode value)
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;
        int ocard = int.Parse( value[ "oCard" ] );
        int oUidx = int.Parse( value[ "oUidx" ] );
        int pGold = int.Parse( value[ "oGold" ] );
        int pExp = int.Parse( value[ "oExp" ] );
        int oLevel = int.Parse( value[ "oLevel" ] );
        int oRuby = int.Parse( value[ "oRuby" ] );
        int oStone = int.Parse( value[ "oStone" ] );
        int oShoes = int.Parse( value[ "oShoes" ] );
        int omaxShoes = int.Parse( value[ "oMaxShoes" ] );
        bool oIsReward = bool.Parse( value[ "oIsReward" ] );

        PlayerData.I.SetRuby( oRuby );
        PlayerData.I.SetGold( pGold );
        PlayerData.I.SetStone( oStone );
        PlayerData.I.Maxshoes = omaxShoes;
        PlayerData.I.Setshoes( oShoes );
        PlayerData.I.Exp = pExp;

        ExpUserReferenceData Expdata = ExpUserTBL.GetData( oLevel );

        bLevelUp = false;
        bStarUp = false;
        if( oLevel > PlayerData.I.UserLevel )
        {
            UserLevelUpPopup Popup = GlobalUI.GetUI<UserLevelUpPopup>( UI_TYPE.UserLevelUpPopup );
            Popup.Apply( Expdata.CashReward , Expdata.apReward ,  oLevel );
            Popup.action = ShowStarRewardPopup;
            bLevelUp = true;
        }

        PlayerData.I.UserLevel = oLevel;
        //AchievementManager.I.Clear( AchievementReferenceData.MissionType.StagePlayCount , StageManager.I.SelectStageIndex );

        if( SceneManager.I.scene == SCENE.GameScene )
        {
            GameObject go = SceneManager.I.GetScene();
            GameScene scene = go.GetComponent<GameScene>();
            scene.ShowGameEnd( ocard , oUidx );
            bStarUp = true;
            if( bLevelUp == false )
                scene.ShowStarRewardPopup( 3 );
          
        }

        MissionManager.I.Clear( MissionReferenceData.MissionType.FieldClear );
        
    }

    void ShowStarRewardPopup()
    {
        if( bStarUp == false )
            return;

        if( SceneManager.I.scene == SCENE.GameScene )
        {
            GameObject go = SceneManager.I.GetScene();
            GameScene scene = go.GetComponent<GameScene>();
            scene.ShowStarRewardPopup( 0 );
        }
    }


    public void SetUserLevelUpReward( int ap, int ruby, SimpleJSON.JSONNode value )
    {
        UserLevelUpPopup Popup = GlobalUI.GetUI<UserLevelUpPopup>( UI_TYPE.UserLevelUpPopup );
        Popup.Apply( ruby , ap , PlayerData.I.UserLevel );
    }

    public void SetShoes(SimpleJSON.JSONNode value)
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;
        int shoes = int.Parse(value["oLastShoes"]);
        PlayerData.I.Setshoes(shoes);
    }

    public void GetThemaReward( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;


        bool[] reward = new bool[ 3 ];

        reward[0] = bool.Parse( oInfo[0][ "cReward_01" ] );
        reward[ 1 ] = bool.Parse( oInfo[0][ "cReward_02" ] );
        reward[ 2 ] = bool.Parse( oInfo[0][ "cReward_03" ] );
        int pThemeIdx = int.Parse( oInfo[ 0 ][ "cThemeIdx" ] );


        int count = 0;

        for( int i =0 ;  i < reward.Length ; i++ )
        {
            if( reward[ i ] == true )
                count++;
        }
        StageManager.I.AddThemaReward( pThemeIdx , count );
    }

    public void SetThemaReward( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;
        int ruby = int.Parse( value[ "oRuby" ] );

        //int reward = ruby - PlayerData.I.Cash;

        //PlayerData.I.SetRuby( ruby );
        PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
        popup.OnEnter();
        popup.SetEx( MainStageUI.currentReward , ruby.ToString() , null ,  false , PopupOk.SubType.ruby );        
    }

    public void GetCollection( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for( int i = 0 ; i < oInfo.Count ; i++ )
        {
            int index = int.Parse( oInfo[ i ][ "cCardIdx" ] );
            CollectionManager.I.AddCard( index );
        }

    }

    public void GetCardSkin( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for( int i = 0 ; i < oInfo.Count ; i++ )
        {
            int index = int.Parse( oInfo[ i ][ "cSkinIdx" ] );            
            InventoryManager.I.AddSkin( index );
        }
        
    }
    
    public void SetCardSkin( SimpleJSON.JSONNode value )
    {
        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>( UI_TYPE.PlayerInfoUI );
        infoui.RecvSelectSkin();
        
    }

    public void BuyCardSkin( SimpleJSON.JSONNode value )
    {
        //SimpleJSON.JSONArray oInfo = value.AsArray;
        int ruby = int.Parse( value[ "oRuby" ] );
        PlayerData.I.SetRuby( ruby );                
        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>( UI_TYPE.PlayerInfoUI );
        infoui.RecvUnLockSkin();
    }

    public void SetRankScore( SimpleJSON.JSONNode value )
    {
        Debug.Log( "랭크 저장 성공" );

        int score = int.Parse( value[ "oScore" ] );
        int prescore = int.Parse( value[ "oPreScore" ] );

        RankingManager.I.SetMyRankPoint( NetManager.CurrentRankType , score , prescore );
        MissionManager.I.Clear( MissionReferenceData.MissionType.RankClear );
        //AchievementManager.I.Clear( AchievementReferenceData.MissionType.RankStagePlayCount );
        if( SceneManager.I.scene == SCENE.GameScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<GameScene>().ShowGameEndSpecial();
        }

    }

    public void InventoryExtend(SimpleJSON.JSONNode value)
    {
        int ruby = int.Parse( value[ "oLastRuby" ] );
        int count = int.Parse( value[ "oMaxCard" ] );

        PlayerData.I.SetRuby( ruby );
        
        InventoryUI Inven = GlobalUI.GetUI<InventoryUI>( UI_TYPE.InventoryUI );
        Inven.OnRecvExtend( count );
    }

    public void GetRank( SimpleJSON.JSONNode value )
    {
        GetRank( value , 1 );
    }
    public void GetRank2( SimpleJSON.JSONNode value )
    {
        GetRank( value , 2 );
    }
    public void GetRank3( SimpleJSON.JSONNode value )
    {
        GetRank( value , 3 );
    }
    public void GetRank4( SimpleJSON.JSONNode value )
    {
        GetRank( value , 4 );
    }
    public void GetRank( SimpleJSON.JSONNode value ,int Type )
    {
        SimpleJSON.JSONArray oRank = SimpleJSON.JSON.Parse( value[ "oRanking" ] ).AsArray;

        RankingManager.I.Clear( Type );
        for( int i =0 ; i < oRank.Count ; i++ )
        {
            int rank = int.Parse( oRank[ i ][ "cRank" ] );
            string name = oRank[ i ][ "cUserName" ];

            if( rank == 0 )
                continue;

            long score = 0;
            if( oRank[ i ][ "cScore" ].Value != "null" )
                score = long.Parse( oRank[ i ][ "cScore" ] );

            RankingManager.I.Add( Type , rank , (int)score , name );
        }

        SimpleJSON.JSONArray oMyRank = SimpleJSON.JSON.Parse( value[ "oMyRank" ] ).AsArray;

        int Myscore = 0;
        int myrank = 0;
        if( oMyRank[ 0 ][ "cScore" ].Value != "" && oMyRank[ 0 ][ "cScore" ].Value != "null" )
            Myscore = int.Parse( oMyRank[ 0 ][ "cScore" ] );

        if( oMyRank[ 0 ][ "cRank" ].Value != "" && oMyRank[ 0 ][ "cRank" ].Value != "null" )
            myrank = int.Parse( oMyRank[ 0 ][ "cRank" ] );

        RankingManager.I.MyRank( Type , myrank , Myscore );

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRecvRankingList( Type );
        }

        //if( NetManager.CurrentRankType == 1)
        //    NetManager.GetRank( 2 );
    }

    public void SetDailyMission( SimpleJSON.JSONNode value )
    {

    }

    public void SetDailyMissionReward( SimpleJSON.JSONNode value )
    {
        PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
        popup.OnEnter();

        int type = int.Parse( value[0][ "oType" ]);
        int ruby = int.Parse( value[ 0 ][ "oRuby" ] );
        int gold = int.Parse( value[ 0 ][ "oGold" ] );
        int stone = int.Parse( value[ 0 ][ "oStone" ] );
        int shoes = int.Parse( value[ 0 ][ "oShoes" ] );

        MissionReferenceData data = MissionManager.I.GetMissionData( type ); 
        switch( data.Rewardtype )
        {
            case MissionReferenceData.RewardType.AP:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.ap );
            break;
            case MissionReferenceData.RewardType.Gold:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.gold );
            break;
            case MissionReferenceData.RewardType.Ruby:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.ruby );
            break;
            case MissionReferenceData.RewardType.Stone:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.stone );
            break;
        }

        PlayerData.I.SetRuby( ruby );
        PlayerData.I.SetGold( gold );
        PlayerData.I.Setshoes( shoes );
        PlayerData.I.SetStone( stone );

        MissionManager.I.OnReward((MissionReferenceData.MissionType)type);
        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRefreshMission();
        }

    }
    
    public void AddSummonSlot( SimpleJSON.JSONNode value )
    {
        int ruby = int.Parse( value[ "oRuby" ] );
        PlayerData.I.SetRuby( ruby );

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().RecvSummonSlotExtend();
        }
    }

    public void GetDailyMission( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for( int i = 0 ; i < oInfo.Count ; i++ )
        {
            int type = int.Parse( oInfo[ i ][ "cType" ] );
            int count = int.Parse( oInfo[ i ][ "cCnt" ] );
            int needcount = int.Parse( oInfo[ i ][ "cNeedCnt" ] );
            bool isreward = bool.Parse( oInfo[ i ][ "cIsReward"] );
            MissionManager.I.Set( type , count , needcount , isreward );
        }
    }

    public void GetRankRewardInfo( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for( int i = 0 ; i < oInfo.Count ; i++ )
        {
            int week = int.Parse( oInfo[ i ][ "cWeek" ] );
            int type = int.Parse( oInfo[ i ][ "cType" ] );
            int rank = int.Parse( oInfo[ i ][ "cRank" ] );
            long cScore = long.Parse( oInfo[ i ][ "cScore" ] );
            bool isreward = int.Parse( oInfo[ i ][ "cIsReward" ] ) == 1 ? true : false;
            RankingManager.I.SetRankRewardInfo( type , isreward );
        }

        if (SceneManager.I.scene == SCENE.LogoScene)
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<StartScene>().ReceiveLogin();
        }
    }

    public void SetRankReward( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;
        //int stone = int.Parse( oInfo[0][ "cLastStone" ] );
        //int gold = int.Parse( oInfo[0][ "cLastGold" ] );

        int rewardStone = int.Parse( oInfo[0][ "cRewardStone" ] );
        rewardGold = int.Parse( oInfo[0][ "cRewardGold" ] );
        int cRank = int.Parse( oInfo[ 0 ][ "cRank" ] );
        //PlayerData.I.SetStone( stone );
        //PlayerData.I.SetGold( gold );

        RankRewardPopup RankReward = GlobalUI.ShowUI(UI_TYPE.RankRewardPopup) as RankRewardPopup; 
        RankReward.Apply(NetManager.CurrentRankRewardType, rewardGold, rewardStone , cRank );
    }

    public void SetGameBuff( SimpleJSON.JSONNode value )
    {
        
        int gold = int.Parse( value[ "oLastGold" ] );

        PlayerData.I.SetGold( gold );
    }

    int rewardGold =0;
    public void OnRankReward()
    {
        PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
        popup.OnEnter();
        popup.SetEx( StringTBL.GetData(902165) , rewardGold.ToString() , null , false , PopupOk.SubType.gold );
    }

    public void SetGameLog( SimpleJSON.JSONNode value )
    {

    }

    public void GetPackage( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for( int i =0 ; i < oInfo.Count ; i++  )
        {
            int uid = int.Parse( oInfo[ i ][ "cUid" ] );
            int ruby = int.Parse( oInfo[ i ][ "cRuby" ] );
            int stone = int.Parse( oInfo[ i ][ "cStone" ] );
            int gold = int.Parse( oInfo[ i ][ "cGold" ] );
            string prdcode = oInfo[ i ][ "cPrdCode" ];
            int maxCount = int.Parse( oInfo[ i ][ "cMaxCnt" ] );
            int count = int.Parse( oInfo[ i ][ "cCnt" ] );

            PackageManager.I.AddPackage( uid , ruby , stone , gold , prdcode , maxCount , count );
        }
//        NetManager.GetPackageHistory();

    }

    public void PackageHistory( SimpleJSON.JSONNode value )
    {
        SimpleJSON.JSONArray oInfo = value.AsArray;

        for( int i = 0 ; i < oInfo.Count ; i++ )
        {
            int uid = int.Parse( oInfo[ i ][ "cUId" ] );
            PackageManager.I.SetBuyPackage( uid  );
        }
    }


    public void BuyPackage( SimpleJSON.JSONNode value )
    {
        
        int cReturn = int.Parse( value[ 0 ][ "cReturn" ] );

        if( cReturn == -11 )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902159 ) );            
            return;
        }

        int ruby = int.Parse( value[0][ "cRuby" ] );
        int stone = int.Parse( value[0][ "cStone" ] );
        int gold = int.Parse( value[0][ "cGold" ] );

        PlayerData.I.SetRuby( ruby );
        PlayerData.I.SetStone( stone );
        PlayerData.I.SetGold( gold );

        GlobalUI.ShowOKPupUp( StringTBL.GetData( 902160 ) );

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRecvBuyPackage( NetManager.packageUID );
        }


    }
    public void BuyCoupon( SimpleJSON.JSONNode value )
    {
        int cReturn = int.Parse( value[ 0 ][ "cReturn" ] );
        if( cReturn == 800 )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902161 ) );
            return;
        }

        if( cReturn == 900 )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902162 ) );
            return;
        }

        if( cReturn != 0 )
            return;

        int ruby = int.Parse( value[ 0 ][ "cRuby" ] );
        int stone = int.Parse( value[ 0 ][ "cStone" ] );
        int gold = int.Parse( value[ 0 ][ "cGold" ] );

        int rewardruby = int.Parse( value[ 0 ][ "cRewardRuby" ] );
        int rewardstone = int.Parse( value[ 0 ][ "cRewardStone" ] );
        int rewardgold = int.Parse( value[ 0 ][ "cRewardGold" ] );

        PlayerData.I.SetRuby( ruby );
        PlayerData.I.SetStone( stone );
        PlayerData.I.SetGold( gold );

        CouponRewardPopup RankReward = GlobalUI.ShowUI( UI_TYPE.CouponRewardPopup ) as CouponRewardPopup;
        RankReward.Apply( StringTBL.GetData( 902163 ) , rewardgold , rewardstone , rewardruby );

        //GlobalUI.ShowOKPupUp( "쿠폰 지급이 완료 되었습니다." );


    }

    public void AchieveReward(SimpleJSON.JSONNode value)
    {
        int gold = int.Parse(value[0]["cGold"]);
        int ruby = int.Parse(value[0]["cRuby"]);
        int stone = int.Parse(value[0]["cStone"]);
        int shoes = int.Parse(value[0]["cShoes"]);
        int NextNum = int.Parse(value[0]["cNextNum"]);

        AchievementReferenceData data = AchievementManager.I.Clear( (AchievementReferenceData.MissionType)NetManager.achieveRewardType );

        PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
        popup.OnEnter();

        switch( data.rewardType )
        {
            case MissionReferenceData.RewardType.AP:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.ap );
            break;
            case MissionReferenceData.RewardType.Gold:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.gold );
            break;
            case MissionReferenceData.RewardType.Ruby:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.ruby );
            break;
            case MissionReferenceData.RewardType.Stone:
            popup.SetEx( StringTBL.GetData( data.name ) , data.RewardValue.ToString( "n0" ) , null , false , PopupOk.SubType.stone );
            break;
        }

        PlayerData.I.SetRuby( ruby );
        PlayerData.I.SetGold( gold );
        PlayerData.I.Setshoes( shoes );
        PlayerData.I.SetStone( stone );

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRefreshAchieve();
        }

    }


    //public void Achieve(SimpleJSON.JSONNode value)
    //{
    //    int gold = int.Parse(value[0]["cType"]);
    //    int ruby = int.Parse(value[0]["cNum"]);
    //    int stone = int.Parse(value[0]["cValue"]);

    //    if( SceneManager.I.scene == SCENE.MainScene )
    //    {
    //        GameObject go = SceneManager.I.GetScene();
    //        go.GetComponent<MainScene>().OnRefresh();
    //    }
    //}

    //public void SetAchieve( SimpleJSON.JSONNode value )
    //{
    //    int goalvalue = int.Parse( value[ 0 ][ "cGoalvalue" ] );
    //    int num = int.Parse( value[ 0 ][ "cNum" ] );
    //    int nvalue = int.Parse( value[ 0 ][ "cValue" ] );

    //    //AchievementManager.I.Add( (AchievementReferenceData.MissionType)NetManager.achievetype , num , nvalue , goalvalue );

    //    if( NetManager.achievetype == 20 )
    //    {
    //        if( SceneManager.I.scene == SCENE.MainScene )
    //        {
    //            GameObject go = SceneManager.I.GetScene();
    //            go.GetComponent<MainScene>().OnRefreshAchieve();
    //        }
    //    }
    //    else
    //        NetManager.GetAchieve( ++NetManager.achievetype );
        
    //}

    public void GetAchieveUser( SimpleJSON.JSONNode value )
    {        
        for( int i=0 ; i < value.Count ; i++ )
        {
            int Type = int.Parse( value[ i ][ "cType" ] );
            int num = int.Parse( value[ i ][ "cNum" ] );
            int nvalue = int.Parse( value[ i ][ "cValue" ] );

            AchievementManager.I.Add( (AchievementReferenceData.MissionType)Type , num , nvalue  );
        }

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRefreshAchieve();
        }
    }

    public void GetMailList( SimpleJSON.JSONNode value )
    {
        MailManager.I.StartMailList();

        for( int i = 0 ; i < value.Count ; i++ )
        {
            Int64 cIdx = Int64.Parse( value[ i ][ "cIdx" ] );
            int itemIndex = int.Parse( value[ i ][ "cItemIdx" ] );
            float cValue = float.Parse( value[ i ][ "cValue" ] );
            bool bExpire = bool.Parse( value[ i ][ "cIsExpire" ] );//  == 1 ? true : false ;
            DateTime cExpireDate = new DateTime();
            if( value[ i ][ "cExpireDate" ].Value.Length > 1)
                cExpireDate = DateTime.Parse( value[i][ "cExpireDate" ].Value , new System.Globalization.CultureInfo( "ko-KR" , true ) );

            int mailType = int.Parse( value[ i ][ "cMailType" ] );


            string mail = GetMailStringByType( ( MailType)mailType );
            MailManager.I.Add( cIdx , itemIndex , cValue , bExpire , cExpireDate , mail );
        }

        MailManager.I.EndMailList();

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRecvMailList();
        }

    }


    string GetMailStringByType( MailType type )
    {
        switch( type )
        {
            case MailType.StageReward:
            return StringTBL.GetData( 902147 );

            case MailType.LevelUpReward:
            return StringTBL.GetData( 902148 );
            case MailType.DailyMission:
            return StringTBL.GetData( 902149 );

            case MailType.RankReward:
            return StringTBL.GetData( 902150 );

            case MailType.AchieveReward:
            return StringTBL.GetData( 902151 );

            case MailType.ThemaReward:
            return StringTBL.GetData( 902155 );

            case MailType.MileageReward:
            return StringTBL.GetData( 902156 );

            case MailType.ShopBuy:
            return StringTBL.GetData( 902157 );

            case MailType.Event:
            return StringTBL.GetData( 902166 );
        }
        return "";
    }

    public void ReceiveMail( SimpleJSON.JSONNode value )
    {
        int gold = int.Parse( value[ 0 ][ "cGold" ] );
        int ruby = int.Parse( value[ 0 ][ "cRuby" ] );
        int stone = int.Parse( value[ 0 ][ "cStone" ] );
        int shoes = int.Parse( value[ 0 ][ "cShoes" ] );        
        float cGoldStone = float.Parse( value[ 0 ][ "cGoldStone" ] );
        int itemIdx = int.Parse( value[ 0 ][ "cItemIdx" ] );
        float cValue = float.Parse( value[ 0 ][ "cValue" ] );

        PlayerData.I.SetRuby( ruby );
        PlayerData.I.SetGold( gold );
        PlayerData.I.Setshoes( shoes );
        PlayerData.I.SetStone( stone );
        PlayerData.I.Setmileage( cGoldStone );


        //PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
        //popup.OnEnter();

        //MailData data = MailManager.I.Get( itemIdx );


        //string title = "";

        //if( data != null )
        //    title = data.Title;

        //switch( ( MailData.MailRewardType)itemIdx )
        //{
        //    case MailData.MailRewardType.ap:
        //    popup.SetEx( title , cValue.ToString( "n0" ) , null , false , PopupOk.SubType.ap );
        //    break;
        //    case MailData.MailRewardType.gold:
        //    popup.SetEx( title , cValue.ToString( "n0" ) , null , false , PopupOk.SubType.gold );
        //    break;
        //    case MailData.MailRewardType.ruby:
        //    case MailData.MailRewardType.ruby2:
        //    popup.SetEx( title , cValue.ToString( "n0" ) , null , false , PopupOk.SubType.ruby );
        //    break;
        //    case MailData.MailRewardType.stone:
        //    popup.SetEx( title , cValue.ToString( "n0" ) , null , false , PopupOk.SubType.stone );
        //    break;
        //    case MailData.MailRewardType.mileage:
        //    popup.SetEx( title , cValue.ToString( "F1" ) , null , false , PopupOk.SubType.mileage );
        //    break;
        //}

        if( SceneManager.I.scene == SCENE.MainScene )
        {
            GameObject go = SceneManager.I.GetScene();
            go.GetComponent<MainScene>().OnRecvMail( NetManager.currentMailIndex );
        }

        MailManager.I.Delete( NetManager.currentMailIndex );
    }


    public void ChangeProperty( SimpleJSON.JSONNode value )
    {
        int cardidx = int.Parse( value[ 0 ][ "cCardIdx" ] );
        long uidx = int.Parse( value[ 0 ][ "cUidx" ] );

        PlayerInfoUI infoui = GlobalUI.GetUI<PlayerInfoUI>( UI_TYPE.PlayerInfoUI );
        infoui.OnRecvProperty( cardidx , uidx );

    }

    public void GetRankList( SimpleJSON.JSONNode value )
    {
        int[] index = new int[ 4 ];
        for( int i = 0 ; i < value.Count ; i++ )
        {
            index[i] = int.Parse( value[ i ][ "cIndex" ] );
        }

        
        Array.Sort( index , delegate ( int a, int b)  { return a.CompareTo( b ); } );

        RankingStageManager.I.SetStageData( index );
    }




    public void CheckReceipt(SimpleJSON.JSONNode value)
	{
		//SimpleJSON.JSONArray oInfo = value.AsArray;

		//var code = oInfo[0]["code"];
		//if (string.IsNullOrEmpty(code) == true) { Logger.E("[CheckRecipt] code is null"); }
		//var message = oInfo[0]["message"];
		//if (string.IsNullOrEmpty(message) == true) { Logger.E("[CheckRecipt] message is null"); }

		//PurchaseManager.I.ResponseCheckReceipt(code, message);
		PurchaseManager.I.ResponseCheckReceipt();
	}

    
    public void CheckError(Result_Enum result , int protocol_identity_index, SimpleJSON.JSONNode value )
    {
        switch (result)
        {
            case Result_Enum.UserInit:
            PlayerData.I.UserIndex = int.Parse( value[ "oUserIdx" ] );
            PlayerData.I.oLinktoken = value[ "oLinkToken" ];
            GlobalUI.ShowUI(UI_TYPE.CreateAccountUI);
                break;

            case Result_Enum.ExistID:            
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 902164 ) );
            break;
            //case Result_Enum.EXISTENT_ID:
            //    UIManager.GetUI<CreateAccountPopup>(UI_TYPE.CREATEACCOUNT_POPUP).ReceiveAccount(false);
            //    break;
            //case Result_Enum.EMPTY_RANKINGREWARD:
            //    break;
            default:                
                GlobalUI.ShowOKPupUp( "ErrorCode :" + result.ToString() + "    CallIndex :" + ((NETCALL)protocol_identity_index).ToString(), OnExit ); //value["oMessage"] +

            break;
        }     

    }

    public void OnExit()
    {
        Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }


}

