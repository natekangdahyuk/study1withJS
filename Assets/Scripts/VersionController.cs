using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class VersionController
{
    [Serializable]
    class Data
    {
        public string addr = "";
        public bool update = false;
        public string app_addr = "";
    }

    static Data _data = null;

    public static int _main = 1;
    public static int _sub = 2;
    public static int _patch = 8;

    public const int BundleCode = 1208;
    static bool _force_update = false;
    public static bool ForceUpdate
    {
        get { return _data.update; }
    }

    public static string AppLink
    {
        get { return _data.app_addr; }
    }

    public static string Mode
    {
        get
        {
#if USE_DEV
			return "dev";
#elif LIVE_TEST
            return "liveTest";
#else
            return "live";
#endif
        }
    }

    public static string Version
    {
        get
        {
            return string.Format("{0}.{1}.{2}", _main, _sub, _patch);
        }
    }

    public static string DisplayVersion
    {
#if USE_DEV
		get { return string.Format("{0}.{1} - {2}", Version, BundleCode, Mode); }
#else
        get { return string.Format("{0}.{1}", Version, BundleCode); }
#endif
    }

    public static string AppName
    {
        get { return "com.taonplay.girls"; }
    }

    public static IEnumerator coVersionDataConnect(Action<string> result)
    {
        byte[] ver_bytes = null;
        string error = null;

        Logger.E("#### acccess : " + NetURL.VersionURL);

        yield return NetReq.coGet(NetURL.VersionURL, (bytes, net_error) =>
        {
            ver_bytes = bytes;
            error = net_error;
        });

        if (string.IsNullOrEmpty(error) == false)
        {
            if (result != null) result(error);
            Logger.E(error);
            yield break;
        }

        var ver_data_str = System.Text.Encoding.UTF8.GetString(ver_bytes);
        Logger.N(ver_data_str);
        _data = null;
        try
        {
            _data = JsonUtility.FromJson<Data>(ver_data_str);
        }
        catch (Exception e)
        {
            Logger.E("Version data exception : " + e.Message);
        }

        if (_data == null)
        {
            if (result != null) result("Version data error");
            yield break;
        }

        NetURL.ServerAddress = _data.addr;

        if (result != null) result(null);
    }
}