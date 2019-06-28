using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MissionItem : MonoBehaviour
{
    [SerializeField]
    Text Count;

    [SerializeField]
    Text[] defaultText;

    [SerializeField]
    Text MissinDesc;

    [SerializeField]
    Text MissinReward;

    [SerializeField]
    Button CompleteBtn;

    [SerializeField]
    Button ReadyBtn;

    [SerializeField]
    RawImage RewardImage;

    [SerializeField]
    GameObject Bg;

    [SerializeField]
    RawImage BgImage;

    [SerializeField]
    GameObject Bg_Complte;

    [SerializeField]
    CanvasGroup CompleteGroup;

    MissinData missiondata;

    private void Awake()
    {
        defaultText[0].text = StringTBL.GetData( 902093 );
        defaultText[ 1 ].text = StringTBL.GetData( 900012 );
    }

    public void Apply( MissinData data )
    {
        missiondata = data;
        Count.text = "("+data.count.ToString("n0") + "/" + data.data.count.ToString("n0") +")";
        MissinDesc.text = StringTBL.GetData( data.data.name );
        MissinReward.text = data.data.RewardValue.ToString( "n0" );
        BgImage.texture = ResourceManager.LoadTexture(data.data.list_Img);
        UIUtil.LoadCurrencyType( RewardImage , data.data.Rewardtype );

        
        SetBtnState();
    }

    void SetBtnState()
    {
        Bg.SetActive( true );
        Bg_Complte.SetActive( false );
        CompleteGroup.alpha = 1f;

        if( missiondata.state == MissionState.Ready )
        {
            ReadyBtn.gameObject.SetActive( true );
            CompleteBtn.gameObject.SetActive( false );
            Count.color = Color.red;
        }
        else if( missiondata.state == MissionState.Reward )
        {
            ReadyBtn.gameObject.SetActive( false );
            CompleteBtn.gameObject.SetActive( true );
            Count.color = Color.white;
        }
        else
        {
            ReadyBtn.gameObject.SetActive( false );
            CompleteBtn.gameObject.SetActive( true );
            CompleteBtn.interactable = false;
            Count.color = Color.gray;

            Bg.SetActive( false );
            Bg_Complte.SetActive( true );
            CompleteGroup.alpha = 0.5f;
        }
    }

    public void Refresh()
    {
     
    }
    

    public void OnComplete()
    {
        NetManager.SetDailyMissionreward( (int)missiondata.data.type );
        missiondata.state = MissionState.Clear;
        SetBtnState();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnRecvComplete()
    {
       

       
    }
}
