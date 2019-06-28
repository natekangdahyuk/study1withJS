using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Singleton<T> where T : class, new()
{
    static T Instance = null;

    public static T I
    {
        get
        {
            if (Instance == null)
            {
                Instance = new T();

            }

            return Instance;
        }
    }

}

