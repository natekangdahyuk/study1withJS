using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public partial class DeckUI : MonoBehaviour
{
    public enum ModeType
    {
        Default,
        MainStageReady,
        TeamSetting,
        LeaderSetting,
    }

    [SerializeField]
    ToggleGroup deckToggleGroup;

    [SerializeField]
    Text Hp;

    [SerializeField]
    Text Buff1;

    [SerializeField]
    Text Buff2;

    [SerializeField]
    GameObject DefaultModeGroup = null;

    [SerializeField]
    GameObject TeamSettingGroup = null;

    [SerializeField]
    GameObject LeaderSettingGroup = null;

    [SerializeField]
    GameObject StageGroup = null;

    [SerializeField]
    Text LeaderText = null;

    [SerializeField]
    Button PrevDeck;

    [SerializeField]
    Button NextDeck;

    [SerializeField]
    Text CurrentDeck;

    [SerializeField]
    GameObject Lock;

    [SerializeField]
    GameObject DefaultGroup;

    //Toggle[] deckToggle;
    public List<Card> deckCardList = new List<Card>();

    public List<LeaderSettingItem> SelectLeaderList = new List<LeaderSettingItem>();

    public Action OnteamSetting = null;

    public Action onCompleteDeck = null;

    public Action onLeaderSetting = null;

    public Action onDeckChange = null;

    [SerializeField]
    GameObject[] CardParent;

    public static T Load<T>(GameObject parent, string name) where T : MonoBehaviour
    {
        T ui = ResourceManager.Load<T>(parent, name);

        ui.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        ui.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        ui.transform.localScale = Vector3.one;
        return ui;
    }


    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            Card card = ResourceManager.Load<Card>(this.gameObject, "Card");            
            card.transform.SetParent(CardParent[i].transform);
            card.transform.localPosition = Vector3.zero;
            //card.ApplyData(cardlist[i]);
            card.OnClick = OnSelectCard;
            deckCardList.Add(card);
            card.gameObject.SetActive(false);
        }

        for( int i = 0 ; i < 10 ; i++ )
        {
            LeaderSettingItem card = ResourceManager.Load<LeaderSettingItem>( this.gameObject , "SelectLeader" );
            card.transform.SetParent( CardParent[ i ].transform );
            card.transform.localPosition = Vector3.zero;
            SelectLeaderList.Add( card );
            card.gameObject.SetActive( false );
        }

    }

    public void SetLock(bool value)
    {
        Lock.SetActive( value );
    }
    public void Apply(ModeType type)
    {
        DefaultGroup.SetActive( true );
        StageGroup.SetActive(false);
        SetLock( false );
        switch (type)
        {
            case ModeType.Default:
                DefaultModeGroup.SetActive(true);
                TeamSettingGroup.SetActive(false);
                LeaderSettingGroup.SetActive(false);
                break;

            case ModeType.MainStageReady:                
                TeamSettingGroup.SetActive( false );
                LeaderSettingGroup.SetActive( false );
                DefaultGroup.SetActive( false );
                StageGroup.SetActive(true);
            break;


            case ModeType.TeamSetting:
                DefaultModeGroup.SetActive(false);
                TeamSettingGroup.SetActive(true);
                LeaderSettingGroup.SetActive(false);
                break;
            case ModeType.LeaderSetting:
                DefaultModeGroup.SetActive(false);
                TeamSettingGroup.SetActive(false);
                LeaderSettingGroup.SetActive(true);
                SetLeaderSettingText(StringTBL.GetData(900032));
                break;
        }

        ApplyDeck(DeckManager.I.GetCurrentDeck());
    }
    
    public void SetLeaderSettingText( string title)
    {
        LeaderText.text = title;
    }


    public void ApplyDeck(Dictionary<int,CardData> cardlist )
    {
        ClearDeckCard();

        int totalHp = 0;
        int totalDefence = 0;
        Buff1.text = "";
        Buff2.text = "";
        foreach( KeyValuePair<int , CardData> value in cardlist )
        {
            int index = (int)Mathf.Log( value.Value.bit , 2 ) - 2;

            if( index < 0 )
                continue;

            deckCardList[ index ].ApplyData( value.Value,true );
            deckCardList[ index ].SetLeader( DeckManager.I.CurrentDeckIndex - 1 );
            deckCardList[ index ].gameObject.SetActive( true );
            deckCardList[ index ].HideTeamGroup();
            totalHp += value.Value.TotalHp;
            totalDefence += value.Value.TotalDefence;

            if ( deckCardList[ index ].cardData.Leader[ DeckManager.I.CurrentDeckIndex-1] )
            {
                Buff1.text = UIUtil.LeaderBuffString( deckCardList[ index ].cardData.leaderBuff , deckCardList[ index ].cardData.leaderBuffValue , deckCardList[index].cardData.property, deckCardList[index].cardData.Class );
            }
            else if( deckCardList[ index ].cardData.SubLeader[ DeckManager.I.CurrentDeckIndex - 1 ] )
            {
                Buff2.text = UIUtil.LeaderBuffString( deckCardList[ index ].cardData.leaderBuff , deckCardList[ index ].cardData.leaderBuffValue, deckCardList[index].cardData.property, deckCardList[index].cardData.Class);
            }
        }

        CurrentDeck.text = "팀" + DeckManager.I.CurrentDeckIndex.ToString();

        if (totalHp <= 0)
            Hp.text = "";
        else
        {
            if (totalDefence > 0)
                Hp.text = totalHp.ToString("n0") + " / " + totalDefence.ToString("n0");
            else
                Hp.text = totalHp.ToString("n0");
        }
        
    }

    public void ApplyDeck(List<Card> cardlist)
    {
        ClearDeckCard();
        int totalHp = 0;
        for (int i = 0; i < cardlist.Count; i++)
        {
            int index = (int)Mathf.Log(cardlist[i].cardData.bit, 2) - 2;

            deckCardList[index].ApplyData(cardlist[i].cardData,true);
            deckCardList[ index ].SetLeader( DeckManager.I.CurrentDeckIndex - 1 );
            deckCardList[index].gameObject.SetActive(true);
            if( cardlist[ i ] != null && cardlist[ i ].bCheck )
                deckCardList[ index ].SetSelect( true );
            totalHp += cardlist[i].cardData.Hp;
        }

        Hp.text = totalHp.ToString("n0");
    }

    public void SelectCheck( Card card )
    {
        for( int i = 0 ; i < deckCardList.Count ; i++ )
        {
            if( deckCardList[i].cardData.CardKey == card.cardData.CardKey )
            {
                deckCardList[ i ].SetSelect( true );
                return;
            }
        }
    }

    public void CheckDeckChangeBtn()
    {
        if(DeckManager.I.CurrentDeckIndex >= 5)
        {
            DeckManager.I.CurrentDeckIndex = 5;
            NextDeck.enabled = false;
        }
        else
            NextDeck.enabled = true;


        if (DeckManager.I.CurrentDeckIndex <= 1)
        {
            DeckManager.I.CurrentDeckIndex = 1;
            PrevDeck.enabled = false;
        }
        else
            PrevDeck.enabled = true;


    }
    public void OnNextDeck()
    {
        DeckManager.I.CurrentDeckIndex++;

        CheckDeckChangeBtn();
                
        ApplyDeck(DeckManager.I.GetCurrentDeck());

        if (onDeckChange != null)
            onDeckChange();

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_page" , GameOption.EffectVoluem );
    }

    public void OnPrevDeck()
    {
        DeckManager.I.CurrentDeckIndex--;

        CheckDeckChangeBtn();

        ApplyDeck(DeckManager.I.GetCurrentDeck());

        if (onDeckChange != null)
            onDeckChange();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_page" , GameOption.EffectVoluem );
    }

    public void ClearDeckCard()
    {
        for (int i = 0; i < deckCardList.Count; i++)
        {
            deckCardList[i].gameObject.SetActive(false);
        }
    }

    public void OnTeamSetting()
    {
        OnteamSetting();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }
   
    public void OnCompleteDeck()
    {
        onCompleteDeck();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnLeaderSetting()
    {
        onLeaderSetting();
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }


}

public partial class DeckUI : MonoBehaviour
{
    public Action<Card> OnClick;
   
    void OnSelectCard(Card card)
    {

        OnClick(card);
    }

    
}