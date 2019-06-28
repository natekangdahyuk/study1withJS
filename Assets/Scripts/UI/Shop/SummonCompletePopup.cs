using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Live2D.Cubism.Rendering;


public class SummonCompletePopup : baseUI
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

    CardData data;
    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
    }

    public void Apply(CardData carddata)
    {
        data = carddata;
        gameObject.SetActive(true);

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

        if( carddata.bBest )
            BitImage.color = new Color( 1 , 1 , 75f / 255f,1f );
        else
            BitImage.color = Color.white;
        if( Live2DModel )
        {
            CubismRenderController controller = Live2DModel.GetComponent<CubismRenderController>();

            if( controller)
            {
                controller.DepthOffset = 5;
                controller.SortingOrder = 29;
            }
               
                
        }

        if( Live2DBG )
        {
            ParticleSystem[] system = Live2DBG.GetComponentsInChildren<ParticleSystem>();

            for( int i = 0 ; i < system.Length ; i++ )
            {
                Renderer render = system[ i ].GetComponent<Renderer>();
                render.sortingOrder = 29;
            }
        }
        //if( GameOption.LowMode == false )
        {
            string str = carddata.Live2DBG;
            str = str.Replace( "bg" , "fx" );
            Live2DFx = ResourceManager.Load( BGEffectParent , str );         
        }
        Invoke( "PlaySound" , 2.2f );
    }

    void PlaySound()
    {
        SoundManager.I.Play( SoundManager.SoundType.voice , data.Voice , GameOption.VoiceVoluem );
    }

    public void OnOk()
    {
        OnExit();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public override void Init()
    {

    }
}
