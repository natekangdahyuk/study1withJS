using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TweenBase : MonoBehaviour
{
    public List<Action> onFinish = new List<Action>();
    public float PlayTime = 1.0f;
        
    protected float CurrentTime = 0.0f;
    protected bool bPlay = false;
    protected bool bReverse = false;
    public bool bLoop = false;

    public bool IsPlay()
    {
        return bPlay;
    }

    public void Play( bool IgnoreTime = false )
    {
        bPlay = true;
        if (IgnoreTime == false)
            CurrentTime = 0.0f;
        bReverse = false;
    }

    public void ReversePlay(bool IgnoreTime = false)
    {
        bPlay = true;

        if(IgnoreTime == false )
            CurrentTime = PlayTime;
        bReverse = true;
    }

    public void Stop()
    {
        bPlay = false;
    }

    public virtual void Reset()
    {

    }

    void Update()
    {
        Tween();
    }

    protected virtual bool Tween()
    {
        if (bPlay == false) return false;

        if( bReverse )
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0)
            {
                if(bLoop)
                {
                    bReverse = false;
                }
                else
                {
                    bPlay = false;
                    Execute();
                }
                
            }
        }
        else
        {
            CurrentTime += Time.deltaTime;
            if (CurrentTime > PlayTime)
            {
                if( bLoop )
                    bReverse = true;
                else
                {
                    bPlay = false;
                    Execute();
                }
                
            }
        }
        

        return true;
    }
    protected void Execute()
    {
        for(int i = 0; i < onFinish.Count; i++)
        {
            onFinish[i]();
        }
    }

}

