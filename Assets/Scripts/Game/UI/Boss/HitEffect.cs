using UnityEngine;
using UnityEngine.UI;
using System;
public class HitEffect : MonoBehaviour
{
    [SerializeField]
    Text Count;

    [SerializeField]
    Text Count2;

    [SerializeField]
    Animation anim;

    public Action<GameObject> endCall;

    public void Play(int count , int count2 = 0)
    {        
        gameObject.SetActive(true);
        Count.text = count.ToString();

        if( Count2 != null  )
        {
            if( count2 != 0 )
            {
                Count2.text = count2.ToString() + "% BonusDamage";
                Count2.gameObject.SetActive( true );
            }
            else
                Count2.gameObject.SetActive( false );
        }
        
        anim.Play();
        Invoke( "End" , 2 );
    }

    public void PlayHeal( int count  )
    {
        gameObject.SetActive( true );
        Count.text = "+"+count.ToString();

      
        anim.Play();
        Invoke( "End" , 2 );
    }

    public void End()
    {
        if(endCall!= null)
            endCall(this.gameObject);
    }
}

