using UnityEngine;
using UnityEngine.UI;


public class RankingRewardItem : MonoBehaviour
{
    [SerializeField]
    Text textRank;

    [SerializeField]
    Text textReward;

    [SerializeField]
    Text textReward2;

    [SerializeField]
    RawImage    image;

    public void Apply( string rank , int point , int point2 , int index )
    {
        textRank.text = rank;
        textReward.text = point.ToString( "n0" );
        textReward2.text = point2.ToString( "n0" );

        image.gameObject.SetActive( true );

        if( index %100 == 1 )
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_1st" );
        else if( index % 100 == 2 )
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_2nd" );
        else if( index % 100 == 3 )
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_3rd" );
        else
            image.gameObject.SetActive( false );


    }
}