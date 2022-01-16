using System;
using UnityEngine;

namespace DIY.Base
{
    public abstract class BaseManager<T>:MonoBehaviour
    {
        public static T Instance;

        protected abstract void Init();//初始化管理器
        public abstract void Reset();//重置管理器
        
        private void Awake()
        {
            Instance = GetComponent<T>();
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
