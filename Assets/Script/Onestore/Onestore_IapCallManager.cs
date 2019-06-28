using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneStore
{
	public class Onestore_IapCallManager
	{
		private static AndroidJavaObject iapRequestAdapter = null;
		private static AndroidJavaClass jc = null;
		private static bool isServiceCreated = false;
		private const int IAP_API_VERSION = 5;

		static void cbServiceAvailable()
		{
			Logger.N("[OneStore, callback] Service Available");
			isServiceCreated = true;			
		}

		//서비스가 연결 및 초기화가 되지 않았을때 다른 명령어가 수행되지 않도록 체크 역할
		public static bool IsServiceAvailable()
		{
			return isServiceCreated;
		}		
		
		static Onestore_IapCallManager()
		{	
		}

		//GameObject 이름이 Onestore_IapCallbackManager 안드로이드로부터 결과를 콜백받기 위해서 초기에 registerCallbackGameObject 를 통해서 등록한다.
		// 이 함수는 callback manager가 직접 호출한다.
		public static void Init()
		{
			if (isServiceCreated == true) return;			

			Logger.N("[OneStore] initalize");

			if (Onestore_IapCallbackManager.regServiceAvailable == null)
			{
				Onestore_IapCallbackManager.regServiceAvailable = cbServiceAvailable;
				jc = new AndroidJavaClass("com.onestore.iap.sdk.unity.IapPlugin");
				iapRequestAdapter = jc.CallStatic<AndroidJavaObject>("initAndGet", "Onestore_IapCallbackManager");
			}
			else
			{
				Logger.E("IAP service inialize already in progressing...");
			}
		}

		public static void ConnectService()
		{
			Logger.N("[OneStore] connect service");
			iapRequestAdapter.Call("connect", "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCh3Y5VMYt/i8WYv3mLHQ8f5n4qFNnIUb+gCZj2J+Xthrh3X7ciHy8MHgzdnsqAunuk1HL8o/vE7sWxi7bf8TE+4QUan9kRJMJnVlvcXyBNLNX7Jh4vkmBEqrSTjlHLh0O+WpXyeyjbICWTsZ+K/SI0lbYgxBxYNOR/kldQTu/k2QIDAQAB");
		}

		public static void isBillingSupported()
		{
			if(IsServiceAvailable() == true)
			{
				iapRequestAdapter.Call("isBillingSupported", IAP_API_VERSION);
			}			
		}

		//inapp과 auto 두 가지 상품 타입이 존재하는데 각각 상품에 대해서 각각 따로 호출해야 한다.  개발사에서 변경 가능한 부분
		public static void getPurchases()
		{
			if (IsServiceAvailable() == true)
			{
				iapRequestAdapter.Call("getPurchase", IAP_API_VERSION, "inapp");
				//iapRequestAdapter.Call("getPurchase", IAP_API_VERSION, "auto");		// 정액 상품을 사용하게 될경우 쓴다
			}
		}

		public static void getProductDetails(string[] products, string productType)
		{
			if (IsServiceAvailable() == true)
			{
				iapRequestAdapter.Call("getProductDetails", new object[] { IAP_API_VERSION, products, productType });
			}
		}

		/*
			  일반적으로 잘 사용하지 않기 때문에 default ("" , false)  값으로 넘겨주며 사용하고자 할때 개발자가 값을 변경해서 넣으면 됩니다. 
			 1. gameUserId: 
			    어플리케이션을 사용 중인 사용자의 고유 인식 번호를 입력합니다. 해당 값은 프로모션 참가 가능 여부 및 프로모션 사용 여부를 판가름 하는 key value로 사용됩니다.
			 2. promotionApplicable: 
			    프로모션 참가 가능 여부를 전달합니다. true : gameUserId로 전달된 사용자는 단일 프로모션에 1회 참가가 가능합니다. false : gameUserId로 전달된 사용자는 프로모션에 참가가 불가능 합니다.
			 */
		public static void buyProduct(string productId, string productType, string payload)
		{
			if(IsServiceAvailable() == true)
			{
				string gameUserId = "";
				bool promotionApplicable = false;
				iapRequestAdapter.Call("launchPurchaseFlow", IAP_API_VERSION, productId, productType, payload, gameUserId, promotionApplicable);
			}			
		}

		public static void consume(string inapp_json)
		{
			if (IsServiceAvailable() == true)
				iapRequestAdapter.Call("consumeItem", IAP_API_VERSION, inapp_json);
		}

		public static void manageRecurringAuto(string auto_json, string command)
		{
			if (IsServiceAvailable() == true)
				iapRequestAdapter.Call("manageRecurringAuto", IAP_API_VERSION, auto_json, command);
		}

		public static void login()
		{
			if (IsServiceAvailable() == true)
				iapRequestAdapter.Call("launchLoginFlow", IAP_API_VERSION);
		}

		public static void destroy()
		{
			if (IsServiceAvailable() == true)
			{
				iapRequestAdapter.Call("dispose");
				isServiceCreated = false;
			}				
		}
	}
}
