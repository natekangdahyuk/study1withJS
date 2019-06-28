using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class StarInfo
{
    public int count;
    public int[] star = new int[3];

    public StarInfo(int num , int[] value )
    {
        count = num;
        star = value;
    }
}

public  class StageManager : Singleton<StageManager>
{
    public StageReferenceData ReferenceData;

    public int[] reachStageIndex = {100,10100,20100 }; //! 현재까지 클리어한 스테이지
    public int SelectStageIndex = 101; //! 게임시작 스테이지    

    
    public Dictionary<int, StarInfo> stageStarList = new Dictionary<int, StarInfo>();   
    public Dictionary<int, int> ThemaReward = new Dictionary<int, int>();

    public bool IsNextStage()
    {
        return StageTBL.GetData(SelectStageIndex + 1) == null ? false : true;
    }

    public void NextStage()
    {
        ApplyStage(++SelectStageIndex);
    }

    public int GetThemaReward( int index )
    {
        int value = 0;
        ThemaReward.TryGetValue( index , out value );

        return value;
    }


    public void AddThemaReward( int index, int value )
    {
        if( ThemaReward.ContainsKey( index ) )
            ThemaReward.Remove( index );

        ThemaReward.Add( index , value );
    }

    public void ApplyStage(int index )
    {
        SelectStageIndex = index;
        ReferenceData = StageTBL.GetData(index);
    }

    public void AddStageClearInfo( int stageIndex , int starCount , bool bLast , int[] star)
    {
        StageReferenceData stage = StageTBL.GetData( stageIndex );

        int[] starValue = new int[ 3 ];
        for( int i =0 ; i < 3 ; i++ )
        {
            for( int z= 0 ; z < 3 ; z++ )
            {
                if( stage.TaskType[ i ] == star[ z ] )
                    starValue[i] = star[ z ];
            }
        }
        

        stageStarList.Add(stageIndex, new StarInfo(starCount, starValue ) );

        
        if ( reachStageIndex[ stage.Difficulty - 1 ] < stageIndex)
            reachStageIndex[ stage.Difficulty -1 ] = stageIndex;

        if (bLast)
        {
            ApplyStage(stageIndex);
        }
    }

    public void ClearStage( int comboCount , int hitCount, int time , int bit)
    {
        int reward = 0;
        StarInfo starInfo = CheckStar(comboCount, hitCount , time , bit , out reward );

        NetManager.SetStage(SelectStageIndex, (byte)starInfo.count);

        string task = "";
        for( int i = 0 ; i < starInfo.star.Length ; i++ )
        {
            task += starInfo.star[ i ].ToString();

            if(i < 2)
                task +="|";
        }

        StageReferenceData stage = StageTBL.GetData( SelectStageIndex );


        NetReceive.CurrentRewardType = stage.StarRewardType;
        NetReceive.CurrentRewardValue = stage.StarRewardValue * reward;
        NetManager.SetStageReward(SelectStageIndex, ReferenceData.GoldReward , ReferenceData.UserExpReward, CardTBL.GetDataByCharacterID( stage.CharRewardList[ 0 ] ).ReferenceID , task );


        if ( reachStageIndex[ stage.Difficulty - 1 ] <= SelectStageIndex)
        {
            reachStageIndex[ stage.Difficulty - 1 ] = SelectStageIndex;
        }        
    }

    public int GetStar( int stageIndex )
    {
        StarInfo value;
        stageStarList.TryGetValue(stageIndex, out value);

        if (value == null)
            return 0;

        return value.count;
    }

    public int GetCurrentStar()
    {
        return GetStar(ReferenceData.ReferenceID);
    }

    public bool GetStar( int stageIndex, int number )
    {
        StarInfo value;
        if( stageStarList.TryGetValue( stageIndex , out value ) == false )
            return false;


        if( value.star[ number ] > 0 )
            return true;
        return false;
    }

    public int GetThemaStar( int thema , int diff )
    {
        List<StageReferenceData> stagelist = StageTBL.GetSubStageList( thema , diff );
        StarInfo value;
        int star = 0;
        for( int i =0 ; i < stagelist.Count ; i++ )
        {
            stageStarList.TryGetValue( stagelist[i].ReferenceID , out value );
            if( value != null )
                star += value.count;
        }

        return star;
    }

    public MonsterReferenceData GetBossData( int monsterIndex )
    {
        return MonsterTBL.GetData(monsterIndex);        
    }

    public MonsterDetailReferenceData GetBossDetailData()
    {
        return MonsterDetailTBL.GetData(ReferenceData.MonsterIndex);
    }


    public StageReferenceData GetData()
    {
        return ReferenceData;
    }

