using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
public class ScrollController : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,   IEventSystemHandler, IPointerClickHandler
{

    float time;
    Vector2 startpos;

    public Action<bool> actoin;
    public Action actoin2;

    void Start()
    {

    }

   
    void Update()
    {

    }

    public virtual void OnInitializePotentialDrag( PointerEventData eventData )
    {

    }


    public virtual void OnBeginDrag( PointerEventData eventData )
    {
        time = Time.time;
        startpos = eventData.position;
    }

    public virtual void OnDrag( PointerEventData eventData )
    {

    }

    public virtual void OnEndDrag( PointerEventData eventData )
    {
        float value = Time.time - time;

        float distance = Vector2.Distance( startpos , eventData.position );

        if( value<= 0.2f )
        {            
            if( distance > 200 )
                actoin( startpos.x > eventData.position.x );          
        }
    }

    public virtual void OnPointerClick( PointerEventData eventData )
    {
        if( actoin2 != null )
            actoin2();
    }

}
