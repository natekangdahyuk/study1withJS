using System.Collections.Generic;
using UnityEngine;
using System;

/*
 loadLoginFlow
 manageRecurringAuto
 buyProduct
 consumeItem
 getPurchase
 isBillingSupported
 getProductDetails
 connect
 */

namespace OneStore
{
	public class Onestore_IapCallbackManager : MonoSinglton<Onestore_IapCallbackManager>
	{
		public static Action regServiceAvailable;

		public static Action<string> regServiceConnection;
		public static Action<string> regIsBillingSupported;

		public static Action<PurchaseData> regGetPurchaseSuccess;
		public static Action<string> regGetPurchaseError;

		public static Action<ProductDetail> regQueryProductsSuccess;
		public static Action<string> regQueryProductsError;

		public static Action<PurchaseData> regGetPurchaseIntentSuccess;
		public static Action<string> regGetPurchaseIntentError;

		public static Action<PurchaseData> regConsumeSuccess;
		public static Action<string> regConsumeError;

		public static Action<PurchaseData> regManageRecurringSuccess;
		public static Action<string> regManageRecurringError;

		public static Action<string> regGetLoginIntent;

		public override void Constructor()
		{
			base.Constructor();
			DontDestroyOnLoad( this.gameObject );

			//#if !UNITY_EDITOR
			//		Onestore_IapCallManager.Init();
			//#endif
		}

		public override void ClearAll()
		{
		}

		enum CBType
		{
			Connected,
			Disconnected,
			NeedUpdate,
			Success,
			Error,
			RemoteEx,
			SecurityEx,
		};

		Dictionary<CBType, string> preDefinedStrings = new Dictionary<CBType, string>() {

		{ CBType.Connected, "onConnected" },
		{ CBType.Disconnected, "onDisconnected" },
		{ CBType.NeedUpdate, "onErrorNeedUpdateException" },
		{ CBType.Success, "onSuccess" },
		{ CBType.Error, "onError" },
		{ CBType.RemoteEx, "onErrorRemoteException" },
		{ CBType.SecurityEx, "onErrorSecurityException" }
	};

		/*
		- descritpion:  connect의 결과를 제공하기 위한 콜백  , 결과값은 1~3 중의 하나의 string으로 받는다.
		- callback 종류:
		1. onConnected: 서비스 Binding 성공
		2. onDisconnected : 서비스 Un-binding 되었을때
		3. onErrorNeedUpdateException: 원스토어 서비스가 최신버전이 아닐 경우 또는 미설치 시에 발생, launchUpdateOrInstallFlow메서드를 이용하여 사용자에세 설치를 유도하거나 개발사에서 직접 처리
		*/

		public void ServiceConnectionListener( string callback )
		{
			Logger.N( "[Listener] " + callback );
			if( callback.Contains( preDefinedStrings[ CBType.Connected ] ) )
			{
				if( regServiceAvailable != null )
					regServiceAvailable();
			}

			if( regServiceConnection != null )
				regServiceConnection( callback );
		}

		/*
		 - descritpion: In-app Purchase v17 지원여부를 확인에 대한 응답
		 - callback 종류:
		1. onSuccess: 상태정보 조회가 성공일 경우
		2. onError : 실패일 경우 애플리케이션으로 에러코드 전달. 숫자로 된  code와 string으로 된 description을 ToString형태로 넘겨준다. 
		3. onErrorRemoteException: 서비스 Un-bind 시점에 Remote call 을 요청할 경우 발생
		4. onErrorSecurityException: 애플리케이션이 변조되거나 정상적이지 않는 APK 형태 일때 발생
		5. onErrorNeedUpdateException: 원스토어 서비스가 최신버전이 아닐 경우 또는 미설치 시에 발생
		*/
		public void BillingSupportedListener( string callback )
		{
			regIsBillingSupported( callback );
		}

		/*
		 - descritpion: IAP v17 구매 내역 조회를 위한 메서드에 대한 응답
		 - callback 종류:
		 1. onSuccess:  구매정보 조회 호출에 대한 성공 응답
			만약 구매한 데이터가 있다면 “onSuccess” + json (productType:  inapp or auto인지 상품타입,  purchaseData: 구매한 원본 json 구매 데이터, signature:  구매한 시그너처)
			만약 구매한 데이터가 없다면 “onSuccess”+ “[]” + producttype(“inapp” or “auto”)와 같은 형식으로 넘어온다. 
		 2. onErrorRemoteException
		 3. onErrorSecurityException
		 4. onErrorNeedUpdateException
		 5. onError: 실패일 경우 애플리케이션으로 에러코드 전달. 숫자로 된  code와 string으로 된 description을 ToString형태로 넘겨준다. 
		*/
		public void QueryPurchaseListener( string callback )
		{
			string data = findStringAfterCBType( callback, CBType.Success );
			if( data.Length > 0 )
			{
				if( data.Contains( "[]" ) == true )
				{
					regGetPurchaseSuccess( null );
				}
				else
				{
					try
					{
						Logger.N( "listen [] call data" );
						Onestore_PurchaseResponse purchaseRes = JsonUtility.FromJson<Onestore_PurchaseResponse>( data );
						PurchaseData purchaseData = JsonUtility.FromJson<PurchaseData>( purchaseRes.purchaseData );
						regGetPurchaseSuccess( purchaseData );
					}
					catch( System.Exception ex )
					{
						Debug.Log( "QueryPurchaseListener Exception " + ex.Message );
						regGetPurchaseError( data ); //success but no data 
					}
				}
			}
			else
			{
				//onError만 뒤에 추가데이터 IapResult 가 있으므로 추가 데이터만 전달해주고 나머지 에러들은 추가 데이터가 없으므로 callback 그대로만 전달해준다. 
				string errorData = findStringAfterCBType( callback, CBType.Error );
				if( errorData.Length > 0 )
				{
					regGetPurchaseError( errorData );
				}
				else
				{
					regGetPurchaseError( callback );
				}
			}

		}

