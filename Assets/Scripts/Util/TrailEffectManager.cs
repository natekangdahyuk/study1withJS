using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Game.Helper;
using Control;

public class TrailEffectManager : MonoSinglton<TrailEffectManager>
{
    public Dictionary<string,GameObjectPool> effectlist = new Dictionary<string, GameObjectPool>();



    public override void ClearAll()
    {

    }

    public void CreateEffect( PROPERTY property )
    {
        string name = "";
        switch( property )
        {

            case PROPERTY.FIRE:
            name = "pref_fx_char_attack_fire";
            break;

            case PROPERTY.WATER:
            name = "pref_fx_char_attack_ice";
            break;

            case PROPERTY.WIND:
            name = "pref_fx_char_attack_wind";
            break;

            case PROPERTY.WHITE:
            name = "pref_fx_char_attack_light";
            break;

            case PROPERTY.BLACK:
            name = "pref_fx_char_attack_dark";
            break;

            case PROPERTY.HEAL:
            name = "pref_fx_char_heal_green";
            break;
        }

        CreateEffect( name );
    }

    public void CreateEffect( string name )
    {
        if( effectlist.ContainsKey( name ) )
        {
            return;
        }

        GameObject pool = new GameObject( "[TilePool " + name + "]" );
        GameObjectPool tilepool = GameObjectPool.MakeComponent( pool , ResourceManager.LoadSrc( name ) , 4 , 2 );

        effectlist.Add( name , tilepool );
    }

    public void Delete( string name , GameObject go )
    {
        GameObjectPool tilepool;
        if( effectlist.TryGetValue( name , out tilepool ) == false )
        {
            return;
        }
        tilepool.Delete( go );
    }

    public void Play( PROPERTY property , int damage, int index , Vector3 StartPos , Vector3 EndPos , int turn , Action<CollisionInfo> call , bool bCritical , int comboCount , int value, string CharacterName = "" )
    {
        string name = "";
                
        switch( property )
        {
            case PROPERTY.FIRE:
            name = "pref_fx_char_attack_fire";        
            break;

            case PROPERTY.WATER:
            name = "pref_fx_char_attack_ice";            
            break;

            case PROPERTY.WIND:
            name = "pref_fx_char_attack_wind";            
            break;

            case PROPERTY.WHITE:
            name = "pref_fx_char_attack_light";            
            break;

            case PROPERTY.BLACK:
            name = "pref_fx_char_attack_dark";            
            break;

            case PROPERTY.HEAL:
            name = "pref_fx_char_heal_green";
            break;
        }

        Play( name , damage , index , StartPos , EndPos , turn, call , comboCount, bCritical, property ,value, CharacterName );
    }

    public void Play( string name , int damage , int index , Vector3 StartPos , Vector3 EndPos , int turn , Action<CollisionInfo> call, int ComboCount ,bool bCritical , PROPERTY property, int value , string CharacterName )
    {

        RandomTrailEffect Effect = null;
        GameObjectPool tilepool;
        if( effectlist.TryGetValue( name , out tilepool ) == false )
        {
            return;
        }

        Effect = tilepool.New().GetComponent<RandomTrailEffect>();
        Effect.transform.parent = null;
        Effect.collisionInfo.damage = damage;
        Effect.collisionInfo.index = index;
        Effect.collisionInfo.Turn = turn;
        Effect.collisionInfo.comboCount = ComboCount;
        Effect.collisionInfo.bCritical = bCritical;
        Effect.collisionInfo.property = property;
        Effect.collisionInfo.value = value;
        Effect.collisionInfo.CharacterName = CharacterName;

        Effect.Name = name;
        Effect.call = call; 
        Effect.Play( StartPos , EndPos );
    }
}