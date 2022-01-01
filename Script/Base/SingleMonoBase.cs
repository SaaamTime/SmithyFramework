using UnityEngine;

namespace SmithyFramework.Base
{
    public abstract class SingleMonoBase<T> : MonoBehaviour where T:Component
    {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                }
                return _instance;
            }
        }

        protected SingleMonoBase() { }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }

}

