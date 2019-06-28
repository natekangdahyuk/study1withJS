using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ToggleEx :Toggle
{


    public override void OnPointerClick( PointerEventData eventData )
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

        base.OnPointerClick( eventData );
    }
}