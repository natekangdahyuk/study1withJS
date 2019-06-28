using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Live2D.Cubism.Rendering;
public class CardAppear : MonoBehaviour
{

    [SerializeField] GameObject CardPosition;
    [SerializeField] Animation anim1;
    [SerializeField] Animation anim2;
    [SerializeField] TweenAlpha tween;
    [SerializeField] Text Name;
    [SerializeField] Text Desc;

    List<GameObject> Live2DModelList = new List<GameObject>();
    List<DeckCard> Cardlist;


    public void Apply( List<DeckCard> _Cardlist )
    {
        Cardlist = _Cardlist;
        for( int i = 0 ; i < Cardlist.Count ; i++ )
        {
            if( Cardlist[ i ].cardData == null )
                continue;

            GameObject Live2DModel = ResourceManager.Load( CardPosition , Cardlist[ i ].cardData.Live2DModel );

            if( Live2DModel != null )
            {
                CubismRenderController controller = Live2DModel.GetComponent<CubismRenderController>();
                               
                controller.DepthOffset = 5;

                Live2DModel.gameObject.SetActive( false );
                Live2DModelList.Add( Live2DModel );               
            }            
        }        
    }

    public void Play( int index )
    {
        int value = (int)Mathf.Log( index , 2 ) - 2;

        for( int i=0 ; i < Live2DModelList.Count ; i++ )
        {
            if( Live2DModelList[ i ] == null )
                continue;

            if( i == value )
            {
                Live2DModelList[ i ].gameObject.SetActive( true );
                tween.render = Live2DModelList[ i ].GetComponent<CubismRenderController>();

                Name.text = Cardlist[ i ].cardData.Name;
                Desc.text = Cardlist[ i ].cardData.oneWord;
            }
            else
                Live2DModelList[ i ].gameObject.SetActive( false );
        }

        anim1.CrossFade("ani_character_insert" ,1f );
        anim2.gameObject.SetActive( true );
        anim2.CrossFade("ani_character_insert_text",1f);
        tween.Play( 0 , 1 );

        
    }



}