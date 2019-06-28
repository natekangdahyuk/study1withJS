using UnityEngine;
using UnityEngine.UI;

public class debuff : MonoBehaviour
{
    [SerializeField]
    Text Text_Count;

    Animation debuffAnim;

    void Awkae()
    {
        debuffAnim = GetComponent<Animation>();
    }

    void OnEnable()
    {
        if(debuffAnim == null )
            debuffAnim = GetComponent<Animation>();

        
    }

    public void Play()
    {
        gameObject.SetActive(true);
        debuffAnim.CrossFade("ani_card_debuff_start", 0);
        Invoke("progress", 1f);
    }
    public void End()
    {
        debuffAnim.CrossFade("ani_card_debuff_clear" ,0);
    }

    public void Burst()
    {
        debuffAnim.CrossFade("ani_card_debuff_burst", 0);
    }

    public void progress()
    {
        if(debuffAnim.IsPlaying ("ani_card_debuff_start"))
        {
            debuffAnim.CrossFade("ani_card_debuff_progress", 0);
        }
    }

    public void SetCount( int count)
    {
        Text_Count.text = count.ToString();
    }
}

