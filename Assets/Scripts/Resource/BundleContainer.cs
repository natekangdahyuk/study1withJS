using UnityEngine;
using System.Collections.Generic;

public class BundleContainer
{
    public class ResData
    {
        public Hash128 _hash128;
        public AssetPatchDataUnit _patch_data = null;
        public AssetBundle _bundle = null;
        public int _ref_count = 0;
        public string _url = null;

        public bool ExistHash()
        {
            if (_patch_data != null)
            {
                return string.IsNullOrEmpty(_patch_data.hash);
            }

            return false;
        }
    }

    Dictionary<string, ResData> _bundleNameMap = new Dictionary<string, ResData>();
    Dictionary<string, ResData> _objectNameMap = new Dictionary<string, ResData>();

    public bool LoadPatchTable(AssetPatchData patch_data)
    {
#if UNITY_ANDROID
		if(patch_data.os_type.CompareTo("Android") != 0)
		{
			Debug.Log("OS type not match with patch list");
			return false;
		}
#elif UNITY_IOS
		Debug.Log(" not implement ios patch logic");
		return false;
#endif
        _bundleNameMap.Clear();
        _objectNameMap.Clear();

        foreach (var unit in patch_data.units)
        {
            var res_data = new ResData
            {
                _patch_data = unit,
                _bundle = null,
                _url = NetURL.GetBundleURL(unit.assetbundle_name),
            };

            if (unit != null)
            {
                if (string.IsNullOrEmpty(unit.hash) == false)
                {
                    res_data._hash128 = Hash128.Parse(unit.hash);
                }
            }

            if (_bundleNameMap.ContainsKey(unit.assetbundle_name) == true)
            {
                Debug.LogError("Bundle name is not unique, duplicated name : " + unit.assetbundle_name);
                return false;
            }

            _bundleNameMap.Add(unit.assetbundle_name, res_data);

            foreach (var data in unit.asset_datas)
            {
                if (_objectNameMap.ContainsKey(data.object_name) == false)
                {
                    _objectNameMap.Add(data.object_name, res_data);
                }
            }
        }

        return true;
    }

    public List<ResData> GetCurrentPatchTargetList()
    {
        var result = new List<ResData>();

        var enumer = _bundleNameMap.GetEnumerator();
        while (enumer.MoveNext())
        {
            var res_data = enumer.Current.Value;
            if (Caching.IsVersionCached(res_data._url, res_data._hash128) == false)
            {
                result.Add(res_data);
            }
        }
        return result;
    }

    public List<ResData> GetRegisterBundleList()
    {
        var result = new List<ResData>();

        var enumer = _bundleNameMap.GetEnumerator();
        while (enumer.MoveNext())
        {
            var res_data = enumer.Current.Value;
            if (Caching.IsVersionCached(res_data._url, res_data._hash128) == true)
            {
                result.Add(res_data);
            }
        }

        if (result.Count > 0)
        {

            var platform_name = "UNITY_EDITOR";
#if UNITY_ANDROID
			platform_name = "Android";
#elif UNITY_IOS
			platform_name = "iOS";
#endif
            var idx = result.FindIndex((x) =>
            {
                return x._patch_data.assetbundle_name.CompareTo(platform_name) == 0;
            });

            if (idx != 0 && idx > -1)
            {
                var platform_res = result[idx];
                result.RemoveAt(idx);
                result.Insert(idx, platform_res);
            }
        }

        return result;

    }

    public ResData GetResDataWithBundleName(string bundle_name)
    {
        ResData res_data = null;
        if (_bundleNameMap.TryGetValue(bundle_name, out res_data) == true)
        {
            return res_data;
        }

        return null;
    }

    public ResData GetResDataWithObjectName(string object_name)
    {
        ResData res_data = null;
        if (_objectNameMap.TryGetValue(object_name, out res_data) == true)
        {
            return res_data;
        }

        return null;
    }

    public AssetBundle GetAssetBundleWithBundleName(string bundle_name)
    {
        var res_data = GetResDataWithBundleName(bundle_name);
        if (res_data != null)
        {
            return res_data._bundle;
        }

        return null;
    }

    public AssetBundle GetAssetBundleWithObjectName(string obj_name)
    {
        ResData res_data = GetResDataWithObjectName(obj_name);
        if (res_data != null)
        {
            return res_data._bundle;
        }

        return null;
    }
}