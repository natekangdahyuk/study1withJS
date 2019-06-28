using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Game.Helper;
using Control;


namespace Game
{
    public class TileSet : MonoBehaviour
    {
        public static int MAX_TILE_COUNT = 16;
        public static int LINE_LENGTH = 4;

        public Tile[,] tileArray = new Tile[LINE_LENGTH, LINE_LENGTH];
        public TileObject[,] tileObjectArray = new TileObject[LINE_LENGTH, LINE_LENGTH];
        public Transform[,] targetArray = new Transform[LINE_LENGTH, LINE_LENGTH];

        public int[,] OldTileValue = new int[LINE_LENGTH, LINE_LENGTH]; //! 전턴데이터
        public int[,] SaveTileValue = new int[LINE_LENGTH, LINE_LENGTH]; //! 전턴데이터

        public List<GameObjectPool> tilePoolList = new List<GameObjectPool>();
        public Action<int, Position> UpgradeNewTileCallback = null;
        
        public void Awake()
        {
           
            for( int i=0 ; i < 11 ; i++ )
            {
                GameObject pool = new GameObject( "[TilePool " + i.ToString() + "]" );
                GameObjectPool tilepool = GameObjectPool.MakeComponent( pool , ResourceManager.LoadSrc( "Tile" ) , 6 , 4 );
                tilePoolList.Add( tilepool );
            }

            for (int y = 0; y < LINE_LENGTH; ++y)
            {
                for (int x = 0; x < LINE_LENGTH; ++x)
                {
                    if (tileObjectArray[x, y] == null)
                    {
                        tileObjectArray[x, y] = GameObject.Find("Stage_Tile" + x.ToString() + y.ToString()).GetComponent<TileObject>();
                    }
                }
            }
        }

        public void Init( Transform target )
        {
            for( int y = 0 ; y < TileSet.LINE_LENGTH ; ++y )
            {
                for( int x = 0 ; x < TileSet.LINE_LENGTH ; ++x )
                {
                    string targetName = StringHelper.Append( "target" , x.ToString() , y.ToString() );
                    Transform targetTransform = target.Find( targetName );
                    if( targetTransform == null )
                        throw new System.NullReferenceException( "[TileManager] Transform is null." );
                                        
                    targetArray[ x , y ] = targetTransform;
                }
            }
        }

        public bool isFull
        {
            get
            {
                for (int y = 0; y < LINE_LENGTH; ++y)
                {
                    for (int x = 0; x < LINE_LENGTH; ++x)
                    {
                        if( tileObjectArray[ x , y ].Curtile == null )
                        {
                            if( tileObjectArray[ x , y ].debuff.tileDebuffState != ActionType.barrier )
                                return false;
                        }

                        
                    }
                }

                return true;
            }
        }

