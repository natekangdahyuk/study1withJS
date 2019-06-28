using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class AssetPatchData
{
    public string os_type;
    public int ver_main;
    public int ver_sub;
    public int ver_patch;
    public string mode;
    public List<AssetPatchDataUnit> units;
}

[Serializable]
public class AssetPatchDataUnit
{
    public int version = 0;
    public uint size_kb = 0;
    public string assetbundle_name;
    public string hash;
    public List<AssetData> asset_datas;
}

[Serializable]
public class AssetData
{
    public string object_name;
    public string path;
}