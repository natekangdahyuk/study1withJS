using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class RankingUI : baseUI
{
    [SerializeField]
    Toggle[] toggle;

    [SerializeField]
    GameObject[] toggle_OnText;

    [SerializeField]
    GameObject[] group;

    [SerializeField]
    Text RemainTime;

    [SerializeField]
    GameObject ContentRank;

    [SerializeField]
    GameObject ContentReward;

    [SerializeField]
    GameObjectPool ObjectPool;

    [SerializeField]
    GameObjectPool RewardObjectPool;

    [SerializeField]
    GameObject Bg_Rank;

    [SerializeField]
    GameObject Bg_Reward;

    [SerializeField]
    GameObject[] Text_Title;


    [SerializeField]
    Text Text_Name;

    [SerializeField]
    Text Text_Score;

    [SerializeField]
    Text Text_Number;

    [SerializeField]
    Button RewardBtn;

    public List<RankingItem> ItemList = new List<RankingItem>();

    public List<RankingRewardItem> RewardItemList = new List<RankingRewardItem>();
    bool Initrank = false;
    RankModeType ModeType;

    public void Awake()
    {
       
    }

    public override void Init()
    {
      
    }

    public void Apply( RankModeType modeType )
    {
        OnEnter();
        ModeType = modeType;
        TopbarUI topbar = (TopbarUI)GlobalUI.ShowUI( UI_TYPE.TopBarUI );
        topbar.ChangeScene( this );

        for( int i = 0 ; i < Text_Title.Length ; i++ )
            Text_Title[ i ].gameObject.SetActive( false );

        Text_Name.text = PlayerData.I.UserID;

        if( RankingManager.I.MaxRankPoint[ (int)modeType - 1 ] <= 0 )
            Text_Score.text = "-";
        else
        {
            if( modeType == RankModeType.Time2048)
                Text_Score.text = UIUtil.GetTimeEx2( RankingManager.I.MaxRankPoint[ (int)modeType - 1 ] );
            else
                Text_Score.text = RankingManager.I.MaxRankPoint[ (int)modeType - 1 ].ToString( "n0" ) + " 점";
        }

        if( RankingManager.I.MyRanking[ (int)modeType-1 ] <= 0 )
            Text_Number.text = "-";
        else
            Text_Number.text = RankingManager.I.MyRanking[ (int)modeType-1 ].ToString( "n0" ) + " 위";
                
        for( int i =0 ; i < ItemList.Count ; i++ )
        {
            ObjectPool.Delete( ItemList[i].gameObject );            
        }
        ItemList.Clear();

        for( int i = 0 ; i < RewardItemList.Count ; i++ )
        {
            RewardObjectPool.Delete( RewardItemList[ i ].gameObject );
        }
        RewardItemList.Clear();


        RewardBtn.interactable = RankingManager.I.GetRankRewardInfo( (int)modeType-1 );

        if( Text_Title.Length >= (int)modeType )
            Text_Title[ (int)modeType - 1 ].gameObject.SetActive( true );

        SetRankList();
        SetReward();
        OnRankToggle( 0 );

        System.DayOfWeek value = System.DayOfWeek.Sunday;
        if( modeType == RankModeType.Mode2048 )
            value = System.DayOfWeek.Sunday;
        else if( modeType == RankModeType.TimeLimit )
            value = System.DayOfWeek.Saturday;
        else if( modeType == RankModeType.Time2048 )
            value = System.DayOfWeek.Thursday;
        else if( modeType == RankModeType.TimeDefence )
            value = System.DayOfWeek.Tuesday;

        RemainTime.text = UIUtil.GetTime( (int)value );

    }

    void SetRankList()
    {
        List<Rank> ranklist = RankingManager.I.rankinglist[ (int)ModeType - 1 ];

        for( int i = 0 ; i < ranklist.Count ; i++ )
        {
            RankingItem item = ObjectPool.New().GetComponent<RankingItem>();
            item.Apply( ranklist[ i ].rank , ranklist[ i ].Name , ranklist[ i ].point , ModeType );

            item.transform.SetParent( ContentRank.transform );
            item.gameObject.SetActive( true );
            ItemList.Add( item );
        }


        if( ranklist.Count <= 6 )
            ContentRank.GetComponent<RectTransform>().sizeDelta = new Vector2( 720 , 680 );
        else
            ContentRank.GetComponent<RectTransform>().sizeDelta = new Vector2( 720 , ranklist.Count * 99 );
    }
    void SetReward()
    {
        int count = 0;

        List<RankModeRewardReferenceData> list = RankModeRewardTBL.GetDataList();
        for( int i = 0 ; i < list.Count ; i++ )
        {
            if( list[ i ].rankType != (int)ModeType )
                continue;

            count++;
            RankingRewardItem item = RewardObjectPool.New().GetComponent<RankingRewardItem>();
            item.Apply( StringTBL.GetData( list[ i ].ranktext ) , list[ i ].rewardRuby , list[ i ].rewardGold , list[ i ].ReferenceID );

            item.transform.SetParent( ContentReward.transform );
            item.gameObject.SetActive( true );

            RewardItemList.Add( item );
        }

        if( count <= 6 )
            ContentReward.GetComponent<RectTransform>().sizeDelta = new Vector2( 720 , 680 );
        else
            ContentReward.GetComponent<RectTransform>().sizeDelta = new Vector2( 720 , count * 99 );
    }

    public void OnReward()
    {
        NetManager.SetRankReward( (int)ModeType );
        RewardBtn.interactable = false;

        RankingManager.I.SetRankRewardInfo( (int)ModeType , true );
        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );
    }

    public void OnRankToggle( int value )
    {
        for( int i = 0 ; i < toggle.Length ; i++ )
        {
            group[ i ].SetActive( toggle[ i ].isOn );
            toggle_OnText[ i ].SetActive( toggle[ i ].isOn );
        }

        Bg_Rank.gameObject.SetActive( false );
        Bg_Reward.gameObject.SetActive( false );

        if( toggle[ 0 ].isOn )
        {
            Bg_Rank.gameObject.SetActive( true );
        }
        else
            Bg_Reward.gameObject.SetActive( true );

        SoundManager.I.Play( SoundManager.SoundType.Effect , "snd_ui_common_button" , GameOption.EffectVoluem );

    }

    
}