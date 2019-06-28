using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Live2D.Cubism.Rendering;

public class TweenAlpha : TweenBase
{
    [SerializeField]
    AnimationCurve ScaleX;
    
    [SerializeField]
    float StartValue = 0;
    [SerializeField]
    float EndValue = 0;

    float scale = 0;
    float factor = 0;

    public Image image;
    public RawImage rawimage;
    CanvasGroup Canvas;
    public CubismRenderController render;
    void Awake()
    {
        image = gameObject.GetComponent<Image>();
        Canvas = gameObject.GetComponent<CanvasGroup>();
        rawimage = gameObject.GetComponent<RawImage>();
    }

    public void Play(float startValue, float endValue )
    {
        if( image != null )
            image.color = new Color(255, 255, 255, startValue);

        if( rawimage != null )
            rawimage.color = new Color( 255 , 255 , 255 , startValue );

        if (Canvas != null)
            Canvas.alpha = startValue;

        if( render != null )
            render.Opacity = StartValue;
        StartValue = startValue;
        EndValue = endValue;
        Play();
    }

    public override void Reset()
    {
        if (image != null)
            scale = image.color.a;

        if( rawimage != null )
            scale = rawimage.color.a;

        if (Canvas != null)
            scale = Canvas.alpha;

        if (ScaleX != null && ScaleX.length > 0)
        {
            scale = StartValue;
        }

        if (image != null)
            image.color = new Color(255, 255, 255, scale);

        if( rawimage != null )
            rawimage.color = new Color( 255 , 255 , 255 , scale );

        if (Canvas != null)
            Canvas.alpha = scale;

        if( render != null )
            render.Opacity = scale;
        base.Reset();
    }
    protected override bool Tween()
    {
        if (base.Tween() == false)
            return false;

        if (image != null)
            scale = image.color.a;

        if( rawimage != null )
            scale = rawimage.color.a;

        if (Canvas != null)
            scale = Canvas.alpha;

        if( render != null )
            scale = render.Opacity;

        if (ScaleX != null && ScaleX.length > 0)
        {
            factor = ScaleX.Evaluate(CurrentTime / PlayTime);
            scale = StartValue * (1f - factor) + EndValue * factor;
        }

        if (image != null)
            image.color = new Color(255, 255, 255, scale);

        if( rawimage != null )
            rawimage.color = new Color( 255 , 255 , 255 , scale );

        if (Canvas != null)
            Canvas.alpha = scale;

        if( render != null )
            render.Opacity = scale;

        return true;
    }



}

