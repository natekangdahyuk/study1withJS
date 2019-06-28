using UnityEngine;
using System;
using System.Collections.Generic;

public class TweenVolume : TweenBase
{
    SoundObject Soundobject;

    [SerializeField]
    AnimationCurve curve;

    float StartValue;
    float EndValue;

    public void Set( SoundObject ob )
    {
        Soundobject = ob;
        StartValue = Soundobject.GetVolume();
        EndValue = 0;
    }

    public override void Reset()
    {
        if( Soundobject )
            Soundobject.SetVolume( StartValue );
    }

    protected override bool Tween()
    {
        if( base.Tween() == false )
            return false;
        
        float factor = curve.Evaluate( CurrentTime / PlayTime );
        float value = StartValue * ( 1f - factor ) + EndValue * factor;
        Soundobject.SetVolume( value );

        return true;
    }
}