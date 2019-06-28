using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PlayerUI : MonoBehaviour
{
 
    [SerializeField]
    Image HpImage;

    [SerializeField]
    Text Text_HP;

    [SerializeField]
    DamageUI damageUI;

    [SerializeField]
    TweenAlpha tween;

    public void ApplyInfo()
    {

    }

    public void ApplyDamage(int damage , int hp , int maxHP , Vector3 pos )
    {        
        HpImage.fillAmount = (float)hp/ maxHP;
        Text_HP.text = hp.ToString("n0") + " / " + maxHP.ToString("n0");

        damageUI.Apply( damage , 0 , 0, pos , -1, false ,0);

        if( hp * 100 / maxHP <= 10 )
        {
            tween.Play();
        }
        else
        {
            tween.Stop();
            tween.Reset();
        }
    }

    public void SetHeal( int value , int hp , int maxHP )
    {
        HpImage.fillAmount = (float)hp / maxHP;
        Text_HP.text = hp.ToString( "n0" ) + " / " + maxHP.ToString( "n0" );

        damageUI.ApplyHeal( value );

        if( hp * 100 / maxHP <= 10 )
        {
            tween.Play();
        }
        else
        {
            tween.Stop();
            tween.Reset();
        }
    }

    public void SetMaxHp(int hp)
    {
        HpImage.fillAmount = 1;
        Text_HP.text = hp.ToString("n0") + " / " + hp.ToString("n0");

        tween.Stop();
        tween.Reset();

    }
}

