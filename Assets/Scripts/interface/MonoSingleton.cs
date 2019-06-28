using UnityEngine;
using System;


public abstract class MonoSinglton<T> : MonoBehaviour where T : MonoSinglton<T>
{
    public static T Instance;

    public static T I
    {
        get { return Instance; }
    }

    public abstract void ClearAll();

    public virtual void Awake()
    {
        Instance = this as T;

        this.Constructor();
    }

    public virtual void Constructor() { }

    protected void Clear()
    {
        Instance = default(T);
    }
}

