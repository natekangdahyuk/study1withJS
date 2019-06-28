using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public struct Position
	{
		public Position( int x = 0, int y = 0 )
		{
			this.x = x;
			this.y = y;
		}

		public int x;
		public int y;

		public override string ToString()
		{
			return Helper.StringHelper.Append( x.ToString(), ",", y.ToString() );
		}
	}
}