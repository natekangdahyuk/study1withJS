using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OneStore;

public class IntroScene : MonoBehaviour
{
    [SerializeField]
    Slider _progress_bar;
    [SerializeField]
    Text _progress_text;

    [SerializeField]
    Text version;

    public GameObject _googleLoginButton;
    bool _run_login_process = false;

    void Awake()
    {
        _progress_bar.gameObject.SetActive(false);
        _progress_text.gameObject.SetActive(false);
        if (_googleLoginButton != null)
            _googleLoginButton.SetActive(false);

    }

    private void Start()
    {
        SceneManager.Create();
        StartCoroutine("coPatch");

        version.text = "ver " + VersionController._main.ToString() + "." + VersionController._sub.ToString() + "." + VersionController._patch.ToString();
    }

    IEnumerator coPatch()
    {
        yield return new WaitForSeconds(4f);

        if (StringTBL.IsEmpty == true)
        {
            // string table 패치 이전에 기본적으로 필요한 요소들을 표시하기 위한
            // 기본 테이블 로드
            StringTBL.Load();
        }


#if USE_PATCH
		_progress_bar.value = 0;
		_progress_text.text = StringTBL.GetData(103);
		_progress_bar.gameObject.SetActive(true);
		_progress_text.gameObject.SetActive(true);

		var next = true;		
		while(next)
		{
			GlobalUI.ShowSpinner();
			
			var ver_complete = false;
			yield return VersionController.coVersionDataConnect((error) =>
			{
				GlobalUI.HideSpinner();

				if (string.IsNullOrEmpty(error) == false)
				{
					var popup = GlobalUI.GetUI<PopupOkCancel>(UI_TYPE.PopupOkCancel);
					if (popup)
					{
						popup.OnEnter();
						popup.Set(StringTBL.GetData(105),
						() => { ver_complete = true; },
						() => { Application.Quit(); },
						true);
					}
				}
				else if (VersionController.ForceUpdate == true)
				{
					var popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
					if (popup)
					{
						popup.OnEnter();						
						popup.Set(StringTBL.GetData(106),
						() =>
						{
							Application.OpenURL(VersionController.AppLink);
						},
						false);
					}
				}
				else
				{
					next = false;
					ver_complete = true;
				}
			});

			yield return new WaitUntil(() => { return ver_complete; });
		}		

		var obj = new GameObject();
		var pi = obj.AddComponent<BundlePatcher>();

		while(pi._CurPatchState == BundlePatcher.PatchState.NONE)
		{
			yield return null;
		}
		
		var loading_str_format = StringTBL.GetData(101);
		var download_str_format = StringTBL.GetData(102);
		while(pi.PatchComplete == false)
		{			
			if(pi._CurPatchState == BundlePatcher.PatchState.BUNDLE_DOWNLOAD)
			{
				_progress_bar.value = pi.PatchDownloadProgress;
				_progress_text.text = string.Format(download_str_format, pi.PatchDownloadProgress * 100.0f);
			}
			else if (pi._CurPatchState == BundlePatcher.PatchState.BUNDLE_REGISTER)
			{
				_progress_bar.value = pi.PatchLoadProgress;
				_progress_text.text = string.Format(loading_str_format, pi.PatchLoadProgress * 100.0f);
			}
			
			yield return null;
		}

		_progress_bar.gameObject.SetActive(false);
		_progress_text.gameObject.SetActive(false);

		pi.Clear();
		GameObject.DestroyImmediate(pi);
#endif

        SceneManager.Instance.Init();
        // table preload
        SpritePackerLoader.I.Preload();

        yield return null;

        // 일단 구글 로그인 테스트 위해 주석처리
        //StartCoroutine("coLogin");					

        LoginReadyProcess();

#if USE_SNS_LOGIN
		StartCoroutine(coSNSLoginProcess());
#else
        StartCoroutine(coDevLoginProcess());
#endif
    }

    public void OnGoogleLogin()
    {
        _run_login_process = true;
    }

    void LoginReadyProcess()
    {
#if USE_SNS_LOGIN
		_run_login_process = false;

		if (FirebaseManager.I.IsGoogleTokenExist == false)
		{
			if (_googleLoginButton != null)
				_googleLoginButton.SetActive(true);
		}
		else
		{
			_run_login_process = true;
		}
#else
        _run_login_process = true;
#endif
    }

