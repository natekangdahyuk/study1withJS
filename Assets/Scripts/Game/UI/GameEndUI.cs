using UnityEngine;
using UnityEngine.UI;
using System;

public partial class GameEndUI : baseUI
{
    [SerializeField]
    GameObject DefaultGroup;

    [SerializeField]
    Text ClearTitle;

    [SerializeField]
    Text taskTitle;

    [SerializeField]
    Image[] taskStar;

    [SerializeField]
    Text[] task;

    
    [SerializeField]
    Text RewardTitle;

    [SerializeField]
    Text GoldTitle;

    [SerializeField]
    Text Gold;

    [SerializeField]
    Text ExpTitle;

    [SerializeField]
    Text Exp;

    [SerializeField]
    Text LvTitle;

    [SerializeField]
    Text Lv;

    [SerializeField]
    Slider SliderLv;

    [SerializeField]
    Text LvPer;

    [SerializeField]
    Button NextStage;

    [SerializeField]
    GameObject ClearGroup;

    [SerializeField]
    GameObject cardPosition;

    [SerializeField]
    GameObject FailedGroup;
    StageReferenceData ReferenceData = null;

    [SerializeField]
    GameObject tip1;

    [SerializeField]
    GameObject tip2;


    bool bContinue = false;
    float currentTime = 0;
    Card card;

    private void Awake()
    {
        gameObject.SetActive(false);
        
        taskTitle.text = String.Format(StringTBL.GetData(850008));
        RewardTitle.text = String.Format(StringTBL.GetData(901006));
        GoldTitle.text = String.Format(StringTBL.GetData(901007));
        ExpTitle.text = String.Format(StringTBL.GetData(901009));
        LvTitle.text = String.Format("Lv.");

        card = ResourceManager.Load<Card>( this.gameObject , "Card_ingame" );        
        card.transform.SetParent( cardPosition.transform );
        card.transform.localPosition = Vector3.zero;
        card.transform.localScale = new Vector3( 1f , 1f , 0f );

    }

    public override void Init()
    {
        
    }

    public void Apply()
    {
        if( GameScene.modeType != ModeType.ModeDefault )
        {
            DefaultGroup.SetActive( false );
            return;
        }

        StageReferenceData ReferenceData = StageManager.I.GetData();
        NextStage.enabled = StageManager.I.IsNextStage();
        Gold.text = ReferenceData.GoldReward.ToString( "n0" );
        Exp.text = ReferenceData.UserExpReward.ToString( "n0" );
        //
        //ClearTitle.text = String.Format( StringTBL.GetData( 850007 ) , ReferenceData.ThemaIndex.ToString() , ReferenceData.SubIndex.ToString() );
        ClearTitle.text =  ReferenceData.ThemaIndex.ToString() + "-" + ReferenceData.SubIndex.ToString();
        int CurrentExp = PlayerData.I.Exp ;

        ExpUserReferenceData Data = ExpUserTBL.GetData( PlayerData.I.UserLevel ); //현재 레벨 데이터

        ExpUserReferenceData OldData = ExpUserTBL.GetData( Data.ReferenceID - 1 );


        int OldExp = 0;

        if( OldData != null )
            OldExp = OldData.exp;

        float per = ( ( (float)(CurrentExp - OldExp ) / ( Data.exp - OldExp) ) );

        if( per < 0 )
            per = 0;

        Lv.text = PlayerData.I.UserLevel.ToString();
        LvPer.text = ( (int)( per * 100 ) ).ToString() + "%";

        SliderLv.value = per;
                
        for( int i = 0 ; i < task.Length ; i++ )
        {
            taskStar[ i ].gameObject.SetActive( StageManager.I.GetStar( ReferenceData.ReferenceID, i ) );
            if( ReferenceData.TaskType[ i ] == 1 || ReferenceData.TaskType[ i ] == 6 || ReferenceData.TaskType[ i ] == 7 || ReferenceData.TaskType[ i ] == 8 )
                task[ i ].text = String.Format( StringTBL.GetData( ReferenceData.TaskInfo[ i ] ) , ReferenceData.TaskValue[ i ].ToString() );
            else
                task[ i ].text = StringTBL.GetData( ReferenceData.TaskInfo[ i ] );
        }
    }

   

