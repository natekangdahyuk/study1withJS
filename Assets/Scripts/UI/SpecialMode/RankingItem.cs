using UnityEngine;
using UnityEngine.UI;


public class RankingItem : MonoBehaviour
{
    [SerializeField]
    Text textRank;

    [SerializeField]
    Text textpoint;

    [SerializeField]
    Text textName;

    [SerializeField]
    RawImage    image;

    public void Apply( int rank, string name, int point , RankModeType modeType )
    {
        textRank.text = rank.ToString();
        textpoint.text = point.ToString("n0");
        textName.text = name;

        if( modeType == RankModeType.Time2048)
            textpoint.text = UIUtil.GetTimeEx2( point );
        else
            textpoint.text = point.ToString( "n0" );

        if( rank == 1 )
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_1st" );
        else if( rank == 2 )
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_2nd" );
        else if( rank == 3 )
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_3rd" );
        else
            image.texture = ResourceManager.LoadTexture( "icon_rankview_medal_normal" );
    }
}