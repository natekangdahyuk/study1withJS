using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TileObject : MonoBehaviour
    {
        Image TileImage;        
        TweenAlpha tweenAlpha;

        debuff[] DebuffImage = new debuff[2];
        
        public Tile Curtile { set; get; } //private
        public TileDebuff debuff = new TileDebuff();
        public RFX1_DeactivateByTime CombineEffect;
        public int CurrentTileValue { get { return Curtile == null ? 0 : Curtile.value; } }

        
        void Awake()
        {
            TileImage = GetComponent<Image>();
            tweenAlpha = GetComponent<TweenAlpha>();
         
            debuff.EndDebuff = EndDebuff;
            CombineEffect = ResourceManager.Load(gameObject, "pref_fx_useitem_combine").GetComponent< RFX1_DeactivateByTime>();
            CombineEffect.transform.position = Vector3.zero;
            CombineEffect.gameObject.SetActive(false);
        }
        public void HaveCard(Tile tile)
        {
            if (Curtile != null)
                Debug.LogError("SetTile error");

            tweenAlpha.Play(0, 1);
            Curtile = tile;
          
        }
                
        public bool IsDebuffAble( ActionType debuffType , int[] debuffValue )
        {
            if (debuff.tileDebuffState != ActionType.None)
                return false;

            if(debuffType == ActionType.trap /*|| debuffType == ActionType.barrier*/ )
            {
                if( Curtile != null )
                    return false;

                return debuff.tileDebuffState == ActionType.None ? true : false;
            }
            else if(debuffType == ActionType.cardDestroy )
            {
                if( Curtile == null )
                    return false;

                bool bDebuff = false;
                for( int i =0 ; i < debuffValue.Length ; i++ )
                {
                    if( Curtile.value == debuffValue[i])
                    {
                        bDebuff = true;
                    }
                }
                if( bDebuff == false )
                    return false;
            }
            else if( debuffType == ActionType.CardDestroy2 )
            {
                if( Curtile == null )
                    return false;

                bool bDebuff = false;
                for( int i = 0 ; i < debuffValue.Length ; i++ )
                {
                    if( Curtile.value == debuffValue[ i ] )
                    {
                        bDebuff = true;
                    }
                }
                if( bDebuff == false )
                    return false;
            }

            if (Curtile == null)
                return false;

             return Curtile.IsDebuffAble();                    
          
        }

        public bool SetDebuff( MonsterActionReferenceData debuffData )
        {
            switch (debuffData.actionType)
            {
                case ActionType.cardDestroy:                
                case ActionType.CardLock:
                case ActionType.CardDestroy2:
                Curtile.SetDebuff(debuffData);
                    break;

                case ActionType.barrier:
                debuff.tileDebuffState = debuffData.actionType;
                debuff.DebuffValue = debuffData.DebuffTurn;
                //DebuffImage[ 0 ].gameObject.SetActive( true );
                break;

                case ActionType.trap:
                debuff.tileDebuffState = debuffData.actionType;
                debuff.DebuffValue = debuffData.DebuffTurn;
                //DebuffImage[ 1 ].gameObject.SetActive( true );
                break;
            }

            return true;
        }

        public void EndDebuff()
        {
            if( ActionType.trap == debuff.tileDebuffState )
            {
                //DebuffImage[ 1 ].gameObject.SetActive( false );
            }
            else if( ActionType.barrier == debuff.tileDebuffState )
            {
                if( Curtile != null )
                    Curtile.tileset.DeleteTile( Curtile.TilePos.x , Curtile.TilePos.y );
                
                //DebuffImage[ 0 ].gameObject.SetActive( false );
            }
        }

        public void TurnEnd()
        {
            if( Curtile!= null )
                Curtile.TurnEnd();

            debuff.ReduceDebuffTime();
        }
        public bool DeleteCard( Tile tile )
        {
            if (tile == Curtile)
            {
                if (tile.bForceCombine)
                {
                    CombineEffect.transform.position = tile.transform.position;
                    CombineEffect.gameObject.SetActive(true);
                }

                tweenAlpha.Play(1, 0);
                Curtile = null;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            debuff.Init();
            Curtile = null;
            //DebuffImage[ 0 ].gameObject.SetActive( false );
            //DebuffImage[ 1 ].gameObject.SetActive( false );
        }
                
    }
}