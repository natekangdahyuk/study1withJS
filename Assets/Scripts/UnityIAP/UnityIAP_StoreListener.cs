/// 결제 콜백 리스너
/// 결제 기능을 사용하려면
/// ProjectSetting의 Scripting Define Symbols에 UNITY_IAP 를 추가해주세요.

#if UNITY_IAP
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[ UnityEngine.HelpURL( "https://docs.unity3d.com/Manual/UnityIAP.html" )]
public class UnityIAP_StoreListener : Singleton<UnityIAP_StoreListener>, IStoreListener
{
	/// <summary>
	/// 구매 관련 인터페이스. 이 클래스 외부에서 사용할 일은 없다.
	/// </summary>
	private IStoreController controller { get; set; }

	/// <summary>
	/// 구글 결제 모듈 확장 인터페이스.
	/// </summary>
	private IGooglePlayStoreExtensions googlePlayStoreExtension;

	/// <summary>
	/// 구매 처리 이벤트.
	/// Product		-> 구매 요청한 상품.
	/// bool		-> 구매 성공 / 실패 여부.
	/// 이 이벤트에서 서버 검증 후 ConfirmPendingPurchase 호출이 필요합니다.
	/// </summary>
	private Action<Product, bool> OnPurchaseProcess;

	#region Initialize
	/// 초기화는 네트워크를 사용할 수 없는 경우에도 실패하지 않습니다.
	/// Unity IAP는 백그라운드에서 계속 초기화를 시도합니다.
	/// Unity IAP에서 설정이 잘못되었거나
	/// 기기 설정에서 IAP가 비활성화되는 등 복구 불가능한 문제가 발생하는 경우에만 초기화가 실패합니다.
	/// 따라서 Unity IAP를 초기화하는 데는 임의의 기간이 소요될 수 있고
	/// 비행기 모드에서는 무한한 기간이 소요될 수 있습니다.
	/// 초기화가 성공적으로 완료되지 않은 경우 사용자가 구매를 시도하지 않도록 스토어를 디자인해야 합니다.

	public class ProductData
	{
		/// <summary>
		/// 게임에서 사용할 상품 ID
		/// </summary>
		public readonly string productID;

		/// <summary>
		/// 상품 유형
		/// </summary>
		public readonly UnityEngine.Purchasing.ProductType type;

		/// <summary>
		/// 스토어 별 상품 ID. 아래 형태와 같은 구조
		/// new IDs {
		///		{"100_gold_coins_google", GooglePlay.Name}
		///		{"100_gold_coins_mac", MacAppStore.Name}
		/// }
		/// </summary>
		public readonly IDs storeSpecificId;

		public ProductData( string productID, UnityEngine.Purchasing.ProductType type, IDs storeSpecificId )
		{
            this.productID = productID;
			this.type = type;
			this.storeSpecificId = storeSpecificId;
		}
	}

	public bool IsServiceAvailable { get { return controller != null; } }

	/// <summary>
	/// 결제 초기화 요청. 유니티 결제 초기화를 위해선 상품목록이 필요합니다.
	/// </summary>
	/// <param name="lstProductData">상품 목록</param>
	public void InitializeIAP( List<ProductData> lstProductData )
	{
		var builder = ConfigurationBuilder.Instance( StandardPurchasingModule.Instance() );
		if( builder == null )
			throw new System.NullReferenceException();

		foreach( ProductData p in lstProductData )
		{
			builder.AddProduct( p.productID, p.type, p.storeSpecificId );
		}

		UnityPurchasing.Initialize( this, builder );

		Debug.LogFormat(
			"Trying to initialize UnityIAP with {0} products."
			, lstProductData != null ? lstProductData.Count.ToString() : "0"
			);
	}

	/// <summary>
	/// 초기화 성공 콜백.
	/// </summary>
	/// <param name="controller"></param>
	/// <param name="extensions">스토어 별 확장 인터페이스</param>
	public void OnInitialized( IStoreController controller, IExtensionProvider extensions )
	{
		this.controller = controller;
		this.googlePlayStoreExtension = extensions.GetExtension<IGooglePlayStoreExtensions>();

		Debug.Log( "UnityIAP Initialize Success!!!" );
	}

	/// <summary>
	/// 초기화 실패 콜백
	/// </summary>
	/// <param name="error"></param>
	public void OnInitializeFailed( InitializationFailureReason error )
	{
		this.controller = null;
		this.googlePlayStoreExtension = null;

		//!< 여기에 결제 초기화 오류 팝업 노출

		Debug.LogErrorFormat( "UnityIAP Initialize Failed. Error : {0}", error.ToString() );
	}
	#endregion

	#region Purchase
	/// <summary>
	/// 구매 요청. 구매 시작할 때 호출.
	/// </summary>
	/// <param name="productID">구매할 상품 ID</param>
	/// <param name="evt">스토어에 구매 요청 후 구매 과정에서 필요한 콜백</param>
	/// <returns></returns>
	public void RequestPurchase( string productID, Action<Product, bool> evt )
	{
		if( controller == null )
		{
			//!< 유니티 결제 모듈 초기화 에러
			throw new System.Exception( "UnityIAP is not initialized." );
		}

		controller.InitiatePurchase( productID );
		OnPurchaseProcess = evt;
	}

	/// <summary>
	/// 서버에서 검증 완료 후 결제 완료를 위해 호출될 메서드.
	/// </summary>
	/// <param name="product">구매 완료 처리해야할 상품</param>
	public void ConfirmPendingPurchase( Product product )
	{
		if( product == null )
			throw new System.NullReferenceException( "UnityIAP Error : Product is Null" );

		if( controller != null )
			controller.ConfirmPendingPurchase( product );
	}

	/// <summary>
	/// ProcessPurchase는 초기화 성공 이후 아무때나 호출될 수 있습니다. ProcessPurchase 핸들러 실행 도중 애플리케이션이 크래시하는 경우, Unity IAP가 다음에 초기화되면 다시 호출되므로 추가적으로 데이터 중복 제거를 구현하는 것이 좋습니다.
	/// </summary>
	/// <param name="e"></param>
	/// <returns>
	/// return Complete -> 애플리케이션이 구매 처리를 완료하였으며 이에 대해 다시 알리지 않습니다.
	/// return Pending -> 애플리케이션이 아직 구매를 처리하고 있는 상태이며 애플리케이션이 다음에 실행되면 IStoreController의 ConfirmPendingPurchase 함수가 호출되지 않는 한 ProcessPurchase가 호출됩니다.
	/// </returns>
	public PurchaseProcessingResult ProcessPurchase( PurchaseEventArgs e )
	{
		NetManager.SetCheckReceipt_Google( e.purchasedProduct );

		if( OnPurchaseProcess != null )
			OnPurchaseProcess( e.purchasedProduct, true );

		//!< OnPurchaseProcess - 구매 처리 이벤트에서 서버 검증이 필요하므로 Pending을 리턴
		return PurchaseProcessingResult.Pending;
	}

	/// <summary>
	/// 구매 실패 콜백
	/// </summary>
	/// <param name="i">구매에 실패한 상품/param>
	/// <param name="p">구매 실패 타입 Enum</param>
	public void OnPurchaseFailed( Product i, PurchaseFailureReason p )
	{
		if( OnPurchaseProcess != null )
			OnPurchaseProcess( i, false );

		Debug.LogErrorFormat(
			"UnityIAP Purchase Failed. ProductID : {0}, Error : {1}"
			, i.definition.id
			, p.ToString()
			);
	}
	#endregion
}
#endif