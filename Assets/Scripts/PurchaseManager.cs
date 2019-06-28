using System;
using System.Collections;
using System.Collections.Generic;
using OneStore;
using UnityEngine;
using IgaworksUnityAOS;

public class PurchaseManager : MonoSinglton<PurchaseManager>
{
	public enum ErrorCode
	{
		None = 0,		
		NeedOneStoreLogin = 1,
		FailGetPurchaseHistory = 2,
		FailUnconsumedPurchase = 3,
		FailPurchase = 4,		
		FailConsume = 5,
		Unknown = 9999,
		InProgress = 10000
	}

	public override void ClearAll()
	{		
	}

	public abstract class InAppProductData
	{
		public enum ProductType
		{
			Shop,
			Package,			
		}

		public bool _open = false;
		public OneStore.ProductDetail _onestore_product_detail = null;
		public abstract string ProductCode { get; }
		public abstract ProductType Type { get; }
	}

	public class InAppShopData : InAppProductData
	{
		public ShopReferenceData _data;
		public override string ProductCode { get { if (_data == null) return ""; return _data.product_code; } }
		public override ProductType Type { get { return ProductType.Shop; } }
	}

	public class InAppPackageData : InAppProductData
	{
		public PackageData _data;
		public override string ProductCode { get { if (_data == null) return ""; return _data.prdCode; } }
		public override ProductType Type { get { return ProductType.Package; } }
	}

	List<InAppProductData> _inapp_product_datas = new List<InAppProductData>();	

	bool _in_transaction = false;
	public bool InTransaction
	{
		get
		{
			return _in_transaction;
		}
	}

	bool _req_products_details_fin = false;
	int _total_req_products_count = 0;
	int _recv_req_products_count = 0;

	bool _req_billing_support = false;
	public bool ReadyForBillingSupport
	{
		get { return _req_billing_support; }
	}

	bool _is_billing_support = false;
	public bool IsBillingSupport
	{
		get { return _is_billing_support; }
	}

	public bool IsServiceAvailable
	{
		get { return Onestore_IapCallManager.IsServiceAvailable(); }
	}

	// 무조건 상품 정보가 한개 이상만 유효해도 문제 되지 않도록 수정
	public bool IsDataVailded { get { return _inapp_product_datas.Count > 0 && _in_transaction == false && _req_products_details_fin == true; } }
	public bool ProductRequestFin { get { return _req_products_details_fin; } }
	
	Action<ErrorCode, string> _cbPurchaseHistory = null;
	Action<PurchaseData, ErrorCode, string> _cbPurchase = null;	
	Action<PurchaseData, string> _cbUnconsume = null;
	Action<PurchaseData, ErrorCode, string> _cbConsume = null;

	PurchaseData _unconsume_purchase_data = null;
	public PurchaseData UnconsumePurchaseData
	{
		get { return _unconsume_purchase_data; }
	}

	public void Init()
	{
		_req_products_details_fin = false;		
		
		Onestore_IapCallManager.Init();
		Onestore_IapCallManager.ConnectService();
	}



	public InAppProductData FindProductData(string product_code)
	{
		if (string.IsNullOrEmpty(product_code) == true) return null;
		product_code = product_code.Trim();
		var p = _inapp_product_datas.Find(x =>
		{
			return x.ProductCode.CompareTo(product_code) == 0;
		});

		if(p == null)
		{
			Logger.E("Fail to find ProductData, " + product_code);
		}

		return p;
	}

	public void PreprarePurchase(Action<ErrorCode, string> cb)
	{
		if(_inapp_product_datas.Count == 0)
		{
			for (int i = (int)ShopType.Ruby; i <= (int)ShopType.MAX; ++i)
			{
				var sg = ShopTBL.GetGroup((ShopType)i);
				if (sg != null)
				{
					for (int j = 0; j < sg.Count; ++j)
					{
						var data = sg[j] as ShopReferenceData;
						if (string.IsNullOrEmpty(data.product_code) == false)
						{
							_inapp_product_datas.Add(new InAppShopData()
							{
								_data = data,
								_open = false,
							});

							Logger.N("[Product,Init] shop, sc : " + data.product_code);
						}
					}
				}				
			}

			for (int i = 0; i < PackageManager.I.PackageList.Count; ++i)
			{
				var data = PackageManager.I.PackageList[i];
				if (string.IsNullOrEmpty(data.prdCode) == false)
				{
					_inapp_product_datas.Add(new InAppPackageData()
					{
						_data = data,
						_open = false
					});

					Logger.N("[Product,Init] shop, pc : " + data.prdCode);
				}
			}
		}		


		StartCoroutine(coPreparePurchase(cb));
	}

