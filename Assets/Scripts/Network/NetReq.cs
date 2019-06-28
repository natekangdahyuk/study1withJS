using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetReq
{
    public static IEnumerator coGet(string url, Action<byte[], string> cb) // byte result, string error
    {
        var req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.isDone == true)
        {
            if (req.isHttpError)
            {
                if (string.IsNullOrEmpty(req.error) == false)
                {
                    if (cb != null) cb(null, string.Format("Http Error : {0}", req.error));
                }
                else
                {
                    if (cb != null) cb(null, "Http Error : error string is null");
                }
            }
            else if (req.isNetworkError)
            {
                if (string.IsNullOrEmpty(req.error) == false)
                {
                    if (cb != null) cb(null, string.Format("Network Error : {0}", req.error));
                }
                else
                {
                    if (cb != null) cb(null, "Network Error : error string is null");
                }
            }
            else if (string.IsNullOrEmpty(req.error) == false)
            {
                if (cb != null) cb(null, req.error);
            }
            else
            {
                if (cb != null)
                {
                    if (req.downloadHandler.data == null || req.downloadHandler.data.Length == 0)
                    {
                        cb(null, "Data Error : bytes is null or 0");
                    }
                    else
                    {
                        cb(req.downloadHandler.data, null);
                    }
                }
            }
        }
        else
        {
            if (cb != null)
            {
                cb(null, @"request is not done");
            }
        }

        req.Dispose();

        yield return null;
    }
}