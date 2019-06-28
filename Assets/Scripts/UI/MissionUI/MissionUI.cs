using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : baseUI
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    GameObject contentAchievement;

    [SerializeField]
    GameObjectPool ObjectPool;

    [SerializeField]
    GameObjectPool AchievementObjectPool;

    public Toggle MissionToggle;

    List<MissionItem> missionlist = new List<MissionItem>();

    List<AchievementItem> AchievementList = new List<AchievementItem>();

    RectTransform rectT = null ;
    void Awake()
    {
        gameObject.SetActive( false );
    }

    public override void Init()
    {
        RectTransform form = GetComponent<RectTransform>();
        form.anchoredPosition = Vector2.zero;
        form.sizeDelta = Vector2.zero;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        RefreshMission();
        OnRefreshAchieve();
        MissionManager.I.SetNew( false );
        AchievementManager.I.SetNew( false );
    }

    public void RefreshMission()
    {
        for( int i = 0 ; i < MissionManager.I.ItemList.Count ; i++ )
        {
            missionlist[ i ].Apply( MissionManager.I.ItemList[ i ] );
        }
    }

    public void OnRefreshAchieve()
    {
        if( gameObject.activeSelf == false )
            return;

        int index = 0;
        foreach( KeyValuePair<AchievementReferenceData.MissionType , List<AchievementData>> value in AchievementManager.I.ItemList )
        {
            for( int i = 0 ; i < value.Value.Count ; i++ )
            {
                if( value.Value[ i ].state != AchievementState.Clear )
                {
                    AchievementList[index].Apply( value.Value[ i ] );
                    
                    break;
                }

                AchievementList[ index ].Apply( value.Value[ i ] );
            }
            index++;
        }

            
    }


    public void OnRefresh()
    {
        
        ApplyMission();
        ApplyAchievement();

        if( MissionToggle.isOn )
            OnMisison();
        else
            OnAchievement();

        OnEnter();
    }


    public void Apply()
    {
        gameObject.SetActive( true );
        if( rectT == null )
            rectT = content.GetComponent<RectTransform>();
        
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );
        OnRefresh();
    }

    public void ApplyMission()
    {
        if( missionlist.Count == 0 )
        {
            for( int i = 0 ; i < MissionManager.I.ItemList.Count ; i++ )
            {
                GameObject go = ObjectPool.New();
                go.SetActive( true );
                go.transform.SetParent( content.transform );
                MissionItem item = go.GetComponent<MissionItem>();
                item.Apply( MissionManager.I.ItemList[ i ] );
                missionlist.Add( item );
            }

            rectT.sizeDelta = new Vector2( rectT.sizeDelta.x , MissionManager.I.ItemList.Count * 160 );
        }
    }

    public void ApplyAchievement()
    {
        //return;

        if( AchievementList.Count == 0 )
        {            
            foreach(KeyValuePair<AchievementReferenceData.MissionType , List<AchievementData>> value in AchievementManager.I.ItemList )
            {
                int index = value.Value.Count - 1;
                for( int i=0 ; i < value.Value.Count ; i++ )
                {
                    if( value.Value[ i ].state != AchievementState.Clear )
                    {
                        index = i;                        
                        break;
                    }                
                }

                AchievementItem item2 = GetNewItem();
                item2.Apply(value.Value[index]);
                AchievementList.Add(item2);

            }
            
            rectT.sizeDelta = new Vector2( rectT.sizeDelta.x , MissionManager.I.ItemList.Count * 160 );
        }
    }

    AchievementItem GetNewItem()
    {
        GameObject go = AchievementObjectPool.New();
        go.SetActive(true);
        go.transform.SetParent(contentAchievement.transform);

        return go.GetComponent<AchievementItem>();
    }

    public void OnMisison()
    {
        content.gameObject.SetActive( true );
        contentAchievement.gameObject.SetActive( false );


    }

    public void OnAchievement()
    {
        content.gameObject.SetActive( false );
        contentAchievement.gameObject.SetActive( true );
    }

 
}

