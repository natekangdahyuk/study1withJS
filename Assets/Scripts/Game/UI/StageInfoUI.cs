using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class StageInfoUI : MonoBehaviour
{
	[SerializeField]
	UnityEngine.UI.Text             text_TurnCount;

	[SerializeField]
	UnityEngine.UI.Text             text_StageTimer;

    [SerializeField]
    UnityEngine.UI.Text text_Stage;

    [SerializeField]
    UnityEngine.UI.Text text_StageEx;

    [SerializeField]
    GameObject[] StageName;

    [SerializeField]
    GameObject Stage2048;

    [SerializeField]
    GameObject StageTime;

    public StringBuilder            sb = new StringBuilder();

    void Awake()
    {
        SetText(text_TurnCount, "-");
    }

    public void UpdateTime( StageBase stBase )
    {
        sb.Length = 0;

        if( GameScene.modeType == ModeType.ModeTimeLimit || GameScene.modeType == ModeType.TimeDefence )
            sb.Append( "남은시간 " );
        else
            sb.Append( "진행시간 " );

        if( 0 < stBase.timeInfo.hours )
            sb.Append( string.Format( "{0}:" , stBase.timeInfo.hours ) );

        sb.Append( string.Format( "{0:D2}:" , stBase.timeInfo.minutes ) );
        sb.Append( string.Format( "{0:D2}" , stBase.timeInfo.seconds ) );

        if( GameScene.modeType == ModeType.Time2048 )
        {
            float value = stBase.timeInfo.deltaTime - stBase.timeInfo.currTime;
            sb.Append( string.Format( ".{0:D2}" , (int)(value * 100) ) );
        }            

        SetText( text_TurnCount , sb.ToString() );
    }

    public void Apply(StageBase stBase)
    {
        text_StageEx.gameObject.SetActive( false );
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            StageReferenceData ReferenceData = StageManager.I.GetData();
            text_Stage.text = ReferenceData.ThemaIndex.ToString() + "-" + ReferenceData.SubIndex.ToString();
        }
        else
        {
            text_Stage.gameObject.SetActive( false );
            StageName[ (int)GameScene.modeType-1 ].gameObject.SetActive( true );            
            text_StageEx.gameObject.SetActive( true );
        }
  
        UpdateTime( stBase );
    }
 
   
    private void SetText( UnityEngine.UI.Text uiText, string desc  )
    {
        if( null != uiText )
        {
            uiText.text = desc;
        }
    }
}
