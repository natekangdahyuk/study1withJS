using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool : MonoBehaviour
{
	private GameObject			goRoot			= null;
	private List<GameObject>	goList			= new List<GameObject>();

	public	GameObject			goPrefab		= null;
	public  int					initSize 		= 16;
	public	int					stepSize		= 8;

	void Awake()
	{
		Init();
	}
	void OnDestroy()
	{
		goList = null;
		goRoot = null;
		goPrefab = null;
	}

	public void Init()
	{
		if( goRoot != null )
			return;

		goRoot = new GameObject( "_GoPool_" );
		if( goRoot != null )
		{
			goRoot.transform.parent = transform;
			goRoot.transform.localPosition = Vector3.zero;
			goRoot.transform.localRotation = Quaternion.identity;
			goRoot.transform.localScale = Vector3.one;
			goRoot.layer = gameObject.layer;
			goRoot.SetActive( false );
		}

		if( goPrefab != null )
		{
			for( int i = 0; i < initSize; ++i )
			{
				Push( Clone() );
			}
		}
	}
	public void Setting( GameObject _goPrefab, int _initSize, int _stepSize )
	{
		goPrefab = _goPrefab;
		initSize = _initSize;
		stepSize = _stepSize;

		Init();

		if( goPrefab != null && goRoot != null && goList.Count == 0 )
		{
			for( int i = 0; i < initSize; ++i )
			{
				Push( Clone() );
			}
		}
	}
	public GameObject New()
	{
		GameObject go = Pop();
		if( go != null )
		{
			return go;
		}

		if( goPrefab != null )
		{
			for( int i = 0; i < stepSize; ++i )
			{
				Push( Clone() );
			}
		}
		return Pop();
	}
	public void Delete( GameObject go )
	{
		Push( go );
	}

	private GameObject Clone()
	{
		return GameObject.Instantiate( goPrefab ) as GameObject;
	}
	private void Push( GameObject go )
	{
		if( go != null )
		{
			go.transform.SetParent( goRoot.transform );
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localScale = Vector3.one;
			go.layer = gameObject.layer;
			go.SetActive( false );
			goList.Add( go );
		}
	}
	private GameObject Pop()
	{
		if( 0 < goList.Count )
		{
			GameObject go = goList[ 0 ];
			goList.Remove( go );
			return go;
		}
		return null;
	}

	static public GameObjectPool MakeComponent( GameObject go, GameObject goPrefab, int initSize, int stepSize )
	{
		GameObjectPool goPool = go.AddComponent<GameObjectPool>();
		if( goPool == null )
		{
			Debug.LogError( " >>> [GameObjectPool] component add failed!!" );
			return null;
		}

		goPool.Setting( goPrefab, initSize, stepSize );
		return goPool;
	}
}
