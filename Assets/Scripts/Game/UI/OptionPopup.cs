using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionPopup : baseUI
{
    public Slider BgVolume;
    public Slider EffectVolume;
    public Slider VoiceVolume;

    public Text BgVolumeValue;
    public Text EffectVolumeValue;
    public Text VoiceVolumeValue;

    public Toggle normal;
    public Toggle low;

    public Toggle[] FrameToggle;

    public Text normalCheckText;
    public Text lowCheckText;

    public Text UserInfo;

    SoundObject bgm;


    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;

        GameObject ob = GameObject.Find( "MainBGM" );
        if(ob)
        {
            bgm = ob.GetComponent<SoundObject>();
        }
        
    }
  

    // Update is called once per frame
    void Update()
    {

    }

    public void Apply()
    {
        OnEnter();
        BgVolume.value = GameOption.BGMVolume * 2f;
        EffectVolume.value = GameOption.EffectVoluem * 2f;
        VoiceVolume.value = GameOption.VoiceVoluem * 2f;

        BgVolumeValue.text = ((int)(GameOption.BGMVolume * 2f*100)).ToString();
        EffectVolumeValue.text = ( (int)( GameOption.EffectVoluem * 2f * 100 ) ).ToString();
        VoiceVolumeValue.text = ( (int)( GameOption.VoiceVoluem * 2f * 100 ) ).ToString();

        low.isOn = GameOption.LowMode;
        normal.isOn = !GameOption.LowMode;
        UserInfo.text = PlayerData.I.UserID;

        if( GameOption.Frame == 50 )
        {
            FrameToggle[ 0 ].isOn = true;
        }
        else if( GameOption.Frame == 40 )
        {
            FrameToggle[ 1 ].isOn = true;
        }
        else
            FrameToggle[ 2 ].isOn = true;

        SetToggle();
    }

    public override void Init()
    {
        
    }

    void SetToggle()
    {
        normalCheckText.gameObject.SetActive( normal.isOn );
        lowCheckText.gameObject.SetActive( low.isOn );
    }

    void SetFrameToggle()
    {
        //normalCheckText.gameObject.SetActive( normal.isOn );
        //lowCheckText.gameObject.SetActive( low.isOn );
    }

    public void OnOk()
    {
        GameOption.BGMVolume = BgVolume.value * 0.5f;
        GameOption.EffectVoluem = EffectVolume.value * 0.5f;
        GameOption.VoiceVoluem = VoiceVolume.value * 0.5f;
        GameOption.LowMode = low.isOn;

        if( FrameToggle[0].isOn )
        {
            GameOption.Frame = 50;
        }
        else if( FrameToggle[ 1 ].isOn )
        {
            GameOption.Frame = 40;
        }
        else
            GameOption.Frame = 30;

        GameOption.SaveOption();

        if( bgm )
        {
            bgm.source.volume = GameOption.BGMVolume;
        }
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnCancel()
    {
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnNormal()
    {
        SetToggle();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnLow()
    {
        SetToggle();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnFrameHigh()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnFrameNormal()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnFrameLow()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnBgVolume( float value )
    {
        BgVolumeValue.text = ( (int)( BgVolume.value * 100f ) ).ToString();
    }

    public void OnEffectVolume( float value )
    {
        EffectVolumeValue.text = ( (int)( EffectVolume.value * 100f ) ).ToString();
    }

    public void OnVoiceVolume( float value )
    {
        VoiceVolumeValue.text = ( (int)( VoiceVolume.value * 100f ) ).ToString();
    }
}