	public IEnumerator coPreparePurchase(Action<ErrorCode, string> cb)
	{	
		bool fin = false;
		string err_msg = null;
		ErrorCode err_code = ErrorCode.None;
		if (_is_billing_support == false)
		{
			yield return StartCoroutine(coBillingSupport((code, msg) =>
			{
				err_code = code;
				err_msg = msg;
				fin = true;
			}));

			yield return new WaitUntil(() => { return fin; });
			fin = false;

			// billing support에 별 문제가 없을경우 아래 로직을 진행
			if (err_code == ErrorCode.None)
			{
				ReqPurchaseHistory((code, msg) =>
				{
					err_msg = msg;
					err_code = code;
					fin = true;					
				});

				yield return new WaitUntil(() => { return fin; });
				fin = false;
			}
		}

		if (cb != null) cb(err_code, err_msg);
	}

	IEnumerator coBillingSupport(Action<ErrorCode, string> cb)
	{
		string err_msg = null;
		ErrorCode err_code = ErrorCode.None;
		bool fin = false;

		_is_billing_support = false;

		// billing support
		Onestore_IapCallbackManager.regIsBillingSupported = (result) =>
		{
			if (result.Contains("onSuccess") == false)
			{
				err_code = ErrorCode.Unknown;
				var onestore_err_code = ParseErrorCode(result);
				if (onestore_err_code != int.MaxValue)
				{
					if(onestore_err_code == 10)
					{
						err_code = ErrorCode.NeedOneStoreLogin;
					}
				}
			}
			else
			{
				_is_billing_support = true;
			}
			fin = true;
		};

		Onestore_IapCallManager.isBillingSupported();
		yield return new WaitUntil(() => { return fin; });
		fin = false;

		// error code가 10이면 
		if (err_code == ErrorCode.NeedOneStoreLogin)
		{
			Onestore_IapCallbackManager.regGetLoginIntent = (result) =>
			{
				Logger.N(result);
				if (result.Contains("onError") == true)
				{
					err_msg = result;
				}
				fin = true;
			};

			Onestore_IapCallManager.login();
			yield return new WaitUntil(() => { return fin; });
		}

		if (cb != null) cb(err_code, err_msg);
	}

	bool _net_chk_recipt = false;
	string _net_chk_recipt_code = null;
	string _net_chk_recipt_msg = null;

	public void CheckReceipt(ProductDetail detail, PurchaseData data, Action<bool> cb)
	{
		StartCoroutine(coCheckReceipt(detail, data, cb));
	}

	public IEnumerator coCheckReceipt(ProductDetail detail, PurchaseData data, Action<bool> cb)
	{
		if(_in_transaction == true)
		{
			if (cb != null) cb(false);
			yield break;
		}

		_in_transaction = true;
		_net_chk_recipt_code = null;
		_net_chk_recipt_msg = null;

		_net_chk_recipt = false;
		NetManager.SetCheckReceipt(detail, data);
		yield return new WaitUntil(() => { return _net_chk_recipt; });

		_in_transaction = false;

		//if (string.IsNullOrEmpty(_net_chk_recipt_code) == true ||
		//	string.IsNullOrEmpty(_net_chk_recipt_msg) == true)
		//{
		//	if (cb != null) cb(false);
		//	yield break;
		//}
		
		if (cb != null) cb(true);
	}

	public void ResponseCheckReceipt(/*string code, string message*/)
	{
		//Logger.N("response check reciept code : " + code);
		//Logger.N("response check reciept message : " + message);

		//_net_chk_recipt_code = code;
		//_net_chk_recipt_msg = message;

		_net_chk_recipt = true;		
	}

