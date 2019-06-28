using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using Game;
using Control;

public class AttackData
{    
    public int AttackCount = 0;    
    public List<int> attacker = new List<int>();
}

public class AttackEvent : Singleton<AttackEvent>
{
    //public List<int> attacker = new List<int>();

    public Dictionary<int,AttackData> attackerGroup = new Dictionary<int,AttackData>();
        
   
    public int ComboCount { get; private set; }

    int currentTurn;
    public Action<int,int,int,Vector3> attackCall;
   
    public void GameStart()
    {
        ComboCount = 0;
        
        attackerGroup.Clear();
    }
    public void SetCombo(bool bCombo)
    {
        if (bCombo)
            ComboCount++;
        else
            ComboCount = 0;
    }

    public void AddAttackList( int TurnCount , Tile tile )
    {
        AttackData data;
        attackerGroup.TryGetValue(TurnCount, out data);
        currentTurn = TurnCount;
        if(data == null )
        {
            data = new AttackData();
            attackerGroup.Add(TurnCount, data);
        }

        data.attacker.Add(tile.index);
        data.AttackCount++;
        
    }

    public void Attack( CollisionInfo e )
    {
        AttackData data;
        attackerGroup.TryGetValue(e.Turn, out data);

        if (data == null)
        {
            return;
        }


        for ( int i =0; i < data.attacker.Count; i++)
        {
            if(data.attacker[i] == e.index)
            {
                data.attacker.Remove(data.attacker[i]);
                break;
            }
        }


        if( data.AttackCount > 1 )
        {
            GameScene.I.GameMgr.Attack( e.damage , data.AttackCount - data.attacker.Count , e.comboCount , e.pos , e.Turn , e.bCritical , data.AttackCount , e.property ,e.value , e.CharacterName );
        }
        else
            GameScene.I.GameMgr.Attack( e.damage , 0 , e.comboCount , e.pos , e.Turn , e.bCritical , 0 , e.property , e.value , e.CharacterName );
        

        if (data.attacker.Count == 0)
            attackerGroup.Remove(e.Turn);
    }
 
}

