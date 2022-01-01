﻿namespace SmithyFramework.Base
{
    public abstract class SingleClassBase<T> : System.IDisposable where T : new()
    {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        protected SingleClassBase() { }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }

}

