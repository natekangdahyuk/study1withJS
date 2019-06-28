using UnityEngine;
using System;
using System.Collections.Generic;

public class CollisionInfo 
{    
    public int damage;
    public int index;
    public int Turn;
    public int comboCount;
    public bool bCritical;
    public PROPERTY property;
    public int value;
    public string CharacterName;
    
    public Vector3 pos;
}


public class RandomTrailEffect : MonoBehaviour
{
    public GameObject[] DeactivatedObjectsOnCollision;
    public GameObject[] EffectsOnCollision;

    public CollisionInfo collisionInfo = new CollisionInfo();
    public float journeyTime = 2.0f;
    public float DestroyTime = 2f;
    public Vector3 CallEffectPosition;    
    public string Name;
    private float startTime;

    Vector3 riseRelCenter;
    Vector3 setRelCenter;
    Vector3 center;
    Vector3 TargetPosition;
    Vector3 StartPosition;

    public Action<CollisionInfo> call = null;
    bool bPlay = false;

    public bool bTest = false ;
    

    CallEffect calleffect;
   
    void Awake()
    {
        calleffect = GetComponent<CallEffect>();

        if(calleffect)
            calleffect.transform.position = CallEffectPosition;


    }
    public void Update()
    {

#if UNITY_EDITOR
        if (bTest)
        {
            Play(Vector3.zero, new Vector3(0, 3f, 0));
            bTest = false;
        }
#endif

        if ( bPlay == false )
            return;

        float fracComplete = ( Time.time - startTime ) / journeyTime;
        transform.position = Vector3.Slerp( riseRelCenter , setRelCenter , fracComplete );
        transform.position += center;

        if( fracComplete >= 1f )
        {
            Stop();
        }

    }



    void Stop()
    {
        bPlay = false;

        foreach( var effect in DeactivatedObjectsOnCollision )
        {
            if( effect != null )
                effect.SetActive( false );
        }

        foreach( var effect in EffectsOnCollision )
        {
            if( effect != null )
                effect.SetActive( true );
        }

        if (call != null)
        {
            call(collisionInfo);
            Invoke("OnHide", DestroyTime);
        }
        

    }

    void OnHide()
    {
        gameObject.SetActive( false );

        if(TrailEffectManager.I)
            TrailEffectManager.I.Delete( Name ,this.gameObject );
    }
    private void OnEnable()
    {             
    }


    void OncallDeactive()
    {

    }
    public void Play( Vector3 StartPos , Vector3 Targetposition )
    {
        gameObject.SetActive(true);

        if (calleffect)
        {
            if (calleffect.SetTargetPosition)
                calleffect.transform.position = StartPos;

            calleffect.Play();
            //Invoke("OncallDeactive", calleffect.GetComponent<RFX1_DeactivateByTime>().DeactivateTime);
        }
        transform.position = StartPos;
        
        startTime = Time.time;     
        bPlay = true;
        
        TargetPosition = Targetposition;
        StartPosition = StartPos;

        collisionInfo.pos = TargetPosition;
        center = ( StartPosition + TargetPosition ) * 0.5F;

        center -= new Vector3( UnityEngine.Random.Range(-1f,1f) ,0 , 0 );

        riseRelCenter = StartPosition - center;
        setRelCenter = TargetPosition - center;


        foreach( var effect in DeactivatedObjectsOnCollision )
        {
            if( effect != null )
                effect.SetActive( true );
        }

        foreach( var effect in EffectsOnCollision )
        {
            if( effect != null )
                effect.SetActive( false );
        }
    }
}