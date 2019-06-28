using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Card : MonoBehaviour
{
    [SerializeField]
    GameObject Mask;

    [SerializeField]
    GameObject Lock;

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
    RawImage StarImage;

    [SerializeField]
    GameObject CharacterPosition;

    [SerializeField]
    GameObject Leader;

    [SerializeField]
    GameObject SubLeader;

    [SerializeField]
    GameObject SkinLock;

    [SerializeField]
    GameObject LevelGroup;

    [SerializeField]
    Text level;

    [SerializeField]
    Text Limit;

    [SerializeField]
    Text teamText;

    [SerializeField]
    GameObject NewCard;

    [SerializeField]
    GameObject teamGroup;

    public GameObject Bonus = null;
    public GameObject Select;
    public GameObject SelectCheck;
    public bool bCheck =false;
    RawImage CharacterImage;


    public int SiblingIndex; //! 렌더링 순서

    public CardData cardData = null;

    public Action<Card> OnClick;

    int Skin = 0;
    int Star = 0;
    int bit = 0;
    public void OnTouch()
    {
        if (OnClick != null)
            OnClick(this);        
    }
   
    public void SetSelect( bool bSelect )
    {
        Select.gameObject.SetActive(bSelect);
    }

    public void SetSelectCheck( bool bCheck )
    {
        SelectCheck.gameObject.SetActive( bCheck );
    }
    public void SetLock( bool bLock )
    {
        Lock.SetActive(bLock);
    }

    public void SetMask(bool bMask)
    {
        Mask.SetActive(bMask);
    }

    public void SetSkinLock( bool bLock )
    {
        SkinLock.SetActive( bLock );
    }

    public void ApplyData(CardData data , bool bIngame = false )
    {
        gameObject.SetActive(true);
        LevelGroup.SetActive( true );
        transform.SetSiblingIndex(0);
        SetSelect(false);
        SetSelectCheck( false );
        
        if( SkinLock )
            SkinLock.gameObject.SetActive( false );      
        
        if( level )
            level.text = "<size=24>Lv </size>" + data.Level.ToString();
        SetLimit(data);
        Lock.SetActive(data.Lock);
        SetLeader(data);
        LoadFrame(data);
        SetStar(data);
        StarImage.gameObject.SetActive( true );
 

        if( Skin == data.Skin)
        {
            if( cardData == data )
                return;
        }
        Skin = data.Skin;
        cardData = data;
        SetNewCard();
        SetTeam();
        UIUtil.LoadClass(ClassImage,cardData.Class);
        SetProperyt( cardData.property );        
        ElementImage.gameObject.SetActive( true );
        

        if (CharacterImage != null)
        {
            GameObject.Destroy(CharacterImage);
            CharacterImage = null;
        }
        CharacterImage = ResourceManager.Load<RawImage>(CharacterPosition, cardData.texture);
        if( cardData.CurrentSkin!= null && cardData.CurrentSkin.texture.Length > 1 )
            CharacterImage.texture = ResourceManager.LoadTexture(cardData.CurrentSkin.texture);
        if (cardData.bit == 2)
        {
            ClassImage.gameObject.SetActive(false);

            if( bIngame )
                ElementImage.gameObject.SetActive(false);
            //level.gameObject.SetActive(false);
            LevelGroup.SetActive( false );
            StarImage.gameObject.SetActive(false);
        }
    }

    public void SetProperyt(PROPERTY propety)
    {
        UIUtil.LoadProperty( ElementImage , propety );
    }

    public void SetTeam()
    {
        if( teamGroup != null )
        {
            if( cardData.GetTeamIndex() > 0 )
            {
                teamGroup.SetActive( true );
                teamText.text = "팀" + cardData.GetTeamIndex().ToString();
            }
            else
            {
                teamGroup.SetActive( false );              
            }

            SetLeader( cardData );
        }
    }
    public void SetStar( CardData data , bool bShowStarImage = false )
    {
        if( bShowStarImage )
            StarImage.gameObject.SetActive( true );

        if (Star == data.Star)
            return;

        Star = data.Star;
        UIUtil.LoadStar(StarImage, data.Star);

       
    }

  

    public void SetLimit(CardData data)
    {
        Limit.gameObject.SetActive(data.Limit > 0 ? true : false);
        Limit.text = "+" + data.Limit.ToString();
    }

    public void SetNewCard()
    {
        if( NewCard == null )
            return;

        if( cardData != null )
            NewCard.SetActive( cardData.bNewCard );
    }
    public void SetSkinMode()
    {
        SetRewardMode();        
        StarImage.gameObject.SetActive( false );        
    }

    public void SetRewardMode()
    {
        ElementImage.gameObject.SetActive( false );
        LevelGroup.SetActive( false );        
        Lock.gameObject.SetActive( false );
        teamGroup.gameObject.SetActive( false );
        ClassImage.gameObject.SetActive( false );
    }

    public void HideTeamGroup()
    {
        teamGroup.gameObject.SetActive( false );

        if (NewCard == null)
            return;

        if (cardData != null)
            NewCard.SetActive(false);
    }

    public void Apply()
    {
        if( SkinLock )
            SkinLock.gameObject.SetActive( false );

        if( gameObject.activeSelf == false )
            gameObject.SetActive(true);

        //transform.SetSiblingIndex(0);
        SetSelect(false);
        SetSelectCheck( false );
        SetLimit( cardData );
        SetNewCard();
        //level.text = cardData.Level.ToString();
        level.text = "<size=24>Lv </size>" + cardData.Level.ToString();
        SetLeader(cardData);
        LoadFrame(cardData);
        SetStar( cardData );
        UIUtil.LoadClass(ClassImage, cardData.Class);
        UIUtil.LoadProperty(ElementImage, cardData.property);        
        

        if (CharacterImage != null)
        {
            GameObject.Destroy(CharacterImage);
            CharacterImage = null;
        }
        CharacterImage = ResourceManager.Load<RawImage>(CharacterPosition, cardData.texture);
        CharacterImage.texture = ResourceManager.LoadTexture(cardData.CurrentSkin.texture );
    }

    public void SetLeader(CardData data)
    {
        Leader.SetActive(data.IsLeader());
        SubLeader.SetActive(data.IsSubLeader());
    }

    public void SetLeader(int index )
    {
        Leader.SetActive( cardData.Leader[ index ] );
        SubLeader.SetActive( cardData.SubLeader[ index ] );
    }

    void LoadFrame( CardData data )
    {
        if( data.bBest )
            NumberImage.color = new Color( 1f , 1f , 75f / 255f , 1f );
        else
            NumberImage.color = Color.white;

        if (bit == data.bit )
            return;

        bit = data.bit;

        //FrameImage.texture = ResourceManager.LoadTexture("tex_card_frame_" + data.bit.ToString());
        BgImage.texture = ResourceManager.LoadTexture("tex_card_bg_" + data.bit.ToString());
        NumberImage.texture = ResourceManager.LoadTexture("tex_card_number_" + data.bit.ToString());

  
    }
}