    int nCard = 0;
    int nUidx =0;
    public void GameEnd( bool bClear , int ocard , int oUidx , bool isFull)
    {
        nCard = ocard;
        nUidx = oUidx;
        DefaultGroup.SetActive( true );
        ClearGroup.SetActive( false );
        FailedGroup.SetActive( false );
        Apply();

        if(nCard > 0 )
        {
            Reward();
        }

        if( bClear )
            ClearGroup.SetActive( true );
        else
        {
            FailedGroup.SetActive( true );

            if( isFull )
            {
                tip1.SetActive( false );
                tip2.SetActive( true );
            }
            else
            {
                tip1.SetActive( true );
                tip2.SetActive( false );
            }
        }
        ShowUI();

        if (StageManager.I.GetCurrentStar() >= 3 && GameOption.bContinuePlay && GameScene.modeType == ModeType.ModeDefault)
        {
            bContinue = true;
            currentTime = Time.realtimeSinceStartup;
        }
    }

    void Update()
    {
        if (bContinue == false)
            return;
        
        if(currentTime +6f <= Time.realtimeSinceStartup)
        {
            bContinue = false;

            if (PlayerData.I.shoes < StageManager.I.ReferenceData.ApCost)
            {
                GlobalUI.ShowOKPupUp(StringTBL.GetData(850020));                
            }
            else
            {
                ReStartOk();
            }
        }
    }
  

    public void ReStart()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            if( PlayerData.I.shoes < StageManager.I.ReferenceData.ApCost )
            {
                GlobalUI.ShowOKPupUp( StringTBL.GetData( 850020 ) );
                return;
            }

            PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
            popup.OnEnter();
            popup.SetEx( StringTBL.GetData( 850019 ) , StageManager.I.ReferenceData.ApCost.ToString() , ReStartOk , null , false );
        }
        else
        {
            if( PlayerData.I.shoes < RankingStageManager.I.CurrentData.ApCost )
            {
                GlobalUI.ShowOKPupUp( StringTBL.GetData( 850020 ) );
                return;
            }
            PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
            popup.OnEnter();
            popup.SetEx( StringTBL.GetData( 850019 ) , RankingStageManager.I.CurrentData.ApCost.ToString() , ReStartOk , null , false );
        }

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        bContinue = false;
    }

    public void ReStartOk()
    {

        if( GameScene.modeType == ModeType.ModeDefault )
        {
            if( InventoryManager.I.IsMaxCount() )
            {
                GlobalUI.ShowOKPupUp( StringTBL.GetData(902181) );
                return;
            }
            NetManager.SetShoes( 1 , StageManager.I.ReferenceData.ApCost );
        }
        else
        {
            NetManager.SetShoes( 1 , RankingStageManager.I.CurrentData.ApCost );
        }

        StartGame();
    }

    public void StartGame()
    {
        GameScene.I.RestartGame();
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive( true );
    }

    public void GameExit()
    {
        bContinue = false;
        GameScene.I.GameExit();
    }

    public void OnMain()
    {
        MainScene.Starttype = MainScene.StartType.None;
        GameExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnStage()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            MainScene.Starttype = MainScene.StartType.Dungeon;
        }
        else
            MainScene.Starttype = MainScene.StartType.Field;

        GameExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnStageReady()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            MainScene.Starttype = MainScene.StartType.DungeonReady;
        }
        else
            MainScene.Starttype = MainScene.StartType.FieldReady;

        GameExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

    }

    public void OnNextStage()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            MainScene.Starttype = MainScene.StartType.DungeonReady;
            StageManager.I.NextStage();
        }
        else
            MainScene.Starttype = MainScene.StartType.FieldReady;

        GameExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    void Reward()
    {
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            CardReferenceData data = CardTBL.GetData( nCard );

            CharacterReferenceData characterData = CharacterTBL.GetData( data.characterIndex );
            CardData cardData = InventoryManager.I.NewCard( nUidx , nCard , 0 , 1 , characterData.star , characterData.DefaultSkin , -1 , false , false , false , 0 , true);
            
            card.ApplyData( cardData );
            ShowUI();
            //CardRewardPopup summon = GlobalUI.GetUI<CardRewardPopup>( UI_TYPE.CardRewardPopup );
            //summon.Apply( card );                
            //summon.EndCall = ShowUI;

        }        
    }      
}