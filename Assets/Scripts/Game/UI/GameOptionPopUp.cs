using System;
using UnityEngine;
using System.Linq;
using System.Text;

public class GameOptionPopUp : baseUI
{    
    void Awake()
    {
        //gameObject.SetActive(false);
    }


    public override void Init()
    {
        Time.timeScale = 0;
    }

    public override bool OnExit()
    {        
        Time.timeScale = 1;
        return base.OnExit();
    }

    public void OnGameExit()
    {
        
        GameScene.I.GameExit();
        OnExit();
    }

    public void OnRestart()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        SceneManager.Instance.ChangeScene( "GameScene" );
        //GameScene.I.RestartGame();
        OnExit();
    }

    public void OnPlay()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        OnExit();
    }

    public void OnMain()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        MainScene.Starttype = MainScene.StartType.None;
        GameScene.I.GameExit();
        OnExit();
    }

    public void OnStage()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            MainScene.Starttype = MainScene.StartType.Dungeon;
        }
        else
            MainScene.Starttype = MainScene.StartType.FieldReady;

        GameScene.I.GameExit();
        OnExit();
    }

    public void OnStageReady()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            MainScene.Starttype = MainScene.StartType.DungeonReady;
        }
        else
            MainScene.Starttype = MainScene.StartType.FieldReady;
        GameScene.I.GameExit();
        OnExit();
    }

}
