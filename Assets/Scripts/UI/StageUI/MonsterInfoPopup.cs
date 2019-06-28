using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MonsterInfoPopup : baseUI
{
    public RawImage MonImage;
    public Text Name;
    public Text Desc;

    public override void Init()
    {

    }


    public void Exit()
    {
        OnExit();
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void Apply(MonsterReferenceData stageData, MonsterDetailReferenceData detailData )
    {
        OnEnter();
        Name.text = StringTBL.GetData( stageData.Name );
        Desc.text = detailData.mob_Info;
        MonImage.texture = ResourceManager.LoadTexture("img_mon_"+stageData.EngName);
    }
}

