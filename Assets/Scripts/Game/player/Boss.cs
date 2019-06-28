using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BossAttackData
{
    public int Turn = 0;    
    public MonsterActionReferenceData ActionData = null;

    public bool CheckAttack()
    {
        Turn--;

        if(Turn <= 0)
        {
            Turn = ActionData.turn+1;
            return true;
        }

        return false;
    }

    public void Init()
    {
        Turn = ActionData.turn + 1;
    }
}

public class Boss : MonoBehaviour
{
    int Turn = 0;
    public int Hp = 0;
    public int MaxHp = 0;
    
    public List<BossAttackData> AttackData = new List<BossAttackData>();

    MonsterReferenceData refernceData;
    public MonsterDetailReferenceData detailData;
    public Vector3 HitPos;
    public Vector3 FirePos;
    public float Radius = 0f;
    Animator animator;

    bool bDie= false;
    string[] snd = new string[6];

    public static Boss Create( MonsterDetailReferenceData detail , GameObject parent )
    {
        MonsterReferenceData data = StageManager.I.GetBossData( detail.MonsterIndex );

        ResourceManager.Load( parent , data.FieldPrefab );
        Boss boss = ResourceManager.Load<Boss>( parent , data.Prefab );
        boss.ApplyData( data , detail );

       

        return boss;
    }

    public float CheckPropertyValue( PROPERTY property )
    {
        if( property == PROPERTY.BLACK )
        {
            if( detailData.property == PROPERTY.WHITE )
                return 1.5f;
        }
        else if( property == PROPERTY.WHITE )
        {
            if( detailData.property == PROPERTY.BLACK )
                return 1.5f;
        }
        else if( property == PROPERTY.FIRE )
        {
            if( detailData.property == PROPERTY.WIND )
                return 1.5f;

            if( detailData.property == PROPERTY.WATER )
                return 0.75f;
        }

        else if( property == PROPERTY.WIND )
        {
            if( detailData.property == PROPERTY.WATER )
                return 1.5f;

            if( detailData.property == PROPERTY.FIRE )
                return 0.75f;
        }

        else if( property == PROPERTY.WATER )
        {
            if( detailData.property == PROPERTY.FIRE )
                return 1.5f;

            if( detailData.property == PROPERTY.WIND )
                return 0.75f;
        }




        return 1;
    }

    public Vector3 GetHitPos()
    {
        Vector3 pos = HitPos;
        pos.x += UnityEngine.Random.Range( -Radius , Radius );
        pos.y += UnityEngine.Random.Range( -Radius , Radius );
        return pos;
    }
    private void Awake()
    {
        HitPos = transform.Find( "FX_mon_hit" ).transform.position;
        Radius = transform.Find( "FX_mon_hit" ).GetComponent<SphereCollider>().radius;
        FirePos = transform.Find( "FX_mon_fire" ).transform.position;
    }
    public void SetDamage( int damage )
    {

        if( bDie )
            return;


        if( animator.GetBool( "mon_attack" ) )
            animator.SetBool( "mon_damage_layer" , true );
        else
            animator.SetBool( "mon_damage" , true );

        //animator.CrossFade("DAMAGE", 1f);
        //animator.CrossFade("DEATH", 1f);
        Hp = Math.Max( 0 , Hp - damage );

        if( Hp <= 0 )
        {

        }

        
    }

    public void Reset()
    {
        Turn = 0;
        Hp = MaxHp;
        currentAttackQueue.Clear();
        CancelInvoke();
        Invoke( "PlayIdleSound" , 0.5f );
    }
    public void GameStart()
    {
        Reset();
        bDie = false;

        for( int i = 0 ; i < AttackData.Count ; i++ )
            AttackData[ i ].Init();

        animator.Play( "IDLE" );
        
    }

    public void SetDeath()
    {
        animator.SetBool( "mon_death" , true );
        bDie = true;
        SoundManager.Instance.Stop( SoundManager.SoundType.Effect , snd[ 3 ] );
        SoundManager.Instance.Play( SoundManager.SoundType.Effect , snd[ 2 ] , GameOption.EffectVoluem , false );
    }

    public void OnEnter()
    {
        SoundManager.Instance.Play( SoundManager.SoundType.Effect , snd[ 4 ] , GameOption.EffectVoluem , false );
        
    }
    
