using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class illustCard : MonoBehaviour
{
   
    [SerializeField]
    RawImage ClassImage;

    [SerializeField]
    RawImage FrameImage;

    [SerializeField]
    RawImage BgImage;
    
    [SerializeField]
    RawImage NumberImage;

    [SerializeField]
    RawImage ElementImage;

    [SerializeField]
    GameObject CharacterPosition;

    [SerializeField]
    RawImage star;

    [SerializeField]
    public Image material;


    public GameObject Select;

    RawImage CharacterImage;


    public int SiblingIndex; //! 렌더링 순서

    public Action<illustCard> OnClick;

    public CharacterReferenceData referenceData;
    public CardReferenceData cardreferenceData;

    int bit = 0;
    int Index = 0;
    public bool bGray = false;
    public void OnTouch()
    {
        if( OnClick != null )
            OnClick( this );
    }

    public void SetSelect( bool bSelect )
    {
        Select.gameObject.SetActive( bSelect );
    }

    public void Refresh()
    {
        bGray = !CollectionManager.I.IsCard( Index );
        if( bGray == false )
        {
            CharacterImage.material = null;
            FrameImage.material = null;
            BgImage.material = null;
            NumberImage.material = null;
            ElementImage.material = null;
            ClassImage.material = null;
        }
        else
        {
            CharacterImage.material = material.material;
            FrameImage.material = material.material;
            BgImage.material = material.material;
            NumberImage.material = material.material;
            ElementImage.material = material.material;
            ClassImage.material = material.material;
        }
    }

    public void ApplyData( int index )
    {
        Index = index;
        CardReferenceData card = CardTBL.GetData(index);
        cardreferenceData = card;
        referenceData = CharacterTBL.GetData(card.characterIndex);        
        gameObject.SetActive( true );
        transform.SetSiblingIndex( 0 );
        SetSelect( false );
        LoadFrame( referenceData.bit );
        UIUtil.LoadProperty(ElementImage, card.Property);

        UIUtil.LoadClass( ClassImage , referenceData.Class );
       
        if( CharacterImage != null )
        {
            GameObject.Destroy( CharacterImage );
            CharacterImage = null;
        }
        
        CharacterImage = ResourceManager.Load<RawImage>( CharacterPosition , referenceData.Texture );
        Refresh();


        NumberImage.color = new Color(1f, 1f, 75f / 255f, 1f);
    }
       

    void LoadFrame( int value )
    {
        if( bit == value )
            return;

        bit = value;

        //FrameImage.texture = ResourceManager.LoadTexture( "tex_card_frame_" + value.ToString() );
        BgImage.texture = ResourceManager.LoadTexture( "tex_card_bg_" + value.ToString() );
        NumberImage.texture = ResourceManager.LoadTexture( "tex_card_number_" + value.ToString() );
        UIUtil.LoadStar( star , referenceData.star );
    }
}
