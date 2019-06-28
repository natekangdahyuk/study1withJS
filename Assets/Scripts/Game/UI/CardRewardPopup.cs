using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Live2D.Cubism.Rendering;


public class CardRewardPopup : baseUI
{

    [SerializeField]
    RawImage ClassImage;

    [SerializeField]
    Text ClassText;

    [SerializeField]
    Text NameText;

    [SerializeField]
    RawImage GradeImage;

    [SerializeField]
    RawImage PropertyImage;

    [SerializeField]
    RawImage BitImage;

    GameObject Live2DModel;
    GameObject Live2DBG;
    GameObject Live2DFx;

    [SerializeField]
    GameObject representParentPosition = null;

    [SerializeField]
    GameObject representParentBGPosition = null;

    [SerializeField]
    GameObject BGEffectParent;

    public Action EndCall = null;
    void Awake()
    {
       
        gameObject.SetActive( false );

    }

    public void Apply( CardData carddata )
    {
        gameObject.SetActive( true );

        gameObject.SetActive( true );

        NameText.text = carddata.Name;

        ClassText.text = UIUtil.ClassString( carddata.Class ) + "/" + UIUtil.PropertyString( carddata.property );

        UIUtil.LoadProperty( PropertyImage , carddata.property );
        UIUtil.LoadClass( ClassImage , carddata.Class );
        UIUtil.LoadStarEx( GradeImage , carddata.Star );
        UIUtil.LoadBit( BitImage , carddata.bit );


        string Live2DStr = carddata.Live2DModel;
        if( Live2DModel != null )
        {
            GameObject.Destroy( Live2DModel.gameObject );
            GameObject.Destroy( Live2DBG.gameObject );
        }

        if( Live2DFx != null )
            GameObject.Destroy( Live2DFx.gameObject );

        Live2DModel = ResourceManager.Load( representParentPosition , carddata.Live2DModel );
        Live2DBG = ResourceManager.Load( representParentBGPosition , carddata.Live2DBG );

        if( Live2DModel )
        {
            CubismRenderController controller = Live2DModel.GetComponent<CubismRenderController>();

            if( controller )
            {
                controller.DepthOffset = 5;
                controller.SortingOrder = 9;
            }


        }
        //if( GameOption.LowMode == false )
        {
            string str = carddata.Live2DBG;
            str = str.Replace( "bg" , "fx" );
            Live2DFx = ResourceManager.Load( BGEffectParent , str );
        }
    }

    public void OnOk()
    {
        OnExit();
        if( EndCall != null )
            EndCall();
    }

    public override void Init()
    {

    }
}
