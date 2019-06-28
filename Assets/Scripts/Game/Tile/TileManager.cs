using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using Game.Helper;
using Control;

namespace Game
{
    public enum TurnState
    {
        WAIT,
        PLAY,
        ForcePlay,
        END,
    }

	[RequireComponent(typeof(AudioSource))]
	public partial class TileManager : MonoBehaviour
	{
        TileSet tileSet;
        TileMoveCaculate moveCaculate = new TileMoveCaculate();

        [SerializeField]
        RawImage mainImage;

        [SerializeField]
        RawImage[] TileImage;


        /// <summary>
        /// 합쳐질 때 사운드
        /// </summary>
        [SerializeField]
		private AudioClip sndCombine;

		private AudioSource mAudioSource;
        
        public Action<int> CreateNewTileCallback = null;
        public Action<int,Position> UpgradeNewTileCallback = null;
        public Action TurnEndCallback = null;
        
        TurnState Turnstate = TurnState.WAIT;

        public int TurnCount = 1;
        public bool isFull{	get{return tileSet.isFull;}       }
        public bool isCombinable { get { return tileSet.isCombinable; } }

        List<Tile> removeTiles = new List<Tile>();

        public GameObject UndoEffect;

        float AutoPlayTime = 0f;
        Swipe CurrentSwipe = Swipe.UpRight;
        void Awake()
        {
            tileSet = gameObject.AddComponent<TileSet>();
            tileSet.UpgradeNewTileCallback = UpgradeNewCard;            
            moveCaculate.tileset = tileSet;
            gameObject.SetActive( false );

            UndoEffect = ResourceManager.Load(gameObject, "pref_fx_useitem_undo");
            UndoEffect.SetActive(false);
        }

        private void Start()
        {
            string name = "tex_stage_01";
            if( GameScene.modeType == ModeType.ModeDefault )
            {
                StageReferenceData ReferenceData = StageManager.I.GetData();
                mainImage.texture = ResourceManager.LoadTexture( ReferenceData.StageTile );
                name = ReferenceData.StageTile;
            }
            else
            {
                mainImage.texture = ResourceManager.LoadTexture( RankingStageManager.I.CurrentData.StageTile );
                name = RankingStageManager.I.CurrentData.StageTile;
            }
                        

            int count = 0;
            for( int i =0 ; i < 4 ; i++ )
            {
                for( int z = 0 ; z < 4 ; z++ )
                {
                    TileImage[count++].texture = ResourceManager.LoadTexture( name+"_tile_"+i.ToString()+z.ToString() );
                }
            }
        }

        
        public bool IsUpgrade()
        {
            return tileSet.IsUpgrade();
        }


      
        public void PostUpdate()
		{
            tileSet.PostUpdate();

            if( GameScene.modeType == ModeType.ModeDefault && GameOption.bAutoPlay && TurnState.WAIT == Turnstate )
            {
                AutoPlayTime += Time.deltaTime;

                if( AutoPlayTime >= 0.5f )
                {
                    SwipeManager.swipeDirection = CurrentSwipe;

                    if( CurrentSwipe == Swipe.UpLeft )
                        CurrentSwipe = Swipe.UpRight;
                    else
                        CurrentSwipe++;

                    AutoPlayTime = 0f;
                }
                else                
                    return;
                
            }
            switch( Turnstate )
            {
                case TurnState.WAIT:
                    {
                        if (Input.GetMouseButtonUp(0) || ( GameScene.modeType == ModeType.ModeDefault && GameOption.bAutoPlay) )
                        {
                            if (SwipeManager.swipeDirection != Swipe.None)
                            {
                                tileSet.StopTween();
                                tileSet.CopyTurn();
                                if (moveCaculate.Calculate(SwipeManager.swipeDirection))
                                {
                                    tileSet.SaveTurn();
                                    Turnstate = TurnState.PLAY;
                                    AttackEvent.I.SetCombo(moveCaculate.combineTiles.Count > 0);                                           
                                }
                            }
                        }
                    }
                    break;

                case TurnState.PLAY:
                    {
                        if( tileSet.IsMoving() == false && moveCaculate.IsMoving() == false )
                        {
                            //if(AttackEvent.I.attacker.Count == 0)
                                Turnstate = TurnState.END;
                        }
                    }
                    break;

                case TurnState.ForcePlay:
                    {
                        if (tileSet.IsMoving() == false && moveCaculate.IsMoving() == false)
                        {
                            //if (AttackEvent.I.attacker.Count == 0)
                            {
                                Turnstate = TurnState.WAIT;
                                TurnEndCallback();
                            }
                                
                        }
                    }
                    break;

                case TurnState.END:
                    {
                        TurnCount++;
                        CreateNewTile();
                        Turnstate = TurnState.WAIT;
                        TurnEndCallback();
                        tileSet.TurnEnd();
                    }
                    break;
            }
		}
        
        void CreateNewTile()
        { 
            CreateNewTileCallback(4);
            if (removeTiles.Count > 0)
                mAudioSource.Play();
        }

        public bool ForceCombine()
        {
            if (Turnstate != TurnState.WAIT)
                return false;

            if (moveCaculate.ForceCombine() == false)
                return false;

            Turnstate = TurnState.ForcePlay;
            return true;
        }

        public bool RandomUpgrade()
        {
            if (Turnstate != TurnState.WAIT)
                return false;

            if (tileSet.RandomUpgrade() == false)
                return false;

            return true;
        }

        public bool Return()
        {
            if( Turnstate != TurnState.WAIT )
            {
                Debug.LogError( " 되돌리기 에러 1" );
                return false;
            }

            if( TurnCount == 1 )
            {
                Debug.LogError( " 되돌리기 에러 2" );
                return false;
            }

            tileSet.Return();
            UndoEffect.SetActive(true);
            return true;
        }

        public void Initialize()
		{
			Transform target = transform.Find( "target" );
			for( int y = 0; y < TileSet.LINE_LENGTH; ++y )
			{
				for( int x = 0; x < TileSet.LINE_LENGTH; ++x )
				{
					string targetName = StringHelper.Append( "target", x.ToString(), y.ToString() );
					Transform targetTransform = target.Find( targetName );
					if( targetTransform == null )
						throw new System.NullReferenceException( "[TileManager] Transform is null." );

					tileSet.targetArray[ x, y ] = targetTransform;
				}
			}
            
            tileSet.Init( target );

            if ( mAudioSource == null )
				mAudioSource = GetComponent<AudioSource>();
		}

		public void Reset()
		{
            Turnstate = TurnState.WAIT;
            TurnCount = 1;
            tileSet.Reset();
        }

        private void UpgradeNewCard(int value, Position TilePosition  )
        {
            UpgradeNewTileCallback(value, TilePosition);
        }

        public Tile NewTile( Vector3 position )
        { 
            return tileSet.NewTile(position);
		}

        public Tile UpgradeTile(Vector3 position, Position TilePosition, int value, int turn)
        {
            return tileSet.NewTile( position, TilePosition, value, turn );
        }


        public void SetDeBuff( BossAttackData data )
        {
            tileSet.SetDeBuff(data);
        }
    }


}