	public void ReqPurchaseHistory(Action<ErrorCode, string> cb)
	{
		Logger.N("[Purchase,History]");
		
		_cbPurchaseHistory = cb;

		Onestore_IapCallbackManager.regGetPurchaseSuccess = regGetUnconumedPurchaseSuccess;
		Onestore_IapCallbackManager.regGetPurchaseError = regGetUnconumedPurchaseError;

		Onestore_IapCallManager.getPurchases();
	}

	void regGetUnconumedPurchaseSuccess(PurchaseData result)
	{
		Logger.N("[GetUnconumedPurchase,Success]");
		if (result != null)
		{
			Logger.N("[History] " + result.ToString());
			if (result.purchaseState != 1)
			{
				_unconsume_purchase_data = result;
			}
		}

		if (_cbPurchaseHistory != null)
		{
			_cbPurchaseHistory(ErrorCode.None, null);
			_cbPurchaseHistory = null;
		}
	}

	void regGetUnconumedPurchaseError(string error)
	{
		Logger.N("[GetUnconumedPurchase,Error] " + error);

		_in_transaction = false;		

		if (_cbPurchaseHistory != null)
		{
			_cbPurchaseHistory(ErrorCode.FailUnconsumedPurchase, error);
			_cbPurchaseHistory = null;
		}
	}

	//void regBillingSupport(string result)
	//{
	//	Logger.N("[Purchase, cb] is billing support " + result);

	//	_req_billing_support = true;		
	//	if(result.Trim().CompareTo("onSuccess") == 0)
	//	{
	//		_is_billing_support = true;

	//		if(_cbBillingSupport != null)
	//		{
	//			_cbBillingSupport(false, null);
	//			_cbBillingSupport = null;
	//		}
	//	}
	//	else
	//	{
	//		if (result.Contains("onErrorIapResult") == true)
	//		{
	//			var error_code = ParseErrorCode(result);
	//			if(error_code != int.MaxValue)
	//			{
	//				if (_cbBillingSupport != null)
	//				{
	//					_cbBillingSupport(true, error_code.ToString());
	//					_cbBillingSupport = null;
	//				}
	//			}
	//		}

	//		if (_cbBillingSupport != null)
	//		{
	//			_cbBillingSupport(true, result);
	//			_cbBillingSupport = null;
	//		}
	//	}
	//}

	public void ReqPurchase(string product_id, string product_type, string payload, Action<PurchaseData, ErrorCode, string> cb)
	{
		Logger.N("[Purchase,Product] id : " + product_id + ",type: " + product_type + ",payload: " + payload);
		if (_in_transaction == true) return;
		_in_transaction = true;

		_cbPurchase = cb;

		Onestore_IapCallbackManager.regGetPurchaseIntentSuccess = PurchaseSuccess;
		Onestore_IapCallbackManager.regGetPurchaseIntentError = PurchaseError;

		Onestore_IapCallManager.buyProduct(product_id, product_type, payload);
	}

	void PurchaseSuccess(PurchaseData result)
	{
		Logger.N("[Purchase,Product,Success] " + result.ToString());

		_in_transaction = false;

		if(_cbPurchase != null)
		{
			_cbPurchase(result, ErrorCode.None, null);
			_cbPurchase = null;
		}
	}

	void PurchaseError(string result)
	{
		Logger.E("[Purchase,Product,Error] " + result);

		_in_transaction = false;		

		if (_cbPurchase != null)
		{
			_cbPurchase(null, ErrorCode.FailPurchase, result);
			_cbPurchase = null;
		}
	}

	// 상품 정보 문의
	public void RequestProducts()
	{
		Logger.N("[Purchase,Product]");

		List<string> products = new List<string>();		
		for(int i = 0; i < _inapp_product_datas.Count; ++i)
		{
			var p = _inapp_product_datas[i].ProductCode;
			if(string.IsNullOrEmpty(p) == false && p.CompareTo("0") != 0)
			{
				products.Add(p);
				Logger.N("Request id : " + p);
				_total_req_products_count++;
			}
		}

		Onestore_IapCallManager.getProductDetails(products.ToArray(), "inapp");
	}

