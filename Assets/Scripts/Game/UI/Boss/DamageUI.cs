using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public partial class DamageUI : MonoBehaviour
{
    [SerializeField]
    GameObjectPool HitObjectPool;

    [SerializeField]
    GameObjectPool CriticalObjectPool;

    [SerializeField]
    GameObjectPool ComboObjectPool;

    [SerializeField]
    GameObjectPool MultiHitObjectPool;

    [SerializeField]
    GameObjectPool HealObjectPool;

    [SerializeField]
    TweenAlpha tween;

    [SerializeField]
    Text damageText;

    [SerializeField]
    Text HitCount;

    public Action HitEndCallback;

    public void ApplyHeal( int value )
    {
        HitEffect hit = HealObjectPool.New().GetComponent<HitEffect>();

        hit.transform.SetParent( this.transform );
        hit.transform.localPosition = new Vector3(192,-420,0);
        hit.PlayHeal( value );
        hit.endCall = HealEnd;

    }

    int CurrentTurn = 0;
    public void Apply( int damage , int hitCount , int comboCount , Vector3 pos , int turn , bool bCritical, int maxAttackerCount)
    {
        if( bCritical )
        {
            HitEffect critical = CriticalObjectPool.New().GetComponent<HitEffect>();

            critical.transform.SetParent( this.transform );
            critical.transform.localPosition = pos * 100f;
            critical.Play( damage );
            critical.endCall = CriticalEnd;
        }
        else
        {
            HitEffect hit = HitObjectPool.New().GetComponent<HitEffect>();

            hit.transform.SetParent( this.transform );
            hit.transform.localPosition = pos * 100f;
            hit.Play( damage );
            hit.endCall = HitEnd;
        }

        if (hitCount > 1)
        {
            if(hitCount == maxAttackerCount)
            {
                HitEffect Combo = MultiHitObjectPool.New().GetComponent<HitEffect>();
                Combo.transform.SetParent(this.transform);
                Combo.transform.localPosition = new Vector3(-380, 135, 0);


                int per = ComboTBL.GetDataHit(maxAttackerCount);

                if (per > 0)
                {
                    per += 100;
                    Combo.Play(maxAttackerCount, per);
                }
                else
                    Combo.Play(maxAttackerCount);

                Combo.endCall = MultiHitEnd;
            }
        }

        if(CurrentTurn != turn )
        {
            if (comboCount > 1)
            {
                HitEffect Combo = ComboObjectPool.New().GetComponent<HitEffect>();
                Combo.transform.SetParent(this.transform);
                Combo.transform.localPosition = new Vector3(-380, 238, 0);


                int per = ComboTBL.GetDataCombo(comboCount);

                if (per > 0)
                {
                    per += 100;
                    Combo.Play(comboCount, per);
                }
                else
                    Combo.Play(comboCount);
                Combo.endCall = ComboEnd;
            }
        }

        CurrentTurn = turn;
        //CriticalGroup.SetActive(criticalCount > 0 ? true : false);
        //CriticalCount.text = criticalCount.ToString();
        //tween.Play();
    }

    public void HealEnd( GameObject go )
    {
        HealObjectPool.Delete( go );
    }

    public void CriticalEnd( GameObject go )
    {
        CriticalObjectPool.Delete( go );
    }
    public void HitEnd( GameObject go )
    {
        HitObjectPool.Delete(go);
    }
    public void ComboEnd(GameObject go)
    {
        ComboObjectPool.Delete(go);
    }

    public void MultiHitEnd( GameObject go )
    {
        MultiHitObjectPool.Delete( go );
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}

public partial class DamageUI : MonoBehaviour
{
    [SerializeField]
    GameObject CriticalGroup;

    [SerializeField]
    Text CriticalCount;
}

public partial class DamageUI : MonoBehaviour
{
    [SerializeField]
    GameObject ComboGroup;


    [SerializeField]
    Text ComboCount;
}