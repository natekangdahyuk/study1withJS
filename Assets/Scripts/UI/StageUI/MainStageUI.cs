using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
public class MainStageUI : baseUI
{
    Button BackStageBtn;
    Button NextStageBtn;
    StageSelectBtn[] StageBtn;

    //[SerializeField]
    //Button rewardBtn;

    [SerializeField]
    Text stageTitle;

    [SerializeField]
    Text stageMainTitle;

    [SerializeField]
    GameObject[] star;

    [SerializeField]
    Slider sliderStar;

    [SerializeField]
    Text StarValue;

    [SerializeField]
    GameObject Reward;

    [SerializeField]
    RawImage Bg;

    [SerializeField]
    Toggle[] DifficultyToggle;

    [SerializeField]
    GameObject[] DifficultyText;

    [SerializeField]
    Button[] rewardBtn;

    int CurrentSelectThemaIndex = 0;
    int CurrentDifficulty = 1;
    int CurrentSelectStageIndex = 0;    
    int rewardValue = 0;
 
    MainStageReadyUI StageReadyUI;
    void Awake()
    {
        StageReadyUI = MainStageReadyUI.Load<MainStageReadyUI>("SubCanvas", "MainStageReadyUI");
        StageReadyUI.OnExit();
        StageBtn = transform.GetComponentsInChildren<StageSelectBtn>();

        
        BackStageBtn = transform.Find("StageBack").GetComponent<Button>();
        NextStageBtn = transform.Find("StageNext").GetComponent<Button>();
    }

    public override void Init()
    {

    }

    public void Apply( int SelectStage )
    {
        OnEnter();

        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI(UI_TYPE.TopBarUI);
        topbar.ChangeScene(this);

        StageReferenceData stageData = StageTBL.GetData(SelectStage);

        if(stageData == null )
        {
            return;
        }

        CurrentSelectStageIndex = SelectStage;       
        
        ApplyClearStage(stageData.ThemaIndex , stageData.Difficulty);
        SelectThema(stageData.ThemaIndex , stageData.Difficulty);
        SetCurrentSlectSubStage(stageData.ThemaIndex, CurrentSelectStageIndex , stageData.Difficulty);
        CurrentDifficulty = stageData.Difficulty;
        if (MainScene.Starttype == MainScene.StartType.DungeonReady)
        {
            StageStart(StageManager.I.SelectStageIndex);
        }

        DifficultyToggle[ stageData.Difficulty - 1 ].isOn = true;
        
        for( int i =0 ; i < DifficultyText.Length ; i++ )
        {            
            DifficultyText[ i ].SetActive( DifficultyToggle[ i ].isOn == true );
        }
    }

    void SelectThema( int themaIndex , int diff )
    {
        ThemaReferenceData themaData = ThemaTBL.GetDataByDifficulty(themaIndex, diff);
        Bg.texture = ResourceManager.LoadTexture(themaData.LobbyTexture);
        stageTitle.text = StringTBL.GetData(themaData.ThemaName);
        stageMainTitle.text = themaIndex.ToString() + "구역";

        CurrentSelectThemaIndex = themaIndex;
        CurrentDifficulty = diff;


        themaData = ThemaTBL.GetDataByDifficulty( themaIndex - 1 , diff );
        BackStageBtn.interactable = themaData == null ? false : true;
        

        themaData = ThemaTBL.GetDataByDifficulty( themaIndex+1 , diff );
        NextStageBtn.interactable = themaData == null ? false : true;

    }

    void ApplyClearStage( int themaIndex , int diff )
    {
        List<StageReferenceData> stagelist = StageTBL.GetSubStageList(themaIndex, diff);

        int nextStage = StageTBL.GetNextStageIndex( StageManager.I.reachStageIndex[ diff-1 ], diff );

        
        for (int i = 0; i < StageBtn.Length; i++)
        {            
            if(stagelist[i].ReferenceID > nextStage )
            {
                if ( StageManager.I.reachStageIndex[ diff - 1 ] == stagelist[i].ReferenceID )
                {
                    StageBtn[i].Apply(stagelist[i], false, 0, diff );
                }
                else
                    StageBtn[i].Apply(stagelist[i], true, 0, diff );
            }
            else
            {                
                StageBtn[i].Apply(stagelist[i], false, StageManager.I.GetStar(stagelist[i].ReferenceID), diff );
            }            
        }
    }

