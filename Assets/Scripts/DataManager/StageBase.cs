using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageBase
{
	public StateTurnInfo	turnInfo		{ private set; get; }
	public int				currTurn		{ get { return ( null != turnInfo ) ? turnInfo.currTurn : 0; } }

	public StateTimeInfo	timeInfo		{ private set; get; }

	public StageBase()
	{
		turnInfo = new StateTurnInfo();
		timeInfo = new StateTimeInfo();
	}
}

public class StateTurnInfo
{
	public int				prevTurn		{ get { return currTurn - 1; } }
	public int				currTurn		{ private set; get; }
	public int				nextTurn		{ get { return currTurn + 1; } }

	public int				maxTurn			{ private set; get; }

	public bool				isAdd			{ get { return ( 0 < maxTurn && currTurn < maxTurn ) ? true : false; } }
	public bool				isSub			{ get { return ( 0 < currTurn ) ? true : false; } }

	public void Set( int curr, int max )
	{
		currTurn = curr;
		maxTurn = max;
	}
	public void Set( int curr )
	{
        if (curr < 1) curr = 1;
		currTurn = curr;
	}
	public void Reset()
	{
		currTurn = 0;
		maxTurn = 0;
	}

	public void Add()
	{
		if( true == isAdd )
		{
			++currTurn;
		}
	}
	public void Sub()
	{
		if( true == isSub )
		{
			--currTurn;
		}
	}
}

public class StateTimeInfo
{
	public  float			deltaTime;
	public int				currTime		{ get { return (int)deltaTime; } }

	public int				hours			{ get { return ( 0 < currTime ) ? currTime / 3600 : 0; } }
	public int				minutes			{ get { return ( 0 < currTime ) ? ( currTime % 3600 ) / 60 : 0; } }
	public int				seconds			{ get { return ( 0 < currTime ) ? currTime % 60 : 0; } }

	public void Reset()
	{
        if( GameScene.modeType == ModeType.ModeTimeLimit || GameScene.modeType == ModeType.TimeDefence )
            deltaTime = 180;
        else
            deltaTime = 0f;
	}
	
	public void RunTime()
	{
        if( GameScene.modeType == ModeType.ModeTimeLimit || GameScene.modeType == ModeType.TimeDefence )
            deltaTime -= Time.deltaTime;
        else
            deltaTime += Time.deltaTime;
    }
}


