using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Control;
using Game;
using UnityEngine;
public class TileMoveCaculate
{
    public List<Tile> combineTiles = new List<Tile>();
    Dictionary<Tile, Position> activeTiles = new Dictionary<Tile, Position>();

    public TileSet tileset;

    public bool IsMoving()
    {
        bool bMove = false;
     
        for (int i = 0; i < combineTiles.Count; i++)
        {
            if (combineTiles[i].IsMoving)
            {
                bMove = true;
            }
        }
        return bMove;
    }

    public bool Calculate(Swipe direction)
    {
        int x, y, lineIdx;
        combineTiles.Clear();
        //!< Move 처리할 한 줄
        Position[] lines = new Position[TileSet.LINE_LENGTH];

        bool bCombine = false;
        switch (direction)
        {
            case Swipe.UpRight:
                for (x = 0; x < TileSet.LINE_LENGTH; ++x)
                {
                    lineIdx = 0;

                    for (y = 0; y < TileSet.LINE_LENGTH; ++y)
                    {
                        lines[lineIdx++] = new Position(x, y);
                    }

                    if (CombineAndMove(lines))
                        bCombine = true;
                }
                break;
            case Swipe.DownLeft:
                for (x = 0; x < TileSet.LINE_LENGTH; ++x)
                {
                    lineIdx = 0;

                    for (y = TileSet.LINE_LENGTH - 1; y >= 0; --y)
                    {
                        lines[lineIdx++] = new Position(x, y);
                    }

                    if (CombineAndMove(lines))
                        bCombine = true;
                }
                break;
            case Swipe.UpLeft:
                for (y = 0; y < TileSet.LINE_LENGTH; ++y)
                {
                    lineIdx = 0;

                    for (x = 0; x < TileSet.LINE_LENGTH; ++x)
                    {
                        lines[lineIdx++] = new Position(x, y);
                    }

                    if (CombineAndMove(lines))
                        bCombine = true;
                }
                break;
            case Swipe.DownRight:
                for (y = 0; y < TileSet.LINE_LENGTH; ++y)
                {
                    lineIdx = 0;

                    for (x = TileSet.LINE_LENGTH - 1; x >= 0; --x)
                    {
                        lines[lineIdx++] = new Position(x, y);
                    }

                    if (CombineAndMove(lines))
                        bCombine = true;
                }
                break;

        }        
        return bCombine;
    }

    private bool CombineAndMove(Position[] lines)
    {
        if (lines == null)
            throw new System.ArgumentException("[TileManager] Invalid Tile Queue!");

        if (CaculateActiveTiles(lines) == false) return false;

        if (CaculateUpgrade())
        {
            CalulateMove(lines);
            return true;
        }

        return CalulateMove(lines);
    }

    private bool CaculateActiveTiles(Position[] lines)
    {
        activeTiles.Clear();
        for (int i = 0; i < lines.Length; ++i)
        {
            Position curPos = lines[i];
            Tile curTile = tileset.tileObjectArray[curPos.x, curPos.y].Curtile;

            if (curTile != null && curTile.combineTarget == null)
                activeTiles.Add(curTile, curPos);
        }

        if (activeTiles.Count == 0)
            return false;

        return true;
    }

