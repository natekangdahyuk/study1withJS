using UnityEngine;
using UnityEngine.UI;
using System;

public class DetailStarPopup : baseUI
{
    [SerializeField]
    Text Title;

    [SerializeField]
    Image[] taskStar;

    [SerializeField]
    Text[] task;

    [SerializeField]
    Text rewardTitle;

    [SerializeField]
    Text reward;

    [SerializeField]
    RawImage rewardImage;


    public override void Init()
    {
        Title.text = StringTBL.GetData(901027);
        rewardTitle.text = StringTBL.GetData(901006);
    }

    public void Exit()
    {
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void Apply(StageReferenceData stageData)
    {
        gameObject.SetActive(true);

        for( int i=0; i < task.Length; i++)
        {
            taskStar[i].gameObject.SetActive( StageManager.I.GetStar( stageData.ReferenceID ,i) );

            if (stageData.TaskType[i] == 1 || stageData.TaskType[ i ] == 6 || stageData.TaskType[ i ] == 7 || stageData.TaskType[i] == 8)
                task[i].text = String.Format(StringTBL.GetData(stageData.TaskInfo[i]), stageData.TaskValue[i].ToString());
            else
                task[i].text = StringTBL.GetData(stageData.TaskInfo[i]);
        }

        reward.text = stageData.StarRewardValue.ToString("n0");

        if(stageData.StarRewardType == 1)
            rewardImage.texture = ResourceManager.LoadTexture( "icon_topbar_gold" );

        else if( stageData.StarRewardType == 3 )
            rewardImage.texture = ResourceManager.LoadTexture( "icon_topbar_power" );

        else if( stageData.StarRewardType == 2 )
            rewardImage.texture = ResourceManager.LoadTexture( "icon_topbar_ruby" );

        else if( stageData.StarRewardType == 4 )
            rewardImage.texture = ResourceManager.LoadTexture( "icon_topbar_summonstone" );



    }
}