    public ThemaReferenceData GetThemaData()
    {
        return ThemaTBL.GetDataByDifficulty(ReferenceData.ThemaIndex , ReferenceData.Difficulty);
    }

    public StarInfo CheckStar(int comboCount , int hitCount , int time , int bit , out int reward )
    {
        reward = 0;
        StarInfo starInfo;
        stageStarList.TryGetValue(SelectStageIndex, out starInfo);

        if(starInfo == null )
        {
            starInfo = new StarInfo(0, new int[3]);

            stageStarList.Add(SelectStageIndex, starInfo);
        }

        int currentcount = starInfo.count;


        for ( int i =0; i < ReferenceData.TaskType.Length; i++ )
        {
            switch (ReferenceData.TaskType[i])
            {
                case 1:
                    {
                        if( ReferenceData.TaskValue[ i ] > bit )
                        {
                            if( CheckStar( starInfo,1 ) )
                            {
                                starInfo.star[ i ] = 1;
                            }                 
                        }
                    }
                break;

                case 2:
                    {
                        if (CheckAttribute(ReferenceData.TaskValue[i]))
                        {
                            if( CheckStar( starInfo , 2 ) )
                            {
                                starInfo.star[ i ] = 2;
                            }

                        }
                            
                    }
                    break;

                case 3:
                    {
                        if (CheckAttributeOut(ReferenceData.TaskValue[i]))
                        {
                            if( CheckStar( starInfo , 3 ) )
                            {
                                starInfo.star[ i ] = 3;
                            }
                        }
                         
                    }
                    break;

                case 4:
                    {
                        if (CheckClass(ReferenceData.TaskValue[i]))
                        {
                            if( CheckStar( starInfo , 4 ) )
                            {
                                starInfo.star[ i ] = 4;
                                
                            }

                        }
                            
                    }
                    break;

                case 5:
                    {
                        if (CheckClassOut(ReferenceData.TaskValue[i]))
                        {
                            if( CheckStar( starInfo , 5 ) )
                            {
                                starInfo.star[ i ] = 5;
                                
                            }

                        }
                            
                    }
                    break;

                case 6:
                    {
                        if(ReferenceData.TaskValue[i] <= comboCount)
                        {
                            if( CheckStar( starInfo , 6 ) )
                            {
                                starInfo.star[ i ] = 6;
                                
                            }
                        }                 
                            
                    }
                    break;
                case 7:
                    {
                        if( ReferenceData.TaskValue[ i ] <= hitCount )
                        {
                            if( CheckStar( starInfo , 7 ) )
                            {
                                starInfo.star[ i ] = 7;
                                
                            }

                        }

                    }
                    break;
                case 8:
                    {
                        if( ReferenceData.TaskValue[ i ] >= time )
                        {
                            if( CheckStar( starInfo , 8 ) )
                            {
                                starInfo.star[ i ] = 8;
                                
                            }

                        }

                    }
                    break;
            }
        }

        
        starInfo.count = 0;
        for ( int i =0; i < starInfo.star.Length; i++)
        {
            if (starInfo.star[i] >= 1)
                starInfo.count++;
        }

        if( currentcount != 3 && starInfo.count == 3)
            reward = 1;

        return starInfo;
    }

    bool CheckStar( StarInfo starInfo , int index )
    {
        for( int i =0 ; i < starInfo.star.Length ; i++ )
        {
            if( starInfo.star[ i ] == index )
                return false;
        }
        return true;
    }
    bool CheckClassOut(int value)
    {
        Dictionary<int, CardData> decklist = DeckManager.I.GetCurrentDeck();

        foreach (var card in decklist)
        {
            if ((int)card.Value.Class == value)
                return false;
        }
        return true;
    }

    bool CheckClass(int value)
    {
        Dictionary<int, CardData> decklist = DeckManager.I.GetCurrentDeck();

        foreach (var card in decklist)
        {
            if ((int)card.Value.Class != value)
                return false;
        }
        return true;
    }


    bool CheckAttributeOut(int value)
    {
        Dictionary<int, CardData> decklist = DeckManager.I.GetCurrentDeck();

        foreach (var card in decklist)
        {
            if ((int)card.Value.property == value)
                return false;
        }
        return true;
    }

    bool CheckAttribute( int value )
    {
        Dictionary<int, CardData> decklist = DeckManager.I.GetCurrentDeck();

        foreach (var card in decklist)
        {
            if ((int)card.Value.property != value)
                return false;
        }
        return true;
    }

    bool CheckBit( int value)
    {
        Dictionary<int, CardData> decklist = DeckManager.I.GetCurrentDeck();

        foreach( var card in decklist)
        {
            if (card.Value.bit >= value)
                return false;
        }

        return true;
    }


}
