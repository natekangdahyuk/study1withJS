using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetURL
{
#if USE_DEV
    static string _res_base_url = @"http://taonplay.iptime.org/Patch/";
#else
    static string _res_base_url = @"http://taonplay.gtmc.hscdn.com/patch/";
#endif

    public static string VersionURL
    {
        get
        {
            return string.Format("{0}ver/v{1}_{2}.json", _res_base_url, VersionController.Version, VersionController.Mode);
        }
    }

#if UNITY_EDITOR
    //static string _addr = "http://58.225.62.74:10001";
    static string _addr = "http://taonplay.iptime.org:10001"; //개발
   //static string _addr ="https://one2048.taonplay.com"; //라이브



#else
	static string _addr = null;
#endif
    public static string ServerAddress
    {
        get
        {
            return _addr;
        }
        set
        {
            _addr = value;
        }
    }

    static StringBuilder _sb = new StringBuilder();
    public static string GetReqURL(string detail_addr)
    {
        _sb.Remove(0, _sb.Length);
        _sb.Append(ServerAddress);
        _sb.Append(detail_addr);
        return _sb.ToString();
    }

    public static string GetPatchURL()
    {
        _sb.Remove(0, _sb.Length);
        _sb.Append(_res_base_url);
#if USE_DEV
		_sb.Append(@"/table/Android/dev_patch.json");
#else
        _sb.Append(@"/table/Android/live_patch.json");
#endif
        return _sb.ToString();
    }

    public static string GetBundleURL(string bundle_name)
    {
        _sb.Remove(0, _sb.Length);
        _sb.Append(_res_base_url);
        _sb.AppendFormat(@"/resources/Android/{0}.u3", bundle_name);
        return _sb.ToString();
    }
}