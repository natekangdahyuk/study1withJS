using UnityEngine;
using UnityEngine.UI;


 public class CardInfoGroup : baseUI
{
    [SerializeField]
    Text Hp;

    [SerializeField]
    Text Defence;

    [SerializeField]
    Text Attack;

    [SerializeField]
    Text heal;

    public override void Init()
    {

    }

    public void Apply(CardData card , bool bResult = false )
    {
       

        if( bResult )
        {
            Hp.color = new Color32( 244 , 47 , 0 , 255 );
            Defence.color = new Color32( 244 , 47 , 0 , 255 );
            Attack.color = new Color32( 244 , 47 , 0 , 255 );
            heal.color = new Color32( 244 , 47 , 0 , 255 );
        }
        else
        {
            Hp.color = new Color32( 221 , 176 , 27 , 255 );
            Defence.color = new Color32( 221 , 176 , 27 , 255 );
            Attack.color = new Color32( 221 , 176 , 27 , 255 );
            heal.color = new Color32( 221 , 176 , 27 , 255 );
        }
        

        if( card.TotalHp <= 0 )
            Hp.text = "-";
        else
            Hp.text = card.TotalHp.ToString();


        if( card.TotalDefence <= 0 )
            Defence.text = "-";
        else
            Defence.text = card.TotalDefence.ToString();

        if( card.Totaldamage <= 0 )
            Attack.text = "-";
        else
            Attack.text = card.Totaldamage.ToString();

        if( card.TotalHeal <= 0 )
            heal.text = "-";
        else
            heal.text = card.TotalHeal.ToString();
    }
}

