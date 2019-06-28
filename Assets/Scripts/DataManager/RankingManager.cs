using UnityEngine;
using System;
using System.Collections.Generic;


public class Rank
{
    public int rank;
    public int point;
    public string Name;

    public Rank( int _rank , int _point  , string _Name )
    {
        rank = _rank;
        point = _point;
        Name = _Name;
    }
}

public class RankRewardInfo
{
    public int Type = 0;
    public bool IsReward = false;
}


public class RankingManager : Singleton<RankingManager>
{
    public int[] OldRankPoint = new int[4];
    public int[] MaxRankPoint = new int[4];
    public int[] MyRanking = new int[4];
    public int[] CurrentRankingPoint = new int[4];
    public int[] MyRankingPer = new int[4];
    public RankRewardInfo[] rewardInfo = new RankRewardInfo[4];

    public List< Rank >[] rankinglist = new List<Rank>[4];

    public void Init()
    {
        for( int i = 0 ; i < rankinglist.Length ; i++ )
            rankinglist[ i ] = new List<Rank>();
    }

    public void SetRankRewardInfo( int type , bool IsReward )
    {
        if( type == 0 )
        {
            for( int i = 0 ; i < rewardInfo.Length ; i++ )
            {
                if( rewardInfo[ i ] == null )
                    rewardInfo[ i ] = new RankRewardInfo();

                rewardInfo[ i ].Type = 0;                
            }
            return;
        }

        if( rewardInfo[ type - 1 ] == null )
            rewardInfo[ type - 1 ] = new RankRewardInfo();

        rewardInfo[ type - 1 ].Type = type;
        rewardInfo[ type - 1 ].IsReward = IsReward;
    }

    public bool GetRankRewardInfo( int type )
    {
        if( rewardInfo[ type ] == null )
            return false;

        if( rewardInfo[ type ].Type == 0 )
            return false;

        return !rewardInfo[ type ].IsReward;
    }


    public void Clear( int type )
    {
        rankinglist[ type - 1 ].Clear();
    }

    public void SetMyRankPoint( int type , int point , int Oldpoint )
    {
        OldRankPoint[ type - 1 ] = Oldpoint;

        if(type == 3 )
        {
            if( CurrentRankingPoint[ type - 1 ] > point )
                MaxRankPoint[ type - 1 ] = point;
        }
        else
        {
            if( CurrentRankingPoint[ type - 1 ] < point )
                MaxRankPoint[ type - 1 ] = point;
        }
        

        CurrentRankingPoint[ type - 1 ] = point;
    }

    public void MyRank( int type , int rank , int point  )
    {        
        MaxRankPoint[ type - 1 ] = point;
        MyRanking[ type - 1 ] = rank;
        //CurrentRankingPoint[ type - 1 ] = point;
    }

    public void Add( int type ,int rank , int point , string name )
    {
        rankinglist[ type - 1 ].Add( new Rank( rank , point , name ) );        
    }
        
}