        public bool isCombinable
        {
            get
            {
                for (int x = 0; x < LINE_LENGTH; ++x)
                {
                    for (int y = 0; y < LINE_LENGTH; ++y)
                    {
                        Tile curTile = tileObjectArray[x, y].Curtile;
                        Tile leftTile = x > 0 ? tileObjectArray[x - 1, y].Curtile : null;
                        Tile rightTile = x < LINE_LENGTH - 1 ? tileObjectArray[x + 1, y].Curtile : null;
                        Tile upTile = y > 0 ? tileObjectArray[x, y - 1].Curtile : null;
                        Tile downTile = y < LINE_LENGTH - 1 ? tileObjectArray[x, y + 1].Curtile : null;

                        if ((leftTile != null && curTile.value.Equals(leftTile.value))
                            || (rightTile != null && curTile.value.Equals(rightTile.value))
                            || (upTile != null && curTile.value.Equals(upTile.value))
                            || (downTile != null && curTile.value.Equals(downTile.value))
                            )
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public void PostUpdate()
        {
#if UNITY_EDITOR
            for (int y = 0; y < LINE_LENGTH; ++y)
            {
                for (int x = 0; x < LINE_LENGTH; ++x)
                {
                    if (tileObjectArray[x, y].Curtile != null)
                        tileObjectArray[x, y].Curtile.name = new Position(x, y).ToString();
                }
            }
#endif
        }

        public Tile NewTile(Vector3 position )
        {
            if (isFull)
                return null;
          
            //!< 새로 생성할 타일의 인덱스
            Position newPos;
            while (true)
            {
                newPos = new Position(UnityEngine.Random.Range(0, LINE_LENGTH), UnityEngine.Random.Range(0, LINE_LENGTH));

                if( tileObjectArray[ newPos.x , newPos.y ].debuff.tileDebuffState == ActionType.barrier )
                    continue;

                if( tileObjectArray[ newPos.x , newPos.y ].Curtile == null )
                    break;
            }

            return NewTile( position , newPos , 4 , GameScene.I.GameMgr.tileMgr.TurnCount );
        }

        public Tile NewTile(int value , Position position )
        {
            if (isFull)
                return null;

            return NewTile( targetArray[ position.x , position.y ].position , position , value,0 );
            
        }

        public Tile NewTile( Vector3 position , Position TilePosition , int value , int turn)
        {
            Tile newTile = tilePoolList[ (int)Mathf.Log(value,2)-1 ].New().GetComponent<Tile>();
            if( newTile == null )
                throw new System.NullReferenceException( "[TileManager] Tile is null." );


            if( newTile.cardData == null  )
            {
                newTile.Init( value , this );
            }
            newTile.transform.parent = transform;
            newTile.Turn = turn;
            newTile.DeletingCallback = DeleteTile;
            SetTile( TilePosition.x , TilePosition.y , newTile );
            newTile.CreateTile( position , targetArray[ TilePosition.x , TilePosition.y ].position );
            
           
            newTile.CurrentComboCount = AttackEvent.I.ComboCount;            
            

          

            return newTile;
        }

        public void Sibling()
        {
            for( int x = 0 ; x < LINE_LENGTH ; ++x )
            {
                for( int y = 0 ; y < LINE_LENGTH ; ++y )
                {
                    if( tileObjectArray[ x , y ].Curtile != null )
                        tileObjectArray[ x , y ].Curtile.transform.SetSiblingIndex( 50 + x * 10 + y );
                }
            }
        }


        public bool IsMoving()
        {
            bool bMove = false;
            for (int x = 0; x < LINE_LENGTH; ++x)
            {
                for (int y = 0; y < LINE_LENGTH; ++y)
                {
                    if (tileObjectArray[x, y].Curtile != null)
                    {
                        if (tileObjectArray[x, y].Curtile.IsMoving)
                        {
                            bMove = true;
                        }
                    }
                }
            }

            return bMove;
        }


        public bool IsUpgrade()
        {
            for (int x = 0; x < LINE_LENGTH; ++x)
            {
                for (int y = 0; y < LINE_LENGTH; ++y)
                {
                    if (tileObjectArray[x, y].Curtile == null)
                        continue;

                    Tile curTile = tileObjectArray[x, y].Curtile;

                    if (curTile.isUpgrade)
                        return true;
                }
            }
            return false;
        }


        public void SetTile(int x, int y, Tile tile)
        {
            tile.TilePos.x = x;
            tile.TilePos.y = y;
            tileObjectArray[x, y].HaveCard(tile);
        }

        public void StopTween()
        {
            for( int x = 0 ; x < LINE_LENGTH ; ++x )
            {
                for( int y = 0 ; y < LINE_LENGTH ; ++y )
                {
                    if( tileObjectArray[ x , y ].Curtile == null )
                        continue;

                    tileObjectArray[ x , y ].Curtile.StopTween();
                }
            }
        }

        void DeleteTile(Tile tile, bool bUpgrade)
        {
            int value = tile.value;
            if (bUpgrade)
            {
                if (tile.moveTarget != null)
                {
                    tile.moveTarget = null;
                }
                
                tileObjectArray[tile.TilePos.x, tile.TilePos.y].DeleteCard(tile);            
                UpgradeNewTileCallback(tile.value * 2, tile.TilePos ); 
            }

            tilePoolList[ (int)Mathf.Log( value , 2 ) - 1 ].Delete( tile.gameObject );                    
        }

        public void DeleteTile(int x, int y)
        {
            if(tileObjectArray[x, y].Curtile != null )
            {
                tilePoolList[ (int)Mathf.Log( tileObjectArray[ x , y ].Curtile.value , 2 ) - 1 ].Delete( tileObjectArray[ x , y ].Curtile.gameObject );
                tileObjectArray[x, y].DeleteCard(tileObjectArray[x, y].Curtile);                
            }
        }

        public void DeleteTileEx( int x , int y )
        {
            if( tileObjectArray[ x , y ].Curtile != null )
            {                
                tileObjectArray[ x , y ].DeleteCard( tileObjectArray[ x , y ].Curtile );
            }
        }

        public void UpgradeTile( int x, int y )
        {
            if( tileObjectArray[ x , y ].Curtile != null )
            {
                tilePoolList[ (int)Mathf.Log( tileObjectArray[ x , y ].Curtile.value , 2 ) - 1 ].Delete( tileObjectArray[ x , y ].Curtile.gameObject );

                int value = tileObjectArray[ x , y ].Curtile.value * 2;
                tileObjectArray[ x , y ].Curtile = null;                
                NewTile( value , new Position(x,y));
                tileObjectArray[x, y].Curtile.RandomBitEffect.SetActive(true);

                Deck.Instance.SetCurrentCard( value , GameScene.I.GameMgr.stageBase.currTurn );
            }
        }

        public void MoveTile(Tile tile , int x, int y)
        {           
            tileObjectArray[tile.TilePos.x, tile.TilePos.y].DeleteCard(tile);
     
            if ( x != -1 )
                SetTile(x, y, tile);
        }

        public void CopyTurn()
        {
            for( int y = 0 ; y < LINE_LENGTH ; ++y )
            {
                for( int x = 0 ; x < LINE_LENGTH ; ++x )
                {
                    SaveTileValue[ x , y ] = tileObjectArray[ x , y ].CurrentTileValue;
                }
            }
        }

        public void SaveTurn()
        {
            for (int y = 0; y < LINE_LENGTH; ++y)
            {
                for (int x = 0; x < LINE_LENGTH; ++x)
                {
                  OldTileValue[x,y] = SaveTileValue[ x, y];
                }
            }
        }

        public void Reset()
        {
            for (int x = 0; x < LINE_LENGTH; ++x)
            {
                for (int y = 0; y < LINE_LENGTH; ++y)
                {
                    DeleteTile(x, y);
                }
            }

            for (int y = 0; y < LINE_LENGTH; ++y)
            {
                for (int x = 0; x < LINE_LENGTH; ++x)
                {                    
                    tileObjectArray[x, y].Reset();
                    OldTileValue[x, y] = 0;
                }
            }
        }

        public List<int> RandomUpgradeList = new List<int>();
        public bool RandomUpgrade()
        {

            RandomUpgradeList.Clear();
            int tileCount = 0;
            for (int x = 0; x < LINE_LENGTH; ++x)
            {
                for (int y = 0; y < LINE_LENGTH; ++y)
                {
                    if( tileObjectArray[ x , y ].Curtile != null && tileObjectArray[ x , y ].Curtile.value < 1024 )
                    {                        
                        tileCount++;
                    }
                }
            }

            if (tileCount == 0)
                return false;

            int maxcount = tileCount + 1;

            if( maxcount > 12 )
                maxcount = 12;

            int UpgradeCount = UnityEngine.Random.Range( Math.Min( 2 , maxcount-1 ) , maxcount ); //! 최소 2개에서 최대 전체타일업그레이드
            
            while(true)
            {
               bool bInsert = true;               
               int value = UnityEngine.Random.Range(0, maxcount-1 );

                for(int i=0; i < RandomUpgradeList.Count; i++)
                {
                    if (value == RandomUpgradeList[i])
                    {
                        bInsert = false;                        
                        break;
                    }

                }

                if (bInsert)
                    RandomUpgradeList.Add(value);

                if (RandomUpgradeList.Count == UpgradeCount)
                    break;               
            }

            int count = 0;
            for (int x = 0; x < LINE_LENGTH; ++x)
            {
                for (int y = 0; y < LINE_LENGTH; ++y)
                {
                    if (tileObjectArray[x, y].Curtile != null && tileObjectArray[x, y].Curtile.value < 1024)
                    {
                        for( int i=0; i < RandomUpgradeList.Count; i++)
                        {
                            if( count == RandomUpgradeList[i])
                            {
                                UpgradeTile(x,y);
                            }
                        }
                        count++;
                    }
                }
            }


            return true;
        }

        public List<TileObject> DebuffAbleList = new List<TileObject>();
        public void SetDeBuff(BossAttackData data)
        {
            DebuffAbleList.Clear();

            for (int x = 0; x < TileSet.LINE_LENGTH; ++x)
            {
                for (int y = 0; y < TileSet.LINE_LENGTH; ++y)
                {
                    if (tileObjectArray[x, y].IsDebuffAble(data.ActionData.actionType , data.ActionData.DebuffValue) ) //! 디버프 설치가능한대 찾음
                        DebuffAbleList.Add(tileObjectArray[x, y]);
                }
            }

            if(DebuffAbleList.Count > 0 ) //!랜덤으로 디버프 셋팅
                DebuffAbleList[UnityEngine.Random.Range(0, DebuffAbleList.Count)].SetDebuff(data.ActionData );            
        }

        public bool Return()
        {
            for (int x = 0; x < TileSet.LINE_LENGTH; ++x)
            {
                for (int y = 0; y < TileSet.LINE_LENGTH; ++y)
                {
                    DeleteTile(x, y);
                }
            }

            for (int x = 0; x < TileSet.LINE_LENGTH; ++x)
            {
                for (int y = 0; y < TileSet.LINE_LENGTH; ++y)
                {
                    if (OldTileValue[x, y] > 0)
                    {
                        NewTile(OldTileValue[x, y], new Position(x, y));
                    }
                }
            }

            return true;
        }

        public void TurnEnd()
        {
            for (int y = 0; y < LINE_LENGTH; ++y)
            {
                for (int x = 0; x < LINE_LENGTH; ++x)
                {                    
                    tileObjectArray[x, y].TurnEnd();
                }
            }
        }
    }
}