	// 일정시간 이상동안 데이터가 꾸준히 들어오지 않으면
	// 임의로 데이터가 잘못되었다고 표시하는 로직이 필요할 수도 있음
	public void SetQueryProductData(OneStore.ProductDetail product_detail)
	{		
		Logger.N("[Purchase,QueryProduct] " + product_detail.ToString());		

		product_detail.productId = product_detail.productId.Trim();
		for (int i = 0; i < _inapp_product_datas.Count; ++i)
		{
			var product_data = _inapp_product_datas[i];
			if (product_data._open == true) continue;
			if (product_data.ProductCode.CompareTo(product_detail.productId) == 0)
			{
				product_data._open = true;
				product_data._onestore_product_detail = product_detail;
				break;
			}			
		}

		_recv_req_products_count++;
		_req_products_details_fin = _recv_req_products_count >= _total_req_products_count;
	}

	// onestore로 부터 상품정보를 아예 받지 못했을경우에 대한처리
	public void SetQueryProductFailed()
	{		
		_req_products_details_fin = true;
		_inapp_product_datas.Clear();
	}

	public bool IsValidProduct(string product_code)
	{
		for(int i = 0; i < _inapp_product_datas.Count; ++i)
		{
			if(_inapp_product_datas[i].ProductCode.CompareTo(product_code) == 0)
			{
				return true;
			}
		}

		return false;
	}

	public OneStore.ProductDetail GetProductDetail(string product_code)
	{
		for (int i = 0; i < _inapp_product_datas.Count; ++i)
		{
			if (_inapp_product_datas[i].ProductCode.CompareTo(product_code) == 0)
			{
				return _inapp_product_datas[i]._onestore_product_detail;
			}
		}

		return null;
	}

	public void ReqConsumeForUnconsumedProducts(Action<PurchaseData, string> cb, PurchaseData purchase_data)
	{
		if (purchase_data == null) return;
		if (_in_transaction == true) return;
		_in_transaction = true;

		_cbUnconsume = cb;

		Onestore_IapCallbackManager.regConsumeSuccess = UnconsumedProductConsumeSuccess;
		Onestore_IapCallbackManager.regConsumeError = UnconsumedProductConsumeError;
		Onestore_IapCallManager.consume(JsonUtility.ToJson(purchase_data));
	}

	void UnconsumedProductConsumeSuccess(PurchaseData result)
	{
		Logger.N("[Purchase,Unconsume,Success] " + result.ToString());

		_in_transaction = false;

		if (_cbUnconsume != null)
		{
			_unconsume_purchase_data = null;
			_cbUnconsume(result, null);
			_cbUnconsume = null;
		}
	}

	void UnconsumedProductConsumeError(string error)
	{
		Logger.N("[Purchase,Unconsume,Error] " + error);

		_in_transaction = false;

		if (_cbUnconsume != null)
		{
			_cbUnconsume(null, error);
			_cbUnconsume = null;
		}
	}

	public void ReqConsume(Action<PurchaseData, ErrorCode, string> cb, PurchaseData product_data)
	{		
		if (_in_transaction == true)
		{
			if (cb != null) cb(null, ErrorCode.InProgress, "in_progress");
			return;
		}

		_in_transaction = true;

		_cbConsume = cb;		

		Onestore_IapCallbackManager.regConsumeSuccess = regConsumeSuccess;
		Onestore_IapCallbackManager.regConsumeError = regConsumeError;
		Onestore_IapCallManager.consume(JsonUtility.ToJson(product_data));
	}

	void regConsumeSuccess(PurchaseData result)
	{
		Logger.N("[Purchase,consume,Success] " + result.ToString());

		_in_transaction = false;

		if (_cbConsume != null)
		{
			_cbConsume(result, ErrorCode.None, null);
			_cbConsume = null;
		}
	}

