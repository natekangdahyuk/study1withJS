using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInfoDetail : baseUI
{    
    public Text Desc;

    public override void Init()
    {

    }


    public void Exit()
    {
        OnExit();
        SoundManager.I.Play(SoundManager.SoundType.Effect, "snd_ui_common_button", GameOption.EffectVoluem);
    }

    public void Apply(CardData data )
    {
        Desc.text = data.referenceData.charInfo;
        OnEnter();
        
    }
}

