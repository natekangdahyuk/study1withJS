using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TweenPosition : TweenBase
{
    [SerializeField]
    AnimationCurve PositionX;
    [SerializeField]
    AnimationCurve PositionY;

    [SerializeField]
    Vector2 StartPos = Vector3.zero;
    [SerializeField]
    Vector2 EndPos = Vector3.zero;

    Vector2 pos = Vector3.zero;
    Vector2 factor = Vector3.zero;
        
    void Awake()
    {
    
    }

    public void Play( Vector3 startpos , Vector3 endpos )
    {
        transform.localPosition = startpos;
        StartPos = startpos;
        EndPos = endpos;
        Play();
    }

    public override void Reset()
    {
        pos = transform.localPosition;

        if(PositionX != null && PositionX.length > 0)
        {
            pos.x = StartPos.x;
        }

        if (PositionY != null && PositionY.length > 0)
        {
            pos.y = StartPos.y;
        }

        transform.localPosition = pos;
        base.Reset();
    }
    protected override bool Tween()
    {
        if (base.Tween() == false)
            return false;

        pos = transform.localPosition;
        if (PositionX != null && PositionX.length > 0 )
        {
            factor.x = PositionX.Evaluate(CurrentTime / PlayTime);
            pos.x = StartPos.x * (1f - factor.x) + EndPos.x * factor.x;
        }

        if (PositionY != null && PositionY.length > 0)
        {
            factor.y = PositionY.Evaluate(CurrentTime / PlayTime);
            pos.y = StartPos.y * (1f - factor.y) + EndPos.y * factor.y;
        }
        transform.localPosition = pos;
        
        return true;
    }

      

}