    void PlayIdleSound()
    {
        SoundManager.Instance.Play( SoundManager.SoundType.Effect , snd[ 3 ] , GameOption.EffectVoluem , true );
    }

    public void ApplyData( MonsterReferenceData data , MonsterDetailReferenceData detail )
    {
        animator = GetComponentInChildren<Animator>();
        refernceData = data;
        detailData = detail;

        if( GameScene.bDebugMode )
            MaxHp = 10000;
        else
            MaxHp = detailData.Hp;

        Hp = MaxHp;
        AttackData.Clear();
        for( int i = 0 ; i < detail.MobAction.Length ; i++ )
        {			
            MonsterActionReferenceData action = MonsterActionTBL.GetData( detail.MobAction[ i ] );
            if( action == null )
				Debug.LogError("## monster load failed : " + detail.MobAction[i]);

			BossAttackData boss = new BossAttackData();
            boss.ActionData = action;
            boss.Turn = action.turn;

            TrailEffectManager.I.CreateEffect( boss.ActionData.AttackEffect );
            AttackData.Add( boss );
        }

        snd[ 0 ] = "snd_mon_" + data.EngName + "_" + "attack1";
        snd[ 1 ] = "snd_mon_" + data.EngName + "_" + "attack2";
        snd[ 2 ] = "snd_mon_" + data.EngName + "_" + "death";
        snd[ 3 ] = "snd_mon_" + data.EngName + "_" + "idle";
        snd[ 4 ] = "snd_mon_" + data.EngName + "_" + "income";
        snd[ 5 ] = "snd_fx_debuff_lock";

        //snd[ 6 ] = "snd_fx_debuff_bomb";
        //snd[ 7 ] = "snd_fx_debuff_death";

        for( int i=0 ; i < snd.Length ; i++ )
            SoundManager.New( SoundManager.SoundType.Effect , SoundManager.Instance , snd[ i ] );

        
    }

    public BossAttackData Attack()
    {
        Turn++;
        
        for( int i = 0 ; i < AttackData.Count ; i++ )
        {
            if( AttackData[ i ].CheckAttack() )
            {
                animator.SetBool( "mon_attack" , true );
                //currentAttack = AttackData[ i ];
                currentAttackQueue.Enqueue(AttackData[i]);
                Invoke("StartAttack" , 0.5f);

                return AttackData[ i ];
            }
        }


        return null;
    }

    public Queue<BossAttackData> currentAttackQueue = new Queue<BossAttackData>();

    //BossAttackData currentAttack;

    public void StartAttack()
    {
        BossAttackData currentAttack = currentAttackQueue.Dequeue();

        if ( currentAttack.ActionData.actionType == ActionType.Special || currentAttack.ActionData.actionType == ActionType.Default )
        {
            int damage = currentAttack.ActionData.AttValue;
            damage = (int)( damage * ( (float)damage / ( damage + Deck.Instance.defence ) ) );
            TrailEffectManager.I.Play( currentAttack.ActionData.AttackEffect , damage , Turn , FirePos , GetRandomAttackPos() , Turn , Attack , 0 , false , detailData.property , 0 ,"");

            if( currentAttack.ActionData.actionType == ActionType.Default )
                SoundManager.Instance.Play( SoundManager.SoundType.Effect , snd[ 0 ] , GameOption.EffectVoluem , false );
            else
            {
                SoundManager.Instance.Play( SoundManager.SoundType.Effect , snd[ 1 ] , GameOption.EffectVoluem , false );
            }
        }
        else
            SoundManager.Instance.Play( SoundManager.SoundType.Effect , snd[ 5 ] , GameOption.EffectVoluem , false );


        if( currentAttack.ActionData != null )
        {
            GameScene.I.GameMgr.tileMgr.SetDeBuff( currentAttack );
        }

    }


    public Vector3 GetRandomAttackPos()
    {
        Vector3 pos = Vector3.zero;
        pos.x += UnityEngine.Random.Range( -0.5f , 0.5f );
        pos.y += UnityEngine.Random.Range( -0.5f , 0.5f ) - 1.5f;
        return pos;
    }

    public void Attack( CollisionInfo e )
    {
        GameScene.I.GameMgr.BossAttack( e.damage  , e.pos );
    }

}