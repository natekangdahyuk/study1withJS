using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{


    public partial class Tile : MonoBehaviour
    {

        //!< 활성화된 타일들
        public static List<Tile> activeTiles = new List<Tile>();

        public static int TileIndex = 0;
        public static int MaxValue = 2048;
        private const float SPEED = 0.62f;

        public Card card;

        [SerializeField]
        private TweenPosition tweenPos;

        [SerializeField]
        private TweenScale tweenScale;

        //!< 현재 Tile이 합쳐질 대상        
        public Tile combineTarget { get; set; }
        public Transform moveTarget { get; set; }
        public bool isUpgrade { get; private set; }
        public bool IsMoving { get { return moveTarget == null ? false : true; } }
        public bool IsMoveAble { get { return debuff.tileDebuffState == ActionType.CardLock ? false : true; } }        

        public bool IsReserveDelete
        {
            get
            {
                if (isUpgrade || combineTarget != null)
                    return true;

                return false;
            }
        }
        public int value;

        public CardData cardData { get { return card.cardData; } }
        public Position TilePos= new Position(-1,-1);
        public Action<Tile, bool> DeletingCallback = null;        
        public TileSet tileset;
        bool bCreate = false;
        public int index =0;
        public int Turn;
        public int CurrentComboCount = 0;
        public GameObject RandomBitEffect;
        
        public bool bForceCombine = false;
        void Awake()
        {            
            card = ResourceManager.Load<Card>( this.gameObject , Helper.StringHelper.Append( "Card_ingame" ) );
            card.GetComponent<RectTransform>().pivot = new Vector2(0.5f,0f);
            card.transform.localPosition = Vector3.zero;            
            card.transform.localScale = new Vector3( 0.01f , 0.01f , 0.01f );                        
            tweenPos.onFinish.Add(TweenPositionEnd);

            if( debuffImage[ 0 ] == null )
            {
                debuffImage[ 0 ] = ResourceManager.Load( gameObject , "pref_card_debuff_lock" ).GetComponent<debuff>();
                debuffImage[ 0 ].gameObject.SetActive( false );
                debuffImage[ 1 ] = ResourceManager.Load( gameObject , "pref_card_debuff_death" ).GetComponent<debuff>();
                debuffImage[ 1 ].gameObject.SetActive( false );

                debuffImage[ 2 ] = ResourceManager.Load( gameObject , "pref_card_debuff_bomb" ).GetComponent<debuff>();
                debuffImage[ 2 ].gameObject.SetActive( false );
            }

            if(debuffEndImage[0] == null )
            {
                debuffEndImage[0] = ResourceManager.Load(gameObject, "pref_fx_debuff_death_burst").gameObject;
                debuffEndImage[0].gameObject.SetActive(false);
                debuffEndImage[1] = ResourceManager.Load(gameObject, "pref_fx_debuff_bomb_burst").gameObject;
                debuffEndImage[1].gameObject.SetActive(false);
            }

            RandomBitEffect = ResourceManager.Load(gameObject, "pref_fx_useitem_upgrade");
            RandomBitEffect.transform.position = Vector3.zero;
            RandomBitEffect.SetActive(false);

          
        }

        private void OnEnable()
        {
            activeTiles.Add(this);
        }
        private void OnDisable()
        {
            activeTiles.Remove(this);

        }

        private void Update()
        {
            if (moveTarget != null)
                Move();
        }

        public void Init( int _value , TileSet _tileset )
        {
            tileset = _tileset;
            card.ApplyData( Deck.I.GetCardByBit( _value ) ,true);
            value = _value;
            bForceCombine = false;
        }

     
        public void MarkUpgrade( bool Force = false)
        {
            isUpgrade = true;
            bForceCombine = Force;
        }

     
        public void SetCombineTarget(Tile target)
        {
            combineTarget = target;            
        }


        public bool Move(Transform target)
        {
            if (false == transform.position.Equals(target.position))
            {
                moveTarget = target;
                return true;
            }

            Debug.LogError("move start error");
            return false;
        }

        private void Move()
        {
            transform.position = Vector2.MoveTowards(transform.position, moveTarget.position, SPEED);

            if (transform.position.Equals(moveTarget.position))
            {
                moveTarget = null; //! 이동이끝남
                              
                if (combineTarget)
                {                    
                    DeletingCallback(combineTarget, true);
                    combineTarget = null;
                    DeletingCallback(this, false);
                }

                if( debuff.tileDebuffState == ActionType.CardDestroy2 )
                {
                    debuffImage[ 2 ].gameObject.SetActive( false );
                    debuff.Init();
                }

            }
        }

        public Vector3 TweenEndPosition;
        public void CreateTile(Vector3 position, Vector3 EndPosition )
        {
            moveTarget = null;
            combineTarget = null;
            InitDebuff();

            isUpgrade = false;
            bCreate = false;
            index = TileIndex;
            TileIndex++;
            bForceCombine = false;
            gameObject.SetActive(true);

            if (value == 4 )
            {
                transform.position = EndPosition;
                TweenPositionEnd();
            }
            else
            {
                if (position != EndPosition)
                    tweenPos.Play(position, EndPosition);
                else
                    transform.position = position;

                TweenEndPosition = EndPosition;
            }
            
        
        }

        public void StopTween()
        {
            if( tweenPos.IsPlay() )
            {
                tweenPos.Stop();
                transform.position = TweenEndPosition;
                TweenPositionEnd();
                //tileset.Sibling();
            }
        }


        public void TweenPositionEnd()
        {            
            tweenScale.Play(Vector3.one, new Vector3(1.35f, 1.35f, 0));
            if (value != 2)
            {
                bool bCritical = cardData.Critical > UnityEngine.Random.Range( 0 , 100 );
                
                int damage = Deck.I.GetBuffAttack( cardData , bCritical );

                //if(bCritical)
                //{
                //    Debug.LogError( "critical" );
                //}
                //damage = (int)((float)damage * GameScene.I.GameMgr.boss.CheckPropertyValue( cardData.property ));


                

                if(card.cardData.Class == CLASS.HEAL)
                    TrailEffectManager.I.Play(PROPERTY.HEAL, damage, index, transform.position, GameScene.I.GameMgr.boss.GetHitPos(), Turn, AttackEvent.I.Attack, bCritical, CurrentComboCount, value , card.cardData.cutName );
                else
                    TrailEffectManager.I.Play( card.cardData.property , damage , index , transform.position , GameScene.I.GameMgr.boss.GetHitPos() , Turn, AttackEvent.I.Attack, bCritical , CurrentComboCount,value, card.cardData.cutName );

                int heal = Deck.I.GetBuffHeal( cardData );

                if( heal >0)
                    GameScene.I.PlayerHeal( heal );

            }
            bCreate = true;

            //tileset.Sibling();


        }

        public void TurnEnd()
        {
            debuff.ReduceDebuffTime();

            if( debuff.tileDebuffState == ActionType.cardDestroy )
            {             
                debuffImage[ 1 ].SetCount( debuff.DebuffValue );
            }
            else if( debuff.tileDebuffState == ActionType.CardLock )
            {                
                debuffImage[ 0 ].SetCount( debuff.DebuffValue );
            }
            else if( debuff.tileDebuffState == ActionType.CardDestroy2 )
            {
                debuffImage[ 2 ].SetCount( debuff.DebuffValue );
            }

        }
    }

    public partial class Tile : MonoBehaviour
    {     
        debuff[] debuffImage = new debuff[3];
        GameObject[] debuffEndImage = new GameObject[2];
        TileDebuff debuff = new TileDebuff();

        
        public void InitDebuff()
        {
            debuff.Init();
            debuff.EndDebuff = EndDebuff;

            for (int i = 0; i < debuffImage.Length; i++)
            {
                debuffImage[i].transform.localPosition = Vector3.zero;
                debuffImage[i].gameObject.SetActive(false);
            }
        }
        public void SetDebuff( MonsterActionReferenceData debuffData )
        {
            debuff.SetDebuff(debuffData.actionType, debuffData.DebuffTurn);

            if( debuffData.actionType == ActionType.cardDestroy )
            {
                debuffImage[1].Play();
                debuffImage[ 1 ].SetCount( debuffData.DebuffTurn );
            }
            else if( debuffData.actionType == ActionType.CardLock )
            {
                debuffImage[ 0 ].Play();
                debuffImage[ 0 ].SetCount( debuffData.DebuffTurn );
            }
            else if( debuff.tileDebuffState == ActionType.CardDestroy2 )
            {
                debuffImage[ 2 ].Play();
                debuffImage[ 2 ].SetCount( debuff.DebuffValue );
            }
        }

        public bool IsDebuffAble()
        {
            if (bCreate == false )
                return false;

            if (IsReserveDelete == true)
                return false;

            return debuff.tileDebuffState == ActionType.None ? true : false;
        }

        public void EndDebuff()
        {
            if( debuff.tileDebuffState == ActionType.cardDestroy )
            {
                Invoke( "DeleteTile" , 0.16f );
                debuffImage[ 1 ].Burst();
                debuffEndImage[0].gameObject.SetActive(true);
                if ( tileset != null )
                    tileset.DeleteTileEx( TilePos.x , TilePos.y );
            }
            else if( debuff.tileDebuffState == ActionType.CardLock )
            {
                debuffImage[ 0 ].End();
            }
            else if( debuff.tileDebuffState == ActionType.CardDestroy2 )
            {
                debuffImage[ 2 ].Burst();
                debuffEndImage[1].gameObject.SetActive(true);
                Invoke( "DeleteTile" , 0.16f );
                if( tileset != null )
                    tileset.DeleteTileEx( TilePos.x , TilePos.y );
            }
        }

        public void DeleteTile()
        {
            if (tileset != null)
            {
                tileset.tilePoolList[ (int)Mathf.Log( value,2 )- 1 ].Delete( gameObject );
            }                
        }
    }
}