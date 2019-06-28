using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Control;

public class GameOptionUI : MonoBehaviour
{
    [SerializeField]
    Button OpenButton;

    [SerializeField]
    Button HideButton;

    [SerializeField]
    Button Item1Button;

    [SerializeField]
    Button Item2Button;

    [SerializeField]
    Button Item3Button;

    [SerializeField]
    TweenPosition tween;

    [SerializeField]
    GameOptionPopUp optionPopup;

    [SerializeField]
    ToggleEx autoToggle;

    [SerializeField]
    ToggleEx ContinueToggle;

    int itemvalue = 0;
    void Awake()
    {
        HideButton.gameObject.SetActive(false);

      
    }

    public void OnOpen()
    {
        tween.Play(true);
        OpenButton.gameObject.SetActive(false);
        HideButton.gameObject.SetActive(true);
    }

    public void OnHide()
    {
        tween.ReversePlay(true);
        HideButton.gameObject.SetActive(false);
        OpenButton.gameObject.SetActive(true);
    }

    public void Apply(List<ItemData> itemList )
    {
     
        for( int i=0; i < itemList.Count; i++ )
        {
            if( itemList[ i ].Type == ItemType.Combine )
            {
                Item1Button.interactable = true;
                Item1Button.GetComponent<Image>().color = new Color( 1 , 1 , 1 , 1 );
            }

            if( itemList[ i ].Type == ItemType.Upgrade )
            {
                Item2Button.interactable = true;
                Item2Button.GetComponent<Image>().color = new Color( 1 , 1 , 1 , 1 );
            }

            if( itemList[ i ].Type == ItemType.Return )
            {
                Item3Button.interactable = true;
                Item3Button.GetComponent<Image>().color = new Color( 1 , 1 , 1 , 1 );
            }
        }

        if( GameScene.modeType == ModeType.ModeDefault )
        {
            StageReferenceData ReferenceData = StageManager.I.GetData();
            autoToggle.gameObject.SetActive( true );
            ContinueToggle.gameObject.SetActive(true);
            if ( ReferenceData.Difficulty == 1 )
            {
                itemvalue = 500;
            }
            else if( ReferenceData.Difficulty == 2)
            {
                itemvalue = 1000;
            }
            else
                itemvalue = 1500;
        }
        else
        {
            autoToggle.gameObject.SetActive( false );
            ContinueToggle.gameObject.SetActive(false);
            itemvalue = 1500;
        }

        if( GameScene.modeType == ModeType.Time2048 )
            Item2Button.interactable = false;


        autoToggle.isOn = GameOption.bAutoPlay;
        ContinueToggle.isOn = GameOption.bContinuePlay;


    }

    public bool IsOptionShow()
    {
        return optionPopup.gameObject.activeSelf;
    }
    public void OnOption()
    {
        optionPopup.OnEnter();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnItem1()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( GameScene.Instance.GameMgr.Gamestate != GameManager.GameState.Play )
            return;


        if( PlayerData.I.Gold < itemvalue )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 850015 ) );
            return;
        }

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();
        popup.SetEx( StringTBL.GetData( 850013 ) , itemvalue.ToString("n0") , Item1Ok , null , false , PopupOkCancel.SubType.gold );

    }

    void Item1Ok()
    {

        GameScene.I.UseItem( ItemType.Combine );
        Item1Button.interactable = false;

        Item1Button.GetComponent<Image>().color = new Color( 1 , 1 , 1 , 0.3f );
        NetManager.SetGameBuff( itemvalue );
    }
    public void OnItem2()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( PlayerData.I.Gold < itemvalue )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 850015 ) );
            return;
        }

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();
        popup.SetEx( StringTBL.GetData( 850012 ) , itemvalue.ToString( "n0" ) , Item2Ok , null , false , PopupOkCancel.SubType.gold );
    }

    void Item2Ok()
    {
        if( GameScene.Instance.GameMgr.Gamestate != GameManager.GameState.Play )
            return;


        GameScene.I.UseItem( ItemType.Upgrade );
        Item2Button.interactable = false;
        Item2Button.GetComponent<Image>().color = new Color( 1 , 1 , 1 , 0.3f );
        NetManager.SetGameBuff( itemvalue );
    }

    public void OnItem3()
    {
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
        if( GameScene.Instance.GameMgr.Gamestate != GameManager.GameState.Play )
            return;


        if( PlayerData.I.Gold < itemvalue )
        {
            GlobalUI.ShowOKPupUp( StringTBL.GetData( 850015 ) );
            return;
        }

        PopupOkCancel popup = GlobalUI.GetUI<PopupOkCancel>( UI_TYPE.PopupOkCancel );
        popup.OnEnter();
        popup.SetEx( StringTBL.GetData( 850014 ) , itemvalue.ToString( "n0" ) , Item3Ok , null , false , PopupOkCancel.SubType.gold );
    }

    void Item3Ok()
    {
        GameScene.I.UseItem( ItemType.Return );
        Item3Button.interactable = false;
        Item3Button.GetComponent<Image>().color = new Color( 1 , 1 , 1 , 0.3f );
        NetManager.SetGameBuff( itemvalue );
    }

    public void OnBack()
    {
        if( optionPopup.gameObject.activeSelf == false )
            optionPopup.OnEnter();
        else
            optionPopup.OnExit();
        
    }

    public void OnAuto()
    {
        GameOption.bAutoPlay = autoToggle.isOn;
        
        if( GameOption.bAutoPlay == false )
        {
            GameOption.bContinuePlay = false;
            ContinueToggle.isOn = GameOption.bContinuePlay;
        }

        GameOption.SaveOption();
        SwipeManager.swipeDirection = Swipe.None; 
    }

    public void OnContinue()
    {
        if( GameScene.modeType != ModeType.ModeDefault )
            return;

        if(StageManager.I.GetCurrentStar() < 3)
        {
            GlobalUI.ShowOKPupUp(StringTBL.GetData(902175));
            return;
        }

        GameOption.bContinuePlay = ContinueToggle.isOn;
        
        if(GameOption.bContinuePlay)
        {
            GameOption.bAutoPlay = true;
            autoToggle.isOn = GameOption.bAutoPlay;
        }
                        
        GameOption.SaveOption();
    }
}