		/*
		 - descritpion: IAP v17 상품 정보 조회를 위한 메서드에 대한 응답
		 - callback 종류:
		 1. onSuccess:  상품정보 조회 호출에 대한 성공 응답 , json 형태 
		 2. onErrorRemoteException
		 3. onErrorSecurityException
		 4. onErrorNeedUpdateException
		 5. onError
		*/
		public void QueryProductsListener( string callback )
		{
			string data = findStringAfterCBType( callback, CBType.Success );
			if( data.Length > 0 )
			{
				ProductDetail productDetail = JsonUtility.FromJson<ProductDetail>( data );
				regQueryProductsSuccess( productDetail );
			}
			else
			{
				string errorData = findStringAfterCBType( callback, CBType.Error );
				regQueryProductsError( errorData );
			}
		}

		/*
		 - descritpion: IAP v17 구매요청을 위한 메서드에 대한 응답
		 - callback 종류:
		 1. onSuccess:  구매요청에 대한 성공 응답
		 2. onErrorRemoteException
		 3. onErrorSecurityException
		 4. onErrorNeedUpdateException
		 5. onError
		*/
		public void PurchaseFlowListener( string callback )
		{
			string data = findStringAfterCBType( callback, CBType.Success );

			if( data.Length > 0 )
			{
				try
				{
					Onestore_PurchaseResponse purchaseRes = JsonUtility.FromJson<Onestore_PurchaseResponse>( data );
					PurchaseData purchaseData = JsonUtility.FromJson<PurchaseData>( purchaseRes.purchaseData );
					regGetPurchaseIntentSuccess( purchaseData );
				}
				catch( System.Exception ex )
				{
					Debug.Log( "PurchaseFlowListener Exception " + ex.Message );
				}
			}
			else
			{
				//onError만 뒤에 추가데이터 IapResult 가 있으므로 추가 데이터만 전달해주고 나머지 에러들은 추가 데이터가 없으므로 callback 그대로만 전달해준다. 
				string errorData = findStringAfterCBType( callback, CBType.Error );
				if( errorData.Length > 0 )
				{
					regGetPurchaseIntentError( errorData );
				}
				else
				{
					regGetPurchaseIntentError( callback );
				}
			}
		}

		/*
		 - descritpion: IAP v17 상품소비 호출 메서드에 대한 응답
		 - callback 종류:
		 1. onSuccess:  소비요청에 대한 성공 응답
		 2. onErrorRemoteException
		 3. onErrorSecurityException
		 4. onErrorNeedUpdateException
		 5. onError
		*/
		public void ConsumeListener( string callback )
		{
			string data = findStringAfterCBType( callback, CBType.Success );
			if( data.Length > 0 )
			{
				PurchaseData purchaseData = JsonUtility.FromJson<PurchaseData>( data );
				regConsumeSuccess( purchaseData );
			}
			else
			{
				string errorData = findStringAfterCBType( callback, CBType.Error );
				if( errorData.Length > 0 )
				{
					regConsumeError( errorData );
				}
				else
				{
					regConsumeError( callback );
				}
			}

		}

		/*
		 - descritpion: 월정액상품(auto)의 상태변경(해지예약 / 해지예약 취소)를 진행
		 - callback 종류:
		 1. onSuccess:  월정액 상태변경 호출에 대한 성공 응답
		 2. onErrorRemoteException
		 3. onErrorSecurityException
		 4. onErrorNeedUpdateException
		 5. onError
		 */
		public void ManageRecurringProductListener( string callback )
		{
			string data = findStringAfterCBType( callback, CBType.Success );
			if( data.Length > 0 )
			{
				PurchaseData response = JsonUtility.FromJson<PurchaseData>( data );
				regManageRecurringSuccess( response );
			}
			else
			{
				string errorData = findStringAfterCBType( callback, CBType.Error );
				if( errorData.Length > 0 )
				{
					regManageRecurringError( errorData );
				}
				else
				{
					regManageRecurringError( callback );
				}
			}
		}

		/*
		 - descritpion: IAP v17 로그인요청을 위한 메서드
		 - callback 종류:
		 1. onSuccess:  성공일 경우
		 2. onErrorRemoteException
		 3. onErrorSecurityException
		 4. onErrorNeedUpdateException
		 5. onError
		 */
		public void LoginFlowListener( string callback )
		{
			regGetLoginIntent( callback );//just pass 
		}

		// 결과 callback string에서 CBType  string을 제외한 다음 문자열을 돌려준다.
		private string findStringAfterCBType( string data, CBType type )
		{
			int length = preDefinedStrings[ type ].Length;
			if( data.Substring( 0, length ).Equals( preDefinedStrings[ type ] ) )
			{
				return data.Substring( length );
			}
			else
			{
				return "";
			}
		}
	}
}