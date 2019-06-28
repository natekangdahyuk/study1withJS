using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    public Action EndAnim;

    [SerializeField]
    Animation black;

    [SerializeField]
    Animation StageTitle;

    [SerializeField]
    Animation StageTitleEx;


    [SerializeField]
    Animation Stage;

    [SerializeField]
    GameObject[] ShowList;

    [SerializeField]
    TweenAlpha bossAlpha;

    [SerializeField]
    TweenPosition bossTweenPos;

    [SerializeField]
    Text StageTitleText;

    [SerializeField]
    GameObject[] modeName;

    private void Start()
    {
      
    }

    public void PlayStart()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            StageReferenceData ReferenceData = StageManager.I.GetData();
            StageTitleText.text = ReferenceData.ThemaIndex.ToString() + "-" + ReferenceData.SubIndex.ToString();
        }
        else if( GameScene.modeType == ModeType.Mode2048 )
        {
            StageTitleText.text = "2048";
            modeName[ (int)GameScene.modeType - 1 ].gameObject.SetActive( true );
            
        }
        else if( GameScene.modeType == ModeType.Time2048 )
        {
            StageTitleText.text = "2048 타임";
            modeName[ (int)GameScene.modeType - 1 ].gameObject.SetActive( true );
            
        }
        else if( GameScene.modeType == ModeType.TimeDefence )
        {
            StageTitleText.text = "타임 디펜스";
            modeName[ (int)GameScene.modeType - 1 ].gameObject.SetActive( true );
            
        }
        else
        {
            StageTitleText.text = "타임 리미트";
            modeName[ (int)GameScene.modeType - 1 ].gameObject.SetActive( true );
            
        }



        black.gameObject.SetActive( true );
        black.Play();
        Invoke( "PlayStage" , 0f );
    }

    void PlayStage()
    {
        Stage.gameObject.SetActive( true );        
        Invoke( "PlayStageTitle" , 1f );
    }

    void PlayStageTitle()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            StageTitle.gameObject.SetActive( true );
            StageTitle.Play();
        }
        else
        {
            StageTitleEx.gameObject.SetActive( true );
            StageTitleEx.Play();            
        }
        Invoke( "PlayEnd" , 2f );

        EndAnim();
        for( int i = 0 ; i < ShowList.Length ; i++ )
            ShowList[ i ].SetActive( true );

        bossAlpha.gameObject.SetActive( true );
        bossAlpha.Play();
        bossTweenPos.Play();
    }
    public void PlayEnd()
    {
        black.gameObject.SetActive( false );
        

    }
}