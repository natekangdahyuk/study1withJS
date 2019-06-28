using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum MissionState
{
    Ready,
    Reward,
    Clear,
}

public class MissinData
{    
    public int count = 0;    
    public MissionState state = MissionState.Ready;
    public MissionReferenceData data = null;
}

public class MissionManager : Singleton<MissionManager>
{
    public List<MissinData> ItemList = new List<MissinData>();

    bool bNew = false;

    public MissionReferenceData GetMissionData( int type )
    {
        for( int i = 0 ; i < ItemList.Count ; i++ )
        {
            if( (int)ItemList[ i ].data.type == type )
            {
                return ItemList[ i ].data;
            }
        }
        return null;
    }

    public void Init()
    {
        List<MissionReferenceData> list = MissionTBL.GetData();

        for( int i = 0 ; i < list.Count ; i++ )
        {
            MissinData data = new MissinData();
            data.data = list[ i ];
            ItemList.Add( data );
        }
    }

    public bool IsNew()
    {       
        return bNew;
    }

    public void SetNew( bool New )
    {
        bNew = New;
    }

    public void Set( int id , int count , int needcount , bool breward )
    {
        for( int i = 0 ; i < ItemList.Count ; i++ )
        {
            if( (int)ItemList[ i ].data.type == id )
            {
                ItemList[ i ].count = count;
                if(breward)
                {
                    ItemList[ i ].state = MissionState.Clear;
                }
                else
                {
                    if( count >= needcount )
                    {
                        ItemList[ i ].state = MissionState.Reward;
                        SetNew( true );
                    }
                }
            }
        }
    }

    public void Clear( MissionReferenceData.MissionType type )
    {
        for( int i =0 ; i < ItemList.Count ; i++ )
        {
            if(ItemList[i].data.type == type )
            {
                if( ItemList[ i ].state != MissionState.Ready )
                    return;
                
                ItemList[ i ].count++;
                NetManager.SetDailyMission( (int)ItemList[ i ].data.type );                

                if( ItemList[ i ].data.count <= ItemList[ i ].count )
                {
                    ItemList[ i ].count = ItemList[ i ].data.count;                  

                    if( ItemList[ i ].state == MissionState.Ready )
                    {
                        ItemList[ i ].state = MissionState.Reward;
                        SetNew( true );


                    }
                }
                return;
            }
        }
    }

    public void OnReward( MissionReferenceData.MissionType type )
    {
        for( int i = 0 ; i < ItemList.Count ; i++ )
        {
            if( ItemList[ i ].data.type == type )
            {
                ItemList[ i ].state = MissionState.Clear;
                break;
            }
        }

        CheckAllClear();
    }

    public void CheckAllClear()
    {
        int count = 0;
        for( int i = 0 ; i < ItemList.Count ; i++ )
        {
            if( ItemList[ i ].state == MissionState.Clear )
            {
                count++;
            }
        }


        for( int i = 0 ; i < ItemList.Count ; i++ )
        {
            if( ItemList[ i ].data.type == MissionReferenceData.MissionType.All )
            {
                ItemList[ i ].count = count;

                if( ItemList[ i ].count >= ItemList.Count - 1 )
                    ItemList[ i ].count = ItemList.Count - 1;

                if( count == ItemList.Count - 1 )
                {
                    if( ItemList[ i ].state == MissionState.Ready )
                    {
                        ItemList[ i ].state = MissionState.Reward;
                        SetNew( true );
                    }

                }
                break;
            }
        }
    }
}
