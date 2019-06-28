using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class Logger
{
    [Conditional("USE_LOG")]
    public static void N(string log)
    {
        UnityEngine.Debug.Log(log);
    }

    [Conditional("USE_LOG")]
    public static void W(string log)
    {
        UnityEngine.Debug.LogWarning(log);
    }

    public static void E(string log)
    {
        UnityEngine.Debug.LogError(log);
    }
}