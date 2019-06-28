using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Live2D.Cubism.Rendering;

public class Live2DModel : MonoBehaviour
{
    [SerializeField]
    GameObject BGParent;

    [SerializeField]
    GameObject BGEffectParent;

    GameObject Live2DBG;
    GameObject Model;
    GameObject Live2DFx;

    public void Apply( SkinReferenceData skin , bool bGray , Material gray )
    {
        if( Live2DBG != null )
            GameObject.Destroy( Live2DBG.gameObject );

        if( Model != null )
            GameObject.Destroy( Model.gameObject );

        if( Live2DFx != null )
            GameObject.Destroy( Live2DFx.gameObject );

        Live2DBG = ResourceManager.Load( BGParent , skin.Live2DBG );
        Live2DBG.transform.localPosition = new Vector3( 0 , 0 , 1000 );
        Model = ResourceManager.Load( BGParent , skin.Live2DModel );

        if( Model )
        {
            CubismRenderController controller = Model.GetComponent<CubismRenderController>();

            if( controller )
            {
                controller.DepthOffset = 5;
                controller.SortingOrder = 13;
            }

            CubismRenderer[] renderer = Model.GetComponentsInChildren<CubismRenderer>();
            Animator anim = Model.GetComponent<Animator>();

            if( bGray )
            {
                anim.enabled = false;
            }
            else
            {
                anim.enabled = true;
            }
            for( int i =0; i < renderer.Length; i++)
            {
                renderer[i].Material = gray;

                if( bGray )
                {
                    renderer[ i ].Material.SetFloat( "_EffectAmount" , 1 );
                }
                else
                {
                    renderer[ i ].Material.SetFloat( "_EffectAmount" , 0 );
                }

            }

        }

        //if(GameOption.LowMode == false )
        {
            string str = skin.Live2DBG;

            str = str.Replace( "bg" , "fx" );
            Live2DFx = ResourceManager.Load( BGEffectParent , str );
        }
        

        gameObject.SetActive( true );
    }
    
    public void SetGray()
    {

    }
}