    private bool CaculateUpgrade()
    {
        bool bUpgrade = false;
        //!< 존재하는 타일이 2개 이상일 경우 Combine 체크
        if (activeTiles.Count >= 2)
        {
            Tile upgradeTarget = null;

            foreach (KeyValuePair<Tile, Position> kvp in activeTiles)
            {
                Tile curTile = kvp.Key;
                Position curPos = kvp.Value;

                if (curTile.IsReserveDelete == true)
                    continue;

                //if (curTile.IsCombineAble == false)
                //    continue;

                if (upgradeTarget == null)
                {
                    upgradeTarget = curTile;
                    continue;
                }

                //bool bBreak = false;
                //if( upgradeTarget.TilePos.x == curTile.TilePos.x)
                //{
                //    if( curTile.TilePos.y > upgradeTarget.TilePos.y )
                //    {
                //        for( int i = upgradeTarget.TilePos.y ; i < curTile.TilePos.y ; i++ )
                //        {
                //            if( tileset.tileObjectArray[ upgradeTarget.TilePos.x , i ].debuff.tileDebuffState == ActionType.barrier )
                //                bBreak = true;
                //        }
                //    }
                //    else
                //    {
                //        for( int i = curTile.TilePos.y ; i < upgradeTarget.TilePos.y ; i++ )
                //        {
                //            TileObject tileob = tileset.tileObjectArray[ upgradeTarget.TilePos.x , i ];
                //            if( tileset.tileObjectArray[ upgradeTarget.TilePos.x , i ].debuff.tileDebuffState == ActionType.barrier )
                //                bBreak = true;
                //        }
                //    }
                    
                //}
                //else
                //{
                //    if( curTile.TilePos.x > upgradeTarget.TilePos.x )
                //    {
                //        for( int i = upgradeTarget.TilePos.x ; i < curTile.TilePos.x ; i++ )
                //        {
                //            if( tileset.tileObjectArray[ i , upgradeTarget.TilePos.y ].debuff.tileDebuffState == ActionType.barrier )
                //                bBreak = true;
                            
                //        }
                //    }
                //    else
                //    {
                //        for( int i = curTile.TilePos.x ; i < upgradeTarget.TilePos.x ; i++ )
                //        {
                //            if( tileset.tileObjectArray[ i , upgradeTarget.TilePos.y ].debuff.tileDebuffState == ActionType.barrier )
                //                bBreak = true;
                //        }
                //    }
                    
                //}

                //if( bBreak )
                //{
                //    upgradeTarget = curTile;//!< 다음 타겟 지정
                //    continue;
                //}


                if (curTile.IsMoveAble == true && upgradeTarget.value.Equals(curTile.value))
                {
                    bUpgrade = true;
                    //!< 업그레이드 마크
                    upgradeTarget.MarkUpgrade();

                    //!< 합쳐질 타일 세팅
                    curTile.SetCombineTarget(upgradeTarget);

                    //!< 합칠 타일 따라서 이동                        
                    curTile.moveTarget = upgradeTarget.transform;                    
                    tileset.MoveTile(curTile, -1 , -1);

                    //!< 합쳐질 타일 수집
                    combineTiles.Add(curTile);

                    //!< 한번 업그레이드 한 타일은 다시 계산 안 함
                    upgradeTarget = null;
                }
                else
                {
                    upgradeTarget = curTile;//!< 다음 타겟 지정
                }
            }

            //!< 합쳐질 타일은 이동 대상에서 제외
            foreach (Tile tile in combineTiles)
                activeTiles.Remove(tile);
        }

        return bUpgrade;
    }

    int moveTargetIdx = 0;
    //! 이동할얘 계산
    private bool CalulateMove(Position[] lines)
    {
        bool bMove = false;
        moveTargetIdx = 0;
        foreach( KeyValuePair<Tile , Position> kvp in activeTiles )
        {
            
            Tile curTile = kvp.Key;
            Position curPos = kvp.Value;

            if( curTile.combineTarget != null )
                continue;

            if( curTile.moveTarget != null )
                continue;


            if( curTile.IsMoveAble == false ) //! 이동불가능한타일이면.. 이동불가능한타일인덱스가져옴
            {
                for( int i =0 ; i < lines.Length ; i++  )
                {
                    if( curTile.TilePos.x == lines[i].x && curTile.TilePos.y == lines[i].y )
                    {
                        moveTargetIdx = i+1;
                        break;
                    }
                }

                if( moveTargetIdx > 3 )
                    break;

                //moveTargetIdx++;
                continue;
            }

            Position movePos =lines[ moveTargetIdx ];
            Transform moveTarget= tileset.targetArray[ movePos.x , movePos.y ];

            //! 현재 타일이랑 이동할곳이랑 같으면 다음으로
            if( curTile.TilePos.x == movePos.x && curTile.TilePos.y == movePos.y )
            {                
                moveTargetIdx++;

                if( moveTargetIdx > 3 )
                    break;
                continue;
            }

            //for( ; moveTargetIdx < lines.Length ; moveTargetIdx++ )
            //{
            //    movePos = lines[ moveTargetIdx ];
            //    moveTarget = tileset.targetArray[ movePos.x , movePos.y ];
            
            //    if( GetMoveAblePos( lines , curTile ) )
            //    {
            //        if( moveTargetIdx == lines.Length - 1 )
            //            return false;

            //        break;
            //    }
            //}



         
            ////// 이동할 곳에 타일이없다면
            if( tileset.tileObjectArray[ movePos.x , movePos.y ].Curtile == null )
            {
                if( curTile.Move( moveTarget ) )
                {
                    //!< 타일 위치 갱신                        
                    tileset.MoveTile( curTile , movePos.x , movePos.y );
                    moveTargetIdx++;
                    bMove = true;
                }
                else
                    Debug.LogError( "move start error1" );
            }
            else //! 이동할 곳에 타일이있다면
            {
                Debug.LogError( "2" );
                //! 이동가능하다면..
                if( tileset.tileObjectArray[ movePos.x , movePos.y ].Curtile.IsMoveAble == false )
                {
                    if( moveTargetIdx >= 3 )
                        break;

                    movePos = lines[ moveTargetIdx++ ];
                    moveTarget = tileset.targetArray[ movePos.x , movePos.y ];

                    //! 현재타일고 이동할얘와같다면,, 합쳐질애..
                    if( curTile.TilePos.x == movePos.x && curTile.TilePos.y == movePos.y )
                        continue;

                    //!이동할곳에 타일이없다면...
                    if( tileset.tileObjectArray[ movePos.x , movePos.y ].Curtile == null )
                    {
                        if( curTile.Move( moveTarget ) )
                        {
                            //!< 타일 위치 갱신                        
                            tileset.MoveTile( curTile , movePos.x , movePos.y );
                            moveTargetIdx++;
                            bMove = true;
                        }
                    }
                    else
                        Debug.LogError( "move start error2" );
                }
            }
        }
        return bMove;
        
    }
    
