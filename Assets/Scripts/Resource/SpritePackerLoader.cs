using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.U2D;

public class SpritePackerLoader : MonoSinglton<SpritePackerLoader>
{
	public class SpriteAtlasData
	{
		public SpriteAtlas _atlas;
		Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

		public SpriteAtlasData(SpriteAtlas atlas)
		{
			_atlas = atlas;
			if (_atlas != null)
			{
				var sprites = new Sprite[_atlas.spriteCount];
				_atlas.GetSprites(sprites);

				for (int i = 0; i < sprites.Length; ++i)
				{
					sprites[i].name = sprites[i].name.Replace("(Clone)", "");
					_sprites.Add(sprites[i].name, sprites[i]);
				}
			}
		}

		public Sprite GetSprite(string name)
		{
			Sprite result = null;
			if (_sprites.TryGetValue(name, out result) == true)
			{
				// log
			}
			return result;
		}
	}

	List<SpriteAtlasData> _atlas_datas = new List<SpriteAtlasData>();

	public bool IsPreloaded
	{
		get { return _atlas_datas.Count > 0; }
	}

	public void Preload()
	{
		var tbl = TBLManager.I.GetTable<SpriteAtlasTBL>(TABLELIST_TYPE.SpriteAtlas);
		var tbl_data_list = tbl.GetList();
		if (tbl_data_list != null)
		{
			for (int i = 0; i < tbl_data_list.Count; ++i)
			{
				var tbl_data = tbl_data_list[i];
				if (tbl_data.Preload == true)
				{
					var atlas = ResourceManager.LoadAtlas(tbl_data.AtlasName);
					if (atlas == null)
					{
						Debug.LogError("[Sprite Atlas] dosen`t exist : " + tbl_data.AtlasName);
					}
					else
					{
						var atlas_data = new SpriteAtlasData(atlas);
						_atlas_datas.Add(atlas_data);
					}
				}
			}
		}
	}

	public SpriteAtlas GetAtlas(string atlas_name)
	{
		for (int i = 0; i < _atlas_datas.Count; ++i)
		{
			var data = _atlas_datas[i];
			if (data._atlas.name.CompareTo(atlas_name) == 0)
			{
				return data._atlas;
			}
		}

		var new_atlas = ResourceManager.LoadAtlas(atlas_name);
		if (new_atlas == null)
		{
			Debug.LogError("Fail to find atlas, " + atlas_name);
		}

		var new_data = new SpriteAtlasData(new_atlas);
		_atlas_datas.Add(new_data);

		return new_atlas;
	}

	SpriteAtlasData GetSpriteData(string atlas_name)
	{
		for (int i = 0; i < _atlas_datas.Count; ++i)
		{
			var data = _atlas_datas[i];
			if (data._atlas.name.CompareTo(atlas_name) == 0)
			{
				return data;
			}
		}

		var new_atlas = ResourceManager.LoadAtlas(atlas_name);
		if (new_atlas == null)
		{
			Debug.LogError("Fail to find atlas, " + atlas_name);
		}

		var new_data = new SpriteAtlasData(new_atlas);
		_atlas_datas.Add(new_data);

		return new_data;
	}

	public Sprite GetSprite(string atlas_name, string sprite_name)
	{
		var data = GetSpriteData(atlas_name);
		if (data != null)
		{
			return data.GetSprite(sprite_name);
		}

		return null;
	}

	// 이 함수는 sprite를 로드하는 내부코드가 동작하지 않으므로
	// sprite atlas를 선 로드한 상황임을 가정한 상황에서만 사용한다.
	public Sprite GetSprite(string sprite_name)
	{
		for (int i = 0; i < _atlas_datas.Count; ++i)
		{
			var data = _atlas_datas[i];
			var sprite = data.GetSprite(sprite_name);
			if (sprite != null)
			{
				return sprite;
			}
		}

		Debug.LogError("Fail to find sprite, " + sprite_name);

		return null;
	}

	public override void ClearAll()
	{
	}
}