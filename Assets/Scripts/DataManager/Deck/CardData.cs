using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SkinData
{
    public int index;
    public bool bGet;
    public SkinReferenceData Skin;
}


public class CardData
{
   
    //! 테이블 데이터
    public CharacterReferenceData referenceData;
    CardReferenceData cardreferenceData;
    StatReferenceData statreferenceData;
    limitbreakReferenceData limitData;
    public SkinReferenceData CurrentSkin;
    public int bit { get { return referenceData.bit; } }
    public int damage { get { return statreferenceData.Stat[(int)STAT.Attack]; } }
    public int Hp {  get { return statreferenceData.Stat[(int)STAT.Hp]; } }
    public int Defence { get { return statreferenceData.Stat[(int)STAT.Defence]; } }
    public int Heal { get { return statreferenceData.Stat[ (int)STAT.Heal ]; } }

    public int Bonusdamage { get { return statreferenceData.Stat[(int)STAT.Bonus_Attack]* ( Level - 1 ); } }
    public int BonusHp { get { return statreferenceData.Stat[(int)STAT.Bonus_Hp] * (Level-1); } }
    public int BonusDefence { get { return statreferenceData.Stat[(int)STAT.Bonus_Defence] * ( Level - 1 ); } }

    public int BonusHeal { get { return statreferenceData.Stat[ (int)STAT.Bonus_Heal ] * ( Level - 1 ); } }    
    public int SellPrice { get { return referenceData.SellPrice; } }

    public int Critical
    {
        get
        {
            return cardreferenceData.PentagonValue[ 3 ] + ( GetSkinValue( ABILITYTYPE.Critical )  );

            //if( CurrentSkin.ablityType == ABILITYTYPE.Critical )
            //    return cardreferenceData.PentagonValue[ 3 ] + CurrentSkin.ablityValue;

            //return cardreferenceData.PentagonValue[ 3 ];
        }
    }

    public int Totaldamage
    {
        get
        {
            float value = damage + Bonusdamage;

            value += ( value * ( GetSkinValue( ABILITYTYPE.Attack ) / 100f ) );

            //if( CurrentSkin.ablityType == ABILITYTYPE.Attack )            
            //    value += (value * ( CurrentSkin.ablityValue / 100f ));
            

            if( limitData != null )
                return (int)( value + (( value ) * (limitData.stat_up / 100f)) );

            return (int)value;
        }
    }
    public int TotalHp
    {
        get
        {
            float value = Hp + BonusHp;
            
            value += ( value * ( GetSkinValue( ABILITYTYPE.Health ) / 100f ) );
            //if( CurrentSkin.ablityType == ABILITYTYPE.Health )
            //    value += ( value * ( CurrentSkin.ablityValue / 100f ) );

            if (limitData != null)
                return (int)( value + (( value ) * (limitData.stat_up / 100f)));

            return (int)value;
        }
    }
    public int TotalDefence
    {
        get
        {
            float value = Defence + BonusDefence;

            value += ( value * ( GetSkinValue( ABILITYTYPE.Defence ) / 100f ) );

            //if( CurrentSkin.ablityType == ABILITYTYPE.Defence )
            //    value += ( value * ( CurrentSkin.ablityValue / 100f ) );

            if (limitData != null)
                return (int)( value + (( value ) * (limitData.stat_up / 100f)));


            return (int)value;
        }
    }

    public int TotalHeal
    {
        get
        {
            float value = Heal + BonusHeal;
            
            value += ( value * ( GetSkinValue( ABILITYTYPE.Heal ) / 100f ) );

            //if( CurrentSkin.ablityType == ABILITYTYPE.Heal )
            //    value += ( value * ( CurrentSkin.ablityValue / 100f ) );

            if( limitData != null )
                return (int)( value + ( ( value ) * ( limitData.stat_up / 100f ) ) );


            return (int)value;
        }
    }

    public CLASS Class { get { return referenceData.Class; } }
    public PROPERTY property { get { return cardreferenceData.Property; } }
    public string texture { get { return referenceData.Texture; } }
    public string Live2DModel { get { return CurrentSkin.Live2DModel; } }
    public string Live2DBG { get { return CurrentSkin.Live2DBG; } }
    public string Voice { get { return CurrentSkin.Sound[ Random.Range( 0 , CurrentSkin.Sound.Length - 1 )] ; } }

    public string TouchSound
    {
        get
        {
            int index = Random.Range(0, referenceData.TouchSound.Length + CurrentSkin.Sound.Length - 1 );

            if(index >= referenceData.TouchSound.Length)
                return CurrentSkin.Sound[Random.Range(0, index - referenceData.TouchSound.Length )];
            else
            {
                return referenceData.TouchSound[index];
            }
        }
    }

    public string oneWord
    {
        get
        {
            return CurrentSkin.oneWord;
        }
    }



    public int ReferenceIndex {  get { return cardreferenceData.ReferenceID; } }

