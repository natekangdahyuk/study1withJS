using UnityEngine;
using UnityEngine.UI;


public class MailItem : MonoBehaviour
{
    public Text title;
    public Text reward;
    public RawImage reardImage;
    public Text type;

    public MailData maildata;

    public void Apply( MailData data )
    {
        maildata = data;
        

        if(data.ItemType == MailData.MailRewardType.mileage)
            reward.text = data.value.ToString( "F1" );
        else
            reward.text = data.value.ToString( "n0" );

        if( data.bExpire == false )
            type.text = StringTBL.GetData(902036);
        else
        {
            type.text = data.ExpireTime.ToString();
        }

        if( data.ItemType == MailData.MailRewardType.gold )
            reardImage.texture = ResourceManager.LoadTexture( "icon_topbar_gold" );

        else if( data.ItemType == MailData.MailRewardType.ap )
            reardImage.texture = ResourceManager.LoadTexture( "icon_topbar_power" );

        else if( data.ItemType == MailData.MailRewardType.ruby || data.ItemType == MailData.MailRewardType.ruby2 )
            reardImage.texture = ResourceManager.LoadTexture( "icon_topbar_ruby" );

        else if( data.ItemType == MailData.MailRewardType.stone )
            reardImage.texture = ResourceManager.LoadTexture( "icon_topbar_summonstone" );

        else if( data.ItemType == MailData.MailRewardType.mileage )
            reardImage.texture = ResourceManager.LoadTexture( "icon_summon_mileagestone" );

        title.text = data.Title;
    }

    public void OnReceive()
    {
        NetManager.ReceiveMail( maildata.UID );
    }
}