using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.U2D;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class ResourceManager : MonoSinglton<ResourceManager>
{
    static BundleContainer _bundle_container = new BundleContainer();
    public static BundleContainer _BundleContainer
    {
        get { return _bundle_container; }
    }

#if UNITY_EDITOR
    static Dictionary<string, string> _editor_res_paths = new Dictionary<string, string>();
    static string GetPath(string object_name)
    {
        if (_editor_res_paths.Count == 0)
        {
            var data_path = Application.dataPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var idx = data_path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            CollectEditorRes(idx, Application.dataPath);
        }

        string path = null;
        _editor_res_paths.TryGetValue(object_name, out path);
        return path;
    }

    static void CollectEditorRes(int start_idx, string path)
    {
        var di = new DirectoryInfo(path);
        var sub_dis = di.GetDirectories();
        if (sub_dis != null)
        {
            for (int i = 0; i < sub_dis.Length; ++i)
            {
                var sub_di = sub_dis[i];
                CollectEditorRes(start_idx, sub_di.FullName);
            }
        }

        var last_str = path.Substring(path.LastIndexOf(@"\") + 1);
        if (string.IsNullOrEmpty(last_str) == false)
        {
            if (last_str.CompareTo("AssetBundles") == 0)
            {
                var files = di.GetFiles();
                if (files != null && files.Length > 0)
                {
                    for (int i = 0; i < files.Length; ++i)
                    {
                        var file = files[i];
                        if (file.Name.ToLower().Contains(".meta") == true) continue;
                        var file_name = file.Name.Substring(0, file.Name.IndexOf('.'));
                        var full_path = file.FullName.Substring(start_idx);

                        //Debug.Log("fila name : " + file_name + ", path : " + full_path);
                        _editor_res_paths.Add(file_name, full_path);
                    }
                }
            }
        }
    }
#endif

    public static UnityEngine.Object GetTable(string prefab) //묶여있는데이터 로드 : 테이블로드
    {
        Debug.Log("## load table : " + prefab);
        var obj = Resources.Load(prefab);
#if UNITY_EDITOR && !USE_PATCH
        if (obj == null)
        {
            var path = GetPath(prefab);
            obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

            if (obj == null)
            {
                Debug.LogError("fail to load object, name : " + prefab);
            }
        }
#endif

#if USE_PATCH
		if (obj == null)
		{
			var bundle = _bundle_container.GetAssetBundleWithObjectName(prefab);
			if (bundle != null)
			{
				Debug.Log("## Table, load from bundle asset : " + prefab);
				var bundle_obj = bundle.LoadAsset(prefab, typeof(TextAsset));
				if (bundle_obj != null)
				{
					obj = bundle_obj as TextAsset;					
				}
				else
				{
					Debug.LogError("## Table, Fail to load bundle asset : " + prefab);
				}
			}
		}
#endif

        return obj;
    }

    public static T Load<T>(GameObject parent, string prefab)
    {
        GameObject Instance = I._Load(parent, prefab);

        if (Instance == null)
            return default(T);

        return Instance.GetComponent<T>();
    }

    public static GameObject Load(GameObject parent, string prefab)
    {
        return I._Load(parent, prefab);
    }

    public static GameObject LoadSrc(string prefab)
    {
        var obj = Resources.Load(prefab) as GameObject;

#if UNITY_EDITOR && !USE_PATCH
        if (obj == null)
        {
            var path = GetPath(prefab);
            if (string.IsNullOrEmpty(path) == false)
            {
                obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            }

            if (obj == null)
            {
                Debug.Log("fail to load src : " + prefab);
            }
        }
#endif

#if USE_PATCH
		if (obj == null)
		{
			var bundle = _bundle_container.GetAssetBundleWithObjectName(prefab);
			if (bundle != null)
			{
				var bundle_obj = bundle.LoadAsset(prefab, typeof(GameObject));
				if (bundle_obj != null)
				{
					obj = bundle_obj as GameObject;
				}				
			}
		}
#endif

        if (obj == null)
        {
            Debug.LogError("## load failed from asset bundle : " + prefab);
        }

        return obj;
    }

    public static Texture2D LoadTexture(string name)
    {
        Texture2D result = null;
        var obj = Resources.Load(name);

#if UNITY_EDITOR && !USE_PATCH
        if (obj == null)
        {
            var path = GetPath(name);
            if (string.IsNullOrEmpty(path) == false)
            {
                obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
                if (obj != null)
                {
                    result = obj as Texture2D;
                }
            }

            if (result == null)
            {
                Debug.Log("fail to load texture : " + path);
            }
        }
#endif

        if (obj == null)
        {
            var bundle = _bundle_container.GetAssetBundleWithObjectName(name);
            if (bundle != null)
            {
                var bundle_obj = bundle.LoadAsset(name, typeof(Texture2D));
                if (bundle_obj != null)
                {
                    result = bundle_obj as Texture2D;
                }
                else
                {
                    Debug.LogError("## load texture failed : " + name);
                }
            }
        }

        if (obj != null && result == null)
        {
            result = obj as Texture2D;
        }

        return result;
    }

    public static AudioClip LoadSound(string name)
    {
        AudioClip result = null;
        var obj = Resources.Load(name);

#if UNITY_EDITOR && !USE_PATCH
        if (obj == null)
        {
            var path = GetPath(name);
            if (string.IsNullOrEmpty(path) == false)
            {
                obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
                if (obj != null)
                {
                    result = obj as AudioClip;
                }
            }

            if (result == null)
            {
                Debug.Log("fail to load AudioClip : " + path);
            }
        }
#endif

        if (obj == null)
        {
            var bundle = _bundle_container.GetAssetBundleWithObjectName(name);
            if (bundle != null)
            {
                var bundle_obj = bundle.LoadAsset(name, typeof(AudioClip));
                if (bundle_obj != null)
                {
                    result = bundle_obj as AudioClip;
                }
                else
                {
                    Debug.LogError("## load AudioClip failed : " + name);
                }
            }
        }

        if (obj != null && result == null)
        {
            result = obj as AudioClip;
        }

        return result;
    }


    private GameObject _Load(GameObject parent, string prefab)
    {
        GameObject go = null;

        go = Resources.Load(prefab) as GameObject;

#if UNITY_EDITOR
        if (go == null)
        {
            var path = GetPath(prefab);
            if (string.IsNullOrEmpty(path) == false)
            {
                go = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            }
        }
#endif

        if (go == null)
        {
            var bundle = _bundle_container.GetAssetBundleWithObjectName(prefab);
            if (bundle != null)
            {
                var bundle_obj = bundle.LoadAsset(prefab, typeof(GameObject));
                if (bundle_obj != null)
                {
                    go = bundle_obj as GameObject;
                }
            }
        }

        if (go == null)
        {
            Debug.LogError("## load failed from asset bundle : " + prefab);
        }
        else
        {
            go = Instantiate(go) as GameObject;
            Vector3 pos = go.transform.localPosition;
            Vector3 scale = go.transform.localScale;

            if (parent != null)
                go.transform.SetParent(parent.transform);

            go.transform.localPosition = pos;
            go.transform.localScale = scale;
        }

        return go;
    }

    public static SpriteAtlas LoadAtlas(string name)
    {
        SpriteAtlas result = null;
        var obj = Resources.Load(name, typeof(SpriteAtlas));

#if UNITY_EDITOR && !USE_PATCH
        if (obj == null)
        {
            var path = GetPath(name);
            if (string.IsNullOrEmpty(path) == false)
            {
                obj = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(SpriteAtlas));
                if (obj != null)
                {
                    result = obj as SpriteAtlas;
                }
            }

            if (result == null)
            {
                Debug.Log("fail to load sprite atlas : " + path);
            }
        }
#endif

        if (obj == null)
        {
            var bundle = _bundle_container.GetAssetBundleWithObjectName(name);
            if (bundle != null)
            {
                var bundle_obj = bundle.LoadAsset(name, typeof(SpriteAtlas));
                if (bundle_obj != null)
                {
                    result = bundle_obj as SpriteAtlas;
                }
                else
                {
                    Debug.LogError("## load AudioClip failed : " + name);
                }
            }
        }

        if (obj != null && result == null)
        {
            result = obj as SpriteAtlas;
        }

        return result;
    }

    public override void ClearAll()
    {

    }
}