    IEnumerator coDevLoginProcess()
    {
#if !USE_SNS_LOGIN
        bool next = false;
        var popup = GlobalUI.GetUI<DevLoginPopup>(UI_TYPE.DevLoginUI);
        popup.OnEnter();
        popup.Set(() =>
        {
            next = true;
        });
        yield return new WaitUntil(() => { return next; });
#endif
        SceneManager.Instance.ChangeScene("StartScene");

        yield return null;
    }

#if USE_SNS_LOGIN
	IEnumerator coSNSLoginProcess()
	{
		Logger.N("## sns Login processing...");
		yield return new WaitUntil(() =>
	    {
			return _run_login_process;
	    });		

		yield return StartCoroutine(coGoogleLogin());

		while (FirebaseManager.I.IsFailedInit == true || FirebaseManager.I.IsFailedLoginProcess == true)
		{
			if (_googleLoginButton != null)
				_googleLoginButton.SetActive(true);

			yield return StartCoroutine(coGoogleLogin());
		}
	}

	IEnumerator coGoogleLogin()
	{
		Logger.N("## google Login processing...");

		var firebase = FirebaseManager.I;
		if (firebase == null) yield break;

		yield return new WaitUntil(() =>
		{
			return firebase.IsInitalized;
		});

		if(firebase.IsFailedInit == true)
		{
			NetManager.SendLog(firebase.InitFailedErrorMsg);

			var wait = false;
			// Firebase Unity SDK is not safe to use here.
			var popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
			popup.OnEnter();
			popup.Set(firebase.InitFailedErrorMsg, () =>
			{
				wait = true;
			}, true);

			yield return new WaitUntil(() => { return wait; });
			// 다시 로그인을 할 수 있는 셋팅으로 돌릴지 어쩔지를 처리할것
			yield break;
		}

		firebase.ProcessGoogleLogin();

		yield return new WaitUntil(() =>
		{
			return firebase.InProgress == false;
		});

		Logger.N("## firebase process fin");

		if (firebase.IsFailedLoginProcess == false)
		{
			//Onestore_IapCallbackManager.Create();			

			// 나중에 app store서비스 연결이 올바르지 않을때 에러 처리를 하던가
			// 게임을 그냥 진행 할 수 있는 형태로 패스할 수 있도록 기능을 수정할것
			PurchaseManager.I.Init();
			yield return new WaitUntil(() => { return PurchaseManager.I.IsServiceAvailable; });
			
			//if(PurchaseManager.I.IsBillingSupport == true)
			//{				
			//	string error_result = null;
			//	bool history_fin = false;
			//	PurchaseManager.I.ReqPurchaseHistory((data, error) =>
			//   {
			//	   history_fin = true;				   
			//	   error_result = error;
			//   });

			//	yield return new WaitUntil(() => { return history_fin; });
				
			//	if(PurchaseManager.I.UncosumedProductsRemain == true)
			//	{
			//		PurchaseData purchase_data = null;
			//		bool unconsume_fin = false;
			//		PurchaseManager.I.ReqConsumeForUnconsumedProducts((data, error) =>
			//	   {
			//		   purchase_data = null;
			//		   error_result = error;
			//		   unconsume_fin = true;
			//	   });

			//		yield return new WaitUntil(() => { return unconsume_fin; });
			//	}

			//	PurchaseManager.I.RequestProducts();
			//	yield return new WaitUntil(() => { return PurchaseManager.I.ProductRequestFin; });

			//	if(PurchaseManager.I.IsDataVailded == false)
			//	{
			//		// 상품 정보가 올바르게 연계 되지 않아서 생기는 팝업 띄우기				
			//	}
			//}

			SceneManager.Instance.ChangeScene("StartScene");
		}
		else
		{
			NetManager.SendLog(firebase.ProcessFailedErrorMsg);

            PlayerPrefs.SetString("google_id", "");
			PlayerPrefs.SetString("google_token", "");
			PlayerPrefs.Save();
			var wait = false;
			// 로그인 프로세스를 실패했을때 어떻게 처리를 할것인지 확인할것 
			var popup = GlobalUI.GetUI<PopupOk>(UI_TYPE.PopupOk);
			popup.OnEnter();
			popup.Set(firebase.ProcessFailedErrorMsg, () =>
			{
                GlobalUI.I.GameEnd();
				wait = true;
			}, true);

			yield return new WaitUntil(() => { return wait; });
		}
	}
#endif
}