    void SetCurrentSlectSubStage( int themaIndex , int SelectStageIndex , int diff )
    {
        List<StageReferenceData> stagelist = StageTBL.GetSubStageList(themaIndex, diff);

        for (int i = 0; i < StageBtn.Length; i++)
        {
            if (stagelist[i].ReferenceID == SelectStageIndex)
                StageBtn[i].Select(true);
            else
                StageBtn[i].Select(false);
        }

        float starper = (float)StageManager.I.GetThemaStar( themaIndex , diff );
        StarValue.text = ((int)( ( starper * 100 ) / 30 )).ToString() + "%";
        sliderStar.value = starper / 30;

        star[ 0 ].SetActive( starper >= 10 ? true : false );
        star[ 1 ].SetActive( starper >= 20 ? true : false );
        star[ 2 ].SetActive( starper >= 30 ? true : false );

        rewardValue = (int)starper / 10;

        ThemaReferenceData themaData = ThemaTBL.GetDataByDifficulty( CurrentSelectThemaIndex , CurrentDifficulty );

        int value = StageManager.I.GetThemaReward( themaData.ReferenceID );

        if( rewardValue >= 3 )
        {
            rewardValue = 3;

            if( value == rewardValue)
                SetReward( 2 );
            else
                SetReward( 1 );
        }
        else if( value >= rewardValue || rewardValue <= 0 )
        {
            SetReward( 0 );
        }
        else
            SetReward( 1 );

    }

    void SetReward( int index )
    {
        for( int i =0 ; i < rewardBtn.Length ; i++ )
        {
            rewardBtn[ i ].gameObject.SetActive( index == i );
        }
    }

    public void SelectSubStage( int index )
    {
        ThemaReferenceData themaData = ThemaTBL.GetDataByDifficulty(CurrentSelectThemaIndex , CurrentDifficulty);

        List<StageReferenceData> stagelist = StageTBL.GetSubStageList(themaData.ThemaNo, CurrentDifficulty);

        // if (stagelist[index - 1].ReferenceID <= StageManager.I.NextStageIndex)
        if(StageBtn[index - 1].IsLock() == false )
        {
            for (int i = 0; i < StageBtn.Length; i++)
            {
                CurrentSelectStageIndex = stagelist[index - 1].ReferenceID;
                StageBtn[i].Select(false);
            }

            StageBtn[index - 1].Select(true);
            StageStart(stagelist[index - 1].ReferenceID);
        }
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_battle_stage" , GameOption.EffectVoluem );
    }
    

    public void StageStart( int StageIndex )
    {
        StageManager.I.ApplyStage(StageIndex);
        GlobalUI.I.AddSubSceneBack(this);

        ThemaReferenceData themaData = ThemaTBL.GetDataByDifficulty( CurrentSelectThemaIndex , CurrentDifficulty );

        StageReadyUI.Apply(StageManager.I.GetData(), themaData );
        OnExit();
    }

    public void OnBackStage()
    {
        CurrentSelectThemaIndex--;
        List<StageReferenceData>  datalist = StageTBL.GetSubStageList(CurrentSelectThemaIndex, CurrentDifficulty);

        if (datalist == null)
        {
            CurrentSelectThemaIndex++;
            return;
        }

        SelectThema(CurrentSelectThemaIndex , CurrentDifficulty);
        ApplyClearStage(CurrentSelectThemaIndex, CurrentDifficulty);
        SetCurrentSlectSubStage(CurrentSelectThemaIndex, CurrentSelectStageIndex, CurrentDifficulty);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_page" , GameOption.EffectVoluem );
    }

    public void OnNextStage()
    {
        CurrentSelectThemaIndex++;
        List<StageReferenceData> stagelist = StageTBL.GetSubStageList(CurrentSelectThemaIndex, CurrentDifficulty);

        if (stagelist == null)
        {
            CurrentSelectThemaIndex--;
            return;
        }

        SelectThema(CurrentSelectThemaIndex, CurrentDifficulty);
        ApplyClearStage(CurrentSelectThemaIndex, CurrentDifficulty);
        SetCurrentSlectSubStage(CurrentSelectThemaIndex, CurrentSelectStageIndex, CurrentDifficulty);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_page" , GameOption.EffectVoluem );
    }


    public void OnNormal(bool bchange)
    {
        SelectThema( CurrentSelectThemaIndex , 1 );
        ApplyClearStage( CurrentSelectThemaIndex , 1 );
        SetCurrentSlectSubStage( CurrentSelectThemaIndex , CurrentSelectStageIndex , 1 );
        DifficultyText[ 0 ].SetActive( true );

    }


