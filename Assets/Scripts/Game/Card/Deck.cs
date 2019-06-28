using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Deck : MonoSinglton<Deck>
{
    
    [SerializeField]
    List<DeckCard> Cardlist = new List<DeckCard>();

    [SerializeField]
    Animation ReadyAnimation;

    [SerializeField]
    CardAppear cardAppear;

    public Action ReadyCallback = null;
    public int CurrentCardIndex = 0;

    public int GetCurrentBit
    {
        get
        {
            if( GameScene.I.GameMgr.stageBase.currTurn == turnCount+1 )
                return CurrentCardIndex-1;
            else
                return CurrentCardIndex;
        }
    }


    public int defence = 0;
    CardData LeaderCard = null;
    CardData SubLeaderCard = null;
    public int turnCount = 0;
        
    public void PlayStartAnimation()
    {
        ReadyAnimation.Play();
    }
    public void StartGame() //! 게임시작 애니매이션에서 콜백으로받아옴 ani_card_ingame_ready
    {
        ReadyCallback();
    }
    
    public Vector3 GetCardPosition( int value )
    {
        for( int i=0; i < Cardlist.Count; i++)
        {
            if (Cardlist[i].Value == value)
                return Cardlist[i].transform.position;
        }

        return Vector3.zero;
    }

    public CardData GetCard(int value)
    {
        for (int i = 0; i < Cardlist.Count; i++)
        {
            if (Cardlist[i].Value == value)
                return Cardlist[i].cardData;
        }
        return null;
    }

    public CardData GetCardByBit(int bit)
    {
        for (int i = 0; i < Cardlist.Count; i++)
        {
            if (Cardlist[i].cardData == null)
                continue;

            if (Cardlist[i].cardData.bit == bit)
                return Cardlist[i].cardData;
        }

        return null;
    }

    public override void Constructor()
    {
        int MaxSiblingIndex = 0;

        for (int i = 0; i < Cardlist.Count; i++)
        {
            Cardlist[i].SetValue((int)Math.Pow(2, i + 2));

            if (MaxSiblingIndex < Cardlist[i].SiblingIndex)
                MaxSiblingIndex = Cardlist[i].SiblingIndex;
        }

        for (int i = 0; i < Cardlist.Count; i++)
        {
            Cardlist[i].SetMaxSiblingIndex(MaxSiblingIndex);
        }

    }

    public void CreateAllCard(Dictionary<int,CardData> CardDatalist , int currentDeck )
    {
        //CreateCard(new CardData(10001, -1), 0);

        foreach( KeyValuePair<int, CardData> value in CardDatalist )
        {
            int index = (int)Mathf.Log(value.Value.bit, 2) - 2;
            CreateCard(value.Value, index);

            defence += value.Value.TotalDefence;

            if(value.Value.Class == CLASS.HEAL)
                TrailEffectManager.Instance.CreateEffect(PROPERTY.HEAL);
            else
                TrailEffectManager.Instance.CreateEffect( value.Value.property );

            if( value.Value.Leader[ currentDeck ] )
            {
                LeaderCard = value.Value;                
            }

            if( value.Value.SubLeader[ currentDeck ] )
            {
                SubLeaderCard = value.Value;                
            }
        }
                
        cardAppear.Apply( Cardlist );
        

    }

    public void CreateCard( CardData data , int index )
    {
        Cardlist[index].ApplyData(data);
    }

    public void SetCurrentCard( int index, int turn )
    {
        if( CurrentCardIndex < index )
        {
            CurrentCardIndex = index;
            turnCount = turn;
            cardAppear.Play( index );

            
            for( int i=0; i < Cardlist.Count; i++)
            {
                if( Cardlist[i].cardData.bit == index )
                {
                    SoundManager.I.Play(SoundManager.SoundType.voice, Cardlist[i].cardData.Voice, GameOption.VoiceVoluem);
                    break;
                }
            }
            
            for ( int i = 0;  i < Cardlist.Count; i++ )
            {
                Cardlist[i].SetMask( index );
            }
        }
    }

    public float GetLeaderComboBuffValue()
    {
        if( LeaderCard != null )
        {
            if( LeaderCard.leaderBuff == LeaderBuff.ComboUp )
            {
                return LeaderCard.leaderBuffValue;
            }
        }
        return 100f;

    }
    public float GetSubLeaderComboBuffValue()
    {
        if( SubLeaderCard != null )
        {
            if( SubLeaderCard.leaderBuff == LeaderBuff.ComboUp )
            {
                return SubLeaderCard.leaderBuffValue;
            }
        }
        return 100f;
    }

    public int GetBuffAttack( CardData card , bool bCritlcal )
    {
        int damage = 0;
        float BuffValue = 0;
       
        if( LeaderCard != null )
        {
            if( LeaderCard.leaderBuff == LeaderBuff.AllAttack )
            {
                BuffValue = LeaderCard.leaderBuffValue;
            }
            else if( LeaderCard.leaderBuff == LeaderBuff.AttributeAttack )
            {
                if( LeaderCard.property == card.property )
                    BuffValue = LeaderCard.leaderBuffValue;
            }
            else if( LeaderCard.leaderBuff == LeaderBuff.ClassAttack )
            {
                if( LeaderCard.Class == card.Class )
                    BuffValue = LeaderCard.leaderBuffValue;
            }
        }

        if( SubLeaderCard != null )
        {
            if( SubLeaderCard.leaderBuff == LeaderBuff.AllAttack )
            {
                BuffValue += SubLeaderCard.leaderBuffValue;
            }
            else if( SubLeaderCard.leaderBuff == LeaderBuff.AttributeAttack )
            {
                if( SubLeaderCard.property == card.property )
                    BuffValue += SubLeaderCard.leaderBuffValue;
            }
            else if( SubLeaderCard.leaderBuff == LeaderBuff.ClassAttack )
            {
                if( SubLeaderCard.Class == card.Class )
                    BuffValue += SubLeaderCard.leaderBuffValue;
            }
        }

        if( bCritlcal == true )
        {            
            int carddamage = (int)( (float)card.Totaldamage * 1.5f );
            damage += ( carddamage + (int)( (float)carddamage * ( BuffValue / 100f ) ) );
        }
        else
            damage += ( card.Totaldamage + (int)( (float)card.Totaldamage * ( BuffValue / 100f ) ) );
       

        return damage;
    }

    public int GetBuffHeal( CardData card  )
    {
        int damage = 0;
        float BuffValue = 0;
       
        damage += ( card.TotalHeal + (int)( (float)card.TotalHeal * ( BuffValue / 100f ) ) );


        return damage;
    }


    public int GetHP()
    {
        int hp = 0;
        
        for( int i=0 ; i < Cardlist.Count ;i++ )
        {
            float BuffValue = 0;
            if( Cardlist[ i ].cardData == null )
                continue;

            if(LeaderCard != null )
            {
                if( LeaderCard.leaderBuff == LeaderBuff.AllHp )
                {
                    BuffValue = LeaderCard.leaderBuffValue;
                }
                else if( LeaderCard.leaderBuff == LeaderBuff.AttributeHp )
                {       
                    if( LeaderCard.property == Cardlist [i].cardData.property )
                        BuffValue = LeaderCard.leaderBuffValue;
                }
                else if( LeaderCard.leaderBuff == LeaderBuff.ClassHp )
                {
                    if( LeaderCard.Class == Cardlist[ i ].cardData.Class )
                        BuffValue = LeaderCard.leaderBuffValue;                    
                }
            }

            if( SubLeaderCard != null )
            {
                if( SubLeaderCard.leaderBuff == LeaderBuff.AllHp )
                {
                    BuffValue += SubLeaderCard.leaderBuffValue;
                }
                else if( SubLeaderCard.leaderBuff == LeaderBuff.AttributeHp )
                {
                    if( SubLeaderCard.property == Cardlist[ i ].cardData.property )
                        BuffValue += SubLeaderCard.leaderBuffValue;
                }
                else if( SubLeaderCard.leaderBuff == LeaderBuff.ClassHp )
                {
                    if( SubLeaderCard.Class == Cardlist[ i ].cardData.Class )
                        BuffValue += SubLeaderCard.leaderBuffValue;
                }
            }

            hp += ( Cardlist[ i ].cardData.TotalHp  + (int)( (float)Cardlist[ i ].cardData.TotalHp * ( BuffValue / 100f ) ) );
        }

        return hp;
    }

    
    public void Reset()
    {
        CurrentCardIndex = 2;
            
        for (int i = 0; i < Cardlist.Count; i++)
        {
            Cardlist[i].Reset();
        }

        Cardlist[0].SetMask(2);
    }

    public override void ClearAll()
    {
     
    }
}



