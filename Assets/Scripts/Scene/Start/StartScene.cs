using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
	[SerializeField]
	private Animation m_Btn_Start_Ani;

    [SerializeField]
    GameObject TouchScreen;

    [SerializeField]
    UserTermsPopup UserTerms;

    [SerializeField]
	Slider _progress_bar;
	[SerializeField]
	Text _progress_text;

    [SerializeField]
    Text version;

    public SoundObject sound;    
    bool bStart = false;

    void Awake()
    {
        TouchScreen.SetActive(false);		

		_progress_bar.gameObject.SetActive(false);
		_progress_text.gameObject.SetActive(false);

        GameOption.LoadOption();
    }

	private void Start()
	{
        version.text = "ver " + VersionController._main.ToString() + "." + VersionController._sub.ToString() + "." + VersionController._patch.ToString();
        StartCoroutine("coStart");
        
    }

	public void OnClickStart()
	{
        if( bStart )
            return;

        bStart = true;
        StartCoroutine( GameStart() );
	}

    public void ReceiveLogin()
    {        
        TouchScreen.SetActive(true);        
    }

	IEnumerator coStart()
	{
		if(SceneManager.I == null)
			SceneManager.Create();

		bool wait = false;
        NetManager.GetSeverTime( () =>
		{
			wait = true;
		});

		yield return new WaitUntil(() => { return wait; });

		// table preload
		if(SpritePackerLoader.I.IsPreloaded == false)
		{
			SpritePackerLoader.I.Preload();
		}

#if USE_SNS_LOGIN
		NetManager.Login(FirebaseManager.I.GoogleSigninID, FirebaseManager.I.Email);
#else
		var private_id = PlayerPrefs.GetString("id", SystemInfo.deviceUniqueIdentifier);
		NetManager.DevLogin(private_id);
#endif
		yield return null;
	}
     

    public void ReceiveCreateAccountSuccess()
    {
        TouchScreen.SetActive(true);
    }

    private IEnumerator GameStart()
	{		
        m_Btn_Start_Ani.Play( "ani_gamestart_2" );
        sound.Play();        
        yield return new WaitForSeconds( m_Btn_Start_Ani.GetClip( "ani_gamestart_2" ).length );

        SceneManager.Instance.ChangeScene("MainScene");        
	}
}