    public void OnHard( bool bchange )
    {
        if( StageManager.I.reachStageIndex[0] >= DefaultDataTBL.GetData(DefaultData.field_stage_hard_open))
        {
            SelectThema(CurrentSelectThemaIndex, 2);
            ApplyClearStage(CurrentSelectThemaIndex, 2);
            SetCurrentSlectSubStage(CurrentSelectThemaIndex, CurrentSelectStageIndex, 2);
            DifficultyText[ 1 ].SetActive( true );
        }
        else
        {
            StageReferenceData data = StageTBL.GetData(DefaultDataTBL.GetData(DefaultData.field_stage_hard_open));
                       
            string str = "Normal " + data.ThemaIndex.ToString() + " - " + data.SubIndex.ToString() + "<color=#BFC0BFFF> 클리어시</color> Hard <color=#BFC0BFFF>난이도가 오픈 됩니다.</color>";

            GlobalUI.ShowOKPupUp(str);

            DifficultyToggle[ CurrentDifficulty - 1 ].isOn = true;
            
        }
        
    }

    public void OnHell( bool bchange )
    {
        if (StageManager.I.reachStageIndex[ 1 ] >= DefaultDataTBL.GetData(DefaultData.field_stage_hell_open))
        {
            SelectThema(CurrentSelectThemaIndex, 3);
            ApplyClearStage(CurrentSelectThemaIndex, 3);
            SetCurrentSlectSubStage(CurrentSelectThemaIndex, CurrentSelectStageIndex, 3);
            DifficultyText[ 2 ].SetActive( true );
        }
        else
        {
            StageReferenceData data = StageTBL.GetData(DefaultDataTBL.GetData(DefaultData.field_stage_hard_open));

            string str = "Hard " + data.ThemaIndex.ToString() + " - " + data.SubIndex.ToString() + " <color=#BFC0BFFF>클리어시</color> Hell <color=#BFC0BFFF>난이도가 오픈 됩니다.</color>";

            GlobalUI.ShowOKPupUp(str);

            DifficultyToggle[ CurrentDifficulty - 1 ].isOn = true;
        }
        
    }

    public static string currentReward ="";
    public void OnReward()
    {
        ThemaReferenceData themaData = ThemaTBL.GetDataByDifficulty( CurrentSelectThemaIndex , CurrentDifficulty );

        int value = StageManager.I.GetThemaReward( themaData.ReferenceID );

        if( value >= rewardValue )
            return;

       
        if (value == 0)
        {
            currentReward = themaData.ThemaNo.ToString() + "구역 33% <color=#BFC0BFFF>달성</color> 1차 보상<color=#BFC0BFFF>을 수령하였습니다.</color>";
        }
        else if (value == 1)
        {
            currentReward = themaData.ThemaNo.ToString() + "구역 66% <color=#BFC0BFFF>달성</color> 2차 보상<color=#BFC0BFFF>을 수령하였습니다.</color>";
        }
        else if (value == 2)
        {
            currentReward = themaData.ThemaNo.ToString() + "구역 100% <color=#BFC0BFFF>달성</color> 3차 보상<color=#BFC0BFFF>을 수령하였습니다.</color>";
        }

        value++;
        NetManager.SetThemaReward( themaData.ReferenceID , value );        
        StageManager.I.AddThemaReward( themaData.ReferenceID , value );


        if(value >= 3)
            SetReward( 2 );
        else if( value >= rewardValue )
            SetReward( 0 );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

    }

    public void OnRewardNext()
    {
        ThemaReferenceData themaData = ThemaTBL.GetDataByDifficulty( CurrentSelectThemaIndex , CurrentDifficulty );

        int value = StageManager.I.GetThemaReward( themaData.ReferenceID );

        PopupOk popup = GlobalUI.GetUI<PopupOk>( UI_TYPE.PopupOk );
        popup.OnEnter();

        string str= "";

        if(value == 0 )
        {
            str = "33% <color=#BFC0BFFF>달성하면</color> 1차 보상<color=#BFC0BFFF>을 얻을 수 있습니다.</color>";
        }
        else if( value == 1 )
        {
            str = "66% <color=#BFC0BFFF>달성하면</color> 2차 보상<color=#BFC0BFFF>을 얻을 수 있습니다.</color>";            
        }
        else if( value == 2 )
        {
            str = "100% <color=#BFC0BFFF>달성하면</color> 3차 보상<color=#BFC0BFFF>을 얻을 수 있습니다.</color>";            
        }


        popup.SetEx( str , themaData.ThemaReward[ value ].ToString() ,null,false, PopupOk.SubType.ruby);
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnRewardComplete()
    {
        GlobalUI.ShowOKPupUp( "모든 보상을 습득하였습니다." );
    }

    
}