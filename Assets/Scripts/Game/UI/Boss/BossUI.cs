using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BossUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] BossSkillGroup;

    [SerializeField]
    RawImage[] BossSkillIcon;

    [SerializeField]
    Text[] BossSkillText;

    [SerializeField]
    DamageUI damageUI;

    [SerializeField]
    Image HpImage;

    [SerializeField]
    RawImage Property;

    [SerializeField]
    Text Text_HP;

    [SerializeField]
    Text Text_Name;

    [SerializeField]
    Text Text_Level;

    Boss boss;

    Animation[] BossSkillGroupAnim = new Animation[5];

    void Awake()
    {
        for (int i = 0; i < BossSkillGroup.Length; i++)
        {
            BossSkillGroupAnim[i] = BossSkillGroup[i].GetComponent<Animation>();

        }

       
    }

    public void ApplyInfo( Boss _boss)
    {
        boss = _boss;
        Text_Name.text = "<color=#ffc652>Lv. " + boss.detailData.Level.ToString() + "</color> " +StringTBL.GetData(boss.detailData.Name);

        for( int i =0; i < BossSkillGroup.Length ; i++)
        {
            if (i >= boss.AttackData.Count)
                BossSkillGroup[i].SetActive(false);
            else
            {
                BossSkillGroup[i].SetActive(true);
                BossSkillGroupAnim[i].Stop();
            }
        }

        for( int i = 0 ; i < boss.AttackData.Count ; i++ )
        {
            BossSkillIcon[ i ].texture = ResourceManager.LoadTexture( boss.AttackData[ i ].ActionData.ActionIcon );
            
        }

        for (int i = 0; i < boss.AttackData.Count; i++)
        {
            BossSkillText[i].text = boss.AttackData[i].Turn.ToString();
        }

        UIUtil.LoadProperty( Property , boss.detailData.property );
        ///Text_Level.text = "Lv. " +boss.detailData.Level.ToString();
    }

    public void TurnEnd()
    {
        for (int i = 0; i < boss.AttackData.Count; i++)
        {
            BossSkillText[i].text = boss.AttackData[i].Turn.ToString();

            if(boss.AttackData[i].Turn == 3 )
            {
                BossSkillGroupAnim[i].CrossFade("ani_ui_boss_attack_count", 1f);
            }           
            if (boss.AttackData[i].Turn == boss.AttackData[i].ActionData.turn+1)
            {
                //BossSkillIcon[i].color = new Color(255, 255, 255, 1);
                BossSkillGroupAnim[i].Stop("ani_ui_boss_attack_count");
                BossSkillGroupAnim[i].Play("ani_ui_boss_attack_active");
            }
        }
    }

    public void ApplyDamage( int damage , int hp , int maxHP , int count , int ComboCount, Vector3 pos ,int turn , bool bCritical, int maxAttackerCount)
    {
        damageUI.Apply(damage , count, ComboCount,  pos, turn, bCritical , maxAttackerCount);

        if( GameScene.modeType == ModeType.ModeDefault )
        {
            HpImage.fillAmount = (float)hp / maxHP;
            Text_HP.text = hp.ToString( "n0" ) + " / " + maxHP.ToString( "n0" );
        }
        else
        {
            Text_HP.text = "SCORE " + GameScene.Instance.GameMgr.TotalDamage.ToString("n0");
        }
        
    }

    public void SetMaxHp( int hp )
    {
        HpImage.fillAmount = 1;
        if( GameScene.modeType == ModeType.ModeDefault )
        {
            Text_HP.text = hp.ToString( "n0" ) + " / " + hp.ToString( "n0" );
        }
        else if(GameScene.modeType == ModeType.Time2048)
        {
            Text_HP.gameObject.SetActive( false );
        }
        else
        {
            Text_HP.text = "SCORE 0";
        }
            
    }
}

