using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum AchievementState
{
    Ready,
    Reward,
    Clear,
}

public class AchievementData
{
    public long count = 0;
    public AchievementState state = AchievementState.Ready;
    public AchievementReferenceData data = null;
}

public class AchievementManager : Singleton<AchievementManager>
{
    public Dictionary<AchievementReferenceData.MissionType,List<AchievementData>> ItemList = new Dictionary<AchievementReferenceData.MissionType,List<AchievementData>>();
    bool bNew = false;

    public bool IsNew()
    {
        return bNew;
    }

    public void SetNew( bool New )
    {
        bNew = New;
    }

    public void Init()
    {
        List<AchievementReferenceData> list = AchievementTBL.GetData();

        for( int i = 0 ; i < list.Count ; i++ )
        {
            AchievementData data = new AchievementData();
            data.data = list[ i ];
            //data.count = 10;

            CheckState( data );
            List<AchievementData> datalist = Get( list[ i ].type , true );
            datalist.Add( data );
        }
    }

    List<AchievementData> Get( AchievementReferenceData.MissionType type , bool bNew = false )
    {
        List<AchievementData> list;
        if( ItemList.TryGetValue(type, out list ) == false )
        {
            if(bNew)
            {
                list = new List<AchievementData>();
                ItemList.Add(type, list);
            }            
        }

        return list;
    }

    public AchievementReferenceData Clear( AchievementReferenceData.MissionType type )
    {
        List<AchievementData> list = Get( type );

        if (list == null)
            return null;

        for ( int i=0 ; i < list.Count ; i++ )
        {
            if( list[ i ].state == AchievementState.Reward )
            {
                if( list[ i ].data.missionMark == 1 )
                    list[ i ].count = 0;

                list[ i ].state = AchievementState.Clear;
                return list[ i ].data;
            }
        }
        return null;
    
    }

    void CheckState( AchievementData Achieve )
    {
        if( Achieve.data.missionMark == 1 )
        {
            if( Achieve.count >= 1 )
            {
                Achieve.state = AchievementState.Reward;
            }
        }
        else
        {
            if( Achieve.count >= Achieve.data.missionValue )
            {
                Achieve.state = AchievementState.Reward;
            }
        }
    }

    public void Add( AchievementReferenceData.MissionType type , int num , int value )
    {
        List<AchievementData> list = Get( type );

        if (list == null)
            return;

        if( list.Count < num )
        {
            for( int i = 0 ; i < list.Count ; i++ )
            {
                list[ i ].state = AchievementState.Clear;
                if( list[ i ].data.missionMark != 1 )
                    list[ i ].count = list[ i ].data.missionValue;
                else
                    list[ i ].count = 1;
            }
            return;
        }

        for( int i=0 ; i < list.Count ; i++ )
        {
            if( list[ i ].data.missionMark != 1 )
                list[ i ].count = value;

            if(i+1 < num)
            {                
                list[ i ].state = AchievementState.Clear;
            }

        }
        list[ num - 1 ].count = value;

        if( list[ num - 1 ].data.missionMark == 1 )
        {
            if( list[ num - 1 ].count >= 1 )
            {
                if( list[ num - 1 ].state != AchievementState.Reward )
                    SetNew( true );

                list[ num - 1 ].state = AchievementState.Reward;             
            }
        }
        else
        {
            if( list[ num - 1 ].count >= list[ num - 1 ].data.missionValue )
            {
                if( list[ num - 1 ].state != AchievementState.Reward )
                    SetNew( true );

                list[ num - 1 ].state = AchievementState.Reward;                
            }
        }        
    }


}
