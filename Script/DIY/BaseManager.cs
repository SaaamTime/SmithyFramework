using System;
using UnityEngine;

namespace DIY
{
    public class BaseManager<T>:MonoBehaviour
    {
        public static T Instance;

        public virtual void InitInstance() { 
            Instance = GetComponent<T>();
        }
    }
}