    private bool GetMoveAblePos( Position[] lines , Tile curTile )
    {
        Position movePos = lines[ moveTargetIdx ];
      
        int index = 0;
        for( ; index < lines.Length ; index++ )
        {
            if( curTile.TilePos.x == lines[ index ].x && curTile.TilePos.y == lines[ index ].y )
                break;
        }

        //for( int i = moveTargetIdx ; i < index ; i++ )
        //{
        //    if( tileset.tileObjectArray[ lines[ i ].x , lines[ i ].y ].debuff.tileDebuffState == ActionType.barrier )
        //        return false;
        //}
        return true;
    }

    public bool ForceCombine()
    {
        activeTiles.Clear();
        for (int x = 0; x < TileSet.LINE_LENGTH; ++x)
        {
            for (int y = 0; y < TileSet.LINE_LENGTH; ++y)
            {
                Tile curTile = tileset.tileObjectArray[x, y].Curtile;

                if (curTile != null && curTile.combineTarget == null)
                    activeTiles.Add(curTile, curTile.TilePos);

            }
        }

        return ForceUpgrade();
    }

    private bool ForceUpgrade()
    {
        bool bUpgrade = false;
        //!< 존재하는 타일이 2개 이상일 경우 Combine 체크
        if (activeTiles.Count < 2)
            return false;

        Tile upgradeTarget = null;
        foreach (KeyValuePair<Tile, Position> value in activeTiles)
        {
            if (value.Key.combineTarget != null)
                continue;

            upgradeTarget = value.Key;

            foreach (KeyValuePair<Tile, Position> kvp in activeTiles)
            {
                Tile curTile = kvp.Key;
                Position curPos = kvp.Value;

                if (upgradeTarget == curTile)
                    continue;

                if (curTile.IsReserveDelete == true)
                    continue;

                if (curTile.combineTarget != null || curTile.isUpgrade)
                    continue;

                if (upgradeTarget.value.Equals(curTile.value))
                {
                    bUpgrade = true;
                    //!< 업그레이드 마크
                    upgradeTarget.MarkUpgrade(true);

                    //!< 합쳐질 타일 세팅
                    curTile.SetCombineTarget(upgradeTarget);
                    curTile.Move(upgradeTarget.transform);                    
                    tileset.MoveTile(curTile ,-1,-1);                   
                    combineTiles.Add(curTile);
                    break;
                }
            }
        }


        //!< 합쳐질 타일은 이동 대상에서 제외
        foreach (Tile tile in combineTiles)
            activeTiles.Remove(tile);


        return bUpgrade;
    }
}
