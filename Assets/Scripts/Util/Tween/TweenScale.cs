using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TweenScale : TweenBase
{
    [SerializeField]
    AnimationCurve ScaleX;
    [SerializeField]
    AnimationCurve ScaleY;

    [SerializeField]
    Vector2 StartScale = Vector3.zero;
    [SerializeField]
    Vector2 EndScale = Vector3.zero;

    Vector3 scale = Vector3.zero;
    Vector2 factor = Vector3.zero;

    void Awake()
    {

    }

    public void Play(Vector3 startscale, Vector3 endscale)
    {
        transform.localScale = startscale;
        StartScale = startscale;
        EndScale = endscale;
        Play();
    }

    public override void Reset()
    {
        scale = transform.localScale;

        if (ScaleX != null && ScaleX.length > 0)
        {
            scale.x = StartScale.x;
        }

        if (ScaleY != null && ScaleY.length > 0)
        {
            scale.y = StartScale.y;
        }

        transform.localScale = scale;
        base.Reset();
    }
    protected override bool Tween()
    {
        if (base.Tween() == false)
            return false;

        scale = transform.localScale;
        if (ScaleX != null && ScaleX.length > 0)
        {
            factor.x = ScaleX.Evaluate(CurrentTime / PlayTime);
            scale.x = StartScale.x * (1f - factor.x) + EndScale.x * factor.x;
        }

        if (ScaleY != null && ScaleY.length > 0)
        {
            factor.y = ScaleY.Evaluate(CurrentTime / PlayTime);
            scale.y = StartScale.y * (1f - factor.y) + EndScale.y * factor.y;
        }
                
        transform.localScale = scale;

        return true;
    }



}