    public string Name { get { return StringTBL.GetData(referenceData.NameIndex); } }
    public string cutName { get { return referenceData.charCode; } }
    public int MaxLevl { get { return GradeDataTBL.GetData(Star).maxlv; } }
    public int DefaultStar {  get { return referenceData.star; } }
    public int MaxExp { get { return ExpCharTBL.GetData(Level).exp; } }
    public int MaxLevelExp { get { return ExpCharTBL.GetData( MaxLevl-1 ).exp; } }
    public int OldMaxExp { get {return Level == 1 ?  0 :ExpCharTBL.GetData(Level-1).exp;}   }
    public int DefaultSkin { get { return referenceData.DefaultSkin; } }
    public LeaderBuff leaderBuff { get { return cardreferenceData.BuffType; } }
    public int leaderBuffValue { get { return cardreferenceData.BuffValue; } }
    public bool bBest { get { return cardreferenceData.best== 1 ? true :false ; } }
    public int CharacterID { get { return referenceData.ReferenceID; } }
    //! 서버에서 받는 데이터
    public bool[] IsUseDeck = new bool[DeckManager.DeckMaxCount];

    public long CardKey { get; private set; }
    public int Exp = 500;
    public int Level = 1;
    
    public int Star = 1;
    public int Limit = 0;
    public bool represent = false; //!대표캐릭터

    public bool[] Leader = new bool[5];

    public bool[] SubLeader = new bool[5];

    public bool Lock = false;

    public int Skin = 1;

    public bool bNewCard = false;
    public SkinData[] skindata = new SkinData[10];

    public void ClearLeader()
    {
        for( int i = 0 ; i < Leader.Length ; i++ )
        {
            Leader[ i ] = false;
            SubLeader[ i ] = false;
        }

    }

    public bool IsLeader()
    {
        for( int i=0 ; i < Leader.Length ; i++ )
        {
            if( Leader[ i ] == true )
                return true;
        }

        return false;
    }

    public bool IsSubLeader()
    {
        for( int i = 0 ; i < SubLeader.Length ; i++ )
        {
            if( SubLeader[ i ] == true )
                return true;
        }

        return false;
    }


    public bool IsSkin( int index)
    {
        for( int i =0 ; i < skindata.Length ; i++ )
        {   
            if( skindata[i] != null )
            {
                if( skindata[ i ].index == index )
                {
                    if( skindata[ i ].index == DefaultSkin )
                        return true;

                    if( InventoryManager.I.IsSkin( skindata[ i ].index ) )
                    {
                        return true;
                    }
                }
            }            
        }
        return false;
    }

    public int GetSkinValue( ABILITYTYPE type )
    {
        int value = 0;
        for( int i = 0 ; i < skindata.Length ; i++ )
        {
            if( skindata[ i ] != null )
            {
                if( skindata[ i ].index == DefaultSkin )
                    continue;

                if( InventoryManager.I.IsSkin( skindata[ i ].index ) )
                {
                    if( skindata[ i ].Skin.ablityType == type )
                        value += skindata[ i ].Skin.ablityValue;

                }
               
            }
        }

        
        return value;
    }

    public CardData( int index , long key , int star = 1)
    {
        Star = star;
        Init(index, key);
    }

    public void Init(int index, long key )
    {
        ApplyData(index, key);

        SelectSkin(referenceData.DefaultSkin); //!임시로 기본스킨
    }

    public CardData()
    {
    }

    public void Copy( CardData data )
    {
        CardKey = data.CardKey;
        Exp = data.Exp;
        Level = data.Level;
        Skin = data.Skin;
        Star = data.Star;
        represent = data.represent;

        for( int i=0 ; i < 5 ; i++ )
        {
            Leader[i] = data.Leader[i];
            SubLeader[i] = data.SubLeader[i];
        }

        SetLimit(data.Limit);        
    }

    public void AddSkin( int skin )
    {
        SelectSkin( skin );
    }
    public void SelectSkin( int skin )
    {
        if (skin == 0)
            return;

        Skin = skin;
        CurrentSkin = SkinTBL.GetData(skin);
    }
       

    public void SetLimit( int limit )
    {
        Limit = limit;
        limitData = LimitbreakTBL.GetData(Limit);
    }

    public void ApplyData( int index , long key , bool bUpdateSkin = true )
    {
        CardKey = key;
        cardreferenceData = CardTBL.GetData(index);
        referenceData = CharacterTBL.GetData(cardreferenceData.characterIndex);
        statreferenceData = StatTBL.GetData(index, Star);

        if( bUpdateSkin )
        {
            for( int i = 0 ; i < referenceData.SkinList.Length ; i++ )
            {
                if( skindata[ i ] == null )
                    skindata[ i ] = new SkinData();

                skindata[ i ].index = referenceData.SkinList[ i ];
                skindata[ i ].Skin = SkinTBL.GetData( skindata[ i ].index );
            }

            SelectSkin( skindata[ 0 ].index );
        }
    }

    public bool IsUse()
    {
        for( int i=0; i < IsUseDeck.Length; i++)
        {
            if (IsUseDeck[i] == true)
                return true;
        }

        return false;
    }

    public int GetTeamIndex()
    {
        for( int i = 0 ; i < IsUseDeck.Length ; i++ )
        {
            if( IsUseDeck[ i ] == true )
                return i+1;
        }

        return -1;
    }


    public bool IsUse( int index )
    {
        if (IsUseDeck[index] == true)
                return true;
       
        return false;
    }

    public void SetDeck( int index , bool bUse )
    {
        IsUseDeck[index] = bUse;
    }

    public bool IsMaxLevel()
    {
        return Level == MaxLevl ? true : false;
    }
    public bool IsPromotion()
    {
        if (GradeDataTBL.GetData(Star).materialCount == 0)
            return false;

        //if (Level != MaxLevl)
        //    return false;

        return true;
    }

    public bool IsLimit()
    {
        return LimitbreakTBL.MaxLimit == Limit;
    }
}

