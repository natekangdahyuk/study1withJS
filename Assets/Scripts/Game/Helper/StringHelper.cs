using UnityEngine;
using System.Text;

namespace Game.Helper
{
	public class StringHelper
	{
		public static string Append( params string[] param )
		{
			StringBuilder sb = new StringBuilder();
			for( int i=0; i < param.Length; ++i )
				sb.Append( param[ i ] );

			return sb.ToString();
		}
	}
}