	void regConsumeError(string error)
	{
		Logger.N("[Purchase,consume,Error] " + error);

		_in_transaction = false;

		if (_cbConsume != null)
		{
			_cbConsume(null, ErrorCode.FailConsume, error);
			_cbConsume = null;
		}
	}

	void CallBackEventHandler(string error)
	{
		error = error.Trim();
		if (error.CompareTo("onErrorRemoteException") == 0)
		{

		}
		else if (error.CompareTo("onErrorSecurityException") == 0)
		{

		}
		else if (error.CompareTo("onErrorNeedUpdateException") == 0)
		{
		}		
	}

	int ParseErrorCode(string err_msg)
	{
		int error_code = int.MaxValue;
		var splits = err_msg.Split(':');
		if (splits.Length > 1)
		{
			var msg = splits[1].Trim();
			var msg_splits = msg.Split(' ');
			if (msg_splits.Length > 0)
			{
				Logger.E("[Purchase] error code : " + msg_splits[0]);
				if(int.TryParse(msg_splits[0], out error_code) == false)
				{
					error_code = int.MaxValue;
				}				
			}
		}

		return error_code;
	}

	public IEnumerator InGamePurchase(string product_code, Action<bool> cb)
	{
		Logger.N("Start Purchase");
		string err_msg = null;
		var err_code = PurchaseManager.ErrorCode.None;
		OneStore.PurchaseData purchase_data = null;
		bool fin = false;

		var product_detail = PurchaseManager.I.GetProductDetail(product_code);
		if (product_detail != null)
		{
			GlobalUI.ShowSpinner();
			Logger.N("## try to buy item");
			PurchaseManager.I.ReqPurchase(product_detail.productId, product_detail.type, product_detail.ToString(), (data, code, msg) =>
			{
				err_code = code;
				err_msg = msg;
				purchase_data = data;
				fin = true;
			});

			yield return new WaitUntil(() => { return fin; });
			fin = false;
			GlobalUI.HideSpinner();

			if (err_code != PurchaseManager.ErrorCode.None)
			{
				var err_popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
				err_popup.OnEnter();
				err_popup.Set(err_msg, () =>
				{
					fin = true;
				}, true);

				yield return new WaitUntil(() => { return fin; });
				fin = false;

				if (cb != null) cb(false);
				yield break;
			}

			if (purchase_data != null)
			{
				GlobalUI.ShowSpinner();
				Logger.N("## check reciept");
				var check_receipt = false;
				PurchaseManager.I.CheckReceipt(product_detail, purchase_data, (result) =>
				{
					check_receipt = result;
					fin = true;
				});

				yield return new WaitUntil(() => { return fin; });
				fin = false;
				GlobalUI.HideSpinner();

				if (check_receipt == false)
				{
					var err_popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
					err_popup.OnEnter();
					err_popup.Set(StringTBL.GetData(902126), () =>
					{
						fin = true;
					}, true);

					yield return new WaitUntil(() => { return fin; });
					fin = false;
				}

				Logger.N("## consume");
				GlobalUI.ShowSpinner();
				PurchaseManager.I.ReqConsume((data, code, msg) =>
				{					
					err_code = code;
					err_msg = msg;
					fin = true;
				}, purchase_data);
				yield return new WaitUntil(() => { return fin; });
				fin = false;

				GlobalUI.HideSpinner();

				if (err_code != PurchaseManager.ErrorCode.None)
				{
					var err_popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
					err_popup.OnEnter();
					err_popup.Set(err_msg, () =>
					{
						fin = true;
					}, true);

					yield return new WaitUntil(() => { return fin; });
					fin = false;

					if (cb != null) cb(false);
				}
				else
				{
					// catergory는 임의지정
					double price = 0.0f;
					if(double.TryParse(product_detail.price, out price) == true)
					{
						IgaworksUnityPluginAOS.Adbrix.purchase(purchase_data.orderId, product_detail.productId, product_detail.title, price, 1, IgaworksUnityPluginAOS.Adbrix.Currency.KR_KRW, "cash");
					}					
					//NetManager.BuyShop(0, shop_data.ReferenceID);
					if (cb != null) cb(true);
				}				
			}
		}
	}
}