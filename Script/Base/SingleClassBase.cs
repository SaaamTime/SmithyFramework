namespace DIY.Base
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

        protected abstract void Init();

        protected SingleClassBase() { 
            Init();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }

}

